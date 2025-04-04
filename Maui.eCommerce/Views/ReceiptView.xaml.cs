using Maui.eCommerce.ViewModels;

namespace Maui.eCommerce.Views;

[QueryProperty(nameof(Total), "total")]
public partial class Receipt : ContentPage
{
    public double Total { get; set; }
	public Receipt()
	{
		InitializeComponent();
        BindingContext = new ReceiptViewModel();
	}

    private void CancelClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//Shop");
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        BindingContext = new ReceiptViewModel();
        (BindingContext as ReceiptViewModel)?.ClearCart();
    }
}