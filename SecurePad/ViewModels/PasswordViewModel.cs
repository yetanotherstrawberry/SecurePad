using SecurePad.Services;
using SecurePad.Views;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SecurePad.ViewModels
{
    public class PasswordViewModel : BaseViewModel
    {

        private string password;
        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }
        public Command EnterAppFingerprintCommand { get; }
        public Command EnterAppPasswordCommand { get; }
        private PasswordService PasswordService { get; }

        private void EnableDataStore() => DependencyService.Register<LocalDataStore>();

        private async Task EnableApp()
        {
            Password = string.Empty;

            EnableDataStore();
            await Shell.Current.GoToAsync(nameof(ItemsPage));
        }

        private async void EnterAppFingerprint()
        {
            IsBusy = true;
            if (await PasswordService.CheckUserBiometrics())
            {
                await EnableApp();
            }
            IsBusy = false;
        }

        private async void EnterAppPassword()
        {
            IsBusy = true;
            if (await PasswordService.CheckUserPassword(Password))
            {
                await EnableApp();
            }
            IsBusy = false;
        }

        public PasswordViewModel()
        {
            PasswordService = DependencyService.Get<PasswordService>();
            EnterAppFingerprintCommand = new Command(EnterAppFingerprint);
            EnterAppPasswordCommand = new Command(EnterAppPassword);
            Password = string.Empty;
        }

    }
}
