using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WorkOrders.Persistence;
using WorkOrders.Services;

namespace WorkOrders.ViewModels
{
	public class BaseViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

        protected readonly IPageService _pageService;
        protected readonly IWorkOrdersRepository _workOrdersRepository;

        public BaseViewModel(IWorkOrdersRepository ordersRepository, IPageService pageService)
        {
            _pageService = pageService;
            _workOrdersRepository = ordersRepository;
		}

		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
