using System;
using System.Collections.Generic;
using System.Text;

namespace PricingPlatform.SharedKernel.Result
{
    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFallback { get; }
        public string? Error { get; }

        protected Result(bool success, string? error, bool isFallback = false)
        {
            IsSuccess = success;
            Error = error;
            IsFallback = isFallback;
        }

        public static Result Success() => new(true, null);
        public static Result Fallback(string reason) => new(true, reason, isFallback: true);
        public static Result Failure(string error) => new(false, error);
    }

    public class Result<T> : Result
    {
        public T? Value { get; }

        private Result(bool success, T? value, string? error, bool isFallback = false)
            : base(success, error, isFallback)
        {
            Value = value;
        }

        public static Result<T> Success(T value) => new(true, value, null);
        public static Result<T> Fallback(T value, string reason) => new(true, value, reason, isFallback: true);
        public static Result<T> Failure(string error) => new(false, default, error);
    }
}
