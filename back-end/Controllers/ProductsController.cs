using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using back_end.Models; // Substitua 'back_end' pelo nome do seu namespace
using back_end.Data; // Substitua 'back_end' pelo nome do seu namespace

namespace back_end.Controllers // Substitua 'back_end' pelo nome do seu namespace
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly YourDbContext _context;

        public ProductsController(YourDbContext context)
        {
            _context = context;
        }

        // Endpoint para inserir múltiplos produtos
        [HttpPost("batchInsert")]
        public async Task<IActionResult> BatchInsert([FromBody] List<Product> products)
        {
            if (products == null || !products.Any())
            {
                return BadRequest("Nenhum produto fornecido.");
            }

            try
            {
                foreach (var product in products)
                {
                    // Adiciona o produto ao contexto
                    _context.Products.Add(product);
                }

                // Salva as mudanças no banco de dados
                await _context.SaveChangesAsync();
                return Ok("Produtos inseridos com sucesso.");
            }
            catch (Exception ex)
            {
                // Retorna um erro em caso de exceção
                return StatusCode(500, $"Erro ao inserir produtos no banco de dados: {ex.Message}");
            }
        }
    }
}
