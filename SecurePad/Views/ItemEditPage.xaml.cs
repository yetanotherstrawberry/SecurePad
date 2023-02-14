using SecurePad.ViewModels;
using Xamarin.Forms;

namespace SecurePad.Views
{
    public partial class ItemEditPage : ContentPage
    {
        public ItemEditPage()
        {

            InitializeComponent();

            BindingContext = new ItemEditViewModel();

        }
    }
}
