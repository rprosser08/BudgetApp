using BudgetApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;

namespace BudgetApp.ViewModels;

public partial class AccountViewModel : ViewModelBase
{
    [ObservableProperty] 
    private int _id;
        
    [ObservableProperty]
    private string? _name;
    
    public AccountViewModel() { }

    public AccountViewModel(Account account)
    {
        _id = account.Id;
        _name = account.Name;
    }

    public Account GetAccount()
    {
        return new Account()
        {
            Id = Id,
            Name = Name
        };
    }
}
