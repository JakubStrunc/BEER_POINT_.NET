using System.ComponentModel.DataAnnotations;

namespace PNET_semestralka.Models
{
    public class SendingAddress
    {

        [Key]
        public int Id { get; set; }

        [StringLength(45)]
        public string Jmeno { get; set; }

        [StringLength(45)]
        public string Prijmeni { get; set; }

        [StringLength(45)]
        public string Ulice { get; set; }
        public int PopisneCislo { get; set; }
        public int OrientacniCislo { get; set; }
        public int Psc { get; set; }

        [StringLength(45)]
        public string Mesto { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
    }
}



