using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    [Table("Empresas")]
    public class Empresas
    {
        [Key]
        public int ID { get; set; }
        public string? NomeFantasia { get;set; }
        public string? RazaoSocial { get;set; }
        public string CNPJ { get;set; }
        public string? InscricaoEstadual { get;set; }
        public string Email { get;set; }
        public string? Responsavel { get;set; }
        public string? Endereco { get;set; }
        public string? Bairro { get;set; }
        public string? Cidade { get;set; }
        public string? UF { get;set; }
        public string? CEP { get;set; }
        public string? Telefone { get;set; }
        public int? Matriz { get;set; }
        public bool? Ativo { get;set; }
    }
}