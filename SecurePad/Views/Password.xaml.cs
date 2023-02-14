using SecurePad.ViewModels;
using Xamarin.Forms;

namespace SecurePad.Views
{
    public partial class PasswordPage : ContentPage
    {
        public PasswordPage()
        {

            InitializeComponent();

            BindingContext = new PasswordViewModel();

        }
    }
}
