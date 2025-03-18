using Maui.eCommerce.ViewModels;

namespace Maui.eCommerce.Views;

public partial class InventoryProductDetail : ContentPage
{
	public InventoryProductDetail()
	{
		InitializeComponent();
        BindingContext = new ProductViewModel();
	}
    private void CancelClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//InventoryManagement");
    }
}