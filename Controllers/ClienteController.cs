using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using DesafioAPI.Data;
using DesafioAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace DesafioAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly AppDbContext _database;

        public ClienteController(AppDbContext database)
        {
            _database = database;
        }

        [HttpPost("login/{id}")]
        public IActionResult Login([FromBody] Cliente credentials, int id)
        {
            try {
                Cliente cliente = _database.Clientes.FirstOrDefault(x => x.Id == id);

                if (cliente != null) {
                    var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("mysymmetrickeyjwtmorse2020"));
                    var credential = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256Signature);
                    
                    var claims = new List<Claim>();
                    claims.Add(new Claim("clienteid", cliente.Id.ToString()));
                    claims.Add(new Claim(ClaimTypes.Role, "admin"));
                    
                    var JWT = new JwtSecurityToken(
                        issuer: "morse",
                        expires: DateTime.Now.AddHours(1),
                        audience: "cliente_comum",
                        signingCredentials: credential,
                        claims: claims
                    );

                    return Ok(new JwtSecurityTokenHandler().WriteToken(JWT));

                }
                else {
                    Response.StatusCode = 401;
                    return new ObjectResult("Não autorizado");
                }
            }
            catch {
                Response.StatusCode = 401;
                return new ObjectResult("Não autorizado");
            }
        }

        [HttpGet]
        public IActionResult ListarCliente()
        {
            try {
                var cliente = _database.Clientes.ToList();

                return Ok(cliente);
            }
            catch {
                Response.StatusCode = 400;
                return new ObjectResult("Não foi possível executar sua requisição");
            }
        }

        [HttpGet("{id}")]
        public IActionResult ListarClienteId(int id)
        {
            try {
                var clientes = _database.Clientes.Where(x => x.Id == id).ToList();

                if (clientes.Count == 0) {
                    Response.StatusCode = 400;
                    return new ObjectResult("Id incorreto ou inexistente");
                }
                
                return Ok(clientes);
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                Response.StatusCode = 400;
                return new ObjectResult("Id incorreto ou inexistente");
            }
        }

        [HttpGet("nome/{nome}")]
        public IActionResult ListarClienteNome(string nome)
        {
            try {
                var clientes = _database.Clientes.Where(x => x.Nome == nome).ToList();

                return Ok(clientes);
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                Response.StatusCode = 400;
                return new ObjectResult("Nome do Cliente incorreto ou inexistente");
            }
        }

        [HttpGet("crescente")]
        public IActionResult ListarClienteCrescente()
        {
            try {
                var clientes = _database.Clientes.OrderBy(x => x.Nome).ToList();

                return Ok(clientes);
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                Response.StatusCode = 400;
                return new ObjectResult("Não foi possível executar sua requisição");
            }
        }
        
        [HttpGet("decrescente")]
        public IActionResult ListarClienteDecrescente()
        {
            try{
                var clientes = _database.Clientes.OrderByDescending(x => x.Nome).ToList();

                return Ok(clientes);
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                Response.StatusCode = 400;
                return new ObjectResult("Não foi possível executar sua requisição");
            }
        }

        [HttpPost]
        public IActionResult CriarCliente(Cliente client)
        {
            try {
                if (string.IsNullOrEmpty(client.Nome)) {
                    Response.StatusCode = 400;
                    return new ObjectResult(new {msg = "Nome não pode ser nulo"});
                }
                if (string.IsNullOrEmpty(client.Email)) {
                    Response.StatusCode = 400;
                    return new ObjectResult(new {msg = "Email não pode ser nulo"});
                }
                if (string.IsNullOrEmpty(client.Senha)) {
                    Response.StatusCode = 400;
                    return new ObjectResult(new {msg = "Senha não pode ser nulo"});
                }
                if (string.IsNullOrEmpty(client.Documento)) {
                    Response.StatusCode = 400;
                    return new ObjectResult(new {msg = "Documento não pode ser nulo"});
                }

                if (ModelState.IsValid) {
                    DateTime date = DateTime.Now;
                    date.ToString("dd/MM/yyyy");
                    client.DataCadastro = date;

                    _database.Clientes.Add(client);
                    _database.SaveChanges();

                    Response.StatusCode = 201;
                    return new ObjectResult("Você criou um Cliente");
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
        public IActionResult EditarCliente([FromBody] Cliente cliente, int id)
        {
            try {
                if (string.IsNullOrEmpty(cliente.Nome)) {
                    Response.StatusCode = 400;
                    return new ObjectResult(new {msg = "Nome não pode ser nulo"});
                }
                if (string.IsNullOrEmpty(cliente.Email)) {
                    Response.StatusCode = 400;
                    return new ObjectResult(new {msg = "Email não pode ser nulo"});
                }
                if (string.IsNullOrEmpty(cliente.Senha)) {
                    Response.StatusCode = 400;
                    return new ObjectResult(new {msg = "Senha não pode ser nulo"});
                }
                if (string.IsNullOrEmpty(cliente.Documento)) {
                    Response.StatusCode = 400;
                    return new ObjectResult(new {msg = "Documento não pode ser nulo"});
                }

                if (ModelState.IsValid) {
                    var clients = _database.Clientes.FirstOrDefault(x => x.Id == id);

                    clients.Nome = cliente.Nome;
                    clients.Email = cliente.Email;
                    clients.Senha = cliente.Senha;
                    clients.Documento = cliente.Documento;
                    clients.DataCadastro = DateTime.Now;

                    _database.SaveChanges();

                    Response.StatusCode = 200;
                    return new ObjectResult("Você editou o Cliente de Id: " + id);
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

        [HttpPatch("editardetalhe/{id}")]
        public IActionResult EditarDetalheCliente([FromBody] Cliente cliente, int id)
        {
            try {
                var client = _database.Clientes.FirstOrDefault(x => x.Id == id);

                if (client != null) {
                    client.Nome = cliente.Nome != null ? cliente.Nome : client.Nome;
                    client.Email = cliente.Email != null ? cliente.Email : client.Email;
                    client.Senha = cliente.Senha != null ? cliente.Senha : client.Senha;
                    client.Documento = cliente.Documento != null ? cliente.Documento : client.Documento;
                     DateTime date = new DateTime(0001, 01, 01);
                    client.DataCadastro = cliente.DataCadastro.Date != date ? cliente.DataCadastro : client.DataCadastro;

                    _database.SaveChanges();

                    Response.StatusCode = 200;
                    return new ObjectResult("Você editou o(s) detalhe(s) do Cliente de Id: " + id);
                }
                else {
                    return new ObjectResult("Cliente não encontrado");
                }
            }
            catch {
                return new ObjectResult("Cliente não encontrado");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeletarCliente(int id)
        {
            try {
                var cliente = _database.Clientes.FirstOrDefault(x => x.Id == id);

                _database.Clientes.Remove(cliente);
                _database.SaveChanges();

                Response.StatusCode = 200;
                return new ObjectResult("Você deletou o Cliente de Id: " + id);
            }
            catch {
                Response.StatusCode = 400;
                return new ObjectResult("Id incorreto ou inexistente");
            }
        }
    }
}