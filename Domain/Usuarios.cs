using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    [Table("Usuarios")]
    public class Usuarios
    {
        [Key]
        public int ID { get; set; }
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public string? Senha { get; set; }
        public string? Perfil { get; set; }
        public int Empresa { get; set; } //Pode ser inserido o obj da empresa
    }
    
    public class UsuariosLogin
    {
        public string? Email { get; set; }
        public string? Senha { get; set; }
    }
}
