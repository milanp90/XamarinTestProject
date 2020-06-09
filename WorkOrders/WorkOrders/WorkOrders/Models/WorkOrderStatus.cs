using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace WorkOrders.Models
{
    public enum WorkOrderStatus
    {
        [Description("Available")]
        Available,
        [Description("Needs Work")]
        NeedsWork,
        [Description("Expired")]
        Expired,
        [Description("Started")]
        Started,
        [Description("Overdue")]
        Overdue,
        [Description("Done")]
        Done
    }
}
