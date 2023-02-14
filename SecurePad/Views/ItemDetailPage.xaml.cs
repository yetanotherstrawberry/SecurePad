using SecurePad.ViewModels;
using Xamarin.Forms;

namespace SecurePad.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {

            InitializeComponent();

            BindingContext = new ItemDetailViewModel();

        }
    }
}
