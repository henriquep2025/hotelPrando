using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StayEasy.Models.DTOs;
using StayEasy.Data;

namespace HotelStayEasy.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoteisController : ControllerBase
    {
        private readonly StayEasyDbContext _context;

        public HoteisController(StayEasyDbContext context)
        {
            _context = context;
        }

        [HttpPost]

        public async Task<ActionResult> CriarHotel(CriarHotelDto dto)
        {
            var novoHotel = new Hotel
            {
                Nome = dto.Nome,
                Cidade = dto.Cidade,
                QtdEstrelas = dto.QtdEstrelas,
            };
            _context.Add(novoHotel);
            await _context.SaveChangesAsync();
            return Ok(novoHotel);
        }

        [HttpGet]

        public async Task<ActionResult<List<HotelDto>>> GetHotel()
        {
            var hotels = await _context.Hoteis.Select(p => new HotelDto
            {
                Id = p.Id,
                Nome = p.Nome,
                QtdEstrelas = p.QtdEstrelas,
            }).ToListAsync();
            return Ok(hotels);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DetalhesHotelDto>> GetHotelById(int id)
        {
            var hotel = await _context.Hoteis
                                .Include(h => h.Quartos)
                                .Where(h => h.Id == id)
                                .Select(h => new DetalhesHotelDto
                                {
                                    Id = h.Id,
                                    Cidade = h.Cidade,
                                    QtdEstrelas = h.QtdEstrelas,
                                    Nome = h.Nome,
                                    Quartos = h.Quartos.Select(q => new QuartoDto
                                    {
                                        Id = q.Id,
                                        Tipo = q.Tipo,
                                        Preco = q.Preco,
                                    }).ToList()

                                }).FirstOrDefaultAsync();
            if (hotel == null)
            {
                return NotFound();
            }

            return Ok(hotel);
        }
    }
}