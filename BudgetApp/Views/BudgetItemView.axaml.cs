using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using BudgetApp.Models;
using BudgetApp.ViewModels;

namespace BudgetApp.Views;

public partial class BudgetItemView : UserControl
{
    public BudgetItemView()
    {
        InitializeComponent();
    }

    private async void NumericUpDown_OnValueChanged(object? sender, NumericUpDownValueChangedEventArgs e)
    {
        if (e.NewValue is null || e.Source is not NumericUpDown { DataContext: BudgetItemViewModel dataContext }) return;
        dataContext.Amount = e.NewValue;
        var budgetItem = dataContext.GetBudgetItem();
        await BudgetItem.SaveBudgetItem(budgetItem, false);
    }

    private async void TextBox_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (e.Source is not TextBox { DataContext: BudgetItemViewModel dataContext }) return;
        var budgetItem = dataContext.GetBudgetItem();
        await BudgetItem.SaveBudgetItem(budgetItem, false);
    }
}