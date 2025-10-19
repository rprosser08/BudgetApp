using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using BudgetApp.Models;
using BudgetApp.ViewModels;

namespace BudgetApp.Views;

public partial class AccountWindow : Window
{
    public AccountWindow()
    {
        InitializeComponent();
        DataContext = new AccountWindowViewModel();
    }
    
    private async void TextBox_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (e.Source is not TextBox { DataContext: AccountViewModel dataContext }) return;
        var account = dataContext.GetAccount();
        await Account.UpdateAccountAsync(account);
    }
}
