using Microsoft.AspNetCore.Mvc;

namespace MechaSoft.Application.Common.Responses;

public sealed class Result<TResult, TOk, TError>
{
    public readonly bool IsSuccess;
    public readonly TOk? Ok;
    public readonly TResult? Value;
    public readonly TError? Error;

    private Result(TResult result)
    {
        IsSuccess = true;
        Value = result;
        Ok = default;
        Error = default;
    }

    private Result(TOk ok)
    {
        IsSuccess = true;
        Value = default;
        Ok = ok;
        Error = default;
    }

    private Result(TError error)
    {
        IsSuccess = false;
        Value = default;
        Ok = default;
        Error = error;
    }

    public T Match<T>(Func<object, OkObjectResult> value1, Func<object, ObjectResult> value2)
    {
        throw new NotImplementedException();
    }

    public static implicit operator Result<TResult, TOk, TError>(TResult value)
    {
        return new Result<TResult, TOk, TError>(value);
    }

    public static implicit operator Result<TResult, TOk, TError>(TOk ok)
    {
        return new Result<TResult, TOk, TError>(ok);
    }

    public static implicit operator Result<TResult, TOk, TError>(TError error)
    {
        return new Result<TResult, TOk, TError>(error);
    }
}
public sealed record Success
{
    public static readonly Success Ok = new();

}