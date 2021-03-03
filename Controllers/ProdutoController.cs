using System.Linq;
using DesafioAPI.Data;
using DesafioAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DesafioAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class ProdutoController : ControllerBase
    {
        private readonly AppDbContext _database;

        public ProdutoController(AppDbContext database)
        {
            _database = database;
        }

        [HttpGet]
        public IActionResult ListarProduto()
        {
            try {
                var produtos = _database.Produtos.Include(x => x.Fornecedor).ToList();

                return Ok(produtos);
            }
            catch {
                Response.StatusCode = 400;
                return new ObjectResult("Não foi possível executar sua requisição");
            }
        }

        [HttpGet("{id}")]
        public IActionResult ListarProdutoId(int id)
        {
            try {
                var produtos = _database.Produtos.Include(p => p.Fornecedor).Where(x => x.Id == id).ToList();

                if (produtos.Count == 0) {
                    Response.StatusCode = 400;
                    return new ObjectResult("Id incorreto ou inexistente");
                }
                
                return Ok(produtos);
            }
            catch {
                Response.StatusCode = 400;
                return new ObjectResult("Id incorreto ou inexistente");
            }
        }

        [HttpGet("nome/{nome}")]
        public IActionResult ListarProdutoNome(string nome)
        {
            try {
                var produtos = _database.Produtos.Include(p => p.Fornecedor).Where(x => x.Nome == nome).ToList();

                return Ok(produtos);
            }
            catch {
                Response.StatusCode = 400;
                return new ObjectResult("Nome incorreto ou inexistente");
            }
        }

        [HttpGet("crescente")]
        public IActionResult ListarProdutoNomeCrescente()
        {
            try {
                var produtos = _database.Produtos.Include(p => p.Fornecedor).OrderBy(x => x.Nome).ToList();

                return Ok(produtos);
            }
            catch {
                Response.StatusCode = 400;
                return new ObjectResult("Nome incorreto ou inexistente");
            }
        }

        [HttpGet("decrescente")]
        public IActionResult ListarProdutoNomeDecrescente()
        {
            try {
                var produtos = _database.Produtos.Include(p => p.Fornecedor).OrderByDescending(x => x.Nome).ToList();

                return Ok(produtos);
            }
            catch {
                Response.StatusCode = 400;
                return new ObjectResult("Nome incorreto ou inexistente");
            }
        }

        [HttpPost]
        public IActionResult CriarProduto(Produto produto)
        {
            try {
                if (string.IsNullOrEmpty(produto.Nome)) {
                    Response.StatusCode = 400;
                    return new ObjectResult(new {msg = "Nome não pode ser nulo"});
                }
                if (string.IsNullOrEmpty(produto.Imagem)) {
                    Response.StatusCode = 400;
                    return new ObjectResult(new {msg = "Imagem não pode ser nulo"});
                }
                if (string.IsNullOrEmpty(produto.Categoria)) {
                    Response.StatusCode = 400;
                    return new ObjectResult(new {msg = "Categoria não pode ser nulo"});
                }
                if (string.IsNullOrEmpty(produto.Codigo)) {
                    Response.StatusCode = 400;
                    return new ObjectResult(new {msg = "Codigo não pode ser nulo"});
                }
                if (produto.Quantidade.Equals(0)) {
                    Response.StatusCode = 400;
                    return new ObjectResult(new {msg = "Quantidade não pode ser menor que 0"});
                }
                if (produto.Valor.Equals(0)) {
                    Response.StatusCode = 400;
                    return new ObjectResult(new {msg = "Valor não pode ser menor que 0"});
                }
                if (produto.ValorPromocao.Equals(0)) {
                    Response.StatusCode = 400;
                    return new ObjectResult(new {msg = "Valor da Promocao não pode ser menor que 0"});
                }
                if (produto.FornecedorId.Equals(0)) {
                    Response.StatusCode = 400;
                    return new ObjectResult(new {msg = "Id do Fornecedor não pode ser menor que 0"});
                }

                if (ModelState.IsValid) {
                    Fornecedor fornecedor = _database.Fornecedores.FirstOrDefault(x => x.Id == produto.FornecedorId);
                    produto.Fornecedor = fornecedor;

                    _database.Produtos.Add(produto);
                    _database.SaveChanges();

                    Response.StatusCode = 201;
                    return new ObjectResult("Você criou um Produto");
                }
                else {
                    return BadRequest(ModelState);
                }
            }
            catch {
                Response.StatusCode = 400;
                return new ObjectResult("Não foi possível executar sua requisição para criar um Produto");
            }
        }

        [HttpPut("{id}")]
        public IActionResult EditarProduto([FromBody] Produto produto, int id)
        {
            try {
                if (string.IsNullOrEmpty(produto.Nome)) {
                    Response.StatusCode = 400;
                    return new ObjectResult(new {msg = "Nome não pode ser nulo"});
                }
                if (string.IsNullOrEmpty(produto.Imagem)) {
                    Response.StatusCode = 400;
                    return new ObjectResult(new {msg = "Imagem não pode ser nulo"});
                }
                if (string.IsNullOrEmpty(produto.Categoria)) {
                    Response.StatusCode = 400;
                    return new ObjectResult(new {msg = "Categoria não pode ser nulo"});
                }
                if (string.IsNullOrEmpty(produto.Codigo)) {
                    Response.StatusCode = 400;
                    return new ObjectResult(new {msg = "Codigo não pode ser nulo"});
                }
                if (produto.Quantidade.Equals(0)) {
                    Response.StatusCode = 400;
                    return new ObjectResult(new {msg = "Quantidade não pode ser menor que 0"});
                }
                if (produto.Valor.Equals(0)) {
                    Response.StatusCode = 400;
                    return new ObjectResult(new {msg = "Valor não pode ser menor que 0"});
                }
                if (produto.ValorPromocao.Equals(0)) {
                    Response.StatusCode = 400;
                    return new ObjectResult(new {msg = "Valor da Promocao não pode ser menor que 0"});
                }
                if (produto.FornecedorId.Equals(0)) {
                    Response.StatusCode = 400;
                    return new ObjectResult(new {msg = "Id do Fornecedor não pode ser menor que 0"});
                }

                if (ModelState.IsValid) {
                    var prod = _database.Produtos.Include(x => x.Fornecedor).FirstOrDefault(x => x.Id == id);

                    prod.Nome = produto.Nome;
                    prod.Codigo = produto.Codigo;
                    prod.Valor = produto.Valor;
                    prod.Promocao = produto.Promocao;
                    prod.ValorPromocao = produto.ValorPromocao;
                    prod.Categoria = produto.Categoria;
                    prod.Imagem = produto.Imagem;
                    prod.Quantidade = produto.Quantidade;
                    prod.Fornecedor = _database.Fornecedores.FirstOrDefault(x => x.Id == produto.FornecedorId);

                    _database.SaveChanges();

                    Response.StatusCode = 200;
                    return new ObjectResult("Você editou o Produto de Id: " + id);
                }
                else {
                    return BadRequest(ModelState);
                }
            }
            catch {
                Response.StatusCode = 400;
                return new ObjectResult("Não foi possível executar sua requisição para editar um Produto");
            }
            
        }
        
        [HttpPatch("editardetalhe/{id}")]
        public IActionResult EditarDetalheProduto([FromBody] Produto produto, int id)
        {
            try {
                var prod = _database.Produtos.FirstOrDefault(x => x.Id == id);

                if (prod != null) {
                    prod.Nome = produto.Nome != null ? produto.Nome : prod.Nome;
                    prod.Imagem = produto.Imagem != null ? produto.Imagem : prod.Imagem;
                    prod.Categoria = produto.Categoria != null ? produto.Categoria : prod.Categoria;
                    prod.Codigo = produto.Codigo != null ? produto.Codigo : prod.Codigo;
                    prod.Quantidade = produto.Quantidade != 0 ? produto.Quantidade : prod.Quantidade;
                    prod.Promocao = produto.Promocao != false ? produto.Promocao : prod.Promocao;
                    prod.Valor = produto.Valor != 0 ? produto.Valor : prod.Valor;
                    prod.ValorPromocao = produto.ValorPromocao != 0 ? produto.ValorPromocao : prod.ValorPromocao;
                    prod.Fornecedor = produto.FornecedorId != 0 ? _database.Fornecedores.FirstOrDefault(x => x.Id == produto.FornecedorId) : _database.Fornecedores.FirstOrDefault(x => x.Id == prod.FornecedorId);

                    _database.SaveChanges();

                    Response.StatusCode = 200;
                    return new ObjectResult("Você editou o(s) detalhe(s) do Produto de Id: " + id);
                }
                else {
                    return new ObjectResult("Produto não encontrado");
                }
            }
            catch {
                return new ObjectResult("Produto não encontrado");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeletarProduto(int id)
        {
            try {
                var produto = _database.Produtos.Include(x => x.Fornecedor).FirstOrDefault(x => x.Id == id);

                _database.Produtos.Remove(produto);
                _database.SaveChanges();

                Response.StatusCode = 200;
                return new ObjectResult("Você deletou o Produto de Id: " + id);
            }
            catch {
                Response.StatusCode = 400;
                return new ObjectResult("Id incorreto ou inexistente");
            }
        }
    }
}