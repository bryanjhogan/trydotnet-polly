# Polly Retries Part 4

### Waiting Before Retrying
The wait and retry policy adds a delay before retrying, this is especially useful for transient faults.

``` cs --region waitAndRetry --source-file .\src\Program.cs --project .\src\PollyDemo.csproj 
```

#### Next: [Probably Fallbacks TBD  &raquo;]() Previous: [Combining Result and Exception Based Retries &laquo;](../retryIfIncorrectStatusOrException.md)