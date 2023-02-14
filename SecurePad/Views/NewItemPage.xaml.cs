using SecurePad.Models;
using SecurePad.ViewModels;
using Xamarin.Forms;

namespace SecurePad.Views
{
    public partial class NewItemPage : ContentPage
    {

        public Item Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();

            BindingContext = new NewItemViewModel();
        }

    }
}
