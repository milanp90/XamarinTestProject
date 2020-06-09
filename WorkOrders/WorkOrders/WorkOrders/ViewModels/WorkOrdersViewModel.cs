using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkOrders.Helpers;
using WorkOrders.Models;
using WorkOrders.Persistence;
using WorkOrders.Services;
using WorkOrders.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WorkOrders.ViewModels
{
    public class WorkOrdersViewModel : BaseViewModel
    {
        private bool _isDataLoaded;

        public ObservableCollection<WorkOrderDetailsViewModel> WorkOrders { get; private set; }
            = new ObservableCollection<WorkOrderDetailsViewModel>();

        public WorkOrder SelectedWorkOrder
        {
            get => null;
            set => OnPropertyChanged();
        }

        public ICommand AddWorkOrderCommand { get; private set; }
        public ICommand SelectWorkOrderCommand { get; private set; }

        public WorkOrdersViewModel(IWorkOrdersRepository ordersRepository, IPageService pageService) 
            : base(ordersRepository, pageService)
        {
            AddWorkOrderCommand = new Command(async () => await AddWorkOrder());
            SelectWorkOrderCommand = new Command<WorkOrderDetailsViewModel>(async wo => await SelectWorkOrder(wo));

            MessagingCenter.Subscribe<WorkOrderFormViewModel>(this, EventConstants.WorkOrderAdded, OnWorkOrderAdded);
            MessagingCenter.Subscribe<WorkOrderDetailsViewModel>(this, EventConstants.WorkOrderCancelled, OnWorkOrderCancelled);
        }

        private void OnWorkOrderAdded(WorkOrderFormViewModel source)
        {
            WorkOrders.Add(new WorkOrderDetailsViewModel(source.WorkOrder, WorkOrdersRepository.Instance, PageService.Instance));
        }

        private void OnWorkOrderCancelled(WorkOrderDetailsViewModel source)
        {
            WorkOrders.Remove(source);
        }

        public async Task LoadData()
        {
            if (_isDataLoaded)
                return;

            _isDataLoaded = true;

            var workOrders = await _workOrdersRepository.GetList();

            foreach (var wo in workOrders)
                WorkOrders.Add(new WorkOrderDetailsViewModel(wo, WorkOrdersRepository.Instance, PageService.Instance));
        }

        private async Task AddWorkOrder()
        {
            await _pageService.PushAsync(new WorkOrderFormPage(new WorkOrder() { ClientInformation = new ClientInformation(), ClientAddress = new Address() }));
        }

        private async Task SelectWorkOrder(WorkOrderDetailsViewModel orderViewModel)
        {
            if (orderViewModel == null)
                return;

            SelectedWorkOrder = null;

            await _pageService.PushAsync(new WorkOrderDetailsPage(orderViewModel));
        }

    }
}
