using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Domain
{
    [Table("Metas")]
    public class Metas
    {
        [Key]
        public int ID { get; set; }
        public int Empresa { get; set; }
        [JsonPropertyName("vendasDiarias")]
        public decimal? Vendas_Dia { get; set; }
        [JsonPropertyName("vendasMensais")]
        public decimal? Vendas_Mes { get; set; }
    }

    public class MetasParam
    {
        public int ID { get; set; }
        public decimal? vendasDiarias { get; set; }
        public decimal? vendasMensais { get; set; }
    }
}
