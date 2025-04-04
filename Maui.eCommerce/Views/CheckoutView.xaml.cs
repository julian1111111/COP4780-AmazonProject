using Maui.eCommerce.ViewModels;

namespace Maui.eCommerce.Views;

public partial class CheckoutView : ContentPage
{
	public CheckoutView()
	{
		InitializeComponent();
		BindingContext = new CheckoutViewModel();
	}

    private void CancelClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//CartManagement");
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        (BindingContext as CheckoutViewModel)?.RefreshProductList();
    }

    private void CompletePurchaseClicked(object sender, EventArgs e)
    {
        (BindingContext as CheckoutViewModel)?.RefreshProductList();
        var total = (BindingContext as CheckoutViewModel)?.Total;
        Shell.Current.GoToAsync($"//Receipt?total={total}");
    }

    private void ShopClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//Shop");
    }
}