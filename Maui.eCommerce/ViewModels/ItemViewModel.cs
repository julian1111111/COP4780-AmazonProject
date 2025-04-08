using Library.eCommerce.Models;
using Library.eCommerce.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Maui.eCommerce.ViewModels
{
    public class ItemViewModel
    {
        public Item Model { get; set; }

        public ItemViewModel()
        {
            Model = new Item();
            SetupCommands();
        }

        public ItemViewModel(Item model)
        {
            Model = model;
            SetupCommands();
        }

        void SetupCommands()
        {
            AddCommand = new Command(DoAdd);
        }

        private void DoAdd()
        {
            ProductServiceProxy.Current.AddOrUpdateInventory(Model);
        }

        public ICommand? AddCommand { get; set; }
    }
}
