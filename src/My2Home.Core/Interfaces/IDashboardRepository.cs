using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using entity = My2Home.Core.Entities;
namespace My2Home.Core.Interfaces
{
    public interface IDashboardRepository
    {
        Task<entity.DashboardViewModel> GetByHostelIdAsync(int hostelId);

    }
}
