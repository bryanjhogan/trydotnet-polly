# Polly Retries Part 5

### Waiting Before Retrying
There are many scenarios where adding a delay before retrying a requst will be beneficial. The Wait and Retry policy adds a delay before retrying, this is especially useful for transient faults.

``` cs --region waitAndRetry --source-file .\src\Program.cs --project .\src\PollyDemo.csproj 
```

#### Next: [Probably Fallbacks TBD  &raquo;]() Previous: [Combining Result and Exception Based Retries &laquo;](../retryIfIncorrectStatusOrException.md)