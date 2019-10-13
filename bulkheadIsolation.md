# Bulkhead Isolation

The Bulkhead Isolation policy lets you restrict the amount of resources any part of your application can use. Using execution and queue slots you can limit the maximum number of requests being executed in parallel and maximum number of queued requests. It is commonly used to throttle incoming requests in the likes of an API.

This policy is difficult to demonstrate in here, but this will give you idea of how it can be used. Thanks to Dylan Reisenberger for his assistance with this piece of code.

``` cs --region bulkhead --source-file .\src\Program.cs --project .\src\PollyDemo.csproj 
```

#### Next: [Policy Wraps &raquo;](../wrap.md)  Previous: [Caching  &laquo;](../caching.md)