using System.Collections.Generic;

namespace Commander.Registration.Dsl
{
    public class CommandRegistry
    {
        private readonly TypePool _types = new TypePool();
        private readonly EntityMatcher _entityMatcher = new EntityMatcher();
        private readonly IList<IConfigurationAction> _policies = new List<IConfigurationAction>();

        public AppliesToExpression Applies { get { return new AppliesToExpression(_types); } }
        public EntityCandidateExpression Entities { get { return new EntityCandidateExpression(_entityMatcher, _types); } }
        public PoliciesExpression Policies { get { return new PoliciesExpression(_policies); } }
    }
}