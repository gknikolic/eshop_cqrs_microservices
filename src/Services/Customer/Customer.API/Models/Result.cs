namespace Customer.API.Models;

public class Result
{
    /// <summary>
    /// Indicates whether the operation was successful.
    /// </summary>
    public bool Succeeded { get; private set; }

    /// <summary>
    /// Provides a message describing the result of the operation.
    /// </summary>
    public string Message { get; private set; }

    /// <summary>
    /// Private constructor to create a result instance.
    /// </summary>
    /// <param name="succeeded">Indicates whether the operation was successful.</param>
    /// <param name="message">Provides a message describing the result.</param>
    private Result(bool succeeded, string message)
    {
        Succeeded = succeeded;
        Message = message;
    }

    /// <summary>
    /// Creates a successful result with a message.
    /// </summary>
    /// <param name="message">The message describing the successful result.</param>
    /// <returns>A Result instance representing success.</returns>
    public static Result Success(string message) => new Result(true, message);

    /// <summary>
    /// Creates a failed result with a message.
    /// </summary>
    /// <param name="message">The message describing the failure.</param>
    /// <returns>A Result instance representing failure.</returns>
    public static Result Failure(string message) => new Result(false, message);
}