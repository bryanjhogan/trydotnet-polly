# Polly Retries Part 3

### Retries Based on a Result
Policies can handle exceptions, as you saw in the previous example. But policies can also check the result that a method  returns. In this case the policy checks the `Status` (this is a custom type) returned - `HandleResult<Status>`. A policy can handle any type of result, e.g. `HandleResult<string>`, `HandleResult<HttpResponseMessage>`, `HandleResult<int>`, etc. 

In this example the policy retries the request up to three times if the result is not a `Status.Success`.

``` cs --region retryIfIncorrectStatus --source-file .\src\Program.cs --project .\src\PollyDemo.csproj 
```

#### Next: [Combining Result and Exception Based Retries  &raquo;](./retryIfIncorrectStatusOrException.md) Previous: [Retrying When an Exception Occurs &laquo;](../retryIfException.md)