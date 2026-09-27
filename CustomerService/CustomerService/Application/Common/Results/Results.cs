namespace CustomerService.Application.Common.Results;

public class Results<T> : Result
{
    private readonly T? _value;

    private Results(T? value, bool isSuccess, Error error)
        : base(isSuccess, error)
    {
        _value = value;
    }

    public T Value
    {
        get
        {
            if (IsFailure) {
                throw new InvalidOperationException("Cannot access the value of a failed result.");
            }

            return _value!;
        }
    }

    public static Results<T> Success(T value)
    {
        return new Results<T>(
            value,
            true,
            Error.None);
    }

    public static new Results<T> Failure(Error error)
    {
        return new Results<T>(
            default,
            false,
            error);
    }
}