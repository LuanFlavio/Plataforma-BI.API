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
        public int? MesDoAno { get; set; }
        public int? Ano { get; set; }
    }
}
