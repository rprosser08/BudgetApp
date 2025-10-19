using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using BudgetApp.Data;
using Microsoft.EntityFrameworkCore;

namespace BudgetApp.Models;

public class Account
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string? Name { get; set; }

    public static async Task<List<Account>> GetAllAccountsAsync()
    {
        await using var context = new AppDbContext();
        return await context.Accounts.OrderBy(account => account.Id).ToListAsync();
    }
    
    public static async Task UpdateAccountAsync(Account account)
    {
        await using var context = new AppDbContext();
        context.Accounts.Update(account);
        await context.SaveChangesAsync();
    }

    public static async Task<int> AddAccountAsync(Account newAccount)
    {
        await using var context = new AppDbContext();
        context.Accounts.Add(newAccount);
        await context.SaveChangesAsync();
        return newAccount.Id;
    }

    public static async Task DeleteAccountAsync(int accountId)
    {
        await using var context = new AppDbContext();
        context.Accounts.Remove(new Account { Id = accountId });
        await context.SaveChangesAsync();
    }
}
