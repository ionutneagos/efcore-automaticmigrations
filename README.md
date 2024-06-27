<!-- GETTING STARTED -->
# About

Contains instructions to use [EFCore.AutomaticMigrations](https://www.nuget.org/packages/EFCore.AutomaticMigrations/) nuget package for Automatic Migrations with Entity Framework Core SQL Databases.

[EFCore.AutomaticMigrations](https://www.nuget.org/packages/EFCore.AutomaticMigrations/) allows you to use code first migrations without having a code file in your project for each change you make.

Please feel free to open any issue you might have or idea for improvements.

## Sample Project

To see this package in action, you can use the minimal api sample project provided in the repository. The sample project demonstrates how to set up and use EFCore.AutomaticMigrations in a real-world scenario. Follow these steps to get started:

1. **Clone the repository:**

    ```sh
    git clone https://github.com/ionutneagos/efcore-automaticmigrations.git
    ```

2. **Open the solution:**

    Open the solution file `EFCore.AutomaticMigrations.Sample.sln` in your preferred IDE.

3. **Update the connection string:**

    Modify the connection string in `appsettings.json` to point to your SQL database.

4. **Run the application:**

    Start the application to see EFCore.AutomaticMigrations in action.

The sample project includes examples of how to configure and use the `DbMigrationsOptions` object, apply migrations, reset the database schema, and view applied migrations. This should help you understand how to integrate EFCore.AutomaticMigrations into your own projects.

Relevant lines of code are:

```cs
// create builder app
var app = builder.Build();

// begin apply automatic migration database to latest version
await using AsyncServiceScope serviceScope = app.Services.CreateAsyncScope();

// ToDoDBContext - entity framework core database context class
await using TodoDbContext? dbContext = serviceScope.ServiceProvider.GetService<TodoDbContext>();

if (dbContext is not null)
{
    // If the database context was successfully resolved from the service provider, we apply migrations.
    // The DbMigrationsOptions object is used to configure automatic data loss prevention and offers other tools like viewing raw SQL scripts for migrations.
    // The database is created automatically if it does not exist, and if it exists, it will be updated to the latest model changes.
    // Pay attention if you are using a PaaS database, like Azure; it will be created automatically using the default SKU, which might affect your costs.

    await dbContext.MigrateToLatestVersionAsync(new DbMigrationsOptions { AutomaticMigrationDataLossAllowed = true });

    // At this stage, the database contains the latest changes.
    // The Todo entity was mapped via Fluent API.
    // The TodoPoco entity was mapped via data annotations.
}
```

<!-- USAGE EXAMPLES -->
## Usage

DbMigrationsOptions object allows to configure migration options:

```cs
/// <summary>
/// Allow auto migration with data lost. 
/// </summary>
public bool AutomaticMigrationDataLossAllowed { get; set; } = false;
/// <summary>
/// Enable to execute auto migration
/// </summary>
public bool AutomaticMigrationsEnabled { get; set; } = true;
/// <summary>
///  Drop all tables from database and recreate based on model snapshot. Useful in scenarios when the data is transient and can be dropped when the schema changes. For example during prototyping, in tests, or for local changes
/// When ResetDatabaseSchema is true AutomaticMigrationsEnabled and AutomaticMigrationDataLossAllowed are set to true
/// </summary>
public bool ResetDatabaseSchema { get; set; } = false;

/// <summary>
/// Permit model snapshot to be updated. The key will be replaced with the appropriate value for each key-value pair provided.
///
</summary>
public Dictionary<string, string> UpdateSnapshot { get; set; } = new Dictionary<string, string>;

```

The package support following ways to apply/view-applied migrations:

1. Using MigrateDatabaseToLatestVersion

* Execute<TContext>(TContext context);
* ExecuteAsync<TContext>(TContext context);
* Execute<TContext, TMigrationsOptions>(TContext context, TMigrationsOptions options);
* ExecuteAsync<TContext, TMigrationsOptions>(TContext context, TMigrationsOptions options);

 ```cs
   // Get context
   var context = services.GetRequiredService<YourContext>();  

   // Apply migrations
   MigrateDatabaseToLatestVersion.Execute(context);
   // Reset database schema
   MigrateDatabaseToLatestVersion.Execute(context, new DbMigrationsOptions { ResetDatabaseSchema = true });

   // Apply migrations async
   await MigrateDatabaseToLatestVersion.ExecuteAsync(context);
   // Reset database schema async
   await MigrateDatabaseToLatestVersion.ExecuteAsync(context, new DbMigrationsOptions { ResetDatabaseSchema = true });
 ```

2. Using Context extensions methods

* MigrateToLatestVersion<TContext>(this TContext context);
* MigrateToLatestVersionAsync<TContext>(this TContext context);
* MigrateToLatestVersion<TContext, TMigrationsOptions>(this TContext context, TMigrationsOptions options);
* MigrateToLatestVersionAsync<TContext, TMigrationsOptions>(this TContext context, TMigrationsOptions options);

 ```cs
  // Get context
  var context = services.GetRequiredService<YourContext>();
  
  // Apply migrations
  context.MigrateToLatestVersion();
 // Reset database schema async
  context.MigrateToLatestVersion(new DbMigrationsOptions { ResetDatabaseSchema = true });

  // Apply migrations async
  context.MigrateToLatestVersionAsync();
  // Reset database schema async
  await context.MigrateToLatestVersionAsync(new DbMigrationsOptions { ResetDatabaseSchema = true });
  ```

3. View applied migrations - return list of MigrationOperation object

 ```cs
   //Get context
   var context = services.GetRequiredService<YourContext>();
   // Get applied migrations
    List<MigrationRaw> appliedMigrations = context.ListAppliedMigrations();
   // Get applied migrations async
    List<MigrationRaw> appliedMigrations = await context.ListAppliedMigrationsAsync();
   
    /// <summary>
    /// Migration Raw object
    /// </summary>
    public class MigrationRaw
    {
        public string MigrationId { get; set; } = string.Empty;
        public DateTime? CreatedDate { get; set; } = null;
    }
  ```

  4. View migration operations which will be applied as raw sql [added in **7.0.5**]

  ```cs
   //Get context
   var context = services.GetRequiredService<YourContext>();
   // List migration operations which will be applied as raw sql
    var migrationOperationsAsRawSql = context.ListMigrationOperationsAsRawSql();
   // ist migration operations which will be applied as raw sql async
    var migrationOperationsAsRawSql = await context.ListMigrationOperationsAsRawSqlAsync();
  ```
  
## Release 7.0.5

### Breaking Changes

 **ListMigrations** and **ListMigrationsAsync** methods are marked as deprecated and will be removed in further. Please use **ListAppliedMigrations** / **ListAppliedMigrationsAsync** instead.

### New Features

 Added the option to list migration operations which will be applied as raw sql via **ListMigrationOperationsAsRawSql** and **ListMigrationOperationsAsRawSqlAsync** context methods.

 Can be useful to check generated sql scripts which will be executed to update the model or to check if migrations needs to be applied.

 ---

 ![image](https://user-images.githubusercontent.com/9897204/231458299-2569013e-0b14-4799-b922-1719278c269b.png)

 ---
