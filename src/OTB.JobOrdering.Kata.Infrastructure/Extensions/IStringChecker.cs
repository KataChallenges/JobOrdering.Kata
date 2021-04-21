using System.Collections.Generic;

namespace OTB.JobOrdering.Kata.Infrastructure.Extensions
{
    public interface IStringChecker
    {
        IEnumerable<string> SplitToLinesIgnoringEmpty(string jobSource);
    }
}