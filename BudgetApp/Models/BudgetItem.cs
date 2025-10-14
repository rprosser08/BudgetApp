using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using BudgetApp.Data;
using Microsoft.EntityFrameworkCore;

namespace BudgetApp.Models;

public class BudgetItem
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string? Description { get; set; }
    public decimal? Amount { get; set; }

    public static async Task SaveBudgetItem(BudgetItem item, bool create=true)
    {
        await using var context = new AppDbContext();
        if (create)
        {
            var newBudgetItem = new BudgetItem() { Description = item.Description, Amount = item.Amount };
            context.BudgetItems.Add(newBudgetItem);
        }
        else
        {
            context.BudgetItems.Update(item);
        }
        context.SaveChanges();
    }
    
    public static async Task<IEnumerable<BudgetItem>?> LoadBudgetItems()
    {
        await using var context = new AppDbContext();
        return await context.BudgetItems.OrderBy(item => item.Id).ToListAsync();
    }

    public static async Task DeleteBudgetItem(int id)
    {
        await using var context = new AppDbContext();
        var item = await context.BudgetItems.FindAsync(id);
        if (item != null)
        {
            context.BudgetItems.Remove(item);
            context.SaveChanges();
        }
    }

    public static int MaxId()
    {
        using var context = new AppDbContext();
        var maxId = context.BudgetItems.Select(item => item.Id).Max();
        return maxId;
    }
}
