using System.Collections.Generic;
using System.Linq;

namespace Commander
{
    public class InvocationResult<TEntity>
        where TEntity : class
    {
        private readonly TEntity _entity;
        private readonly IList<InvocationProblem> _problems;
        public InvocationResult(TEntity entity)
        {
            _entity = entity;
            _problems = new List<InvocationProblem>();
        }

        public bool HasProblems { get { return _problems.Any(); } }

        public void AddProblem(InvocationProblem problem)
        {
            _problems.Add(problem);
        }

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