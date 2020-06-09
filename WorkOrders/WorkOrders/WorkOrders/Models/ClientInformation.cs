using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace WorkOrders.Models
{
    [Table("ClientInformations")]
    [Serializable]
    public class ClientInformation
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [ForeignKey(typeof(WorkOrder))]
        public int WorkOrderId { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }
        public string PrimaryPhoneNumber { get; set; }
        public string SecondaryPhoneNumber { get; set; }
    }
}
