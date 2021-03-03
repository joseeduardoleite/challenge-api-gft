using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DesafioAPI.Models
{
    public class Venda
    {
        public int Id { get; set; }
        public double TotalCompra { get; set; }
        public DateTime Data { get; set; }
        public List<Produto> Produtos { get; set; }
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
    }
}