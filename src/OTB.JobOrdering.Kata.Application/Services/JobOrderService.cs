using OTB.JobOrdering.Kata.Infrastructure.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace OTB.JobOrdering.Kata.Application.Services
{
    public class JobOrderService : IJobOrderService
    {
        private const string JobSplitString = "=>";
        private readonly IList<char> _jobIds = new List<char>();
        private readonly IDictionary<char, IList<char>> _jobRules = new Dictionary<char, IList<char>>();
        private readonly ICollection<char> _finishedJobs = new List<char>();
        private readonly Stack<char> _jobProcessingStack = new Stack<char>();
        private readonly IStringChecker _stringChecker;
        private readonly ISequenceRepository _sequenceRepository;

        public JobOrderService()
        {
            _stringChecker = new StringChecker();
            _sequenceRepository = new SequenceRepository(_jobIds, _jobRules);
        }

        public string OrderJobs(string jobName)
        {
            if (string.IsNullOrEmpty(jobName))
            {
                return "";
            }
            var jobsData =  _stringChecker.SplitToLinesIgnoringEmpty(jobName);

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
            return _sequenceRepository.GetJobSequence();
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

    }
}