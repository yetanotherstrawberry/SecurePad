using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
using SecurePad.Helpers;
using SecurePad.Interfaces;
using SecurePad.Resources;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace SecurePad.Services
{
    internal class PasswordService
    {

        public byte[] EncryptionKey { get; private set; } = null;
        private IToast Toast { get; } = DependencyService.Get<IToast>();

        private readonly AuthenticationRequestConfiguration config = new AuthenticationRequestConfiguration(AppResources.APP_NAME, AppResources.AUTH_MSG);

        private const string EncryptionKeyName = "EncryptionKey";
        private const string UserPasswordHash = "PasswordHash";
        private const int AesKeySizeBytes = 32; // 16/24/32B = 128/192/256b

        public async Task SetHashedUserPassword(string password)
        {
            var passwordBytes = Encoding.Default.GetBytes(password);
            var passwordHash = AesClass.CreateHashedKey(passwordBytes);
            var passwordBase64 = Convert.ToBase64String(passwordHash);
            await SecureStorage.SetAsync(UserPasswordHash, passwordBase64);
            Toast.ShowToast(AppResources.PASSWD_CHNG_TOAST);
        }

        public async Task<bool> CheckUserPassword(string userPassword)
        {
            try
            {
                if (string.IsNullOrEmpty(userPassword))
                {
                    Toast.ShowToast(AppResources.PASSWD_EMPTY);
                    return false;
                }

                var userPasswordBytes = Encoding.Default.GetBytes(userPassword);
                var userPasswordHash = AesClass.CreateHashedKey(userPasswordBytes);

                var userPasswordHashBase64 = await SecureStorage.GetAsync(UserPasswordHash);
                if (string.IsNullOrEmpty(userPasswordHashBase64))
                {
                    Toast.ShowToast(AppResources.PASSWD_UNSET);
                    return false;
                }

                var hashedPasswdBytes = Convert.FromBase64String(userPasswordHashBase64);

                if (userPasswordHash.SequenceEqual(hashedPasswdBytes))
                {
                    EncryptionKey = await GetEncryptionKey();
                    Toast.ShowToast(AppResources.AUTH_SUCCESS);
                    return true;
                }
                else
                {
                    Toast.ShowToast(AppResources.AUTH_FAILED);
                }
            }
            catch (Exception e)
            {
                Toast.ShowToast(e.Message);
            }

            return false;
        }

        public async Task<bool> CheckUserBiometrics()
        {
            try
            {
                if (await CrossFingerprint.Current.IsAvailableAsync(allowAlternativeAuthentication: false))
                {
                    if ((await CrossFingerprint.Current.AuthenticateAsync(config)).Authenticated)
                    {
                        EncryptionKey = await GetEncryptionKey();
                        Toast.ShowToast(AppResources.AUTH_SUCCESS);
                        return true;
                    }
                    else Toast.ShowToast(AppResources.AUTH_FAILED);
                }
                else Toast.ShowToast(AppResources.BIO_OFF);
            }
            catch (Exception e)
            {
                Toast.ShowToast(e.Message);
            }

            return false;
        }

        private async Task<byte[]> GetEncryptionKey()
        {
            var keyBase64 = await SecureStorage.GetAsync(EncryptionKeyName);
            byte[] ret;

            if (string.IsNullOrEmpty(keyBase64))
            {
                var passwdBytes = new byte[AesKeySizeBytes];
                using (var crypto = new RNGCryptoServiceProvider())
                {
                    crypto.GetNonZeroBytes(passwdBytes);
                }
                await SecureStorage.SetAsync(EncryptionKeyName, Convert.ToBase64String(passwdBytes));
                ret = passwdBytes;
                Toast.ShowToast(AppResources.ENC_KEY_CREATED);
            }
            else ret = Convert.FromBase64String(keyBase64);

            return ret;
        }

    }
}
