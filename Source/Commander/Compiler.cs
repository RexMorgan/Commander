using System;
using Commander.Registration;
using Commander.Registration.Nodes;
using Commander.Runtime;

namespace Commander
{
    public delegate ICompilationContext Compiler(CommandGraph graph, Action<ICommandContext> context, CommandCall call);
}