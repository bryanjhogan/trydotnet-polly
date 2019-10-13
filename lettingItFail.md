# Polly Retries Part 1

### Letting a request fail - not using Polly yet
Before adding Polly let's see how an application without Polly might behave.
Here we are trying to write to a database, but the request will fail, and an exception will be thrown.  

``` cs --region lettingItFail --source-file .\src\Program.cs --project .\src\PollyDemo.csproj 
```

#### Next: [Retrying When an Exception Occurs  &raquo;](../retryIfException.md) Previous: [Home &laquo;](../README.md)