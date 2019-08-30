# Polly Retries Part 1

### Retrying When an Exception Occurs
The Polly NuGet package has been added and we are going to use the Retry with the call to the database. 
The policy states that if an exception occurs, it will retry up to three times.

Note how you execute the unrelible code inside the policy. `retryPolicy.Execute(() => errorProneCode.QueryTheDatabase());`


``` cs --region retryIfException --source-file .\src\Program.cs --project .\src\PollyDemo.csproj 
```

#### Next: [Retrying Based on a Result  &raquo;](./retryIfIncorrectStatus.md) Previous: [Home &laquo;](../README.md)