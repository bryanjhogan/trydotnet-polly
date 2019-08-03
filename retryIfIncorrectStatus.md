# Polly Retries Part 2

### Retries Based on a Result
The policy retries the request up to three times if an the result is not a success.

``` cs --region retryIfIncorrectStatus --source-file .\src\Program.cs --project .\src\PollyDemo.csproj 
```

#### Next: [Combining Result and Exception Based Retries  &raquo;](./retryIfIncorrectStatusOrException.md) Previous: [Retrying When an Exception Occurs &laquo;](../retryIfException.md)