using System.Reflection;
using FubuCore;
using FubuCore.Reflection;
using FubuMVC.Core.Diagnostics;
using FubuMVC.Core.Registration.Conventions;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Registration.Routes;

namespace Commander.Diagnostics
{
    public class CommanderDiagnosticUrlPolicy : IUrlPolicy
    {
        public const string CommanderDiagnosticsUrlRoot = DiagnosticUrlPolicy.DIAGNOSTICS_URL_ROOT + "/commander";
        public bool Matches(ActionCall call, FubuMVC.Core.Diagnostics.IConfigurationObserver log)
        {
            return call.HandlerType == typeof(CommandGraphWriter);
        }

        public IRouteDefinition Build(ActionCall call)
        {
            MethodInfo method = call.Method;
            var definition = call.ToRouteDefinition();
            definition.Append(CommanderDiagnosticsUrlRoot + "/" + UrlFor(method));
            if (call.InputType().CanBeCastTo<ChainRequest>())
            {
                definition.AddRouteInput(new RouteInput(ReflectionHelper.GetAccessor<ChainRequest>(x => x.Id)), true);
            }
            return definition;
        }

        public static string UrlFor(MethodInfo method)
        {
            return method.Name.ToLower();
        }

        public static string RootUrlFor(MethodInfo method)
        {
            return ("~/" + CommanderDiagnosticsUrlRoot + "/" + UrlFor(method)).ToAbsoluteUrl();
        }

    }
}