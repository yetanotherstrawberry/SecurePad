using SecurePad.Services;
using Xamarin.Forms;

namespace SecurePad
{
    public partial class App : Application
    {
        public App()
        {

            InitializeComponent();
            DependencyService.Register<PasswordService>();
            MainPage = new AppShell();

        }
    }
}
