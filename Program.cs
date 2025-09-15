using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TodoApp.Services;
using TodoApp.Handlers;
using Spectre.Console;

namespace TodoApp;

/// <summary>
/// Main program class for the Todo Terminal Application
/// </summary>
public class Program
{
    /// <summary>
    /// Entry point of the application
    /// </summary>
    /// <param name="args">Command line arguments</param>
    /// <returns>Exit code</returns>
    public static async Task<int> Main(string[] args)
    {
        try
        {
            // Configure services
            var serviceProvider = ConfigureServices();
            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
            
            logger.LogInformation("Todo Manager application starting...");

            // Get required services
            var displayService = serviceProvider.GetRequiredService<DisplayService>();
            var menuService = serviceProvider.GetRequiredService<MenuService>();
            var addTaskHandler = serviceProvider.GetRequiredService<AddTaskHandler>();

            // Main application loop
            while (true)
            {
                try
                {
                    // Show main menu
                    displayService.ShowMainMenu();
                    
                    // Get user selection
                    var selection = await menuService.GetMenuSelectionAsync();
                    
                    // Handle menu selection
                    switch (selection)
                    {
                        case 1: // Add Task
                            logger.LogDebug("User selected: Add Task");
                            await addTaskHandler.HandleAsync();
                            break;
                            
                        case 2: // List All Tasks
                            logger.LogDebug("User selected: List All Tasks");
                            displayService.ShowError("This feature is not yet implemented.");
                            await menuService.WaitForKeyPressAsync();
                            break;
                            
                        case 3: // List by Status
                            logger.LogDebug("User selected: List by Status");
                            displayService.ShowError("This feature is not yet implemented.");
                            await menuService.WaitForKeyPressAsync();
                            break;
                            
                        case 4: // Mark Task Complete
                            logger.LogDebug("User selected: Mark Task Complete");
                            displayService.ShowError("This feature is not yet implemented.");
                            await menuService.WaitForKeyPressAsync();
                            break;
                            
                        case 5: // Delete Task
                            logger.LogDebug("User selected: Delete Task");
                            displayService.ShowError("This feature is not yet implemented.");
                            await menuService.WaitForKeyPressAsync();
                            break;
                            
                        case 6: // Search Tasks
                            logger.LogDebug("User selected: Search Tasks");
                            displayService.ShowError("This feature is not yet implemented.");
                            await menuService.WaitForKeyPressAsync();
                            break;
                            
                        case 7: // Show Summary
                            logger.LogDebug("User selected: Show Summary");
                            displayService.ShowError("This feature is not yet implemented.");
                            await menuService.WaitForKeyPressAsync();
                            break;
                            
                        case 8: // Clear Completed Tasks
                            logger.LogDebug("User selected: Clear Completed Tasks");
                            displayService.ShowError("This feature is not yet implemented.");
                            await menuService.WaitForKeyPressAsync();
                            break;
                            
                        case 9: // Help
                            logger.LogDebug("User selected: Help");
                            ShowHelp(displayService);
                            await menuService.WaitForKeyPressAsync();
                            break;
                            
                        case 0: // Exit
                            logger.LogDebug("User selected: Exit");
                            displayService.ShowSuccess("Thank you for using Todo Manager! Goodbye! 👋");
                            logger.LogInformation("Todo Manager application exiting normally");
                            return 0;
                            
                        default:
                            logger.LogWarning("Invalid menu selection: {Selection}", selection);
                            displayService.ShowError("Invalid selection. Please choose a valid option.");
                            await menuService.WaitForKeyPressAsync();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error in main application loop");
                    displayService.ShowError("An unexpected error occurred. Please try again.");
                    await menuService.WaitForKeyPressAsync();
                }
            }
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteException(ex);
            Console.WriteLine("A critical error occurred. Press any key to exit...");
            Console.ReadKey();
            return 1;
        }
    }

    /// <summary>
    /// Configures the dependency injection container
    /// </summary>
    /// <returns>Configured service provider</returns>
    private static ServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        // Add logging
        services.AddLogging(builder =>
        {
            builder.AddConsole();
            builder.SetMinimumLevel(LogLevel.Information);
        });

        // Register services
        services.AddSingleton<TodoService>();
        services.AddSingleton<FileService>();
        services.AddSingleton<DisplayService>();
        services.AddSingleton<MenuService>();

        // Register handlers
        services.AddTransient<AddTaskHandler>();
        services.AddTransient<ListTasksHandler>();
        services.AddTransient<CompleteTaskHandler>();
        services.AddTransient<DeleteTaskHandler>();

        return services.BuildServiceProvider();
    }

    /// <summary>
    /// Shows help information to the user
    /// </summary>
    /// <param name="displayService">Display service for formatted output</param>
    private static void ShowHelp(DisplayService displayService)
    {
        var helpText = """
        [bold blue]📋 Todo Manager Help[/]

        [yellow]Available Commands:[/]
        [green]1. Add Task[/]         - Create a new todo item with description and optional due date
        [green]2. List All Tasks[/]   - Show all tasks in a formatted table
        [green]3. List by Status[/]   - Filter tasks by status (Pending, Completed, Overdue)
        [green]4. Mark Complete[/]    - Mark a specific task as completed
        [green]5. Delete Task[/]      - Remove a task permanently
        [green]6. Search Tasks[/]     - Find tasks by keyword in description
        [green]7. Show Summary[/]     - Display task statistics and counts
        [green]8. Clear Completed[/]  - Remove all completed tasks
        [green]9. Help[/]             - Show this help information
        [green]0. Exit[/]             - Close the application

        [yellow]Task Status Colors:[/]
        [green]✅ Completed[/] - Task has been finished
        [yellow]⏳ Pending[/]   - Task is waiting to be done
        [red]⚠️ Overdue[/]    - Task is past its due date

        [yellow]Tips:[/]
        • Task descriptions can be up to 200 characters
        • Due dates are optional but help with organization
        • Completed tasks are automatically timestamped
        • All data is saved to a JSON file in your home directory
        """;

        var panel = new Panel(helpText)
        {
            Border = BoxBorder.Rounded,
            BorderStyle = Style.Parse("blue"),
            Padding = new Padding(2, 1, 2, 1)
        };

        AnsiConsole.Write(panel);
    }
}