using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    [Table("VendasMensal")]
    public class VendasMensais : Vendas
    {
    }

    public class VendasMensaisParam
    {
        public int? ID { get; set; }
        public int? Mes { get; set; }
        public DateTime? DataInicial { get; set; }
        public DateTime? DataFinal { get; set; }
        public int? Ano { get; set; }
    }
}
