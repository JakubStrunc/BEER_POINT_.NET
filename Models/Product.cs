using System.ComponentModel.DataAnnotations;

namespace PNET_semestralka.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [StringLength(45)]
        public string Nazev { get; set; }
        public int Cena { get; set; }
        public int Mnozstvi { get; set; }

        [StringLength(100)]
        public string Popis { get; set; }

        [StringLength(45)]
        public string Photo { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }


        public int SellerId { get; set; }
        public Seller Seller { get; set; }
    }

}



