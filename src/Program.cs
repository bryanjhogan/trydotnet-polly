using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Linq;
namespace PollyForNonHttpRequests
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
                case "BoolResponse":
                    BoolResponse();
                    break;
                case "test":
                    Test();
                    break;
                case "status":
                    MyStatus();
                    break;
            }
        }

        public static void Test()
        {
            #region test
            Console.WriteLine("Hello World!");
            #endregion
        }

        // static void Main(string[] args)
        // {
        //     Program p = new Program();
        //     p.CheckForException();
        //     p.CheckForNonZeroOrExeption();
        //     p.CheckingSizeOfReturnedList();
        //     p.CheckingBool();
        //     Console.WriteLine("Press any key to exit...");
        //     Console.ReadKey();
        // }

        private void CheckForException()
        {
            ErrorProneCode errorProneCode = new ErrorProneCode();

            RetryPolicy retryIfException = Policy.Handle<Exception>()
                .Retry(4, (exception, retryCount) =>
                {
                    Console.WriteLine($"Got a response of {exception.Message} (expected 0), retrying {retryCount}");
                });

            retryIfException.Execute(errorProneCode.DoSomethingThatMightThrowException);
        }
        private void CheckForNonZeroOrExeption()
        {
            ErrorProneCode errorProneCode = new ErrorProneCode();

            // Retry if the response is not 0 or there is a DivideByZeroException
            RetryPolicy<int> retryPolicyNeedsAResponseOfOne = Policy.HandleResult<int>(i => i != 0)
                .Or<DivideByZeroException>()
                .Retry(4, (response, retryCount) =>
                {
                    Console.WriteLine($"Got a response of {response.Result} (expected 0), retrying {retryCount}");
                });

            int number = retryPolicyNeedsAResponseOfOne.Execute(() => errorProneCode.GetSomeNumber());

            Console.WriteLine($"Got expected reponse = {number}\n\n");
        }

        private void CheckingSizeOfReturnedList()
        {
            ErrorProneCode errorProneCode = new ErrorProneCode();

            // Retry if the IEnumerable does not contain three items 
            RetryPolicy<IEnumerable<int>> retryPolicyNeedsResponeWithTwoNumbers = Policy.HandleResult<IEnumerable<int>>(l => l.Count() != 3)
               .Retry(4, (response, retryCount) =>
               {
                   Console.WriteLine($"Got a reponse with {response.Result.Count()} entries (expected 3), retrying {retryCount}");
               });

            var numbers = retryPolicyNeedsResponeWithTwoNumbers.Execute(() => errorProneCode.GetListOfNumbers());

            Console.WriteLine($"Got expected reponse of {numbers.Count()} entries\n\n");
        }

        public static void BoolResponse()
        {
            #region BoolResponse
            ErrorProneCode errorProneCode = new ErrorProneCode();

            // Retry if the result is not true
            RetryPolicy<bool> retryPolicyNeedsTrueResponse = Policy.HandleResult<bool>(b => b != true)
               .Retry(4, (response, retryCount) =>
               {
                   Console.WriteLine($"Got a reponse of {response.Result} entries (expected true), retrying {retryCount}");
               });

            bool result = retryPolicyNeedsTrueResponse.Execute(() => errorProneCode.GetBool());

            Console.WriteLine($"Got expected reponse of {result}\n\n");
            #endregion
        }

        public static void MyStatus()
        {
            ErrorProneCode errorProneCode = new ErrorProneCode();
            #region status

            // Retry if the result is not true
            RetryPolicy<Status> retryPolicyNeedsTrueResponse = Policy.HandleResult<Status>(s => s!= Status.Success)
               .Retry(4, (response, retryCount) =>
               {
                   Console.WriteLine($"Got a reponse of {response.Result}  (expected Success), retrying {retryCount}");
               });

            Status result = retryPolicyNeedsTrueResponse.Execute(() => errorProneCode.GetStatus());

            Console.WriteLine($"Got expected reponse of {result}\n\n");
            #endregion
        }
    }

    public enum Status {
        Success,
        Fail,
        Unknown,
        ExceptionOccurred
        };
}
