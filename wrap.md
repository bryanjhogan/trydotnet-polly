# Policy Wraps

Policy wraps let you combine multiple polices together in a nested fashion. This example shows a fallback policy wrapping a retry policy. If after all the retries have been executed and no successful response has been received, the fallback policy kicks to return a `Status.Unknown`. 

``` cs --region wrap --source-file .\src\Program.cs --project .\src\PollyDemo.csproj 
```

#### Previous: [Bulkhead Isolation &laquo;](bulkheadIsolation.md)