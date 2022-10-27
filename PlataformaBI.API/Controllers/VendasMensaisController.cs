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
    public class VendasMensaisController : GsoftController
    {
        private readonly GsoftDbContext _context;
        //private readonly ConcurrentDictionary<string, Session> sessions;

        public VendasMensaisController(GsoftDbContext context, ConcurrentDictionary<string, Session> sessions)
            : base(sessions)
        {
            _context = context;
            //this.sessions = sessions;
        }

        /// <summary>
        /// Retorna os dados do vendasMensais conforme o CNPJ da empresa
        /// </summary>
        [HttpGet]
        public IActionResult Get([FromQuery] PageParams<VendasMensaisParam> pageParams)
        {
            if (!UserAuthenticated)
                return Unauthorized();

            var vendasMensais = BuscarVendas(pageParams);

            if (vendasMensais == null)
            {
                return NoContent();
            }

            Response.AddPagination(vendasMensais.CurrentPage, vendasMensais.PageSize, vendasMensais.TotalCount, vendasMensais.TatalPages);

            return Ok(vendasMensais);
        }

        [NonAction]
        public PageList<VendasMensais> BuscarVendas(PageParams<VendasMensaisParam> pageParams)
        {
            IEnumerable<VendasMensais> vendasMensais;

            if (pageParams.Termo != null)
            {
                //DATA FINAL = DateTime.Today.AddDays(-1);
                //DATA FINAL = DateTime.Today.AddDays(-1);

                vendasMensais = _context
                    .vendasMensal
                    .Where(p =>
                        (p.Empresa.Equals(Session.usuarioLogado.Empresa)) &&
                        (pageParams.Termo.Ano != null ? p.Ano.Equals(pageParams.Termo.Ano) : true) &&
                        (pageParams.Termo.MesDoAno != null ? p.MesDoAno.Equals(pageParams.Termo.MesDoAno) : true) &&
                        (pageParams.Termo.ID != null ? p.ID.Equals(pageParams.Termo.ID) : true)
                    )
                    .ToArray();
            }
            else
            {
                vendasMensais = _context
                        .vendasMensal
                        .Where(p => p.Empresa.Equals(Session.usuarioLogado.Empresa))
                        .ToArray();
            }

            return PageList<VendasMensais>.CreateAsync(vendasMensais, pageParams.PageNumber, pageParams.PageSize);
        }
    }
}
