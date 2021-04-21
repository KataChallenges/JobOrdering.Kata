using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OTB.JobOrdering.Kata.Infrastructure.Extensions
{
    public class SequenceRepository : ISequenceRepository
    {
        private readonly ICollection<char> _finishedJobs = new List<char>();
        private readonly IEnumerable<char> _jobIds;
        private readonly Stack<char> _jobProcessingStack = new Stack<char>();
        private readonly IDictionary<char, IList<char>> _jobRules;

        public SequenceRepository(IEnumerable<char> jobIds, IDictionary<char, IList<char>> rulesDictionary)
        {
            _jobIds = jobIds;
            _jobRules = rulesDictionary;
        }

        public string GetJobSequence()
        {
            _jobProcessingStack.Clear();
            _finishedJobs.Clear();

            var stringBuilder = new StringBuilder();
            foreach (var job in _jobIds)
                Process(job, stringBuilder);

            return stringBuilder.ToString();
        }


        private void Process(char c, StringBuilder stringBuilder)
        {
            if (_jobProcessingStack.Contains(c))
                throw new InvalidOperationException("Circular dependency, please avoid.");

            _jobProcessingStack.Push(c);
            InternalProcess(c, stringBuilder);
            _jobProcessingStack.Pop();
        }

        private void InternalProcess(char c, StringBuilder stringBuilder)
        {
            if (_finishedJobs.Contains(c))
                return;

            if (_jobRules.ContainsKey(c))
                foreach (var ruleJobId in _jobRules[c])
                    Process(ruleJobId, stringBuilder);

            stringBuilder.Append(c);
            _finishedJobs.Add(c);
        }
    }
}