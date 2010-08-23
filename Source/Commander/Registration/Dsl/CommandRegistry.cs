using Commander.Util;

namespace Commander.Registration.Dsl
{
    public class CommandRegistry
    {
        private readonly TypePool _types = new TypePool();
        public AppliesToExpression Applies { get { return new AppliesToExpression(_types); } }
    }
}