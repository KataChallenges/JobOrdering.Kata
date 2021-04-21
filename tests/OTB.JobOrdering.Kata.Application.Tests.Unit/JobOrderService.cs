using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace OTB.JobOrdering.Kata.Application.Tests.Unit
{
    public class JobOrderService : IJobOrderService
    {
        private const string JobSplitString = "=>";
        private readonly IList<char> _jobIds = new List<char>();
        private readonly IDictionary<char, IList<char>> _jobRules = new Dictionary<char, IList<char>>();


        private readonly ICollection<char> _finishedJobs = new List<char>();
        private readonly Stack<char> _jobProcessingStack = new Stack<char>();

        public JobOrderService()
        {

        }

        public string OrderJobs(string jobName)
        {
            if (string.IsNullOrEmpty(jobName))
            {
                return "";
            }
            var jobsData = SplitToLinesIgnoringEmpty(jobName);

            foreach (var job in jobsData)
            {
                var data = job.Split(new[] { JobSplitString }, StringSplitOptions.RemoveEmptyEntries);
                if (data.Length > 1)
                {
                    Register(data[0].Trim().ElementAt(0), data[1].Trim().ElementAt(0));
                }
                else if (data.Length > 0)
                {
                    Register(data[0].Trim().ElementAt(0));
                }
                else
                {
                    throw new Exception($"Invalid job: {job}");
                }
            }
            return GetJobSequence();
        }


        public void Register(char jobId, char dependsOn)
        {
            Register(jobId);
            if (_jobRules.ContainsKey(jobId))
                _jobRules[jobId].Add(dependsOn);
            else
                _jobRules[jobId] = new List<char> { dependsOn };
        }

        public void Register(char jobId)
        {
            if (!_jobIds.Contains(jobId))
                _jobIds.Add(jobId);
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


        private IEnumerable<string> SplitToLinesIgnoringEmpty(string jobSource)
        {
            if (string.IsNullOrEmpty(jobSource))
            {
                yield break;
            }

            using (var stringReader = new StringReader(jobSource))
            {
                string readString;
                while (stringReader.Peek() != -1
                       && !string.IsNullOrEmpty(readString = stringReader.ReadLine()))
                {
                    yield return readString;
                }
            }
        }

    }
}