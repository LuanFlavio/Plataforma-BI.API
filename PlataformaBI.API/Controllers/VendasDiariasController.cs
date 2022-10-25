using DataAccess;
using Domain;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PlataformaBI.API.Services;
using System.Collections.Concurrent;

namespace PlataformaBI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class VendasDiariasController : GsoftController
    {
        private readonly GsoftDbContext _context;
        //private readonly ConcurrentDictionary<string, Session> sessions;

        public VendasDiariasController(GsoftDbContext context, ConcurrentDictionary<string, Session> sessions)
            :base(sessions)
        {
            _context = context;
            //this.sessions = sessions;
        }

        /// <summary>
        /// Retorna os dados do vendasDiarias conforme o CNPJ da empresa
        /// </summary>
        [HttpGet]
        public IActionResult Get([FromQuery] PageParams<VendasDiarias> pageParams)
        {
            if(!UserAuthenticated)
                return Unauthorized();

            if (pageParams.Termo == null || pageParams.Termo.Empresa == 0)
            {
                return BadRequest("MissingParams");
            }

            var vendasDiarias = BuscarVendas(pageParams);

            if (vendasDiarias == null)
            {
                return NoContent();
            }

            return Ok(vendasDiarias);
        }

        [NonAction]
        public PageList<VendasDiarias> BuscarVendas(PageParams<VendasDiarias> pageParams)
        {

            if(pageParams.Termo.Data == null)
            {
                pageParams.Termo.Data = DateTime.Today.AddDays(-1);
            }

            //DATA FINAL = DateTime.Today.AddDays(-1);
            //DATA FINAL = DateTime.Today.AddDays(-1);

            IEnumerable<VendasDiarias> vendasDiarias = _context
                                        .vendasDiarias
                                        .Where(p => p.Empresa.Equals(pageParams.Termo.Empresa) && p.Data.Equals(pageParams.Termo.Data))
                                        .ToArray();

            return PageList<VendasDiarias>.CreateAsync(vendasDiarias, pageParams.PageNumber, pageParams.PageSize);
        }
    }
}
