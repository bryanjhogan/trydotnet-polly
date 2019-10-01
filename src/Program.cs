using Polly;
using Polly.CircuitBreaker;
using Polly.Fallback;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;

namespace PollyTryDemo
{
    class Program
    {

        static void Main(string region = null,
            string session = null,
            string package = null,
            string project = null,
            string[] args = null)
        {
            //AdvancedCircuitBreaker();
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
               case "basicCircuitBreaker":
                    BasicCircuitBreaker();
                    break;
                case "advancedCircuitBreaker":
                    AdvancedCircuitBreaker();
                    break;
            }
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
                Console.WriteLine($"Target A (call {loop})  status: {statusAResult}");
                Status statusBResult = circuitBreakerPolicy.Execute(() => errorProneCode.TargetB());
                Console.WriteLine($"Target B (call {loop}) status: {statusBResult}");
            }

            #endregion
        }

        public static void AdvancedCircuitBreaker()
        {
            ErrorProneCode errorProneCode = new ErrorProneCode();

            #region advancedCircuitBreaker
            Console.WriteLine("A  BrokenCircuitException is expected in this demo.");

            var advancedCircuitBreakerPolicy = Policy
                .HandleResult<Status>(r => r == Status.Fail)
                .AdvancedCircuitBreaker(.5,  TimeSpan.FromSeconds(3), 6, TimeSpan.FromSeconds(5), OnBreak, OnReset, OnHalfOpen );

            for (int loop = 1; loop <= 4; loop++)
            {
                Status statusAResult = advancedCircuitBreakerPolicy.Execute(() => errorProneCode.TargetA());
                Console.WriteLine($"Target A (call {loop})  status: {statusAResult}");
                Status statusBResult = advancedCircuitBreakerPolicy.Execute(() => errorProneCode.TargetB());
                Console.WriteLine($"Target B (call {loop}) status: {statusBResult}");
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
            Console.WriteLine($"{quantity} items available");
            #endregion
        }
        public static void LettingItFail()
        {
            ErrorProneCode errorProneCode = new ErrorProneCode();
            #region lettingItFail

            int rowsWritten =  errorProneCode.QueryTheDatabase();
            Console.WriteLine($"Received response of {rowsWritten}");

            #endregion
        }
        public static void RetryIfException()
        {
            ErrorProneCode errorProneCode = new ErrorProneCode();
            #region retryIfException

            RetryPolicy retryPolicy = Policy.Handle<Exception>()
    .Retry(3, (exception, retryCount) =>
    {
        Console.WriteLine($"{exception.GetType()} thrown, retrying {retryCount}");
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
        Console.WriteLine($"Received a response of {response.Result}, retrying {retryCount}");
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
        Console.WriteLine($"Request failed, retrying {retryCount}");
    });

            Status result = retryPolicy.Execute(() => errorProneCode.CallRemoteService());

            Console.WriteLine($"Received response of {result}");
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

            Console.WriteLine($"Received response of {result}");
            #endregion
        }
        
        // private void CheckForException()
        // {
        //     ErrorProneCode errorProneCode = new ErrorProneCode();

        //     RetryPolicy retryIfException = Policy.Handle<Exception>()
        //         .Retry(4, (exception, retryCount) =>
        //         {
        //             Console.WriteLine($"Got a response of {exception} (expected 0), retrying {retryCount}");
        //         });

        //     retryIfException.Execute(errorProneCode.DoSomethingThatMightThrowException);
        // }
        // private void CheckForNonZeroOrException()
        // {
        //     ErrorProneCode errorProneCode = new ErrorProneCode();

        //     // Retry if the response is not 0 or there is a DivideByZeroException
        //     RetryPolicy<int> retryPolicyNeedsAResponseOfOne = Policy.HandleResult<int>(i => i != 0)
        //         .Or<DivideByZeroException>()
        //         .Retry(4, (response, retryCount) =>
        //         {
        //             Console.WriteLine($"Got a response of {response.Result} (expected 0), retrying {retryCount}");
        //         });

        //     int number = retryPolicyNeedsAResponseOfOne.Execute(() => errorProneCode.QueryTheDatabase());

        //     Console.WriteLine($"Got expected reponse = {number}\n\n");
        // }

        // private void CheckingSizeOfReturnedList()
        // {
        //     ErrorProneCode errorProneCode = new ErrorProneCode();

        //     // Retry if the IEnumerable does not contain three items
        //     RetryPolicy<IEnumerable<int>> retryPolicyNeedsResponeWithTwoNumbers = Policy.HandleResult<IEnumerable<int>>(l => l.Count() != 3)
        //        .Retry(4, (response, retryCount) =>
        //        {
        //            Console.WriteLine($"Got a response with {response.Result.Count()} entries (expected 3), retrying {retryCount}");
        //        });

        //     var numbers = retryPolicyNeedsResponeWithTwoNumbers.Execute(() => errorProneCode.GetListOfNumbers());

        //     Console.WriteLine($"Got expected response of {numbers.Count()} entries\n\n");
        // }

        // public static void BoolResponse()
        // {
        //     #region BoolResponse
        //     ErrorProneCode errorProneCode = new ErrorProneCode();

        //     // Retry if the result is not true
        //     RetryPolicy<bool> retryPolicyNeedsTrueResponse = Policy.HandleResult<bool>(b => b != true)
        //        .Retry(4, (response, retryCount) =>
        //        {
        //            Console.WriteLine($"Got a response of {response.Result} entries (expected true), retrying {retryCount}");
        //        });

        //     bool result = retryPolicyNeedsTrueResponse.Execute(() => errorProneCode.GetBool());

        //     Console.WriteLine($"Got expected response of {result}\n\n");
        //     #endregion
        // }


    }
}
