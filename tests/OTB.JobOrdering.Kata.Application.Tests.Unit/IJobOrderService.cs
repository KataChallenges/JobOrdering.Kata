using System.Collections.Generic;

namespace OTB.JobOrdering.Kata.Application.Tests.Unit
{
    public interface IJobOrderService
    {
        string OrderJobs(string jobName);
    }
}