using TodoApp.Services;
using Microsoft.Extensions.Logging;

namespace TodoApp.Handlers;

/// <summary>
/// Handler for adding new todo items
/// </summary>
public class AddTaskHandler
{
    private readonly TodoService _todoService;
    private readonly MenuService _menuService;
    private readonly DisplayService _displayService;
    private readonly ILogger<AddTaskHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the AddTaskHandler class
    /// </summary>
    public AddTaskHandler(
        TodoService todoService,
        MenuService menuService,
        DisplayService displayService,
        ILogger<AddTaskHandler> logger)
    {
        _todoService = todoService;
        _menuService = menuService;
        _displayService = displayService;
        _logger = logger;
    }

    /// <summary>
    /// Handles the add task workflow
    /// </summary>
    /// <returns>Task representing the async operation</returns>
    public async Task HandleAsync()
    {
        try
        {
            _logger.LogInformation("Starting add task workflow");

            // Step 1: Get task description from user
            string description;
            try
            {
                description = await _menuService.GetTaskDescriptionAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get task description from user");
                _displayService.ShowError("Failed to get task description. Please try again.");
                return;
            }

            // Step 2: Get optional due date from user
            DateTime? dueDate;
            try
            {
                dueDate = await _menuService.GetDueDateAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get due date from user");
                _displayService.ShowError("Failed to get due date. Please try again.");
                return;
            }

            // Step 3: Create the task with loading indicator
            try
            {
                await _displayService.ShowLoadingAsync("Creating task...", async () =>
                {
                    var newTodo = await _todoService.AddAsync(description, dueDate);
                    _displayService.ShowTaskCreated(newTodo);
                });

                _logger.LogInformation("Successfully completed add task workflow for description: {Description}", description);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validation error while creating task");
                _displayService.ShowError($"Validation Error: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Failed to save task");
                _displayService.ShowError($"Failed to save task: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while creating task");
                _displayService.ShowError("An unexpected error occurred while creating the task. Please try again.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in add task handler");
            _displayService.ShowError("An unexpected error occurred. Please try again.");
        }
        finally
        {
            // Always wait for user acknowledgment before returning to menu
            await _menuService.WaitForKeyPressAsync();
        }
    }
}