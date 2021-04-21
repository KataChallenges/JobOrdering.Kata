using System.Collections.Generic;
using System.IO;

namespace OTB.JobOrdering.Kata.Infrastructure.Extensions
{
    public class StringChecker : IStringChecker
    {
        public IEnumerable<string> SplitToLinesIgnoringEmpty(string jobSource)
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