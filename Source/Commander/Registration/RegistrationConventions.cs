using System.Collections.Generic;

namespace Commander.Registration
{
    public static class RegistrationConventions
    {
        public static void Configure(this IEnumerable<IConfigurationAction> actions, CommandGraph graph)
        {
            actions.Each(x => x.Configure(graph));
        }
    }
}