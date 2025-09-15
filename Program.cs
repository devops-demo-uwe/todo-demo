using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TodoApp.Services;
using TodoApp.Handlers;

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

            // Get the main application service (TODO: implement)
            // var app = serviceProvider.GetRequiredService<IApplicationService>();

            // Run the application (TODO: implement main loop)
            // await app.RunAsync();

            // TODO: Implement main menu loop
            Console.WriteLine("Hello from Todo Manager! 👋");
            Console.WriteLine("=== Todo Manager ===");
            Console.WriteLine("Application scaffolding completed successfully!");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();

            return 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
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
}