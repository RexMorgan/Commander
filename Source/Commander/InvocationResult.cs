using System.Collections.Generic;
using System.Linq;

namespace Commander
{
    public class InvocationResult<TEntity>
        where TEntity : class
    {
        private readonly TEntity _entity;
        private readonly IEnumerable<InvocationProblem> _problems;
        public InvocationResult(TEntity entity, IEnumerable<InvocationProblem> problems)
        {
            _entity = entity;
            _problems = problems;
        }

        public bool HasProblems { get { return _problems.Any(); } }

        public IEnumerable<InvocationProblem> Problems
        {
            get { return _problems; }
        }

        public TEntity Entity
        {
            get { return _entity; }
        }
    }
}