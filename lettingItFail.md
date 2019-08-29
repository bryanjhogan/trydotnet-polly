# Polly Retries Part 1

### Letting a request fail - not using Polly yet

Here we are trying to write to a database, but the request will fail and an exception thrown.

``` cs --region lettingItFail --source-file .\src\Program.cs --project .\src\PollyDemo.csproj 
```

#### Next: [Retrying When an Exception Occurs  &raquo;](./retryIfException.md) Previous: [Home &laquo;](../README.md)