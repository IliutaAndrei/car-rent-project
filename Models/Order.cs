namespace CarRent.Models
{
    public class Order
    {


        public int OrderID { get; set; }
        
        public decimal Price { get; set; }

        public int CarID { get; set; }
        public Car? Car { get; set; }

        public DateTime OrderDate { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}
