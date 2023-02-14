using SecurePad.Resources;
using SecurePad.Services;
using System;
using System.Linq;
using Xamarin.Forms;

namespace SecurePad.ViewModels
{
    public class NewPasswordViewModel : BaseViewModel
    {

        private const int PasswdMinLength = 6, PasswdMinDigits = 1, PasswdMinLower = 1, PasswdMinUpper = 1;

        private string password;
        public string NewPassword
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        public Command EnterAppCommand { get; }
        public Command CancelCommand { get; }
        private PasswordService PasswordService { get; }

        public NewPasswordViewModel()
        {
            PasswordService = DependencyService.Get<PasswordService>();
            NewPassword = string.Empty;
            EnterAppCommand = new Command(EnterApp);
            CancelCommand = new Command(OnCancel);
        }

        private async void OnCancel()
        {
            await Shell.Current.GoToAsync("..");
        }

        private async void EnterApp()
        {
            try
            {
                IsBusy = true;

                if (NewPassword.Length < PasswdMinLength ||
                    NewPassword.Count(x => char.IsLower(x)) < PasswdMinLower ||
                    NewPassword.Count(x => char.IsUpper(x)) < PasswdMinUpper ||
                    NewPassword.Count(x => char.IsDigit(x)) < PasswdMinDigits)
                {
                    Toast.ShowToast(string.Format(AppResources.PASSWD_WEAK_TOAST, PasswdMinLength, PasswdMinUpper, PasswdMinLower, PasswdMinDigits));
                }
                else
                {
                    await PasswordService.SetHashedUserPassword(NewPassword);
                    await Shell.Current.GoToAsync("..");
                }
            }
            catch (Exception ex)
            {
                Toast.ShowToast(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

    }
}
