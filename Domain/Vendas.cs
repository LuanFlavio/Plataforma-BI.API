using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class Vendas
    {
        [Key]
        public int ID { get; set; }
        public int Empresa { get; set; }
        public int? MesDoAno { get; set; }
        public int? Ano { get; set; }
        public decimal? ValorTotal { get; set; }
        public decimal? QtdeVendas { get; set; }
        public decimal? QtdeItens { get; set; }
        public decimal? TicketMedio { get; set; }
        public decimal? QtdeMediaItens { get; set; }
        public decimal? ValorMedioItens { get; set; }
        public decimal? MetaProjecaoValor { get; set; }
        public decimal? MetaExecucaoPerc { get; set; }
    }
}
