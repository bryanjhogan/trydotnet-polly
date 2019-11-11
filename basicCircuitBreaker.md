# Circuit Breaker 1

### Basic circuit breaker
The basic circuit breaker breaks the circuit if a set number of consecutive of errors occur. When the circuit breaker is open no traffic will cross it, instead a `BrokenCircuitException` will be thrown. 

``` cs --region basicCircuitBreaker --source-file .\src\Program.cs --project .\src\PollyDemo.csproj 
```

#### Next: [Advanced Circuit Breaker &raquo;](advancedCircuitBreaker.md) Previous: [Timeouts &laquo;](timeout.md)