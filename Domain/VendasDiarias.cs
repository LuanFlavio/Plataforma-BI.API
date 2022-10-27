using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    [Table("VendasDiarias")]
    public class VendasDiarias : Vendas
    {
        public DateTime? Data { get; set; }
        public int? SemanaDoAno { get; set; }
    }

    public class VendasDiariasParam
    {
        public int? ID { get; set; }
        public DateTime? Data { get; set; }
        public int? SemanaDoAno { get; set; }
        public int? MesDoAno { get; set; }
        public int? Ano { get; set; }
    }
}
