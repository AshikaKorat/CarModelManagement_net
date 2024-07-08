using CarModelManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Service.Services;

namespace CarModelManagement.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class SalesmanReportController : ControllerBase
    {
        private readonly ISalesmanRepository _salesmanService;

        public SalesmanReportController(ISalesmanRepository salesmanService)
        {
            _salesmanService = salesmanService;
        }
        [HttpGet("GetSalesmanCommission")]
        public ActionResult<IEnumerable<CommissionReport>> GetSalesmanCommission()
        {
            var sales =  _salesmanService.GetSalesmanSales();
            var carSales =  _salesmanService.GetSalesmanCarSales();
            var commissions =  _salesmanService.GetCarModelCommissions();

            var commissionReports = new List<CommissionReport>();

            // Group sales by salesman
            var salesGroupedBySalesman = sales.GroupBy(s => s.Salesman);

            foreach (var salesmanGroup in salesGroupedBySalesman)
            {
                var salesmanName = salesmanGroup.Key + "( " + salesmanGroup.FirstOrDefault().AdditionalDetails + " )";
                var salesmanSales = salesmanGroup.ToList();

                // Calculate total sales amount for the salesman
                var totalSalesAmount = salesmanSales.Sum(s => s.LastYearTotalSaleAmount);

                // Fetch car sales for the current salesman
                var salesmanCarSales = carSales.ToList();

                // Calculate commission based on rules
                decimal totalCommission = 0;

                foreach (var carSale in salesmanCarSales)
                {
                    foreach (var commission in commissions)
                    {
                        int carsSold = 0;
                        switch (commission.Brand)
                        {
                            case "Audi":
                                carsSold = carSale.CarsSoldForAudi;
                                break;
                            case "Jaguar":
                                carsSold = carSale.CarsSoldForJaguar;
                                break;
                            case "Land Rover":
                                carsSold = carSale.CarsSoldForLandRover;
                                break;
                            case "Renault":
                                carsSold = carSale.CarsSoldForRenault;
                                break;
                        }

                        if (carsSold > 0)
                        {
                            // Add fixed commission
                            totalCommission += carsSold * decimal.Parse(commission.FixedCommission.TrimStart('$'));

                            // Add class-wise commission
                            decimal classCommission = 0;
                            switch (carSale.Class)
                            {
                                case "A":
                                    classCommission = commission.ClassACommission;
                                    break;
                                case "B":
                                    classCommission = commission.ClassBCommission;
                                    break;
                                case "C":
                                    classCommission = commission.ClassCCommission;
                                    break;
                            }

                            totalCommission += (classCommission / 100) * commission.PriceThreshold * carsSold;

                            // Additional 2% commission if previous year's sales exceed $500,000 (applicable only for Class A)
                            if (carSale.Class == "A" && totalSalesAmount > 500000)
                            {
                                totalCommission += 0.02m * totalSalesAmount;
                            }
                        }
                    }
                }

                // Create a CommissionReport object and add it to the list
                var commissionReport = new CommissionReport
                {
                    Salesman = salesmanName,
                    TotalSalesAmount = totalSalesAmount,
                    TotalCommission = totalCommission
                };

                commissionReports.Add(commissionReport);
            }
            return new JsonResult(commissionReports);
        }
    }
}
