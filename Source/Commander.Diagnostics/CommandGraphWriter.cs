using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Commander.Registration;
using Commander.Registration.Nodes;
using FubuMVC.Core;
using FubuMVC.Core.Diagnostics;
using FubuMVC.Core.Urls;
using HtmlTags;

namespace Commander.Diagnostics
{
    public class CommandGraphWriter
    {
        private readonly CommandGraph _graph;
        private readonly IUrlRegistry _urls;
        private readonly string _diagnosticsNamespace;
        public CommandGraphWriter(CommandGraph graph, IUrlRegistry urls)
        {
            _graph = graph;
            _urls = urls;
            _diagnosticsNamespace = GetType().Namespace;
        }

        [Description("show all command chains"), DiagnosticsAction]
        [UrlPattern(DiagnosticUrlPolicy.DIAGNOSTICS_URL_ROOT + "/commander")]
        public HtmlDocument Chains()
        {
            var content = new DivTag("Chains")
                .Child(WriteCommandChainTable("New Chains", _graph.ChainsForNew, _chains, new EntityColumn()))
                .Child(WriteCommandChainTable("Existing Chains", _graph.ChainsForExisting, _chains, new EntityColumn()));

            return BuildDocument("Registered Command Chains", content);
        }

        public HtmlDocument Chain(ChainRequest chainRequest)
        {
            var title = "Command Chain " + chainRequest.Id;

            var commandChain = _graph.FindChain(chain => chain.UniqueId == chainRequest.Id);
            if (commandChain == null)
            {
                return BuildDocument("Unknown chain", new HtmlTag("span").Text("No command chain registered with ID: " + chainRequest.Id));
            }

            var content = new HtmlTag("div").AddClass("main-content");

            var document = new HtmlTag("div");

            var nodeTable = new TableTag();
            nodeTable.AddHeaderRow(header =>
            {
                header.Header("Category");
                header.Header("Description");
                header.Header("Type");
            });
            foreach (var node in commandChain)
            {

                var description = node.ToString();
                nodeTable.AddBodyRow(row =>
                {
                    row.Cell().Text(node.Category.ToString());
                    row.Cell().Text(description);
                    row.Cell().Text(node.GetType().FullName);
                    if (description.Contains(_diagnosticsNamespace))
                    {
                        row.AddClass(BehaviorGraphWriter.FUBU_INTERNAL_CLASS);
                    }
                });
            }


            var logDiv = new HtmlTag("div").AddClass("convention-log");
            var ul = logDiv.Add("ul");

            var observer = _graph.Observer;
            commandChain.Each(
                node => observer.GetLog(node).Each(
                            entry => ul.Add("li").Text(entry)));

            content.AddChildren(new[]{
                document, 
                new HtmlTag("h3").Text("Nodes:"),
                nodeTable,
                new HtmlTag("h3").Text("Log:"),
                logDiv});

            return BuildDocument(title, content);
        }

        private HtmlDocument BuildDocument(string title, params HtmlTag[] tags)
        {
            return DiagnosticHtml.BuildDocument(_urls, title, tags);
        }

        private HtmlTag WriteCommandChainTable(string heading, IEnumerable<CommandChain> chains, params IColumn[] columns)
        {
            chains = chains.OrderBy(c => c.EntityType.Name);
            var container = new DivTag(heading)
                .Child(new HtmlTag("h3").Text(heading));
            var table = new TableTag();
            table.Attr("cellspacing", "0");
            table.Attr("cellpadding", "0");
            table.AddHeaderRow(row => columns.Each(c => row.Header(c.Header())));

            chains.Each(chain => table.AddBodyRow(row => columns.Each(col => col.WriteBody(chain, row, row.Cell()))));

            return container.Child(table);
        }

        private IColumn _chains
        {
            get
            {
                return new ChainColumn();
            }
        }

        private IColumn _calls
        {
            get
            {
                return new CallColumn();
            }
        }
    }

    public interface IColumn
    {
        string Header();
        void WriteBody(CommandChain chain, HtmlTag row, HtmlTag cell);
        string Text(CommandChain chain);
    }

    public class ChainColumn : IColumn
    {
        public string Header()
        {
            return "Chain";
        }

        public void WriteBody(CommandChain chain, HtmlTag row, HtmlTag cell)
        {
            cell.Child(new LinkTag(Text(chain), "commander/chain/" + chain.UniqueId).AddClass("chainId"));
        }

        public string Text(CommandChain chain)
        {
            return chain.UniqueId.ToString();
        }
    }

    public class EntityColumn : IColumn
    {
        public string Header()
        {
            return "Entity";
        }

        public void WriteBody(CommandChain chain, HtmlTag row, HtmlTag cell)
        {
            string text = Text(chain);

            cell.Text(text);
        }

        public string Text(CommandChain chain)
        {
            return chain.EntityType.AssemblyQualifiedName;
        }
    }

    public class CallColumn : IColumn
    {
        public string Header()
        {
            return "Call(s)";
        }

        public void WriteBody(CommandChain chain, HtmlTag row, HtmlTag cell)
        {
            string text = Text(chain);

            cell.Text(text);
        }

        public string Text(CommandChain chain)
        {
            var descriptions = chain.Calls.Select(x => x.Description).ToArray();
            return descriptions.Length == 0 ? " -" : descriptions.Join(", ");
        }
    }

    public class ChainRequest
    {
        public Guid Id { get; set; }
    }
}