using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using BudgetApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BudgetApp.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<BudgetItemViewModel> BudgetItems { get; } = new();
    
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddBudgetItemCommand))] // This attribute will invalidate the command each time this property changes
    private string? _newDescription;
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddBudgetItemCommand))]
    private decimal? _newAmount;

    public MainWindowViewModel()
    {
        LoadBudgetItems();
    }

    private async void LoadBudgetItems()
    {
        var items = await BudgetItem.LoadBudgetItems();
        if (items == null) return;
        foreach (var item in items)
        {
            BudgetItems.Add(new BudgetItemViewModel(item, RemoveBudgetItem));
        }
    }

    public void RemoveBudgetItem(BudgetItemViewModel item)
    {
        BudgetItems.Remove(item);
    }

    private bool CanAddBudgetItem() => !string.IsNullOrWhiteSpace(NewDescription) && NewAmount is not null;
    [RelayCommand (CanExecute = nameof(CanAddBudgetItem))]
    private async Task AddBudgetItem()
    {
        var newBudgetItem = new BudgetItem
        {
            Description = NewDescription,
            Amount = NewAmount
        };
        await BudgetItem.SaveBudgetItem(newBudgetItem);
        newBudgetItem.Id = BudgetItem.MaxId();
        
        var newBudgetItemViewModel = new BudgetItemViewModel(newBudgetItem, RemoveBudgetItem);
        BudgetItems.Add(newBudgetItemViewModel);

        NewDescription = null;
        NewAmount = null;
    }
}