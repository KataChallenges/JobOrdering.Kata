using System.Collections.Generic;

namespace OTB.JobOrdering.Kata.Application.Tests.Unit
{
    internal interface IJobOrderService
    {
        IEnumerable<char> OrderJobs(string v);
    }
}