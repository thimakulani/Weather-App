using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weather_App.Droid
{
    [Activity(Label = "Weather App", Icon = "@drawable/app_icon", MainLauncher =true, Theme = "@style/MyTheme.Splash", NoHistory = true)]
    public class SplashScreen : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            
        }
        protected override void OnResume()
        {
            base.OnResume();
            Task startWork = new Task(() =>
            {
                Task.Delay(3000);
            });
            startWork.ContinueWith(t =>
            {

                Intent intent = new Intent(Application.Context, typeof(MainActivity));
                StartActivity(intent);

            }, TaskScheduler.FromCurrentSynchronizationContext());
            startWork.Start();
        }
    }
}