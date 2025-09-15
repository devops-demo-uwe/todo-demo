# Copilot Instructions: Todo Terminal Application

## Project Overview
A simple, efficient command-line todo application built with .NET Core. The application provides users with an interactive menu-driven terminal interface for managing their daily tasks, offering essential task management functionality without the complexity of a GUI application or command-line arguments.

## Core Features
- **Add Tasks:** Create new todo items with descriptions and optional due dates
- **List Tasks:** Display all tasks with status indicators (pending, completed, overdue)
- **Mark Complete:** Mark tasks as completed with timestamps
- **Delete Tasks:** Remove tasks from the list permanently
- **Search Tasks:** Find tasks by keywords in descriptions
- **Task Persistence:** Save and load tasks from a JSON file
- **Status Summary:** Show counts of pending, completed, and overdue tasks
- **Clear Completed:** Bulk remove all completed tasks
- **Interactive Menu:** Fully menu-driven interface with numbered options and user prompts

## Functional Specifications

### Task Properties
- **ID:** Unique identifier (auto-generated)
- **Description:** Task description (required, max 200 characters)
- **Created Date:** Automatically set when task is created
- **Due Date:** Optional date when task should be completed
- **Completed Date:** Set when task is marked as complete
- **Status:** Calculated property (Pending, Completed, Overdue)

### Interactive Menu Interface
The application starts with a main menu offering numbered options:
```
=== Todo Manager ===
1. Add Task
2. List All Tasks
3. List by Status
4. Mark Task Complete
5. Delete Task
6. Search Tasks
7. Show Summary
8. Clear Completed Tasks
9. Help
0. Exit
```

### User Interaction Flow
- **Add Task:** Prompt for description, then optionally for due date
- **List Tasks:** Display all tasks with option to filter by status
- **Complete Task:** Show task list with IDs, prompt for task ID to complete
- **Delete Task:** Show task list with IDs, prompt for task ID to delete with confirmation
- **Search:** Prompt for search keyword, display matching results
- **Navigation:** Use numbered menu options and clear prompts for all user input

### Data Storage
- Store tasks in `tasks.json` file in user's home directory
- Automatic backup creation before modifications
- Graceful handling of corrupted or missing data files

### User Experience
- Color-coded output (green for completed, red for overdue, yellow for due soon)
- Clear error messages with helpful suggestions
- Confirmation prompts for destructive operations
- Progress indicators for bulk operations
- Intuitive menu navigation with input validation
- Clear screen and formatted displays for better readability

## Technical Requirements
- **.NET 9** explicit Main method (no top-level statements)
- **Console application** with modern terminal features
- **System.Text.Json** for data serialization
- **Spectre.Console** for enhanced terminal UI, colors, and interactive prompts
- **No external databases** - file-based storage only
- **No command-line argument parsing libraries needed** - purely interactive

## Technical Architecture

### Project Structure
```
TodoApp/
├── Program.cs (application entry point with main menu loop)
├── Models/
│   ├── TodoItem.cs
│   └── TaskStatus.cs
├── Services/
│   ├── TodoService.cs
│   ├── FileService.cs
│   ├── DisplayService.cs
│   └── MenuService.cs
├── Handlers/
│   ├── AddTaskHandler.cs
│   ├── ListTasksHandler.cs
│   ├── CompleteTaskHandler.cs
│   └── [other interactive handlers]
└── TodoApp.csproj
```

### Key Components
- **TodoItem Model:** Core data structure for tasks
- **TodoService:** Business logic for task operations
- **FileService:** Handle JSON persistence and file operations
- **DisplayService:** Format and colorize console output
- **MenuService:** Handle interactive menu display and user input validation
- **Interactive Handlers:** Handle each menu option with user prompts and input validation

## Coding Standards
- Use **top-level programs** (no explicit Main method)
- Enable **nullable reference types** and handle nullability properly
- Use **async/await** for all I/O operations
- Implement **proper error handling** with try-catch blocks
- Follow **C# naming conventions** (PascalCase for public members)
- Use **dependency injection** with Microsoft.Extensions.DependencyInjection
- Add **XML documentation** for public APIs
- Prefer **record types** for immutable data structures

## Security Guidelines
- **Input validation** for all user inputs (task descriptions, dates, IDs, menu selections)
- **Path traversal protection** when handling file operations
- **Safe JSON deserialization** with proper exception handling
- **File permission validation** before read/write operations
- **Data sanitization** for display output
- **Menu input bounds checking** to prevent invalid selections

## Development Guidelines

### User Interaction Rules
- **Never close a GitHub issue without checking with the user first** - Always ask for confirmation before closing any issue, even if implementation appears complete
- **Do not create more code than requested** - Before adding additional functionality not immediately covered by the specification, ask the user for permission
- **Stick to the scope** - Focus on the specific requirements and avoid feature creep without explicit user approval
- **Confirm before major changes** - Always verify with the user before making significant architectural or design decisions

### Code Quality Standards
- **Write clean, readable code** - Use meaningful variable names, clear method signatures, and logical organization
- **Follow DRY principles** - Avoid code duplication by creating reusable services and utilities
- **Implement proper error handling** - Use appropriate exception handling and provide meaningful error messages to users
- **Add comprehensive comments** - Document complex logic, business rules, and API integrations
- **Create testable code** - Write code that can be easily unit tested and mocked

### Performance Guidelines
- **Optimize file I/O operations** - Use async methods and proper disposal patterns
- **Efficient data structures** - Use appropriate collections for task storage and retrieval
- **Memory management** - Dispose of resources properly and avoid memory leaks
- **Startup performance** - Keep application startup time under 500ms
- **Responsive menu navigation** - All menu operations should complete within 2 seconds

### Documentation Requirements
- **Update README files** - Include installation, usage instructions, and menu navigation examples
- **Document menu structure** - Provide clear descriptions of all interactive options
- **API documentation** - Document public methods and their parameters
- **Error code documentation** - Document all possible error scenarios
- **Performance characteristics** - Document expected performance for large task lists

### Testing Standards
- **Write unit tests** - Cover core business logic and edge cases
- **Test file operations** - Mock file system operations for reliable testing
- **Test menu interactions** - Verify all menu options and input validation work correctly
- **Test error scenarios** - Validate proper error handling and user feedback
- **Integration tests** - Test complete workflows from menu selection to file output

### Collaboration Guidelines
- **Communicate changes clearly** - Explain the reasoning behind significant modifications
- **Ask for clarification** - Request additional details when requirements are unclear
- **Provide implementation options** - Offer multiple approaches when appropriate
- **Consider maintainability** - Write code that future developers can easily understand and modify
- **Follow established patterns** - Use consistent architectural patterns throughout the project

## Custom Instructions
- **Error messages should be helpful** - Always suggest next steps or corrections
- **Menu options should feel intuitive** - Follow common interactive application conventions
- **Output should be scannable** - Use colors, spacing, and formatting effectively
- **Handle edge cases gracefully** - Empty lists, corrupted files, invalid dates, invalid menu selections
- **Maintain backwards compatibility** - Don't break existing task files with updates
- **Provide clear navigation** - Always show current context and available options

## External Dependencies
- **Spectre.Console:** Rich terminal UI with colors, tables, progress bars, and interactive prompts
- **System.Text.Json:** High-performance JSON serialization
- **Microsoft.Extensions.DependencyInjection:** Service container for dependency injection
- **Microsoft.Extensions.Logging:** Structured logging for debugging

## Environment Configuration
- **Development:** Use debug logging and detailed error messages
- **Release:** Optimize for performance and user-friendly error messages
- **Testing:** Mock file system and use in-memory storage for tests

### Source Control Management (SCM) Guidelines

#### Git Workflow Standards
- **Always use feature branches** - Never commit directly to main branch
- **Follow branch naming conventions**: 
  - Features: `feature/issue-{#}-{brief-description}`
  - Bug fixes: `fix/issue-{#}-{brief-description}`
  - Documentation: `docs/{brief-description}`
- **Use conventional commit messages**:
  ```
  {type}: {description}
  
  - {detailed-change-1}
  - {detailed-change-2}
  
  Addresses issue #{number}: {issue-title}
  ```

#### GitHub Issue Management
- **Link all commits to GitHub issues** using issue numbers in commit messages
- **Update issue progress** with concise, actionable comments
- **Use only approved labels** from the standard label set:
  - **Type**: `bug`, `enhancement`, `documentation`, `question`
  - **Priority**: `low-priority`, `medium-priority`, `high-priority`, `critical`  
  - **Status**: `in-progress`, `ready-for-review`, `blocked`, `needs-testing`
  - **Component**: `frontend`, `backend`, `database`, `api`, `security`
- **Never create custom labels** without explicit user approval
- **Maximum 3-4 labels per issue** for clarity

#### Pull Request Workflow
- **Always ask user permission before creating pull requests**
- **Use concise PR descriptions** with essential information only:
  ```markdown
  ## {Feature Name}
  
  **Summary:** {Brief description}
  
  **Changes:**
  - {Change 1}
  - {Change 2}
  
  **Testing:** {Brief testing status}
  
  **Closes:** #{issue-number}
  ```

#### Terminal Command Execution

When working with Git/GitHub CLI:
- **Use temporary files for descriptions**: When creating issues or pull requests with the `gh` CLI, always write the description to a temporary file and use the `--body-file <filename>` option. Do not pass the description as a string directly, as this can fail with long or complex content.
- **Manage temporary files properly**: 
  - Create unique temporary file names for each issue (e.g., `issue_desc_$(date +%s).md`)
  - Always clean up temporary files after successful issue/PR creation
  - Use error handling to ensure cleanup even if commands fail
- **Pre-validate before creation**:
  - **Check GitHub CLI authentication**: `gh auth status`
  - **Verify label existence**: `gh label list` before creating issues to avoid retry loops
  - **Validate repository state** before branch operations
- **Execute commands step-by-step** for better error handling
- **Always confirm destructive operations** (reset, force push, etc.)

#### Standard SCM Workflow
```bash
# 1. Prepare workspace
git checkout main
git pull origin main
git status  # Verify clean state

# 2. Pre-validate GitHub environment
gh auth status
gh label list  # Verify available labels before issue creation

# 3. Create feature branch
git checkout -b feature/issue-{#}-{description}

# 4. Make changes, then commit
git add .
git commit -m "feat: {description}

- {detail 1}
- {detail 2}

Addresses issue #{number}: {issue title}"

# 5. Push and track branch
git push -u origin feature/issue-{#}-{description}

# 6. Update issue status (using validated labels)
gh issue edit {number} --add-label "in-progress"

# 7. Create PR with temporary file (with user permission)
temp_file="pr_desc_$(date +%s).md"
echo "{concise description}" > "$temp_file"
gh pr create --title "feat: {Title}" --body-file "$temp_file"
rm "$temp_file"  # Clean up temporary file
```

#### Issue Creation Workflow
```bash
# 1. Pre-validate environment
gh auth status
available_labels=$(gh label list --json name --jq '.[].name')

# 2. Create temporary description file
temp_file="issue_desc_$(date +%s).md"
cat > "$temp_file" << 'EOF'
## Description
{Issue description here}

## Acceptance Criteria
- {Criterion 1}
- {Criterion 2}
EOF

# 3. Create issue with validated labels only
gh issue create --title "{Issue Title}" --body-file "$temp_file" --label "{validated-label1,validated-label2}"

# 4. Clean up temporary file
rm "$temp_file"
```

### Code Quality Standards
- **Write clean, readable code** - Use meaningful variable names, clear function signatures, and logical organization
- **Follow DRY principles** - Avoid code duplication by creating reusable components and utilities
- **Implement proper error handling** - Use appropriate exception handling and provide meaningful error messages
- **Add comprehensive comments** - Document complex logic, business rules, and API integrations
- **Create testable code** - Write code that can be easily unit tested and mocked

### Performance Guidelines
- **Optimize for scalability** - Consider performance implications of code decisions
- **Use efficient data structures** - Choose appropriate collections and algorithms
- **Implement caching strategies** - Cache expensive operations and frequently accessed data
- **Minimize external API calls** - Batch requests and implement retry policies
- **Follow responsive design principles** - Ensure good performance across different devices

### Documentation Requirements
- **Update README files** - Keep project documentation current and comprehensive
- **Document API endpoints** - Provide clear API documentation with examples
- **Maintain technical specifications** - Keep architecture and design documents updated
- **Create user guides** - Document user-facing features and workflows
- **Record configuration steps** - Document setup and deployment procedures

### Testing Standards
- **Write unit tests** - Cover critical business logic and edge cases
- **Implement integration tests** - Test API endpoints and database interactions
- **Create end-to-end tests** - Validate complete user workflows
- **Test error scenarios** - Verify proper handling of failure cases
- **Maintain test coverage** - Aim for [specific coverage percentage] test coverage

### Collaboration Guidelines
- **Communicate changes clearly** - Explain the reasoning behind significant modifications
- **Ask for clarification** - Request additional details when requirements are unclear
- **Provide implementation options** - Offer multiple approaches when appropriate
- **Consider maintainability** - Write code that future developers can easily understand and modify
- **Follow established patterns** - Use consistent architectural patterns throughout the project
