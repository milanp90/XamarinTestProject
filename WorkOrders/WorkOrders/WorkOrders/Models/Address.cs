using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using SQLiteNetExtensions.Attributes;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WorkOrders.Models
{
    [Table("Addresses")]
    [Serializable]
    public class Address
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [ForeignKey(typeof(WorkOrder))]
        public int WorkOrderId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string FullAddress { get; set; }

        public async Task SetCoordinatesBasedOnAddress()
        {
            var locations = await Geocoding.GetLocationsAsync(FullAddress);

            var location = locations?.FirstOrDefault();
            if (location != null)
            {
                Latitude = location.Latitude;
                Longitude = location.Longitude;
            }
        }
    }
}
