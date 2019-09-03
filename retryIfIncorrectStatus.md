# Polly Retries Part 3

### Retries Based on a Result
Policies can handle exceptions `Handle<Exception>`, as you saw in the previous example. Policies can also check the any result type that a method might return. In this case the policy checks `Status` returned - `HandleResult<Status>`. A policy can handle any type of result, e.g. `HandleResult<string>`, `HandleResult<HttpResponseMessage>`, `HandleResult<int>`, etc. 

In this example the policy retries the request up to three times if an the result is not a success.

``` cs --region retryIfIncorrectStatus --source-file .\src\Program.cs --project .\src\PollyDemo.csproj 
```

#### Next: [Combining Result and Exception Based Retries  &raquo;](./retryIfIncorrectStatusOrException.md) Previous: [Retrying When an Exception Occurs &laquo;](../retryIfException.md)