using System;
using System.Collections.Generic;
using DesafioAPI.Data;
using DesafioAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DesafioAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PopularController : ControllerBase
    {
        private readonly AppDbContext _database;

        public PopularController(AppDbContext database)
        {
            _database = database;
        }

        [HttpPost]
        public IActionResult Popular()
        {
            foreach (var client in _database.Clientes) {
                if (client.Nome == "Eduardo") {
                    return RedirectToAction("PopularExist");
                }
            }
            Cliente cliente1 = new Cliente { Nome = "Eduardo", Email = "eduardo@gft.com", Senha = "eduardo", DataCadastro = DateTime.Now, Documento = "110.110.110-01" };
            Cliente cliente2 = new Cliente { Nome = "José", Email = "jose@gft.com", Senha = "jose", DataCadastro = DateTime.Now, Documento = "111.111.111-11" };

            Fornecedor fornecedor1 = new Fornecedor { Nome = "Camilo", Cnpj = "0076/000179"};
            Fornecedor fornecedor2 = new Fornecedor { Nome = "Leite", Cnpj = "0043/000112" };
            Fornecedor fornecedor3 = new Fornecedor { Nome = "Malikoski", Cnpj = "0043/000156" };

            Produto produto1 = new Produto { Nome = "C#", Codigo = "121", Valor = 1000,
                                                Promocao = true, ValorPromocao = 550, Categoria = "Cursos", Imagem = "cursoc#.jpg", Quantidade = 5, Fornecedor = fornecedor1};

            Produto produto2 = new Produto { Nome = "Python", Codigo = "122", Valor = 1200,
                                                Promocao = true, ValorPromocao = 600, Categoria = "Cursos", Imagem = "cursopython.jpg", Quantidade = 10, Fornecedor = fornecedor2};

            Produto produto3 = new Produto { Nome = "ASP.NET Core", Codigo = "123", Valor = 1600,
                                                Promocao = false, ValorPromocao = 500, Categoria = "Cursos", Imagem = "cursoasp.jpg", Quantidade = 9, Fornecedor = fornecedor3};

            Produto produto4 = new Produto { Nome = "JavaScript", Codigo = "124", Valor = 1000,
                                                Promocao = true, ValorPromocao = 700, Categoria = "Cursos", Imagem = "cursojs.jpg", Quantidade = 7, Fornecedor = fornecedor1};

            Produto produto5 = new Produto { Nome = "Flutter", Codigo = "125", Valor = 740,
                                                Promocao = false, ValorPromocao = 600, Categoria = "Cursos", Imagem = "cursoflutter.jpg", Quantidade = 90, Fornecedor = fornecedor2};

            Produto produto6 = new Produto { Nome = "Azure", Codigo = "126", Valor = 1800,
                                                Promocao = true, ValorPromocao = 890, Categoria = "Cursos", Imagem = "cursoazure.jpg", Quantidade = 100, Fornecedor = fornecedor3};

            Produto produto7 = new Produto { Nome = "Ruby", Codigo = "127", Valor = 1000,
                                                Promocao = false, ValorPromocao = 800, Categoria = "Cursos", Imagem = "cursoruby.jpg", Quantidade = 45, Fornecedor = fornecedor2};
            
            Produto produto8 = new Produto { Nome = "C++", Codigo = "128", Valor = 3500,
                                                Promocao = true, ValorPromocao = 2300, Categoria = "Cursos", Imagem = "cursoc++.jpg", Quantidade = 12, Fornecedor = fornecedor1};

            Produto produto9 = new Produto { Nome = "C", Codigo = "129", Valor = 5000,
                                                Promocao = false, ValorPromocao = 3000, Categoria = "Cursos", Imagem = "cursoc.jpg", Quantidade = 1000, Fornecedor = fornecedor3};

            Produto produto10 = new Produto { Nome = "AWS", Codigo = "130", Valor = 4000,
                                                Promocao = true, ValorPromocao = 3000, Categoria = "Cursos", Imagem = "cursoaws.jpg", Quantidade = 160, Fornecedor = fornecedor2};

            Produto produto11 = new Produto { Nome = "Dart", Codigo = "131", Valor = 1800,
                                                Promocao = false, ValorPromocao = 1200, Categoria = "Cursos", Imagem = "cursodart.jpg", Quantidade = 600, Fornecedor = fornecedor1};


            List<Produto> listProdutos = new List<Produto>();
            listProdutos.Add(produto6);
            listProdutos.Add(produto5);
            listProdutos.Add(produto4);
            listProdutos.Add(produto3);
            listProdutos.Add(produto2);
            listProdutos.Add(produto1);

            DateTime date = DateTime.Now;
            date.ToString("dd/MM/yyyy");
            Venda venda1 = new Venda { Data = date, Cliente = cliente2, Produtos = listProdutos };

            double totalCompra;
            if (produto1.Promocao == true) {
                totalCompra = produto1.ValorPromocao * produto1.Quantidade;
            }
            else {
                totalCompra = produto1.Valor * produto1.Quantidade;
            }
            if (produto2.Promocao == true) {
                totalCompra += produto2.ValorPromocao * produto2.Quantidade;
            }
            else {
                totalCompra += produto2.Valor * produto2.Quantidade;
            }
            if (produto3.Promocao == true) {
                totalCompra += produto3.ValorPromocao * produto3.Quantidade;
            }
            else {
                totalCompra += produto3.Valor * produto3.Quantidade;
            }
            if (produto4.Promocao == true) {
                totalCompra += produto4.ValorPromocao * produto4.Quantidade;
            }
            else {
                totalCompra += produto4.Valor * produto4.Quantidade;
            }
            if (produto5.Promocao == true) {
                totalCompra += produto5.ValorPromocao * produto5.Quantidade;
            }
            else {
                totalCompra += produto5.Valor * produto5.Quantidade;
            }
            if (produto6.Promocao == true) {
                totalCompra += produto6.ValorPromocao * produto6.Quantidade;
            }
            else {
                totalCompra += produto6.Valor * produto6.Quantidade;
            }

            venda1.TotalCompra = totalCompra;


            List<Produto> listProdutos2 = new List<Produto>();
            listProdutos2.Add(produto7);
            listProdutos2.Add(produto8);
            listProdutos2.Add(produto9);

            Venda venda2 = new Venda { Data = date, Cliente = cliente1, Produtos = listProdutos2 };

            double totalCompra2;
            if (produto9.Promocao == true) {
                totalCompra2 = produto9.ValorPromocao * produto9.Quantidade;
            }
            else {
                totalCompra2 = produto9.Valor * produto9.Quantidade;
            }
            if (produto8.Promocao == true) {
                totalCompra2 += produto8.ValorPromocao * produto8.Quantidade;
            }
            else {
                totalCompra2 += produto8.Valor * produto8.Quantidade;
            }
            if (produto7.Promocao == true) {
                totalCompra2 += produto7.ValorPromocao * produto7.Quantidade;
            }
            else {
                totalCompra2 += produto7.Valor * produto7.Quantidade;
            }

            venda2.TotalCompra = totalCompra2;


            List<Produto> listProduto3 = new List<Produto>();
            listProduto3.Add(produto10);
            listProduto3.Add(produto11);

            Venda venda3 = new Venda { Data = date, Cliente = cliente2, Produtos = listProduto3 };

            double totalCompra3;
            if (produto10.Promocao == true) {
                totalCompra3 = produto10.ValorPromocao * produto10.Quantidade;
            }
            else {
                totalCompra3 = produto10.Valor * produto10.Quantidade;
            }
            if (produto11.Promocao == true) {
                totalCompra3 += produto11.ValorPromocao * produto11.Quantidade;
            }
            else {
                totalCompra3 += produto11.Valor * produto11.Quantidade;
            }

            venda3.TotalCompra = totalCompra3;
            
            _database.Add(cliente1);
            _database.Add(cliente2);
            _database.Add(fornecedor1);
            _database.Add(fornecedor2);
            _database.Add(fornecedor3);
            _database.Add(produto1);
            _database.Add(produto2);
            _database.Add(produto3);
            _database.Add(produto4);
            _database.Add(produto5);
            _database.Add(produto6);
            _database.Add(venda1);
            _database.Add(venda2);
            _database.Add(venda3);

            _database.SaveChanges();

            Response.StatusCode = 201;
            return new ObjectResult("Populado com sucesso");
        }

        [HttpGet("exist")]
        public IActionResult PopularExist()
        {
            Response.StatusCode = 400;
            return new ObjectResult("Você já populou o banco");
        }
    }
}