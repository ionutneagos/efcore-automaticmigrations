<!-- GETTING STARTED -->
## About the repo

Contains instructions to use Automatic Migrations for Entity Framework Core for SQL Databases for [EFCore.AutomaticMigrations](https://www.nuget.org/packages/EFCore.AutomaticMigrations/) nuget package. Please feel free to open any issue you might have or idea for improvements.

<!-- USAGE EXAMPLES -->
## Usage

DbMigrationsOptions object allows to configure migration options:
  ```cs
/// <summary>
/// Allow auto migration with data lost. 
/// </summary>
public bool AutomaticMigrationDataLossAllowed { get; set; } = true;
/// <summary>
/// Enable to execute auto migration
/// </summary>
public bool AutomaticMigrationsEnabled { get; set; } = true;
/// <summary>
///  Drop all tables from database and recreate based on model snapshot. Useful in scenarios when the data is transient and can be dropped when the schema changes. For example during prototyping, in tests, or for local caches
/// When ResetDatabaseSchema is true AutomaticMigrationsEnabled and AutomaticMigrationDataLossAllowed are set to true
/// </summary>
public bool ResetDatabaseSchema { get; set; } = false;
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
    List<MigrationRaw> appliedMigrations = context.ListMigrations();
   // Get applied migrations async
    List<MigrationRaw> appliedMigrations = await context.ListMigrationsAsync();
   
    /// <summary>
    /// Migration Raw object
    /// </summary>
    public class MigrationRaw
    {
        public string MigrationId { get; set; } = string.Empty;
        public DateTime? CreatedDate { get; set; } = null;
    }
  ``` 
