using WorkOrders.Models;
using WorkOrders.Persistence;
using WorkOrders.Services;
using WorkOrders.ViewModels;
using Xamarin.Forms;

namespace WorkOrders.Views
{
	public partial class WorkOrderFormPage : ContentPage
	{
		public WorkOrderFormPage(WorkOrder workOrder)
		{
			InitializeComponent();

            BindingContext = new WorkOrderFormViewModel(
                workOrder ?? new WorkOrder() {ClientAddress = new Address(), ClientInformation = new ClientInformation()}, WorkOrdersRepository.Instance, PageService.Instance);
		}
	}
}