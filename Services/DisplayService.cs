using TodoApp.Models;

namespace TodoApp.Services;

/// <summary>
/// Service for handling console display formatting and colors
/// </summary>
public class DisplayService
{
    /// <summary>
    /// Initializes a new instance of the DisplayService class
    /// </summary>
    public DisplayService()
    {
        // TODO: Implement constructor
    }

    /// <summary>
    /// Displays the main menu
    /// </summary>
    public void ShowMainMenu()
    {
        // TODO: Implement method
    }

    /// <summary>
    /// Displays a list of todo items with formatting
    /// </summary>
    /// <param name="todos">Todo items to display</param>
    public void ShowTodoList(IEnumerable<TodoItem> todos)
    {
        // TODO: Implement method
    }

    /// <summary>
    /// Displays a summary of todo statistics
    /// </summary>
    /// <param name="todos">Todo items for statistics</param>
    public void ShowSummary(IEnumerable<TodoItem> todos)
    {
        // TODO: Implement method
    }

    /// <summary>
    /// Displays an error message with formatting
    /// </summary>
    /// <param name="message">Error message to display</param>
    public void ShowError(string message)
    {
        // TODO: Implement method
    }

    /// <summary>
    /// Displays a success message with formatting
    /// </summary>
    /// <param name="message">Success message to display</param>
    public void ShowSuccess(string message)
    {
        // TODO: Implement method
    }
}