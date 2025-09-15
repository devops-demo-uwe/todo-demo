using TodoApp.Models;
using Microsoft.Extensions.Logging;

namespace TodoApp.Services;

/// <summary>
/// Service for managing todo items business logic
/// </summary>
public class TodoService
{
    private readonly FileService _fileService;
    private readonly ILogger<TodoService> _logger;
    private const int MaxDescriptionLength = 200;

    /// <summary>
    /// Initializes a new instance of the TodoService class
    /// </summary>
    public TodoService(FileService fileService, ILogger<TodoService> logger)
    {
        _fileService = fileService;
        _logger = logger;
    }

    /// <summary>
    /// Gets all todo items
    /// </summary>
    /// <returns>Collection of todo items</returns>
    public async Task<IEnumerable<TodoItem>> GetAllAsync()
    {
        try
        {
            var todos = await _fileService.LoadAsync();
            _logger.LogInformation("Retrieved {Count} todo items", todos.Count());
            return todos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve todo items");
            throw;
        }
    }

    /// <summary>
    /// Adds a new todo item
    /// </summary>
    /// <param name="description">Task description</param>
    /// <param name="dueDate">Optional due date</param>
    /// <returns>The created todo item</returns>
    public async Task<TodoItem> AddAsync(string description, DateTime? dueDate = null)
    {
        try
        {
            // Validate description
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException("Task description is required.", nameof(description));
            }

            if (description.Length > MaxDescriptionLength)
            {
                throw new ArgumentException($"Task description cannot exceed {MaxDescriptionLength} characters.", nameof(description));
            }

            // Validate due date if provided
            if (dueDate.HasValue && dueDate.Value < DateTime.Today)
            {
                throw new ArgumentException("Due date cannot be in the past.", nameof(dueDate));
            }

            // Load existing todos to generate new ID
            var existingTodos = await _fileService.LoadAsync();
            var todoList = existingTodos.ToList();
            
            // Generate new ID
            var maxId = todoList.Any() ? todoList.Max(t => t.Id) : 0;
            var newId = maxId + 1;

            // Create new todo item
            var newTodo = new TodoItem
            {
                Id = newId,
                Description = description.Trim(),
                CreatedDate = DateTime.Now,
                DueDate = dueDate,
                CompletedDate = null
            };

            // Add to collection and save
            todoList.Add(newTodo);
            var saveSuccess = await _fileService.SaveAsync(todoList);

            if (!saveSuccess)
            {
                throw new InvalidOperationException("Failed to save the new todo item.");
            }

            _logger.LogInformation("Successfully created todo item with ID {Id}: {Description}", newId, description);
            return newTodo;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add todo item: {Description}", description);
            throw;
        }
    }

    /// <summary>
    /// Marks a todo item as completed
    /// </summary>
    /// <param name="id">Todo item ID</param>
    /// <returns>True if successful, false otherwise</returns>
    public async Task<bool> CompleteAsync(int id)
    {
        // TODO: Implement method
        await Task.CompletedTask;
        return false;
    }

    /// <summary>
    /// Deletes a todo item
    /// </summary>
    /// <param name="id">Todo item ID</param>
    /// <returns>True if successful, false otherwise</returns>
    public async Task<bool> DeleteAsync(int id)
    {
        // TODO: Implement method
        await Task.CompletedTask;
        return false;
    }
}