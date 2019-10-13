# Caching

<!-- ### Advanced circuit breaker -->
The Cache policy lets you store and retrieve a result from an in memory or distributed cache rather than making a request to the underlying service. 

This cache is set to store a response for 1.5 seconds.

``` cs --region caching --source-file .\src\Program.cs --project .\src\PollyDemo.csproj 
```

#### Next: [Bulkhead Isolation &raquo;](../bulkheadIsolation.md) Previous: [Advanced Circuit Breaker  &laquo;](../advancedCircuitBreaker.md)