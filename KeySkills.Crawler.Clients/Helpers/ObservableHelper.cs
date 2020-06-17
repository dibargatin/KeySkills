using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace KeySkills.Crawler.Clients.Helpers
{
    public static class ObservableHelper
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

        /// <summary>
        /// Filter out observable items
        /// </summary>
        /// <param name="source">Observable sequence</param>
        /// <param name="predicate">Predicate for filtering</param>
        /// <param name="value">Expected value of the predicate</param>
        /// <typeparam name="T">Type of observable item</typeparam>
        /// <returns>Filtered observable sequence</returns>
        public static IObservable<T> Where<T>(
            this IObservable<T> source, 
            Func<T, Task<bool>> predicate, 
            bool value = true
        ) =>
            source.SelectMany(async item => new {
                ShouldInclude = await predicate(item) == value,
                Item = item
            })
            .Where(x => x.ShouldInclude)
            .Select(x => x.Item);        
    }
}