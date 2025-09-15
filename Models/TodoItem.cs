using System.Text.Json.Serialization;

namespace TodoApp.Models;

/// <summary>
/// Represents a todo item with all necessary properties
/// </summary>
public class TodoItem
{
    /// <summary>
    /// Unique identifier for the todo item
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// Description of the task (required, max 200 characters)
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Date and time when the task was created
    /// </summary>
    [JsonPropertyName("createdDate")]
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// Optional due date for the task
    /// </summary>
    [JsonPropertyName("dueDate")]
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// Date and time when the task was completed (null if not completed)
    /// </summary>
    [JsonPropertyName("completedDate")]
    public DateTime? CompletedDate { get; set; }

    /// <summary>
    /// Calculated property representing the current status of the task
    /// </summary>
    [JsonIgnore]
    public TaskStatus Status
    {
        get
        {
            if (CompletedDate.HasValue)
                return TaskStatus.Completed;

            if (DueDate.HasValue && DueDate.Value < DateTime.Now)
                return TaskStatus.Overdue;

            return TaskStatus.Pending;
        }
    }
}