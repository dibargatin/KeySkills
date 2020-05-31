using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace KeySkills.Crawler.Clients.Helpers
{
    public class ObservableHelper
    {
        public static IObservable<TResult> GenerateFromAsync<TState, TResult>(
            Func<Task<TState>> initialState,
            Func<TState, bool> condition,
            Func<TState, Task<TState>> iterate,
            Func<TState, TResult> resultSelector,
            IScheduler scheduler = null)
        {
            return Observable.Create<TResult>(async observable => 
                (scheduler ?? Scheduler.Default).Schedule(
                    await initialState(), 
                    async (state, self) => {                        
                        try 
                        { 
                            observable.OnNext(resultSelector(state));

                            if (condition(state)) self(await iterate(state));
                            else observable.OnCompleted();
                        }
                        catch (Exception ex)
                        {
                            observable.OnError(ex);
                        }
                    }));
        }
    }
}