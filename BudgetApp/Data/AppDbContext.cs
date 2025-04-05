using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using BudgetApp.Models;

namespace BudgetApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext() { }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<BudgetItem> BudgetItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Retrieve the connection string from environment variables
        string? server = Environment.GetEnvironmentVariable("DB_SERVER");
        string? database = Environment.GetEnvironmentVariable("DB_NAME");
        string? username = Environment.GetEnvironmentVariable("DB_USER"); 
        string? password = Environment.GetEnvironmentVariable("DB_PASSWORD"); 
        
        if (string.IsNullOrEmpty(server) || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(database))
        {
            throw new InvalidOperationException("One or more environment variables are missing.");
        }
        
        string connectionString = $"Server={server};Port=5432;Database={database};User Id={username};Password={password};";
        optionsBuilder.UseNpgsql(connectionString);
    }
}