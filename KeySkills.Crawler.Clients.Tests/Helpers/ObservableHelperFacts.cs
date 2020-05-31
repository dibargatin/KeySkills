using System;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using FluentAssertions;
using KeySkills.Crawler.Clients.Helpers;
using Xunit;

namespace KeySkills.Crawler.Clients.Tests
{
    public class ObservableHelperFacts
    {
        public class Generate_Should
        {
            [Fact]
            public async void ReturnExpectedResult() =>
                (await ObservableHelper.GenerateFromAsync(
                        () => Task.Run(() => 0),
                        prev => prev < 3,
                        prev => Task.Run(() => prev + 1),
                        result => result
                    ).ToArray().ToTask()
                ).Should().BeEquivalentTo(new[] {0,1,2,3});
            
            [Fact]
            public async void ReturnInitialStateWithoutCondition() =>
                (await ObservableHelper.GenerateFromAsync(
                        () => Task.Run(() => 0),
                        prev => prev < 0,
                        prev => Task.Run(() => prev / 0), // this shouldn't be invoked
                        result => result
                    ).ToArray().ToTask()
                ).Should().BeEquivalentTo(new[] {0});

            public static TheoryData<IObservable<int>> ThrowData =>
                new TheoryData<IObservable<int>> {
                    {
                        ObservableHelper.GenerateFromAsync(
                            () => Task.Run(() => { 
                                var x = 0;
                                return 0 / x; // throw on initialization
                            }),
                            prev => prev < 3,
                            prev => Task.Run(() => prev + 1), 
                            result => result
                        )
                    },
                    {
                        ObservableHelper.GenerateFromAsync(
                            () => Task.Run(() => 0),
                            prev => prev / 0 < 3, // throw on checking condition
                            prev => Task.Run(() => prev + 1), 
                            result => result
                        )
                    },
                    {
                        ObservableHelper.GenerateFromAsync(
                            () => Task.Run(() => 0),
                            prev => prev < 3,
                            prev => Task.Run(() => prev / 0), // throw on iteration
                            result => result
                        )
                    },
                    {
                        ObservableHelper.GenerateFromAsync(
                            () => Task.Run(() => 0),
                            prev => prev < 3, 
                            prev => Task.Run(() => prev + 1), 
                            result => result / 0 // throw on getting result
                        )
                    }
                };

            [Theory]
            [MemberData(nameof(ThrowData))]
            public void ThrowWhenExceptionHandlerIsNotSupplied(IObservable<int> observable) =>
                this.Invoking(_ => observable.ToTask())
                    .Should().ThrowAsync<DivideByZeroException>();

            [Theory]
            [MemberData(nameof(ThrowData))]
            public void NotThrowWhenExceptionHandlerIsSupplied(IObservable<int> observable) =>
                this.Invoking(_ => observable.Subscribe(_ => {}, err => {}))
                    .Should().NotThrow();
        }
    }
}