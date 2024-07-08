using Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public interface ISalesmanRepository
    {
        IEnumerable<SalesmanSales> GetSalesmanSales();
        IEnumerable<SalesmanCarSales> GetSalesmanCarSales();
        IEnumerable<CarModelCommissions> GetCarModelCommissions();
    }
}
