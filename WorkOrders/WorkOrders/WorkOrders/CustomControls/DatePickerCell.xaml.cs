using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorkOrders.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DatePickerCell : ViewCell
    {
        public static readonly BindableProperty LabelTextProperty = BindableProperty.Create("LabelText", typeof(string), typeof(DatePickerCell));

        public string LabelText
        {
            get => (string)GetValue(LabelTextProperty);
            set => SetValue(LabelTextProperty, value);
        }

        public static readonly BindableProperty DateValueProperty = BindableProperty.Create("DateValue", typeof(DateTime), typeof(DatePickerCell), DateTime.Now, BindingMode.TwoWay);

        public DateTime DateValue
        {
            get => (DateTime)GetValue(DateValueProperty);
            set => SetValue(DateValueProperty, value);
        }

        public DatePickerCell()
        {
            InitializeComponent();
        }
    }
}