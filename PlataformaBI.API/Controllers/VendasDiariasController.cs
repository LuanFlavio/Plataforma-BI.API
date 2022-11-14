using DataAccess;
using Domain;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PlataformaBI.API.Services;
using PlataformaBI.API.Utils;
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
        public IActionResult Get([FromQuery] PageParams<VendasDiariasParam> pageParams)
        {
            if(!UserAuthenticated)
                return Unauthorized();

            var vendasDiarias = BuscarVendas(pageParams);

            if (vendasDiarias == null)
            {
                return NoContent();
            }

            Response.AddPagination(vendasDiarias.CurrentPage, vendasDiarias.PageSize, vendasDiarias.TotalCount, vendasDiarias.TatalPages);

            return Ok(vendasDiarias);
        }

        [NonAction]
        public PageList<VendasDiarias> BuscarVendas(PageParams<VendasDiariasParam> pageParams)
        {
            IEnumerable<VendasDiarias> vendasDiarias;

            if (pageParams.Termo != null)
            {
                //DATA FINAL = DateTime.Today.AddDays(-1);
                //DATA FINAL = DateTime.Today.AddDays(-1);

                if(pageParams.Termo.DataInicial == null & pageParams.Termo.DataFinal != null)
                {
                    pageParams.Termo.DataInicial = pageParams.Termo.DataFinal;
                }
                if(pageParams.Termo.DataInicial != null & pageParams.Termo.DataFinal == null)
                {
                    pageParams.Termo.DataFinal = pageParams.Termo.DataInicial;
                }

                vendasDiarias = _context
                    .vendasDiarias
                    .Where(p =>
                        (p.Empresa.Equals(Session.usuarioLogado.Empresa)) &&
                        (pageParams.Termo.Ano != null ? p.Ano.Equals(pageParams.Termo.Ano) : true) &&
                        (pageParams.Termo.MesDoAno != null ? p.MesDoAno.Equals(pageParams.Termo.MesDoAno) : true) &&
                        (pageParams.Termo.SemanaDoAno != null ? p.SemanaDoAno.Equals(pageParams.Termo.SemanaDoAno) : true) &&
                        (pageParams.Termo.DataInicial != null ? (p.Data >= pageParams.Termo.DataInicial && p.Data <= pageParams.Termo.DataFinal) : true) &&
                        (pageParams.Termo.ID != null ? p.ID.Equals(pageParams.Termo.ID) : true)
                    )
                    .ToArray();
            }
            else
            {
                vendasDiarias = _context
                        .vendasDiarias
                        .Where(p => p.Empresa.Equals(Session.usuarioLogado.Empresa))
                        .ToArray();
            }

            return PageList<VendasDiarias>.CreateAsync(vendasDiarias, pageParams.PageNumber, pageParams.PageSize);
        }
    }
}
