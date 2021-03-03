using System;
using System.Globalization;
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
    public class VendaController : ControllerBase
    {
        private readonly AppDbContext _database;

        public VendaController(AppDbContext database)
        {
            _database = database;
        }

        [HttpGet]
        public IActionResult ListarVenda()
        {
            try {
                var x = _database.Vendas.Include(x => x.Produtos).Include(x => x.Cliente).FirstOrDefault();
                var venda = _database.Vendas.Include(x => x.Produtos).Include(x => x.Cliente).ToList();

                foreach (var item in venda) {
                    for (int i = 0; i < item.Produtos.Count; i++) {
                        item.Produtos[i].Fornecedor = _database.Fornecedores.FirstOrDefault(x => x.Id == item.Produtos[i].FornecedorId);
                    }
                    item.Data.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                }

                return Ok(venda);
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                Response.StatusCode = 400;
                return new ObjectResult("Não foi possível executar sua requisição");
            }
        }

        [HttpGet("{id}")]
        public IActionResult ListarVendaId(int id)
        {
            try {
                var venda = _database.Vendas.Include(x => x.Produtos).Include(x => x.Cliente).Where(x => x.Id == id).FirstOrDefault();

                foreach (var item in venda.Produtos) {
                    for (int i = 0; i < venda.Produtos.Count; i++) {
                        if (venda.Produtos[i].Id == item.Id) {
                            var produtos = _database.Produtos.Include(x => x.Fornecedor).FirstOrDefault(x => x.Id == item.Id);
                            item.Id++;
                            venda.Produtos[i] = produtos;
                        }
                        else {
                            item.Id++;
                            i--;
                        }
                    }
                    break;
                }

                return Ok(venda);
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                Response.StatusCode = 400;
                return new ObjectResult("Id incorreto ou inexistente");
            }
        }

        [HttpGet("nomecliente/{nome}")]
        public IActionResult ListarVendaNomeCliente(string nome)
        {
            try {
                var venda = _database.Vendas.Include(x => x.Produtos).Include(x => x.Cliente).Where(x => x.Cliente.Nome == nome).FirstOrDefault();

                foreach (var item in venda.Produtos) {
                    for (int i = 0; i < venda.Produtos.Count; i++) {
                        if (venda.Produtos[i].Id == item.Id) {
                            var produtos = _database.Produtos.Include(x => x.Fornecedor).FirstOrDefault(x => x.Id == item.Id);
                            item.Id++;
                            venda.Produtos[i] = produtos;
                        }
                        else {
                            item.Id++;
                            i--;
                        }
                    }
                    break;
                }

                return Ok(venda);
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                Response.StatusCode = 400;
                return new ObjectResult("Nome do Cliente incorreto ou inexistente");
            }
        }

        [HttpGet("datacrescente")]
        public IActionResult ListarVendaDataCrescente()
        {
            try {
                var venda = _database.Vendas.Include(x => x.Produtos).Include(x => x.Cliente).OrderBy(x => x.Data).ToList();

                foreach (var item in venda) {
                    for (int i = 0; i < item.Produtos.Count; i++) {
                        item.Produtos[i].Fornecedor = _database.Fornecedores.FirstOrDefault(x => x.Id == item.Produtos[i].FornecedorId);
                    }
                }

                return Ok(venda);
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                Response.StatusCode = 400;
                return new ObjectResult("Não foi possível executar sua requisição");
            }
        }

        [HttpGet("datadecrescente")]
        public IActionResult ListarVendaDataDecrescente()
        {
            try {
                var venda = _database.Vendas.Include(x => x.Produtos).Include(x => x.Cliente).OrderByDescending(x => x.Data).ToList();

                foreach (var item in venda) {
                    for (int i = 0; i < item.Produtos.Count; i++) {
                        item.Produtos[i].Fornecedor = _database.Fornecedores.FirstOrDefault(x => x.Id == item.Produtos[i].FornecedorId);
                    }
                }

                return Ok(venda);
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                Response.StatusCode = 400;
                return new ObjectResult("Não foi possível executar sua requisição");
            }
        }

        [HttpPost]
        public IActionResult CriarVenda(Venda venda)
        {
            try {
                if (venda.ClienteId.Equals(0)) {
                    Response.StatusCode = 400;
                    return new ObjectResult(new {msg = "Id do Cliente não pode ser menor que 0"});
                }
                if (venda.Produtos.Count == 0) {
                    Response.StatusCode = 400;
                    return new ObjectResult(new {msg = "Produtos são de preenchimento obrigatório (no mínimo 1)"});
                }
                DateTime date = new DateTime(0001, 01, 01);
                if (venda.Data.Date == date) {
                    Response.StatusCode = 400;
                    return new ObjectResult(new {msg = "Data é de preenchimento obrigatório"});
                }

                if (ModelState.IsValid) {
                    var client = _database.Clientes.FirstOrDefault(x => x.Id == venda.ClienteId);
                    venda.Cliente = client;
                    
                    venda.Data.ToString("dd/MM/yyyy");

                    double totalCompra = 0;
                    foreach (var item in venda.Produtos) {     
                        for (int i = 0; i < venda.Produtos.Count; i++) {
                            if (venda.Produtos[i].Id == item.Id) {
                                var produtos = _database.Produtos.Include(x => x.Fornecedor).FirstOrDefault(x => x.Id == item.Id);
                                item.Id++;
                                venda.Produtos[i] = produtos;
                                
                                if (venda.Produtos[i].Promocao == true) {
                                    totalCompra += venda.Produtos[i].ValorPromocao * venda.Produtos[i].Quantidade;
                                }
                                else {
                                    totalCompra += venda.Produtos[i].Valor * venda.Produtos[i].Quantidade;
                                }
                            }
                            else {
                                item.Id++;
                                i--;
                            }
                        }
                        break;
                    }

                    venda.TotalCompra = totalCompra;

                    _database.Vendas.Add(venda);
                    _database.SaveChanges();

                    Response.StatusCode = 201;
                    return new ObjectResult("Você criou uma Venda");
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
        public IActionResult EditarVenda([FromBody] Venda vendaRecebida, int id)
        {
            try {
                if (vendaRecebida.ClienteId.Equals(0)) {
                    Response.StatusCode = 400;
                    return new ObjectResult(new {msg = "Id do Cliente não pode ser menor que 0"});
                }
                if (vendaRecebida.Produtos.Count == 0) {
                    Response.StatusCode = 400;
                    return new ObjectResult(new {msg = "Produtos são de preenchimento obrigatório (no mínimo 1)"});
                }
                DateTime date = new DateTime(0001, 01, 01);
                if (vendaRecebida.Data == date) {
                    Response.StatusCode = 400;
                    return new ObjectResult(new {msg = "Data é de preenchimento obrigatório"});
                }

                if (ModelState.IsValid) {
                    var venda = _database.Vendas.Include(x => x.Produtos).Include(x => x.Cliente).FirstOrDefault(x => x.Id == id);

                    venda.Data = vendaRecebida.Data;
                    venda.Cliente = _database.Clientes.FirstOrDefault(x => x.Id == vendaRecebida.ClienteId);

                    if (venda.Produtos.Count == 0) {
                        venda.Produtos = vendaRecebida.Produtos;
                    }

                    foreach (var item in venda.Produtos) {
                        for (int i = 0; i < vendaRecebida.Produtos.Count; i++) {
                            if (venda.Produtos.Count < vendaRecebida.Produtos.Count){
                                venda.Produtos.Add(vendaRecebida.Produtos[i]);
                                i--;
                            }
                            else {
                                venda.Produtos[i] = _database.Produtos.FirstOrDefault(x => x.Id == vendaRecebida.Produtos[i].Id);
                            }
                        }
                        break;
                    }

                    foreach (var item in venda.Produtos) {
                        for (int i = 0; i < venda.Produtos.Count; i++) {
                            venda.Produtos[i].Fornecedor = _database.Fornecedores.FirstOrDefault(x => x.Id == venda.Produtos[i].FornecedorId);
                        }
                        break;
                    }

                    _database.SaveChanges();

                    Response.StatusCode = 200;
                    return new ObjectResult("Você editou a Venda de Id: " + id);
                }
                else {
                    return BadRequest(ModelState);
                }
            }
            catch {
                Response.StatusCode = 400;
                return new ObjectResult("Id incorreto ou inexistente");
            }
        }

        [HttpPatch("editardetalhe/{id}")]
        public IActionResult EditarDetalheVenda([FromBody] Venda venda, int id)
        {
            try {
                var vendaBanco = _database.Vendas.Include(x => x.Produtos).Include(x => x.Cliente).FirstOrDefault(x => x.Id == id);

                if (vendaBanco != null) {
                    DateTime date = new DateTime(0001, 01, 01);
                    vendaBanco.Data = venda.Data.Date != date ? venda.Data : vendaBanco.Data;
                    vendaBanco.Cliente = venda.ClienteId != 0 ? _database.Clientes.FirstOrDefault(x => x.Id == venda.ClienteId) : _database.Clientes.FirstOrDefault(x => x.Id == vendaBanco.ClienteId);

                    if (venda.Produtos != null) {
                        foreach (var item in venda.Produtos) {
                            for (int i = 0; i < venda.Produtos.Count; i++) {
                                if (vendaBanco.Produtos.Count < venda.Produtos.Count) {
                                    vendaBanco.Produtos.Add(venda.Produtos[i]);
                                    i--;
                                }
                                else {
                                    vendaBanco.Produtos[i] = venda.Produtos[i] != null ? _database.Produtos.FirstOrDefault(x => x.Id == venda.Produtos[i].Id) : _database.Produtos.FirstOrDefault(x => x.Id == vendaBanco.Produtos[i].Id);
                                }
                            }
                            break;
                        }

                        foreach (var item in vendaBanco.Produtos) {
                            for (int i = 0; i < vendaBanco.Produtos.Count; i++) {
                                venda.Produtos[i].Fornecedor = _database.Fornecedores.FirstOrDefault(x => x.Id == vendaBanco.Produtos[i].FornecedorId);
                            }
                            break;
                        }
                    }

                    foreach (var item in vendaBanco.Produtos) {
                        for (int i = 0; i < vendaBanco.Produtos.Count; i++) {
                            vendaBanco.Produtos[i].Fornecedor = _database.Fornecedores.FirstOrDefault(x => x.Id == vendaBanco.Produtos[i].FornecedorId);
                        }
                        break;
                    }

                    _database.SaveChanges();

                    Response.StatusCode = 200;
                    return new ObjectResult("Você editou o(s) detalhe(s) da Venda de Id: " + id);
                }
                else {
                    Response.StatusCode = 400;
                    return new ObjectResult("Venda não encontrada");
                }
            }
            catch {
                Response.StatusCode = 400;
                return new ObjectResult("Venda não encontrada");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeletarVenda(int id)
        {
            try {
                var venda = _database.Vendas.Include(x => x.Cliente).Include(x => x.Produtos).FirstOrDefault(x => x.Id == id);

                _database.Vendas.Remove(venda);
                _database.SaveChanges();

                Response.StatusCode = 200;
                return new ObjectResult("Você deletou a venda de Id: " + id);
            }
            catch {
                Response.StatusCode = 400;
                return new ObjectResult("Id incorreto ou inexistente");
            }
        }
    }
}