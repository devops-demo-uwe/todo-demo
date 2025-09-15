using Spectre.Console;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace TodoApp.Services;

/// <summary>
/// Service for handling menu interactions and user input validation
/// </summary>
public class MenuService
{
    private readonly ILogger<MenuService> _logger;
    private const int MaxDescriptionLength = 200;

    /// <summary>
    /// Initializes a new instance of the MenuService class
    /// </summary>
    public MenuService(ILogger<MenuService> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Gets user input for menu selection
    /// </summary>
    /// <returns>Selected menu option</returns>
    public async Task<int> GetMenuSelectionAsync()
    {
        await Task.CompletedTask;
        
        var selection = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[green]📋 What would you like to do?[/]")
                .PageSize(10)
                .AddChoices(new[] {
                    "1. Add Task",
                    "2. List All Tasks", 
                    "3. List by Status",
                    "4. Mark Task Complete",
                    "5. Delete Task",
                    "6. Search Tasks",
                    "7. Show Summary",
                    "8. Clear Completed Tasks",
                    "9. Help",
                    "0. Exit"
                }));

        // Extract the number from the selection
        var optionNumber = int.Parse(selection.Split('.')[0]);
        _logger.LogDebug("User selected menu option: {Option}", optionNumber);
        return optionNumber;
    }

    /// <summary>
    /// Prompts user for task description input
    /// </summary>
    /// <returns>Task description</returns>
    public async Task<string> GetTaskDescriptionAsync()
    {
        await Task.CompletedTask;

        var description = AnsiConsole.Prompt(
            new TextPrompt<string>("[yellow]📝 Enter task description:[/]")
                .PromptStyle("green")
                .ValidationErrorMessage("[red]Task description is required and cannot be empty![/]")
                .Validate(input =>
                {
                    if (string.IsNullOrWhiteSpace(input))
                        return ValidationResult.Error("[red]Task description is required![/]");
                    
                    if (input.Length > MaxDescriptionLength)
                        return ValidationResult.Error($"[red]Task description cannot exceed {MaxDescriptionLength} characters![/]");
                    
                    return ValidationResult.Success();
                }));

        _logger.LogDebug("User entered task description: {Description}", description);
        return description.Trim();
    }

    /// <summary>
    /// Prompts user for due date input
    /// </summary>
    /// <returns>Due date or null if not provided</returns>
    public async Task<DateTime?> GetDueDateAsync()
    {
        await Task.CompletedTask;

        var wantsDueDate = AnsiConsole.Confirm("[yellow]📅 Would you like to set a due date?[/]");
        
        if (!wantsDueDate)
        {
            _logger.LogDebug("User chose not to set a due date");
            return null;
        }

        var dueDateInput = AnsiConsole.Prompt(
            new TextPrompt<string>("[yellow]📅 Enter due date (MM/dd/yyyy or MM-dd-yyyy):[/]")
                .PromptStyle("green")
                .AllowEmpty()
                .ValidationErrorMessage("[red]Please enter a valid date format (MM/dd/yyyy or MM-dd-yyyy) or press Enter to skip![/]")
                .Validate(input =>
                {
                    if (string.IsNullOrWhiteSpace(input))
                        return ValidationResult.Success();

                    if (TryParseDate(input, out var parsedDate))
                    {
                        if (parsedDate < DateTime.Today)
                            return ValidationResult.Error("[red]Due date cannot be in the past![/]");
                        
                        return ValidationResult.Success();
                    }
                    
                    return ValidationResult.Error("[red]Invalid date format! Use MM/dd/yyyy or MM-dd-yyyy[/]");
                }));

        if (string.IsNullOrWhiteSpace(dueDateInput))
        {
            _logger.LogDebug("User skipped due date entry");
            return null;
        }

        if (TryParseDate(dueDateInput, out var dueDate))
        {
            _logger.LogDebug("User entered due date: {DueDate}", dueDate);
            return dueDate;
        }

        return null;
    }

    /// <summary>
    /// Prompts user for task ID input
    /// </summary>
    /// <returns>Task ID</returns>
    public async Task<int> GetTaskIdAsync()
    {
        await Task.CompletedTask;
        
        var taskId = AnsiConsole.Prompt(
            new TextPrompt<int>("[yellow]🔢 Enter task ID:[/]")
                .PromptStyle("green")
                .ValidationErrorMessage("[red]Please enter a valid task ID (positive number)![/]")
                .Validate(id =>
                {
                    if (id <= 0)
                        return ValidationResult.Error("[red]Task ID must be a positive number![/]");
                    
                    return ValidationResult.Success();
                }));

        _logger.LogDebug("User entered task ID: {TaskId}", taskId);
        return taskId;
    }

    /// <summary>
    /// Shows confirmation prompt for destructive operations
    /// </summary>
    /// <param name="message">Confirmation message</param>
    /// <returns>True if confirmed, false otherwise</returns>
    public async Task<bool> GetConfirmationAsync(string message)
    {
        await Task.CompletedTask;
        
        var confirmed = AnsiConsole.Confirm($"[yellow]{message}[/]");
        _logger.LogDebug("User confirmation result: {Confirmed} for message: {Message}", confirmed, message);
        return confirmed;
    }

    /// <summary>
    /// Prompts user to press any key to continue
    /// </summary>
    public async Task WaitForKeyPressAsync()
    {
        await Task.CompletedTask;
        AnsiConsole.MarkupLine("[dim]Press any key to continue...[/]");
        Console.ReadKey(true);
    }

    /// <summary>
    /// Tries to parse a date string in multiple formats
    /// </summary>
    /// <param name="input">Input date string</param>
    /// <param name="date">Parsed date if successful</param>
    /// <returns>True if parsing was successful</returns>
    private static bool TryParseDate(string input, out DateTime date)
    {
        var formats = new[] { "MM/dd/yyyy", "MM-dd-yyyy", "M/d/yyyy", "M-d-yyyy" };
        
        foreach (var format in formats)
        {
            if (DateTime.TryParseExact(input, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            {
                return true;
            }
        }

        date = default;
        return false;
    }
}