# Polly Retries Part 3

### Combining Result and Exception Based Retries
The policy retries the request up to three times if an the result is not a success or if an exception occurred.

``` cs --region retryIfIncorrectStatusOrException --source-file .\src\Program.cs --project .\src\PollyDemo.csproj 
```

#### Next: [Waiting Before Retrying  &raquo;](./waitAndRetry.md) Previous: [Retries Based on a Result &laquo;](../retryIfIncorrectStatus.md)