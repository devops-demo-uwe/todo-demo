using TodoApp.Models;
using Spectre.Console;
using Microsoft.Extensions.Logging;

namespace TodoApp.Services;

/// <summary>
/// Service for handling console display formatting and colors
/// </summary>
public class DisplayService
{
    private readonly ILogger<DisplayService> _logger;

    /// <summary>
    /// Initializes a new instance of the DisplayService class
    /// </summary>
    public DisplayService(ILogger<DisplayService> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Displays the main menu
    /// </summary>
    public void ShowMainMenu()
    {
        AnsiConsole.Clear();
        
        var rule = new Rule("[bold blue]📋 Todo Manager[/]")
        {
            Style = Style.Parse("blue")
        };
        AnsiConsole.Write(rule);
        AnsiConsole.WriteLine();

        var panel = new Panel(
            new Markup("[yellow]Welcome to your personal task manager![/]\n" +
                      "[dim]Organize your tasks, track progress, and stay productive.[/]"))
        {
            Border = BoxBorder.Rounded,
            BorderStyle = Style.Parse("blue"),
            Padding = new Padding(2, 1, 2, 1)
        };
        
        AnsiConsole.Write(panel);
        AnsiConsole.WriteLine();
    }

    /// <summary>
    /// Displays a list of todo items with formatting
    /// </summary>
    /// <param name="todos">Todo items to display</param>
    public void ShowTodoList(IEnumerable<TodoItem> todos)
    {
        var todoList = todos.ToList();
        
        if (!todoList.Any())
        {
            AnsiConsole.MarkupLine("[yellow]📝 No tasks found. Add your first task to get started![/]");
            return;
        }

        var table = new Table()
            .AddColumn("[bold]ID[/]")
            .AddColumn("[bold]Description[/]")
            .AddColumn("[bold]Status[/]")
            .AddColumn("[bold]Created[/]")
            .AddColumn("[bold]Due Date[/]")
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Blue);

        foreach (var todo in todoList.OrderBy(t => t.Id))
        {
            var statusMarkup = GetStatusMarkup(todo.Status);
            var dueDateDisplay = todo.DueDate?.ToString("MM/dd/yyyy") ?? "[dim]No due date[/]";
            var createdDisplay = todo.CreatedDate.ToString("MM/dd/yyyy");

            table.AddRow(
                todo.Id.ToString(),
                todo.Description,
                statusMarkup,
                createdDisplay,
                dueDateDisplay
            );
        }

        AnsiConsole.Write(table);
        _logger.LogDebug("Displayed {Count} todo items", todoList.Count);
    }

    /// <summary>
    /// Displays a summary of todo statistics
    /// </summary>
    /// <param name="todos">Todo items for statistics</param>
    public void ShowSummary(IEnumerable<TodoItem> todos)
    {
        var todoList = todos.ToList();
        var pendingCount = todoList.Count(t => t.Status == Models.TaskStatus.Pending);
        var completedCount = todoList.Count(t => t.Status == Models.TaskStatus.Completed);
        var overdueCount = todoList.Count(t => t.Status == Models.TaskStatus.Overdue);
        var totalCount = todoList.Count;

        var table = new Table()
            .AddColumn("[bold]Status[/]")
            .AddColumn("[bold]Count[/]")
            .AddColumn("[bold]Percentage[/]")
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Blue)
            .Title("[bold blue]📊 Task Summary[/]");

        if (totalCount > 0)
        {
            table.AddRow("[green]✅ Completed[/]", completedCount.ToString(), $"{(completedCount * 100.0 / totalCount):F1}%");
            table.AddRow("[yellow]⏳ Pending[/]", pendingCount.ToString(), $"{(pendingCount * 100.0 / totalCount):F1}%");
            table.AddRow("[red]⚠️ Overdue[/]", overdueCount.ToString(), $"{(overdueCount * 100.0 / totalCount):F1}%");
            table.AddRow("[bold]📋 Total[/]", totalCount.ToString(), "100.0%");
        }
        else
        {
            table.AddRow("[dim]No tasks[/]", "0", "0%");
        }

        AnsiConsole.Write(table);
        _logger.LogDebug("Displayed summary for {Count} todo items", totalCount);
    }

    /// <summary>
    /// Displays an error message with formatting
    /// </summary>
    /// <param name="message">Error message to display</param>
    public void ShowError(string message)
    {
        var panel = new Panel(new Text(message, new Style(Color.White)))
        {
            Border = BoxBorder.Heavy,
            BorderStyle = Style.Parse("red"),
            Header = new PanelHeader("[red bold]❌ Error[/]"),
            Padding = new Padding(2, 1, 2, 1)
        };
        
        AnsiConsole.Write(panel);
        _logger.LogDebug("Displayed error message: {Message}", message);
    }

    /// <summary>
    /// Displays a success message with formatting
    /// </summary>
    /// <param name="message">Success message to display</param>
    public void ShowSuccess(string message)
    {
        var panel = new Panel(new Text(message, new Style(Color.White)))
        {
            Border = BoxBorder.Heavy,
            BorderStyle = Style.Parse("green"),
            Header = new PanelHeader("[green bold]✅ Success[/]"),
            Padding = new Padding(2, 1, 2, 1)
        };
        
        AnsiConsole.Write(panel);
        _logger.LogDebug("Displayed success message: {Message}", message);
    }

    /// <summary>
    /// Shows a task creation confirmation
    /// </summary>
    /// <param name="todo">The created todo item</param>
    public void ShowTaskCreated(TodoItem todo)
    {
        var content = new Markup($"[bold]Task successfully created![/]\n\n" +
                               $"[yellow]ID:[/] {todo.Id}\n" +
                               $"[yellow]Description:[/] {todo.Description}\n" +
                               $"[yellow]Created:[/] {todo.CreatedDate:MM/dd/yyyy HH:mm}\n" +
                               $"[yellow]Due Date:[/] {(todo.DueDate?.ToString("MM/dd/yyyy") ?? "No due date")}\n" +
                               $"[yellow]Status:[/] {GetStatusMarkup(todo.Status)}");

        var panel = new Panel(content)
        {
            Border = BoxBorder.Heavy,
            BorderStyle = Style.Parse("green"),
            Header = new PanelHeader("[green bold]✅ Task Added[/]"),
            Padding = new Padding(2, 1, 2, 1)
        };
        
        AnsiConsole.Write(panel);
        _logger.LogInformation("Displayed task creation confirmation for task ID {Id}", todo.Id);
    }

    /// <summary>
    /// Shows a loading spinner for async operations
    /// </summary>
    /// <param name="message">Loading message</param>
    /// <param name="operation">Async operation to perform</param>
    public async Task ShowLoadingAsync(string message, Func<Task> operation)
    {
        await AnsiConsole.Status()
            .Start(message, async ctx =>
            {
                ctx.Spinner(Spinner.Known.Star);
                ctx.SpinnerStyle(Style.Parse("green"));
                await operation();
            });
    }

    /// <summary>
    /// Gets the appropriate markup for a task status
    /// </summary>
    /// <param name="status">Task status</param>
    /// <returns>Formatted status markup</returns>
    private static string GetStatusMarkup(Models.TaskStatus status)
    {
        return status switch
        {
            Models.TaskStatus.Completed => "[green]✅ Completed[/]",
            Models.TaskStatus.Overdue => "[red]⚠️ Overdue[/]",
            Models.TaskStatus.Pending => "[yellow]⏳ Pending[/]",
            _ => "[dim]Unknown[/]"
        };
    }
}