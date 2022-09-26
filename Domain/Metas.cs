using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    [Table("Metas")]
    public class Metas
    {
        [Key]
        public int ID { get; set; }
        public int Empresa { get; set; }
        public decimal? VendasDiarias { get; set; }
        public decimal? VendasMensais { get; set; }
    }
}
