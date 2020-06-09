using System;
using System.Threading.Tasks;
using System.Windows.Input;
using WorkOrders.Helpers;
using WorkOrders.Models;
using WorkOrders.Persistence;
using WorkOrders.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WorkOrders.ViewModels
{
	public class WorkOrderFormViewModel : BaseViewModel
	{
		public WorkOrder WorkOrder { get; set; }

		public ICommand SaveCommand { get; private set; }

        private bool _isSaveExecuting;
        public bool IsSaveExecuting
		{
            get => _isSaveExecuting;
            set
            {
                _isSaveExecuting = value;
                ((Command)SaveCommand).ChangeCanExecute();
            }
        }

        public DateTime LatestWorkStartDate
        {
            get => WorkOrder.LatestWorkStartDate;
            set
            {
                if (value > WorkOrder.LatestWorkEndDate || value == WorkOrder.LatestWorkStartDate)
                {
                    OnPropertyChanged();
					return;
                }

                WorkOrder.LatestWorkStartDate = value;
				OnPropertyChanged();
            }
        }

        public DateTime LatestWorkEndDate
        {
            get => WorkOrder.LatestWorkEndDate;
            set
            {
                if (value < WorkOrder.LatestWorkStartDate || value == WorkOrder.LatestWorkEndDate)
                {
                    OnPropertyChanged();
					return;
				}

                WorkOrder.LatestWorkEndDate = value;
                OnPropertyChanged();
			}
        }

        public DateTime WorkStartedOnDate
        {
            get => WorkOrder.WorkStartedOnDate;
            set
            {
                if (value > WorkOrder.WorkEndedOnDate || value == WorkOrder.WorkStartedOnDate)
                {
                    OnPropertyChanged();
                    return;
                }

                WorkOrder.WorkStartedOnDate = value;
                OnPropertyChanged();
            }
        }

        public DateTime WorkEndedOnDate
        {
            get => WorkOrder.WorkEndedOnDate;
            set
            {
                if (value < WorkOrder.WorkStartedOnDate || value == WorkOrder.WorkEndedOnDate)
                {
                    OnPropertyChanged();
                    return;
                }

                WorkOrder.WorkEndedOnDate = value;
                OnPropertyChanged();
            }
        }

        public WorkOrderFormViewModel(WorkOrder order, IWorkOrdersRepository ordersRepository, IPageService pageService)
		: base(ordersRepository, pageService)
		{
            WorkOrder = order ?? throw new ArgumentNullException(nameof(order));

            SaveCommand = new Command(async () => await Save(), () => !IsSaveExecuting);
		}

		async Task Save()
		{
			IsSaveExecuting = true;

			if (string.IsNullOrWhiteSpace(WorkOrder.Title) || string.IsNullOrWhiteSpace(WorkOrder.Description) ||
				string.IsNullOrWhiteSpace(WorkOrder.ShortDescription))
			{
				await _pageService.DisplayAlert("Error", "Please enter work order information.", "OK");
                IsSaveExecuting = false;
                return;
			}

			if (WorkOrder.ShortDescription.Length > 50)
			{
				await _pageService.DisplayAlert("Error", "Short description must have les than 50 characters.", "OK");
                IsSaveExecuting = false;
                return;
			}

			if (string.IsNullOrWhiteSpace(WorkOrder.ClientAddress.FullAddress))
			{
				await _pageService.DisplayAlert("Error", "Please enter address information.", "OK");
                IsSaveExecuting = false;
                return;
			}

			try
			{
				await WorkOrder.ClientAddress.SetCoordinatesBasedOnAddress();
			}
			catch (Exception)
			{
				await _pageService.DisplayAlert("Error", "Location coordinates could not be found based on address you entered.", "OK");
			}

			if (WorkOrder.Id == 0)
			{
				await _workOrdersRepository.Add(WorkOrder);

				MessagingCenter.Send(this, EventConstants.WorkOrderAdded);
			}
			else
			{
				await _workOrdersRepository.Update(WorkOrder);

				MessagingCenter.Send(this, EventConstants.WorkOrderUpdated);
			}

			await _pageService.PopAsync();
            IsSaveExecuting = false;
        }
	}
}
