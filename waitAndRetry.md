# Polly Retries Part 4

### Waiting Before Retrying
The policy retries the request up to three times if an the result is not a success or if an exception occurred.

``` cs --region waitAndRetry --source-file .\src\Program.cs --project .\src\PollyDemo.csproj 
```

#### Next: [xxxx  &raquo;](./Strings.md) Previous: [Combining Result and Exception Based Retries &laquo;](../retryIfIncorrectStatusOrException.md)