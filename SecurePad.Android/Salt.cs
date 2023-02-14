using Android.Provider;
using SecurePad.Droid;
using SecurePad.Interfaces;
using Xamarin.Forms;
using Application = Android.App.Application;

[assembly: Dependency(typeof(AndroidSalt))]
namespace SecurePad.Droid
{
    public class AndroidSalt : ISalt
    {

        public string GetSalt() => Settings.Secure.GetString(Application.Context.ContentResolver, Settings.Secure.AndroidId);

    }
}
