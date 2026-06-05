namespace Shopping.Web.Models;

public class ResultDto
{
    /// <summary>
    /// Indicates whether the operation was successful.
    /// </summary>
    public bool Succeeded { get; private set; }

    /// <summary>
    /// Provides a message describing the result of the operation.
    /// </summary>
    public string Message { get; private set; }

    public ResultDto() { } // for serialization

    /// <summary>
    /// Private constructor to create a result instance.
    /// </summary>
    /// <param name="succeeded">Indicates whether the operation was successful.</param>
    /// <param name="message">Provides a message describing the result.</param>
    private ResultDto(bool succeeded, string message)
    {
        Succeeded = succeeded;
        Message = message;
    }

    /// <summary>
    /// Creates a successful result with a message.
    /// </summary>
    /// <param name="message">The message describing the successful result.</param>
    /// <returns>A Result instance representing success.</returns>
    public static ResultDto Success(string message) => new ResultDto(true, message);

    /// <summary>
    /// Creates a failed result with a message.
    /// </summary>
    /// <param name="message">The message describing the failure.</param>
    /// <returns>A Result instance representing failure.</returns>
    public static ResultDto Failure(string message) => new ResultDto(false, message);
}