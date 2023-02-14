using SecurePad.Views;
using Xamarin.Forms;

namespace SecurePad
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {

            InitializeComponent();

            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            Routing.RegisterRoute(nameof(ItemsPage), typeof(ItemsPage));
            Routing.RegisterRoute(nameof(PasswordPage), typeof(PasswordPage));
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(ItemEditPage), typeof(ItemEditPage));
            Routing.RegisterRoute(nameof(NewPasswordPage), typeof(NewPasswordPage));

        }
    }
}
