using System.Collections.ObjectModel;
using System.Threading.Tasks;
using BudgetApp.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BudgetApp.ViewModels;

public partial class AccountWindowViewModel : ViewModelBase
{
    public ObservableCollection<AccountViewModel> AccountNames { get; }
    
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddNewAccountCommand))]
    private string? _newAccount;
    
    public AccountWindowViewModel()
    {
        AccountNames = new ObservableCollection<AccountViewModel>();
        PrepareAccountNames();
    }

    private async void PrepareAccountNames()
    {
        AccountNames.Clear();
        var accountNamesLoaded = await Account.GetAllAccountsAsync();
        if (accountNamesLoaded.Count > 0)
        {
            foreach (var account in accountNamesLoaded)
            {
                AccountNames.Add(new AccountViewModel(account));
            }
        }
    }

    private bool CanAddAccount() => !string.IsNullOrWhiteSpace(NewAccount);
    
    [RelayCommand (CanExecute = nameof(CanAddAccount))]
    private async Task AddNewAccountAsync()
    {
        if (NewAccount is null) return;
        var newAccountId = await Account.AddAccountAsync(new Account
        {
            Name = NewAccount
        });
        AccountNames.Add(new AccountViewModel(new Account
        {
            Id = newAccountId,
            Name = NewAccount
        }));
        NewAccount = null;
    }

    [RelayCommand]
    private async Task DeleteAccountAsync(AccountViewModel accountViewModel)
    {
        AccountNames.Remove(accountViewModel);
        await Account.DeleteAccountAsync(accountViewModel.Id);
    }
}
