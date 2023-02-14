using Android.Widget;
using SecurePad.Droid;
using SecurePad.Interfaces;
using Xamarin.Forms;
using Application = Android.App.Application;

[assembly: Dependency(typeof(AndroidToast))]
namespace SecurePad.Droid
{
    public class AndroidToast : IToast
    {

        public void ShowToast(string message) => Toast.MakeText(Application.Context, message, ToastLength.Long).Show();

    }
}
