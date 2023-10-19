/// <summary>
/// Represents a log message.
/// </summary>
public class LogMessage
{
    /// <summary>
    /// Gets or sets the text content of the log message.
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Gets or sets the type of the log message (e.g., INFO, WARNING, ERROR).
    /// </summary>
    public string MessageType { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the log message was created.
    /// </summary>
    public DateTime MessageDateTime { get; set; }
}
