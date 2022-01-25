using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System.Threading.Tasks;
using Xamarin.Essentials;
using System;
using System.Threading;

namespace FireBaseThing1
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        CancellationTokenSource cts;
        Location thisLoc; EditText et1, et2;
        Button bt1, bt2;
        TextView tv1;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            GetCurrentLocation();
            et1 = (EditText)FindViewById(Resource.Id.et1);
            et2 = (EditText)FindViewById(Resource.Id.et2);
            bt1 = (Button)FindViewById(Resource.Id.bt1);
            bt2 = (Button)FindViewById(Resource.Id.bt2);
            tv1 = (TextView)FindViewById(Resource.Id.tv1);
            bt1.Click += Bt1_Click;
            bt2.Click += Bt2_Click;

        }

        private void Bt2_Click(object sender, EventArgs e)
        {
            if (thisLoc == null)
            {
                Toast.MakeText(this, "Waiting for your location, please try again in a few seconds", ToastLength.Short).Show();
            }
            else if (et2.Text != "")
            {
                FirebaseHelper.Add(new LocationClass(thisLoc, "person-" + et2.Text));
            }
            else
            {
                Toast.MakeText(this, "enter your name", ToastLength.Short).Show();
            }
        }

        private void Bt1_Click(object sender, EventArgs e)
        {
            doit();
        }

        private async void doit()
        {
            if (thisLoc == null)
            {
                Toast.MakeText(this, "Waiting for your location, please try again in a few seconds", ToastLength.Short).Show();
            }
            else if (et1.Text != "")
            {
                string name ="person-"+ et1.Text;
                try
                {
                    Toast.MakeText(this, "Searching For The Person", ToastLength.Short).Show();
                    LocationClass lc = await FirebaseHelper.Get(name);
                    if (lc == null)
                        Toast.MakeText(this, "There Is No Person With This Name", ToastLength.Short).Show();
                    else
                    {
                        Location temp = lc.getLocation();
                        tv1.Text = "your distance from " + et1.Text + " is " + Location.CalculateDistance(thisLoc, temp, DistanceUnits.Kilometers) * 1000 + " meters";
                    }
                }
                catch (Exception exc)
                {
                    Toast.MakeText(this, "Connection with server failed", ToastLength.Short).Show();
                }
            }
            else
            {
                Toast.MakeText(this, "Please Enter Name", ToastLength.Short).Show();
            }
        }
        async Task GetCurrentLocation()//asks for Geolocation
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(30));
                cts = new CancellationTokenSource();
                Location location1 = await Geolocation.GetLocationAsync(request, cts.Token);


                if (location1 != null)
                {
                    thisLoc = location1;
                }
                else
                {
                    Toast.MakeText(this, "no loc", ToastLength.Short).Show();
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                Toast.MakeText(this, "please enable your device GPS", ToastLength.Long).Show();
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}