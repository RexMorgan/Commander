using System;
using System.Linq.Expressions;
using FubuCore;

namespace Commander.Registration.Dsl
{
    public class TypeCandidateExpression
    {
        private readonly ITypeMatcher _matcher;
        private readonly TypePool _types;

        public TypeCandidateExpression(ITypeMatcher matcher, TypePool types)
        {
            _matcher = matcher;
            _types = types;
        }

        public TypeCandidateExpression ExcludeTypes(Expression<Func<Type, bool>> filter)
        {
            _matcher.TypeFilters.Excludes += filter;
            return this;
        }

        public TypeCandidateExpression Exclude<T>()
        {
            _matcher.TypeFilters.Excludes += (type => type.Equals(typeof(T)));
            return this;
        }

        public TypeCandidateExpression IncludeTypesNamed(Expression<Func<string, bool>> filter)
        {
            var typeParam = Expression.Parameter(typeof(Type), "type"); // type =>
            var nameProp = Expression.Property(typeParam, "Name");  // type.Name
            var invokeFilter = Expression.Invoke(filter, nameProp); // filter(type.Name)
            var lambda = Expression.Lambda<Func<Type, bool>>(invokeFilter, typeParam); // type => filter(type.Name)

            return IncludeTypes(lambda);
        }

        public TypeCandidateExpression IncludedTypesInNamespaceContaining<T>()
        {
            _matcher.TypeFilters.Includes += (type => type.Namespace == typeof (T).Namespace);
            return this;
        }

        public TypeCandidateExpression IncludeTypes(Expression<Func<Type, bool>> filter)
        {
            _matcher.TypeFilters.Includes += filter;
            return this;
        }

        public TypeCandidateExpression IncludeTypesImplementing<T>()
        {
            return IncludeTypes(type => !type.IsOpenGeneric() && type.IsConcreteTypeOf<T>());
        }

        public TypeCandidateExpression IncludeTypesClosing(Type openType)
        {
            if (!openType.IsOpenGeneric())
            {
                throw new ApplicationException("This scanning operation can only be used with open generic types");
            }

            return IncludeTypes(type => type.ImplementsInterfaceTemplate(openType));
        }

        public TypeCandidateExpression ExcludeNonConcreteTypes()
        {
            _matcher.TypeFilters.Excludes += type => !type.IsConcrete();
            return this;
        }

        public TypeCandidateExpression IncludeType<T>()
        {
            _types.AddType(typeof(T));
            _matcher.TypeFilters.Includes += type => type == typeof(T);
            return this;
        }
    }
}