using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Essentials;

namespace FireBaseThing1
{
    public class LocationClass
    {
        public double longitude;
        public double latitude;
        public string name;
        
        public LocationClass()
        {

        }
        public LocationClass(double lat, double longi,string name)
        {
            this.longitude = longi;
            this.latitude = lat;
            this.name = name;
        }
        public LocationClass(Location l, string name)
        {
            this.longitude = l.Longitude;
            this.latitude = l.Latitude;
            this.name = name;
        }
        public Location getLocation()
        {
            return (new Location(latitude, longitude));
        }
    }
}