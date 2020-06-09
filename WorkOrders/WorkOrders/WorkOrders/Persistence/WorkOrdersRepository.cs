using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SQLite;
using SQLiteNetExtensionsAsync.Extensions;
using WorkOrders.Models;
using WorkOrders.ViewModels;

namespace WorkOrders.Persistence
{
	public class WorkOrdersRepository : IWorkOrdersRepository
	{
        private readonly SQLiteAsyncConnection _connection;

        static WorkOrdersRepository()
        {
        }

        private WorkOrdersRepository()
        {
			var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Nova.db3");
            _connection = new SQLiteAsyncConnection(path);
			_connection.CreateTableAsync<WorkOrder>();
            _connection.CreateTableAsync<Address>();
            _connection.CreateTableAsync<ClientInformation>();
		}

        public static WorkOrdersRepository Instance { get; } = new WorkOrdersRepository();

        public async Task<IEnumerable<WorkOrder>> GetList()
        {
            return await _connection.GetAllWithChildrenAsync<WorkOrder>();
        }

		public async Task Add(WorkOrder workOrder)
		{
			await _connection.InsertWithChildrenAsync(workOrder);
		}

		public async Task Update(WorkOrder workOrder)
		{
			await _connection.UpdateWithChildrenAsync(workOrder);
		}

		public async Task<WorkOrder> Get(int id)
		{
			return await _connection.GetWithChildrenAsync<WorkOrder>(id);
		}

        public async Task Delete(WorkOrder workOrder)
        {
            await _connection.DeleteAsync(workOrder, true);
        }
	}
}
