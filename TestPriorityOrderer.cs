using Xunit;
using Xunit.Sdk;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public class TestPriorityOrderer : ITestCaseOrderer
{
    IEnumerable<TTestCase> ITestCaseOrderer.OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases)
    {
        var sorted = testCases.ToList();

        sorted.Sort((x, y) =>
        {
            var xName = x.TestMethod.Method.Name;
            var yName = y.TestMethod.Method.Name;

            // Extract number from TC_001, TC_002, etc.
            var xMatch = Regex.Match(xName, @"TC_(\d+)");
            var yMatch = Regex.Match(yName, @"TC_(\d+)");

            if (xMatch.Success && yMatch.Success)
            {
                int xNum = int.Parse(xMatch.Groups[1].Value);
                int yNum = int.Parse(yMatch.Groups[1].Value);
                return xNum.CompareTo(yNum);
            }

            return string.Compare(xName, yName, StringComparison.Ordinal);
        });

        return sorted;
    }
}