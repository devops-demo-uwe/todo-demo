namespace TodoApp.Models;

/// <summary>
/// Represents the status of a todo item
/// </summary>
public enum TaskStatus
{
    /// <summary>
    /// Task is pending completion
    /// </summary>
    Pending,

    /// <summary>
    /// Task has been completed
    /// </summary>
    Completed,

    /// <summary>
    /// Task is overdue (past due date and not completed)
    /// </summary>
    Overdue
}