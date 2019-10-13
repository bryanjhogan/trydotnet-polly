# Circuit Breaker 2

### Advanced circuit breaker
The advanced circuit breaker breaks the circuit if a threshold of failures is reached within a specified time period and with a minimum number of requests reached in that time period. 

In this example the circuit breaks when there are 50% failures, in a 3 second sampling window, with a minimum throughput of 6 request in the 3 second window; if the circuit breaks it will be open for 5 seconds. When the circuit breaker is open no traffic will cross it, instead a `BrokenCircuitException` will be thrown. 

``` cs --region advancedCircuitBreaker --source-file .\src\Program.cs --project .\src\PollyDemo.csproj 
```

#### Next: [Caching &raquo;](../caching.md) Previous: [Basic Circuit Breaker  &laquo;](../basicCircuitBreaker.md)