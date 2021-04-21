using System.Collections.Generic;

namespace OTB.JobOrdering.Kata.Infrastructure.Extensions
{
    public interface ISequenceRepository
    {
        string GetJobSequence();
    }
}