using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Site.Enums;

namespace Site.Entidade
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public DateTime DataCadastro { get; set; }
        public Status Status { get; set; }
        public int Nivel { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public Usuario UsuarioCadastro { get; set; }
        public Usuario UsuarioAlteracao { get; set; }
    }
}
