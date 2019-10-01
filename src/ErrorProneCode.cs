using System;

namespace PollyTryDemo
{
    public class ErrorProneCode
    {
        int _writeToSomeDbCounter = 0;
        int _queryTheDatabaseCounter = 0;
        int _getStatusCounter = 0;
        int _callRemoteServiceCounter = 0;

        int _circuitBeakerTargetACounter = 0;
        int _circuitBeakerTargetBCounter = 0;

        int _getSomeNumberThatMightBeCacheable = 0;
        
        public Status TargetA()
        {
            _circuitBeakerTargetACounter ++; 
            if (_circuitBeakerTargetACounter >= 2)
            {
                return Status.Fail;
            }
            return Status.Success;
        }

        public Status TargetB()
        {
            _circuitBeakerTargetBCounter ++; 
            if (_circuitBeakerTargetBCounter >= 2)
            {
                return Status.Fail;
            }
            return Status.Success;
        }


        public int WriteToSomeDb()
        {
            _writeToSomeDbCounter++; 
            if (_writeToSomeDbCounter <= 3)
            {
                throw new InsufficientMemoryException("You ran out of memory!");
            }
            return 1;
        }

        public void MakeRequestToADeadService(){
            throw new Exception("An error occurred, I hope you have Polly!");
        }

        
        public int GetQuantityAvailable(){
            throw new Exception("An error occurred, I hope you have Polly!");
        }

        public int GetSomeNumberThatMightBeCacheable()
        {
            _getSomeNumberThatMightBeCacheable++;
            Console.WriteLine("GetSomeNumberThatMightBeCacheable method called, returned value will be cached.");
            return _getSomeNumberThatMightBeCacheable;
        }
        public int QueryTheDatabase()
        {
            _queryTheDatabaseCounter++;
            if (_queryTheDatabaseCounter == 1)
            {
                throw new Exception("An error occurred, I hope you have Polly!");
            }
            if (_queryTheDatabaseCounter == 2)
            {
                throw new InsufficientMemoryException("You ran out of memory!");
            }
            if(_queryTheDatabaseCounter == 3)
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
    }
}
