using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class Email
    {
        [Required]
        [Key]
        public int Id { get; set; }
        public string smtp { get; set; }
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }
        public string senha { get; set; }
        public int porta { get; set; }
        public bool SSL { get; set; }
        public bool ativo { get; set; }
        public string corpoEmail { get; set; }
        public string tituloEmail { get; set; }
    }
}
