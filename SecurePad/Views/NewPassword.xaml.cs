using SecurePad.ViewModels;
using Xamarin.Forms;

namespace SecurePad.Views
{
    public partial class NewPasswordPage : ContentPage
    {
        public NewPasswordPage()
        {

            InitializeComponent();

            BindingContext = new NewPasswordViewModel();

        }
    }
}
