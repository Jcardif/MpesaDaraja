# Error Handling

## DarajaResult\<T>

All API methods return `DarajaResult<T>` — check `IsSuccess` before accessing the response.

```csharp
var result = await mpesaExpress.InitiateStkPush(payload);

if (result.IsSuccess)
{
    var response = result.Value!;
}
else
{
    Console.WriteLine($"Code: {result.Error!.ErrorCode}");
    Console.WriteLine($"Message: {result.Error.ErrorMessage}");
}
```

| Property | Type | Description |
|---|---|---|
| `IsSuccess` | `bool` | `true` if the API call succeeded |
| `Value` | `T?` | Response object (null on failure) |
| `Error` | `DarajaError?` | Error details (null on success) |

## DarajaException

Thrown only during authentication (`InitializeDarajaAsync`). All other API calls use `DarajaResult<T>`.

```csharp
try
{
    await gateway.InitializeDarajaAsync();
}
catch (DarajaException ex)
{
    Console.WriteLine(ex.Message);

    if (ex.Error is not null)
        Console.WriteLine($"Code: {ex.Error.ErrorCode}");
}
```

## Error Scenarios

| Scenario | Behavior |
|---|---|
| Invalid credentials | `DarajaException` thrown during authentication |
| API returns error JSON | `DarajaResult` with parsed `DarajaError` |
| API returns empty response | `DarajaResult` with generic error message |
| API returns non-JSON error | `DarajaResult` with raw content as error message |
