using System.Runtime.CompilerServices;
using WorkOrders.Persistence;
using WorkOrders.Services;
using WorkOrders.ViewModels;
using Xamarin.Forms;

namespace WorkOrders.Views
{
	public partial class WorkOrdersPage : ContentPage
	{
		public WorkOrdersPage()
		{
			InitializeComponent();

            ViewModel = new WorkOrdersViewModel(WorkOrdersRepository.Instance, PageService.Instance);
		}

		protected override async void OnAppearing()
		{
            base.OnAppearing();

			await ViewModel.LoadData();
		}

		void OnWorkOrderSelected(object sender, SelectedItemChangedEventArgs e)
		{
			ViewModel.SelectWorkOrderCommand.Execute(e.SelectedItem);
		}

		public WorkOrdersViewModel ViewModel
        {
            get => BindingContext as WorkOrdersViewModel;
            set => BindingContext = value;
        }
    }
}
