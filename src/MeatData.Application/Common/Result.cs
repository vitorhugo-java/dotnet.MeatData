using System;
using System.Collections.Generic;
using System.Text;

namespace MeatData.Application.Common;

public class Result<T>
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public T? Value { get; }
    public string? ErrorMessage { get; }
    public string? ErrorCode { get; }

    private Result(T value)
    {
        IsSuccess = true;
        Value = value;
    }

    private Result(string errorMessage, string errorCode)
    {
        IsSuccess = false;
        ErrorMessage = errorMessage;
        ErrorCode = errorCode;
    }

    public static Result<T> Success(T value) => new(value);

    public static Result<T> Failure(string message, string code = "GENERIC_ERROR")
        => new(message, code);
}