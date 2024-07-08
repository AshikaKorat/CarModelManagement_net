using Dapper;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class SalesmanRepository : ISalesmanRepository
    {
        private readonly IDbConnection _dbConnection;

        public SalesmanRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public IEnumerable<SalesmanSales> GetSalesmanSales()
        {
            return _dbConnection.Query<SalesmanSales>("SELECT * FROM SalesmanSales");
        }
        public IEnumerable<SalesmanCarSales> GetSalesmanCarSales()
        {
            return _dbConnection.Query<SalesmanCarSales>("SELECT * FROM SalesmanCarSales");
        }
        public IEnumerable<CarModelCommissions> GetCarModelCommissions()
        {
            return _dbConnection.Query<CarModelCommissions>("SELECT * FROM CarModelCommissions");
        }
    }
}
