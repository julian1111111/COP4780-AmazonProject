using Library.eCommerce.Models;
using Maui.eCommerce.ViewModels;

namespace Maui.eCommerce.Views;

public partial class ShopView : ContentPage
{
    public ShopView()
    {
        InitializeComponent();
        BindingContext = new ShopViewModel();
    }

    private void SearchClicked(object sender, EventArgs e)
    {
        (BindingContext as ShopViewModel)?.RefreshProductList();
    }

    private void AddToCartClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var item = button.BindingContext as Item;
        if (item != null)
        {
            (BindingContext as ShopViewModel)?.AddToCart(item.Id, item.Quantity);
        }
    }

    private void ViewCartClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//CartManagement");
    }

    private void CheckoutClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//Checkout");
    }

    private void CancelClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//MainPage");
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        (BindingContext as ShopViewModel)?.RefreshProductList();
    }
}