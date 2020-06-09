using System;
using WorkOrders.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorkOrders
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new WorkOrdersPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
