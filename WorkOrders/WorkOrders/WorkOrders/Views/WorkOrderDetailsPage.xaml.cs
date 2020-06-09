using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrders.Models;
using WorkOrders.Persistence;
using WorkOrders.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WorkOrders.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WorkOrderDetailsPage : ContentPage
    {
        public WorkOrderDetailsPage(WorkOrderDetailsViewModel workOrderDetails)
        {
            InitializeComponent();

            ViewModel = workOrderDetails;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (!ViewModel.HasSecurityRisks)
            {
                WorkOrderSection.Remove(SecurityRiskCell);
            }
            else if (!WorkOrderSection.Contains(SecurityRiskCell))
            {
                WorkOrderSection.Add(SecurityRiskCell);
            } 
        }

        public WorkOrderDetailsViewModel ViewModel
        {
            get => BindingContext as WorkOrderDetailsViewModel;
            set => BindingContext = value;
        }
    }
}