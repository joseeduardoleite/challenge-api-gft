using System;
using System.Linq;
using DesafioAPI.Data;
using DesafioAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DesafioAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class FornecedorController : ControllerBase
    {
        private readonly AppDbContext _database;

        public FornecedorController(AppDbContext database)
        {
            _database = database;
        }

        [HttpGet]
        public IActionResult ListarFornecedor()
        {
            try {
                var fornecedor = _database.Fornecedores.ToList();

                return Ok(fornecedor);
            }
            catch {
                Response.StatusCode = 400;
                return new ObjectResult("Não foi possível executar sua requisição");
            }
        }

        [HttpGet("{id}")]
        public IActionResult ListarFornecedorId(int id)
        {
            try {
                var fornecedor = _database.Fornecedores.Where(x => x.Id == id).ToList();

                if (fornecedor.Count == 0) {
                    Response.StatusCode = 400;
                    return new ObjectResult("Id incorreto ou inexistente");
                }
                
                return Ok(fornecedor);
            }
            catch {
                Response.StatusCode = 400;
                return new ObjectResult("Id incorreto ou inexistente");
            }
        }

        [HttpGet("nome/{nome}")]
        public IActionResult ListarFornecedorNome(string nome)
        {
            try {
                var fornecedor = _database.Fornecedores.Where(x => x.Nome == nome).ToList();

                return Ok(fornecedor);
            }
            catch {
                Response.StatusCode = 400;
                return new ObjectResult("Nome do Fornecedor incorreto ou inexistente");
            }
        }

        [HttpGet("crescente")]
        public IActionResult ListarFornecedorCrescente()
        {
            try {
                var fornecedor = _database.Fornecedores.OrderBy(x => x.Nome).ToList();

                return Ok(fornecedor);
            }
            catch {
                Response.StatusCode = 400;
                return new ObjectResult("Não foi possível executar sua requisição");
            }
        }

        [HttpGet("decrescente")]
        public IActionResult ListarFornecedorDecrescente()
        {
            try {
                var fornecedor = _database.Fornecedores.OrderByDescending(x => x.Nome).ToList();

                return Ok(fornecedor);
            }
            catch {
                Response.StatusCode = 400;
                return new ObjectResult("Não foi possível executar sua requisição");
            }
        }

        [HttpPost]
        public IActionResult CriarFornecedor(Fornecedor fornecedor)
        {
            try {
                if (string.IsNullOrEmpty(fornecedor.Nome)) {
                    Response.StatusCode = 400;
                    return new ObjectResult(new {msg = "Nome não pode ser nulo"});
                }
                if (string.IsNullOrEmpty(fornecedor.Cnpj)) {
                    Response.StatusCode = 400;
                    return new ObjectResult(new {msg = "Cnpj não pode ser nulo"});
                }

                if (ModelState.IsValid) {
                    _database.Fornecedores.Add(fornecedor);
                    _database.SaveChanges();

                    Response.StatusCode = 201;
                    return new ObjectResult("Você criou um Fornecedor");
                }
                else {
                    return BadRequest(ModelState);
                }
            }
            catch {
                Response.StatusCode = 400;
                return new ObjectResult("Não foi possível executar sua requisição");
            }
        }

        [HttpPut("{id}")]
        public IActionResult EditarFornecedor([FromBody] Fornecedor fornecedor, int id)
        {
            try
            {
                if (string.IsNullOrEmpty(fornecedor.Nome)) {
                    Response.StatusCode = 400;
                    return new ObjectResult(new {msg = "Nome não pode ser nulo"});
                }
                if (string.IsNullOrEmpty(fornecedor.Cnpj)) {
                    Response.StatusCode = 400;
                    return new ObjectResult(new {msg = "Cnpj não pode ser nulo"});
                }

                if (ModelState.IsValid) {
                    var fornecedores = _database.Fornecedores.FirstOrDefault(x => x.Id == id);

                    fornecedores.Nome = fornecedor.Nome;
                    fornecedores.Cnpj = fornecedor.Cnpj;

                    _database.SaveChanges();

                    Response.StatusCode = 200;
                    return new ObjectResult("Você editou o Fornecedor de Id: " + id);
                }
                else {
                    return BadRequest(ModelState);
                }
            }
            catch {
                Response.StatusCode = 400;
                return new ObjectResult("Não foi possível executar sua solicitação");
            }
        }

        [HttpPatch("editardetalhe/{id}")]
        public IActionResult EditarDetalheFornecedor([FromBody] Fornecedor fornecedor, int id)
        {
            try {
                var forn = _database.Fornecedores.FirstOrDefault(x => x.Id == id);

                if (forn != null) {
                    forn.Nome = fornecedor.Nome != null ? fornecedor.Nome : forn.Nome;
                    forn.Cnpj = fornecedor.Cnpj != null ? fornecedor.Cnpj : forn.Cnpj;

                    _database.SaveChanges();

                    Response.StatusCode = 200;
                    return new ObjectResult("Você editou o(s) detalhe(s) do Fornecedor de Id: " + id);
                }
                else {
                    return new ObjectResult("Fornecedor não encontrado");
                }
            }
            catch {
                return new ObjectResult("Fornecedor não encontrado");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeletarFornecedor(int id)
        {
            try {
                var fornecedor = _database.Fornecedores.FirstOrDefault(x => x.Id == id);

                _database.Fornecedores.Remove(fornecedor);
                _database.SaveChanges();
                
                Response.StatusCode = 200;
                return new ObjectResult("Você deletou o Fornecedor de Id: " + id);
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                return new ObjectResult("Id incorreto ou inexistente");
            }
        }
    }
}