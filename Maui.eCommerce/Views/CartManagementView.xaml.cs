using Library.eCommerce.Models;
using Maui.eCommerce.ViewModels;

namespace Maui.eCommerce.Views;

public partial class CartManagementView : ContentPage
{
	public CartManagementView()
	{
		InitializeComponent();
        BindingContext = new CartManagementViewModel();
	}

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
		(BindingContext as CartManagementViewModel)?.RefreshProductList();
    }

    private void CancelClicked(object sender, EventArgs e)
    {
		Shell.Current.GoToAsync("//Shop");
    }

	private void SearchClicked(object sender, EventArgs e)
    {
        (BindingContext as CartManagementViewModel)?.RefreshProductList();
    }

    private void EditClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var item = button.BindingContext as Item;
        if (item != null)
        {
            (BindingContext as CartManagementViewModel)?.RemoveFromCart(item.Id, item.Product.Quantity - item.Quantity);
        }
    }

    private void RemoveClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var item = button.BindingContext as Item;
        if (item != null)
        {
            (BindingContext as CartManagementViewModel)?.RemoveFromCart(item.Id, item.Product.Quantity);
        }
    }

    private void CheckoutClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//Checkout");
    }
}