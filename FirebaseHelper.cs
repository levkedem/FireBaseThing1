using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Database;
using Firebase.Database.Query;
using Xamarin.Essentials;

namespace FireBaseThing1
{
    public class FirebaseHelper
    {
        public static FirebaseClient client= new FirebaseClient("https://davarfirebase1-default-rtdb.europe-west1.firebasedatabase.app/");
        private static string database = "locationsDb";

        public static async Task<List<LocationClass>> GetAll()
        {
            return (await client.
                Child(database).OnceAsync<LocationClass>()).Select(item => new LocationClass
                {
                    longitude = item.Object.longitude,
                    latitude= item.Object.latitude,
                    name = item.Object.name
                }).ToList();
        }

        public static async Task Add(LocationClass loc)
        {
            await client
                .Child(database)
                .PostAsync(loc);
        }

        public static async Task<LocationClass> Get(string name)
        {
            var allPersons = await GetAll();
            await client
              .Child(database)
              .OnceAsync<LocationClass>();
            return allPersons.Where(a => a.name == name).FirstOrDefault();
        }

        public static async Task Update(LocationClass state)
        {
            var toUpdatePerson = (await client
              .Child(database)
              .OnceAsync<LocationClass>()).Where(a => a.Object.name == state.name).FirstOrDefault();

            await client
              .Child(database)
              .Child(toUpdatePerson.Key)
              .PutAsync(state);
        }
        public static async Task Delete(string name)
        {
            var toDeletePerson = (await client
              .Child(database)
              .OnceAsync<LocationClass>()).Where(a => a.Object.name == name).FirstOrDefault();
            await client.Child(database).Child(toDeletePerson.Key).DeleteAsync();

        }
    }
}