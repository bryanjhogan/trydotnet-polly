using Polly;
using Polly.CircuitBreaker;
using Polly.Fallback;
using Polly.Retry;
using Polly.Caching.Memory;
using Polly.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
// using System.Runtime.Caching;
using Microsoft.Extensions.Caching.Memory;
using Polly.Bulkhead;
using Polly.Timeout;
using System.Threading.Tasks;

namespace PollyTryDemo
{
    class Program
    {
        static async Task Main(string region = null,
            string session = null,
            string package = null,
            string project = null,
            string[] args = null)
        {
            await Bulkhead();
            switch(region)
            {
                case "lettingItFail":
                    LettingItFail();
                    break;
                case "retryIfException":
                    RetryIfException();
                    break;
                case "retryIfIncorrectStatus":
                    RetryIfIncorrectStatus();
                    break;
                case "retryIfIncorrectStatusOrException":
                    RetryIfIncorrectStatusOrException();
                    break;
                case "waitAndRetry":
                    WaitAndRetry();
                    break;
                case "fallingBack":
                    FallingBack();
                    break;
               case "fallingBackAndReturningADefault":
                    FallingBackAndReturningADefault();
                    break;
                case "timeout":
                    Timeout();
                    break;
                case "basicCircuitBreaker":
                    BasicCircuitBreaker();
                    break;
                case "advancedCircuitBreaker":
                    AdvancedCircuitBreaker();
                    break;
                case "caching":
                    Caching();
                    break;
                case "bulkhead":
                    await Bulkhead();
                    break;
            }
        }

        public static void LettingItFail()
        {
            ErrorProneCode errorProneCode = new ErrorProneCode();
            #region lettingItFail

            int rowsWritten =  errorProneCode.QueryTheDatabase();
            Console.WriteLine($"Received response of {rowsWritten}.");

            #endregion
        }

        public static void RetryIfException()
        {
            ErrorProneCode errorProneCode = new ErrorProneCode();
            #region retryIfException

            RetryPolicy retryPolicy = Policy.Handle<Exception>()
    .Retry(3, (exception, retryCount) =>
    {
        Console.WriteLine($"{exception.GetType()} thrown, retrying {retryCount}.");
    });

            int result = retryPolicy.Execute(() => errorProneCode.QueryTheDatabase());

            Console.WriteLine($"Received response of {result}");
            #endregion
        }

       public static void RetryIfIncorrectStatus()
        {
            ErrorProneCode errorProneCode = new ErrorProneCode();
            #region retryIfIncorrectStatus

            RetryPolicy<Status> retryPolicy = Policy.HandleResult<Status>(s => s!= Status.Success)
    .Retry(3, (response, retryCount) =>
    {
        Console.WriteLine($"Received a response of {response.Result}, retrying {retryCount}.");
    });

            Status result = retryPolicy.Execute(() => errorProneCode.GetStatus());

            Console.WriteLine($"Received response of {result}");
            #endregion
        }

        public static void RetryIfIncorrectStatusOrException()
        {
            ErrorProneCode errorProneCode = new ErrorProneCode();
            #region retryIfIncorrectStatusOrException

            RetryPolicy<Status> retryPolicy = Policy.HandleResult<Status>(s => s!= Status.Success)
    .Or<Exception>()
    .Retry(3, (responseOrException, retryCount, ctx) =>
    {
        Console.WriteLine($"Request failed, retrying {retryCount}.");
    });

            Status result = retryPolicy.Execute(() => errorProneCode.CallRemoteService());

            Console.WriteLine($"Received response of {result},");
            #endregion
        }
        
        public static void WaitAndRetry()
        {
            ErrorProneCode errorProneCode = new ErrorProneCode();
            #region waitAndRetry

            RetryPolicy<Status> retryPolicy = Policy.HandleResult<Status>(s => s!= Status.Success)
    .WaitAndRetry(3,
    sleepDurationProvider: (retryCount) => TimeSpan.FromSeconds(retryCount),
    onRetry: (response, delay, retryCount, ctx) =>
    {
        Console.WriteLine($"Received a response of {response.Result}.");
        Console.WriteLine($"Slept for {delay.Seconds} second(s) before retrying.");
    });

            Status result = retryPolicy.Execute(() => errorProneCode.GetStatus());

            Console.WriteLine($"Received response of {result}.");
            #endregion
        }

        public static void FallingBack()
        {
            ErrorProneCode errorProneCode = new ErrorProneCode();
            #region fallingBack

            FallbackPolicy fallbackPolicy = Policy.Handle<Exception>()
    .Fallback(() => PageAnAdmin() );

            fallbackPolicy.Execute(() => errorProneCode.MakeRequestToADeadService());

            #endregion
        }

        private static void PageAnAdmin(){
            Console.WriteLine($"An exception occurred and the fallback policy kicked in, let me page someone for you...");
        }
        public static void FallingBackAndReturningADefault()
        {
            ErrorProneCode errorProneCode = new ErrorProneCode();
            #region fallingBackAndReturningADefault

            FallbackPolicy<int> fallbackPolicy = Policy<int>
    .Handle<Exception>()
    .Fallback<int>(0);

            int quantity = fallbackPolicy.Execute(() => errorProneCode.GetQuantityAvailable());
            Console.WriteLine($"{quantity} items available.");
            #endregion
        }        

        public static void Timeout()
        {
            ErrorProneCode errorProneCode = new ErrorProneCode();

            #region timeout

            var timeoutPolicy = Policy.Timeout(1, TimeoutStrategy.Pessimistic, OnTimeout); 
            
            int result = timeoutPolicy.Execute( () => errorProneCode.SomeSlowComplexProcess());
            Console.WriteLine($"{result}");
            #endregion
        }

        private static Task OnTimeoutAsync(Context context, TimeSpan timeSpan, Task task)
        {
            Console.WriteLine("Polly timeout policy terminated request because it was taking too long.");
            return Task.CompletedTask;
        }

        private static void OnTimeout(Context context, TimeSpan timeSpan, Task task)
        {
            Console.WriteLine("Polly timeout policy terminated request because it was taking too long.");
        }
        private static void OnBulkheadRejected(Context context)
        {
            Console.WriteLine("Execution and queue slots full. Requests will be rejected.");
        }

        public static void BasicCircuitBreaker()
        {
            ErrorProneCode errorProneCode = new ErrorProneCode();

            #region basicCircuitBreaker
            Console.WriteLine("A BrokenCircuitException is expected in this demo.");

            var circuitBreakerPolicy = Policy
                .HandleResult<Status>(r => r == Status.Fail)
                .CircuitBreaker(2, TimeSpan.FromSeconds(3), OnBreak, OnReset, OnHalfOpen);

            for (int loop = 1; loop <= 3; loop++)
            {
                Status statusAResult = circuitBreakerPolicy.Execute(() => errorProneCode.TargetA());
                Console.WriteLine($"Target A (call {loop}) status: {statusAResult}.");
                Status statusBResult = circuitBreakerPolicy.Execute(() => errorProneCode.TargetB());
                Console.WriteLine($"Target B (call {loop}) status: {statusBResult}.");
            }

            #endregion
        }

        public static void AdvancedCircuitBreaker()
        {
            ErrorProneCode errorProneCode = new ErrorProneCode();

            #region advancedCircuitBreaker
            Console.WriteLine("A BrokenCircuitException is expected in this demo.");

            var advancedCircuitBreakerPolicy = Policy
                .HandleResult<Status>(r => r == Status.Fail)
                .AdvancedCircuitBreaker(.5,  TimeSpan.FromSeconds(3), 6, TimeSpan.FromSeconds(5), OnBreak, OnReset, OnHalfOpen );

            for (int loop = 1; loop <= 4; loop++)
            {
                Status statusAResult = advancedCircuitBreakerPolicy.Execute(() => errorProneCode.TargetA());
                Console.WriteLine($"Target A (call {loop}) status: {statusAResult}.");
                Status statusBResult = advancedCircuitBreakerPolicy.Execute(() => errorProneCode.TargetB());
                Console.WriteLine($"Target B (call {loop}) status: {statusBResult}.");
                Thread.Sleep(1000);
            }

            #endregion
        }

        private static void OnHalfOpen()
        {
            Console.WriteLine("Circuit test, one request will be allowed.");
        }

        private static void OnReset()
        {
            Console.WriteLine("Circuit closed, request allowed.");
        }

        private static void OnBreak(DelegateResult<Status> status, TimeSpan timeSpan)
        {
            Console.WriteLine($"Circuit open, too many failures, requests blocked.");
        }

        public static void Caching()
        {
            ErrorProneCode errorProneCode = new ErrorProneCode();
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var memoryCacheProvider = new MemoryCacheProvider(memoryCache);
            
            #region caching
            CachePolicy<int> cachePolicy =
    Policy.Cache<int>(memoryCacheProvider, TimeSpan.FromSeconds(1.5));
 
            for (int loop = 1; loop <= 10; loop++)
            {
                int result = cachePolicy.Execute(context => errorProneCode.GetSomeNumberThatMightBeCacheable(), new Context("ContextKey"));
                
                Console.WriteLine($"result={result}. cachePolicy executed {loop} time(s). GetSomeNumberThatMightBeCacheable method really called {result} time(s).");
                Thread.Sleep(500);
            }

            #endregion
        }

        public static async Task Bulkhead()
        {
            ErrorProneCode errorProneCode = new ErrorProneCode();
            await Task.Delay(1);
            #region bulkhead

            AsyncBulkheadPolicy bulkheadPolicyAsync = Policy.BulkheadAsync(2, 3, OnBulkheadRejectedAsync);
            
            for (int loop = 0; loop < 10; loop++)
            {
                var result = bulkheadPolicyAsync.ExecuteAsync( async () => await errorProneCode.SomeSlowComplexProcessAsync());
            }
           
            #endregion
        }

        private static Task OnBulkheadRejectedAsync(Context context)
        {
            Console.WriteLine("Execution and queue slots full. Requests will be rejected.");
            return Task.CompletedTask;
        }
    }
}
