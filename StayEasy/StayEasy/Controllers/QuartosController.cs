using StayEasy.Data;
using StayEasy.Models;
using StayEasy.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace StayEasy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuartosController : ControllerBase
    {
        private readonly StayEasyDbContext _context;

        public QuartosController(StayEasyDbContext context)
        {
            _context = context;
        }

        [HttpPost]

        public async Task<IActionResult> CriarQuarto(CriarQuartoDto dto)
        {
            var novoQuarto = new Quarto
            {
                HotelId = dto.HotelId,
                Tipo = dto.Tipo,
                Preco = dto.Preco,
            };

            _context.Quartos.Add(novoQuarto);
            await _context.SaveChangesAsync();

            return Ok("Novo Quarto Cadastrado");
        }

        [HttpGet]

        public async Task<ActionResult<List<QuartoDto>>> GetQuartos()
        {
            var quarto = await _context.Quartos.Include(a => a.Hotel).Select(a => new Quarto
            {
                Tipo = a.Tipo,
                Preco = a.Preco,
                HotelId=a.HotelId,

            })
             .ToListAsync();

            return Ok(quarto);
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<QuartoDto>> GetQuartoById(int id)
        {
            var quarto = await _context.Quartos.Include(q => q.Hotel)
                .Where(q => q.Id == id)
                .Select(q => new QuartoDto
                {
                    Id = id,
                    Preco = q.Preco,
                    Tipo = q.Tipo,
                    NomeHotel = q.Hotel.Nome
                }).FirstOrDefaultAsync();
            if (quarto == null)
            {
                return NotFound();
            }

            return Ok(quarto);
        }
    }
}
