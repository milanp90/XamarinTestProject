using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SQLite;
using SQLiteNetExtensions.Attributes;
using WorkOrders.Helpers;
using Xamarin.Forms;

namespace WorkOrders.Models
{
    [Table("WorkOrders")]
    [Serializable]
    public class WorkOrder
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; }
        [MaxLength(50)]
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string SecurityRisks { get; set; }

        public DateTime LatestWorkStartDate { get; set; } = DateTime.Now.Date;
        public DateTime LatestWorkEndDate { get; set; } = DateTime.Now.Date;

        public DateTime WorkStartedOnDate { get; set; } = DateTime.Now.Date;
        public DateTime WorkEndedOnDate { get; set; } = DateTime.Now.Date;

        [OneToOne("WorkOrderId", CascadeOperations = CascadeOperation.All)]
        public Address ClientAddress { get; set; }

        [OneToOne("WorkOrderId", CascadeOperations = CascadeOperation.All)]
        public ClientInformation ClientInformation { get; set; }
    }
}
