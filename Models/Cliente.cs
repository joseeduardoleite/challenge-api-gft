using System;
using System.ComponentModel.DataAnnotations;

namespace DesafioAPI.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Documento { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}