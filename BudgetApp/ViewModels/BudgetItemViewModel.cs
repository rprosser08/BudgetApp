using System;
using System.Threading.Tasks;
using BudgetApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BudgetApp.ViewModels;

public partial class BudgetItemViewModel : ViewModelBase
{
    [ObservableProperty]
    private int _id;
    
    [ObservableProperty]
    private string? _description;
    
    [ObservableProperty]
    private decimal? _amount;
    
    private readonly Action<BudgetItemViewModel>? _onDeleted;
    
    public BudgetItemViewModel() { }

    public BudgetItemViewModel(BudgetItem item, Action<BudgetItemViewModel> onDeleted)
    {
        Id = item.Id;
        Description = item.Description;
        Amount = item.Amount;
        _onDeleted = onDeleted;
    }

    public BudgetItem GetBudgetItem()
    {
        return new BudgetItem()
        {
            Id = this.Id,
            Description = this.Description,
            Amount = this.Amount,
        };
    }

    [RelayCommand]
    public async Task DeleteBudgetItem(BudgetItemViewModel item)
    {
        await BudgetItem.DeleteBudgetItem(item.Id);
        _onDeleted?.Invoke(this);
    }
}
