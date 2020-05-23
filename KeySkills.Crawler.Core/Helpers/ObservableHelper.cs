using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace KeySkills.Crawler.Core.Helpers
{
    public class ObservableHelper
    {
        public static IObservable<TResult> Generate<TResult>(
            Func<Task<TResult>> initialState,
            Func<TResult, bool> condition,
            Func<TResult, Task<TResult>> iterate,
            Func<TResult, TResult> resultSelector,
            IScheduler scheduler = null)
        {
            return Observable.Create<TResult>(async observable => 
                (scheduler ?? Scheduler.Default).Schedule(
                    await initialState(), 
                    async (state, self) => {
                        if (!condition(state)) {
                            observable.OnNext(resultSelector(state));
                            self(await iterate(state));
                        }
                        else observable.OnCompleted();
                    }));
        }
    }
}