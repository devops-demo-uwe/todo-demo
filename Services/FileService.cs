using System.Text.Json;
using TodoApp.Models;
using Microsoft.Extensions.Logging;

namespace TodoApp.Services;

/// <summary>
/// Service for handling file operations and JSON persistence
/// </summary>
public class FileService
{
    private readonly ILogger<FileService> _logger;
    private readonly string _filePath;
    private readonly JsonSerializerOptions _jsonOptions;

    /// <summary>
    /// Initializes a new instance of the FileService class
    /// </summary>
    public FileService(ILogger<FileService> logger)
    {
        _logger = logger;
        _filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "tasks.json");
        _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        
        // Ensure the directory exists
        var directory = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }

    /// <summary>
    /// Loads todo items from JSON file
    /// </summary>
    /// <returns>Collection of todo items</returns>
    public async Task<IEnumerable<TodoItem>> LoadAsync()
    {
        try
        {
            if (!File.Exists(_filePath))
            {
                _logger.LogInformation("Tasks file does not exist, returning empty collection");
                return new List<TodoItem>();
            }

            var json = await File.ReadAllTextAsync(_filePath);
            if (string.IsNullOrWhiteSpace(json))
            {
                _logger.LogInformation("Tasks file is empty, returning empty collection");
                return new List<TodoItem>();
            }

            var todos = JsonSerializer.Deserialize<List<TodoItem>>(json, _jsonOptions);
            _logger.LogInformation("Successfully loaded {Count} todo items", todos?.Count ?? 0);
            return todos ?? new List<TodoItem>();
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to deserialize tasks from JSON file");
            throw new InvalidOperationException("The tasks file is corrupted. Please check the file format.", ex);
        }
        catch (IOException ex)
        {
            _logger.LogError(ex, "IO error while loading tasks from file");
            throw new InvalidOperationException("Unable to read the tasks file. Please check file permissions.", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while loading tasks");
            throw;
        }
    }

    /// <summary>
    /// Saves todo items to JSON file
    /// </summary>
    /// <param name="todos">Collection of todo items to save</param>
    /// <returns>True if successful, false otherwise</returns>
    public async Task<bool> SaveAsync(IEnumerable<TodoItem> todos)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(todos);

            // Create backup before saving
            await CreateBackupAsync();

            var todoList = todos.ToList();
            var json = JsonSerializer.Serialize(todoList, _jsonOptions);

            // Use atomic write operation
            var tempPath = _filePath + ".tmp";
            await File.WriteAllTextAsync(tempPath, json);
            
            // Atomic move operation
            if (File.Exists(_filePath))
            {
                File.Delete(_filePath);
            }
            File.Move(tempPath, _filePath);

            _logger.LogInformation("Successfully saved {Count} todo items", todoList.Count);
            return true;
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to serialize tasks to JSON");
            return false;
        }
        catch (IOException ex)
        {
            _logger.LogError(ex, "IO error while saving tasks to file");
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while saving tasks");
            return false;
        }
    }

    /// <summary>
    /// Creates a backup of the current data file
    /// </summary>
    /// <returns>True if successful, false otherwise</returns>
    public async Task<bool> CreateBackupAsync()
    {
        try
        {
            if (!File.Exists(_filePath))
            {
                _logger.LogDebug("No tasks file exists to backup");
                return true;
            }

            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var backupPath = Path.ChangeExtension(_filePath, $".backup_{timestamp}.json");
            
            // Copy file asynchronously
            using var sourceStream = new FileStream(_filePath, FileMode.Open, FileAccess.Read);
            using var destinationStream = new FileStream(backupPath, FileMode.Create, FileAccess.Write);
            await sourceStream.CopyToAsync(destinationStream);
            
            _logger.LogInformation("Created backup at {BackupPath}", backupPath);
            return true;
        }
        catch (IOException ex)
        {
            _logger.LogWarning(ex, "Failed to create backup, proceeding anyway");
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Unexpected error while creating backup");
            return false;
        }
    }
}