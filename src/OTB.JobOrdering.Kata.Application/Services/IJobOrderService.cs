using System.Collections.Generic;

namespace OTB.JobOrdering.Kata.Application.Services
{
    public interface IJobOrderService
    {
        string OrderJobs(string jobName);
    }
}