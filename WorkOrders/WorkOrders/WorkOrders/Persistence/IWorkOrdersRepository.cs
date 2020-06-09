using System.Collections.Generic;
using System.Threading.Tasks;
using WorkOrders.Models;

namespace WorkOrders.Persistence
{
	public interface IWorkOrdersRepository
	{
		Task<IEnumerable<WorkOrder>> GetList();
		Task<WorkOrder> Get(int id);
		Task Add(WorkOrder workOrder);
		Task Update(WorkOrder workOrder);
        Task Delete(WorkOrder workOrder);
	}
}
