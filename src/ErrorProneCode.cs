using System;
using System.Collections.Generic;

namespace PollyTryDemo
{
    public class ErrorProneCode
    {
        int _queryTheDatabaseCounter = 0;
        int _getListOfNumbersCounter = 0;
        int _getBoolCounter = 0;
        int _getStatusCounter = 0;
        int _exceptionCounter = 0;

        public void DoSomethingThatMightThrowException()
        {
            _exceptionCounter++;
            if (_exceptionCounter <= 3)
            {
                throw new Exception("Bad things happened");
            }
        }
        public int QueryTheDatabase()
        {
            _queryTheDatabaseCounter++;
            if (_queryTheDatabaseCounter == 1)
            {
                throw new NullReferenceException();
            }
            if (_queryTheDatabaseCounter == 2)
            {
                throw new InsufficientMemoryException();
            }
            if (_queryTheDatabaseCounter == 3)
            {
                throw new StackOverflowException();
            }
            if(_queryTheDatabaseCounter == 4)
            {
                return 0;
            }
            return -1;
        }

        public IEnumerable<int> GetListOfNumbers()
        {
            _getListOfNumbersCounter++;
            if (_getListOfNumbersCounter == 1)
            {
                return new int[] { 1, 2, 3, 4, 5 };
            }
            if (_getListOfNumbersCounter == 2)
            {
                return new int[] { 1, 2 };
            }
            if (_getListOfNumbersCounter == 3)
            {
                return new int[] { 1, 2, 3 };
            }
            return new int[] { 1, 2, 3, 4, 5, 6 };
        }

        public bool GetBool()
        {
            _getBoolCounter++;
            if(_getBoolCounter <=3)
            {
                return false;
            }
            return true;
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
                    return Status.Success;
            }
            return Status.Fail;
        }
    }
}
