using System;
using System.Collections.Generic;

namespace PollyTryDemo
{
    public class ErrorProneCode
    {
        static int _writeToSomeDbCounter = 0;
        int _queryTheDatabaseCounter = 0;
        int _getStatusCounter = 0;
        int _callRemoteServiceCounter = 0;

        public int WriteToSomeDb()
        {
            _writeToSomeDbCounter++; // seems to be an issue with static variables in try.net
            if (_writeToSomeDbCounter %4 == 1)
            {
                throw new NotImplementedException("Someone forgot to implement some code.");
            }
            if (_writeToSomeDbCounter %4  == 2)
            {
                throw new InsufficientMemoryException("You ran out of memory!");
            }
            if (_writeToSomeDbCounter %4  == 3)
            {
                throw new StackOverflowException("The stack overflowed.");
            }
            if(_writeToSomeDbCounter %4 == 0)
            {
                throw new AccessViolationException("You don't have permission.");
            }
            return -1;
        }


        public int QueryTheDatabase()
        {
            _queryTheDatabaseCounter++;
            if (_queryTheDatabaseCounter == 1)
            {
                throw new NotImplementedException("Someone forgot to implement some code.");
            }
            if (_queryTheDatabaseCounter == 2)
            {
                throw new InsufficientMemoryException("You ran out of memory!");
            }
            if (_queryTheDatabaseCounter == 3)
            {
                throw new StackOverflowException("Not the website.");
            }
            if(_queryTheDatabaseCounter == 4)
            {
                return 0;
            }
            return -1;
        }

        public Status GetStatus()
        {
            _getStatusCounter++;
            switch (_getStatusCounter)
            {
                case 1:
                    return Status.ExceptionOccurred;
                case 2: 
                    return Status.Fail;
                case 3:
                    return Status.Unknown;
                case 4: 
                    _getStatusCounter = 0;
                    return Status.Success;
            }
            return Status.Fail;
        }

        public Status CallRemoteService()
        {
            _callRemoteServiceCounter++;
            switch (_callRemoteServiceCounter)
            {
                case 1:
                    return Status.ExceptionOccurred;
                case 2: 
                    throw new OutOfMemoryException();
                case 3:
                    return Status.Unknown;
                case 4: 
                    return Status.Success;
            }
            return Status.Fail;
        }

        // public Status CallAnotherRemoteService()
        // {
        //     _callAnotherRemoteServiceCounter++;
        //     switch (_callAnotherRemoteServiceCounter)
        //     {
        //         case 1:
        //             return Status.ExceptionOccurred;
        //         case 2: 
        //             return Status.Fail;
        //         case 3:
        //             return Status.Unknown;
        //         case 4: 
        //             return Status.Success;
        //     }
        //     return Status.Fail;
        // }

        // public IEnumerable<int> GetListOfNumbers()
        // {
        //     _getListOfNumbersCounter++;
        //     if (_getListOfNumbersCounter == 1)
        //     {
        //         return new int[] { 1, 2, 3, 4, 5 };
        //     }
        //     if (_getListOfNumbersCounter == 2)
        //     {
        //         return new int[] { 1, 2 };
        //     }
        //     if (_getListOfNumbersCounter == 3)
        //     {
        //         return new int[] { 1, 2, 3 };
        //     }
        //     return new int[] { 1, 2, 3, 4, 5, 6 };
        // }

        
        // public void DoSomethingThatMightThrowException()
        // {
        //     _exceptionCounter++;
        //     if (_exceptionCounter <= 3)
        //     {
        //         throw new Exception("Bad things happened");
        //     }
        // }
    }
}
