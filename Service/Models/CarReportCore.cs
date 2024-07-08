namespace Service.Models
{
    public class SalesmanSales
    {
        public int Sr { get; set; }
        public string Salesman { get; set; }
        public string AdditionalDetails { get; set; }
        public decimal LastYearTotalSaleAmount { get; set; }
    }

    public class SalesmanCarSales
    {
        public string Salesman { get; set; }
        public string Class { get; set; }
        public int CarsSoldForAudi { get; set; }
        public int CarsSoldForJaguar { get; set; }
        public int CarsSoldForLandRover { get; set; }
        public int CarsSoldForRenault { get; set; }
    }

    public class CarModelCommissions
    {
        public string Brand { get; set; }
        public string FixedCommission { get; set; }
        public decimal PriceThreshold { get; set; }
        public decimal ClassACommission { get; set; }
        public decimal ClassBCommission { get; set; }
        public decimal ClassCCommission { get; set; }
    }

}
