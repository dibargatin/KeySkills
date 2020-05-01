using System;
using Xunit;
using FluentAssertions;

namespace KeySkills.Crawler.Core.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            "ABCDEFGHI".Should()
                .StartWith("AB")
                .And.EndWith("HI")
                .And.Contain("EF")
                .And.HaveLength(9);
        }
    }
}
