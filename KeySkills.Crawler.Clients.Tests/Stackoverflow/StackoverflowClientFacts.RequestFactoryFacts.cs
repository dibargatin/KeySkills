using System;
using FluentAssertions;
using Xunit;
using static KeySkills.Crawler.Clients.Stackoverflow.StackoverflowClient;

namespace KeySkills.Crawler.Clients.Tests
{
    public partial class StackoverflowClientFacts
    {
        public class RequestFactoryFacts
        {
            public class CreateRequest_Should
            {
                private RequestFactory _requestFactory = 
                    new RequestFactory {
                        EndpointUri = new Uri("https://stackoverflow.com/jobs/feed")
                    };

                [Fact]
                public void ReturnRequestToExpectedUri() =>
                    _requestFactory.CreateRequest().RequestUri
                        .Should().Be(_requestFactory.EndpointUri);
                        
            }            
        }
    }
}