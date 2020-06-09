using System;
using System.Collections.Generic;
using System.Text;
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
    public class WorkOrderDetailsViewModel : BaseViewModel
    {
        public WorkOrder WorkOrder { get; set; }
        public bool HasSecurityRisks => !string.IsNullOrWhiteSpace(WorkOrder.SecurityRisks);
        public bool IsInProgress => DateTime.Now.Date >= WorkOrder.WorkStartedOnDate && DateTime.Now.Date <= WorkOrder.WorkEndedOnDate;
        public WorkOrderStatus OrderStatus => GetStatus();
        public string OrderStatusText => EnumHelper.GetEnumDescription(OrderStatus);

        public string LatestWorkStartDate => WorkOrder.LatestWorkStartDate.ToShortDateString();
        public string LatestWorkEndDate => WorkOrder.LatestWorkEndDate.ToShortDateString();
        public string WorkStartedOnDate => WorkOrder.WorkStartedOnDate.ToShortDateString();
        public string WorkEndedOnDate => WorkOrder.WorkEndedOnDate.ToShortDateString();
        public ICommand CancelWorkOrderCommand { get; private set; }
        public ICommand EditWorkOrderCommand { get; private set; }
        public ICommand OpenMapCommand { get; private set; }

        public WorkOrderDetailsViewModel(WorkOrder order, IWorkOrdersRepository ordersRepository, IPageService pageService)
        : base(ordersRepository, pageService)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            CancelWorkOrderCommand = new Command<WorkOrder>(async wo => await CancelWorkOrder(wo));
            EditWorkOrderCommand = new Command<WorkOrder>(async wo => await EditWorkOrder(wo));
            OpenMapCommand = new Command<WorkOrderDetailsViewModel>(async wo => await OpenMap(wo));

            MessagingCenter.Subscribe<WorkOrderFormViewModel>(this, EventConstants.WorkOrderUpdated, OnWorkOrderUpdated);

            WorkOrder = order;
        }

        private void OnWorkOrderUpdated(WorkOrderFormViewModel source)
        {
            if (source.WorkOrder.Id == WorkOrder.Id)
            {
                WorkOrder = source.WorkOrder;
                OnPropertyChanged(nameof(WorkOrder));
                OnPropertyChanged(nameof(HasSecurityRisks));
                OnPropertyChanged(nameof(IsInProgress));
                OnPropertyChanged(nameof(OrderStatus));
                OnPropertyChanged(nameof(OrderStatusText));
                OnPropertyChanged(nameof(LatestWorkStartDate));
                OnPropertyChanged(nameof(LatestWorkEndDate));
                OnPropertyChanged(nameof(WorkStartedOnDate));
                OnPropertyChanged(nameof(WorkEndedOnDate));
            }
        }

        private async Task CancelWorkOrder(WorkOrder workOrder)
        {
            if (await _pageService.DisplayAlert("Warning", $"Are you sure you want to cancel this work order?", "Yes", "No"))
            {
                MessagingCenter.Send(this, EventConstants.WorkOrderCancelled);

                await _workOrdersRepository.Delete(workOrder);

                await _pageService.PopAsync();
            }
        }

        private async Task EditWorkOrder(WorkOrder workOrder)
        {
            var workOrderCopy = CloneHelper.DeepClone(workOrder);
            await _pageService.PushAsync(new WorkOrderFormPage(workOrderCopy));
        }

        private async Task OpenMap(WorkOrderDetailsViewModel workOrder)
        {
            var location = new Location(workOrder.WorkOrder.ClientAddress.Latitude, workOrder.WorkOrder.ClientAddress.Longitude);
            var options = new MapLaunchOptions { Name = workOrder.WorkOrder.ClientAddress.FullAddress };

            try
            {
                await Map.OpenAsync(location, options);
            }
            catch (Exception)
            {
                await _pageService.DisplayAlert("Error", "Something went wrong while trying to open map application.", "OK");
            }
        }

        private WorkOrderStatus GetStatus()
        {
            var now = DateTime.Now.Date;
            var workNotStarted = now < WorkOrder.WorkStartedOnDate;
            var workInProgress = now >= WorkOrder.WorkStartedOnDate && now <= WorkOrder.WorkEndedOnDate;

            var workPeriodStarted = WorkOrder.LatestWorkStartDate >= now;
            var workPeriodExpired = WorkOrder.LatestWorkEndDate < now;

            if (workNotStarted)
            {
                if (workPeriodExpired)
                {
                    return WorkOrderStatus.Expired;
                }

                if (workPeriodStarted)
                {
                    return WorkOrderStatus.NeedsWork;
                }

                return WorkOrderStatus.Available;
            }

            if (workInProgress)
            {
                if (workPeriodExpired)
                {
                    return WorkOrderStatus.Overdue;
                }

                return WorkOrderStatus.Started;
            }

            return WorkOrderStatus.Done;
        }
    }
}
