using PNET_semestralka.Models;

namespace PNET_semestralka.Models
{
    public record OrderItem
    {
  
        public int Pocet { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }
    }

}




