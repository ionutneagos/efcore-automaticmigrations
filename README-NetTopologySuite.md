<!-- GETTING STARTED -->
## About

Contains instructions to use [EFCore.AutomaticMigrations.NetTopologySuite](https://www.nuget.org/packages/EFCore.AutomaticMigrations.NetTopologySuite/) nuget package for Automatic Migrations with Entity Framework Core NetTopologySuite for SQL Databases. Please feel free to open any issue you might have or idea for improvements.

<!-- USAGE EXAMPLES -->
## Usage
Enable mapping to spatial types via NTS
```cs
options.UseSqlServer( @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Database", x => x.UseNetTopologySuite());
``` 

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

 
 
