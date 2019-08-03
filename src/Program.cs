using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Linq;

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
            switch(region)
            {
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
            }
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

            Console.WriteLine($"Received reponse of {result}");
            #endregion
        }
        public static void RetryIfIncorrectStatus()
        {
            ErrorProneCode errorProneCode = new ErrorProneCode();
            #region retryIfIncorrectStatus

            RetryPolicy<Status> retryPolicy = Policy.HandleResult<Status>(s => s!= Status.Success)
    .Retry(3, (response, retryCount) =>
    {
        Console.WriteLine($"Received a reponse of {response.Result}, retrying {retryCount}");
    });

            Status result = retryPolicy.Execute(() => errorProneCode.GetStatus());

            Console.WriteLine($"Received reponse of {result}");
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

            Console.WriteLine($"Received reponse of {result}");
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
        Console.WriteLine($"Received a reponse of {response.Result}.");
        Console.WriteLine($"Slept for {delay.Seconds} second(s) before retrying.");
    });

            Status result = retryPolicy.Execute(() => errorProneCode.GetStatus());

            Console.WriteLine($"Received reponse of {result}");
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
        // private void CheckForNonZeroOrExeption()
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
        //            Console.WriteLine($"Got a reponse with {response.Result.Count()} entries (expected 3), retrying {retryCount}");
        //        });

        //     var numbers = retryPolicyNeedsResponeWithTwoNumbers.Execute(() => errorProneCode.GetListOfNumbers());

        //     Console.WriteLine($"Got expected reponse of {numbers.Count()} entries\n\n");
        // }

        // public static void BoolResponse()
        // {
        //     #region BoolResponse
        //     ErrorProneCode errorProneCode = new ErrorProneCode();

        //     // Retry if the result is not true
        //     RetryPolicy<bool> retryPolicyNeedsTrueResponse = Policy.HandleResult<bool>(b => b != true)
        //        .Retry(4, (response, retryCount) =>
        //        {
        //            Console.WriteLine($"Got a reponse of {response.Result} entries (expected true), retrying {retryCount}");
        //        });

        //     bool result = retryPolicyNeedsTrueResponse.Execute(() => errorProneCode.GetBool());

        //     Console.WriteLine($"Got expected reponse of {result}\n\n");
        //     #endregion
        // }


    }
}
