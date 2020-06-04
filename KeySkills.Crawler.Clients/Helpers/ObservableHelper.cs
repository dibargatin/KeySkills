using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace KeySkills.Crawler.Clients.Helpers
{
    public class ObservableHelper
    {
        /// <summary>
        /// Executes asynchronous functions while the condition is true
        /// </summary>
        /// <param name="initialState">Initial state to begin execution</param>
        /// <param name="condition">Predicate to stop execution</param>
        /// <param name="iterate">Async function to execute</param>
        /// <param name="resultSelector">Function to transform iteration state to the result type</param>
        /// <param name="scheduler">Scheduler to perform execution</param>
        /// <typeparam name="TState">Type of the iteration state</typeparam>
        /// <typeparam name="TResult">Type of the execution result</typeparam>
        /// <returns>Observable sequence of produced elements</returns>
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