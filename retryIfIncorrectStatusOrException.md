# Polly Retries Part 4

### Combining Result and Exception Based Retries
You can combine handling of exceptions and results. 

The policy retries the request up to three times if the result is not a `Status.Success` or if an exception occurred.

``` cs --region retryIfIncorrectStatusOrException --source-file .\src\Program.cs --project .\src\PollyDemo.csproj 
```

#### Next: [Waiting Before Retrying  &raquo;](./waitAndRetry.md) Previous: [Retries Based on a Result &laquo;](retryIfIncorrectStatus.md)