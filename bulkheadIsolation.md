# Bulkhead Isolation

The Bulkhead Isolation policy lets you restrict the amount of resources any part of your application can use. Through the use of execution and queue slots you can limit the maximum number of parallel request and maximum number of queued requests. 

``` cs --region bulkhead --source-file .\src\Program.cs --project .\src\PollyDemo.csproj 
```

####  Previous: [Caching  &laquo;](../caching.md)