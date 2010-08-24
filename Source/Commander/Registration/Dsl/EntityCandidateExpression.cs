using System;
using System.Linq.Expressions;
using FubuCore;

namespace Commander.Registration.Dsl
{
    public class EntityCandidateExpression
    {
        private readonly EntityMatcher _matcher;
        private readonly TypePool _types;

        public EntityCandidateExpression(EntityMatcher matcher, TypePool types)
        {
            _matcher = matcher;
            _types = types;
        }

        public EntityCandidateExpression ExcludeTypes(Expression<Func<Type, bool>> filter)
        {
            _matcher.TypeFilters.Excludes += filter;
            return this;
        }

        public EntityCandidateExpression Exclude<T>()
        {
            _matcher.TypeFilters.Excludes += (type => type.Equals(typeof(T)));
            return this;
        }

        public EntityCandidateExpression IncludeTypesNamed(Expression<Func<string, bool>> filter)
        {
            var typeParam = Expression.Parameter(typeof(Type), "type"); // type =>
            var nameProp = Expression.Property(typeParam, "Name");  // type.Name
            var invokeFilter = Expression.Invoke(filter, nameProp); // filter(type.Name)
            var lambda = Expression.Lambda<Func<Type, bool>>(invokeFilter, typeParam); // type => filter(type.Name)

            return IncludeTypes(lambda);
        }

        public EntityCandidateExpression IncludedTypesInNamespaceContaining<T>()
        {
            _matcher.TypeFilters.Includes += (type => type.Namespace == typeof (T).Namespace);
            return this;
        }

        public EntityCandidateExpression IncludeTypes(Expression<Func<Type, bool>> filter)
        {
            _matcher.TypeFilters.Includes += filter;
            return this;
        }

        public EntityCandidateExpression IncludeTypesImplementing<T>()
        {
            return IncludeTypes(type => !type.IsOpenGeneric() && type.IsConcreteTypeOf<T>());
        }

        public EntityCandidateExpression ExcludeNonConcreteTypes()
        {
            _matcher.TypeFilters.Excludes += type => !type.IsConcrete();
            return this;
        }

        public EntityCandidateExpression IncludeType<T>()
        {
            _types.AddType(typeof(T));
            _matcher.TypeFilters.Includes += type => type == typeof(T);
            return this;
        }
    }
}