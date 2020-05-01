using System;
using Xunit;
using FluentAssertions;

namespace KeySkills.Crawler.App.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            1.Should().BeGreaterThan(0);
        }
    }
}
