using DataAccess;
using Domain;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace PlataformaBI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class VendasDiariasController : Controller
    {
        private readonly GsoftDbContext _context;

        public VendasDiariasController(GsoftDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna os dados do vendasDiarias conforme o CNPJ da empresa
        /// </summary>
        [HttpGet]
        public IActionResult Get([FromQuery] PageParams<VendasDiarias> pageParams)
        {
            var vendasDiarias = BuscarVendas(pageParams);

            if (vendasDiarias == null)
            {
                return NoContent();
            }

            return Ok(vendasDiarias);
        }

        [NonAction]
        public IActionResult BuscarVendas(PageParams<VendasDiarias> pageParams)
        {
            if(pageParams.Termo == null || pageParams.Termo.Empresa == 0)
            {
                return BadRequest("MissingParams");
            }

            IEnumerable<VendasDiarias> vendasDiarias = _context
                                        .vendasDiarias
                                        .Where(p => p.Empresa.Equals(pageParams.Termo.Empresa) && p.Data.Equals(pageParams.Termo.Data))
                                        .ToArray();

            return Ok(PageList<VendasDiarias>.CreateAsync(vendasDiarias, pageParams.PageNumber, pageParams.PageSize));
        }
    }
}
