using System;
using System.Collections;
using System.Collections.Generic;
using Commander.Commands;

namespace Commander.Diagnostics
{
    public class CommandReport : FubuMVC.Core.Diagnostics.TimedReport, IEnumerable<ICommandDetails>
    {
        private readonly IList<ICommandDetails> _details = new List<ICommandDetails>();

        public CommandReport(ICommand behavior)
        {
            Description = behavior.ToString();
            CommandType = behavior.GetType();
        }

        public Type CommandType { get; set; }
        public string Description { get; private set; }

        public IEnumerator<ICommandDetails> GetEnumerator()
        {
            return _details.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void AddDetail(ICommandDetails detail)
        {
            _details.Add(detail);
        }
    }
}