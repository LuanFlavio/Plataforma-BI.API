using DataAccess;
using Domain;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PlataformaBI.API.Services;
using PlataformaBI.API.Utils;
using System.Collections.Concurrent;
using System.Linq;

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

            var result = BuscarVendas(pageParams);

            if (result.resultado == null & result.mensagem == null)
            {
                return NoContent();
            }
            if(result.resultado == null)
            {
                return BadRequest(result.mensagem);
            }

            var vendasMensais = result.resultado;

            

            Response.AddPagination(vendasMensais.CurrentPage, vendasMensais.PageSize, vendasMensais.TotalCount, vendasMensais.TatalPages);

            return Ok(vendasMensais);
        }

        [NonAction]
        public Result<PageList<VendasMensais>> BuscarVendas(PageParams<VendasMensaisParam> pageParams)
        {
            Result<PageList<VendasMensais>> result = new()
            {
                resultado = null,
                mensagem = null
            };

            var isRange = false;

            IEnumerable<VendasMensais> vendasMensais;

            if (pageParams.Termo != null)
            {
                if (pageParams.Termo.DataInicial == null & pageParams.Termo.DataFinal != null)
                {
                    pageParams.Termo.DataInicial = pageParams.Termo.DataFinal;
                }
                if (pageParams.Termo.DataInicial != null & pageParams.Termo.DataFinal == null)
                {
                    pageParams.Termo.DataFinal = pageParams.Termo.DataInicial;
                }

                if(pageParams.Termo.DataInicial != pageParams.Termo.DataFinal)
                {
                    isRange = true;
                }

                if (isRange)
                {
                    vendasMensais = _context
                        .vendasMensal.AsEnumerable()
                        .Where(p =>
                            (p.Empresa.Equals(Session.usuarioLogado.Empresa)) &
                            (pageParams.Termo.Ano != null ? p.Ano.Equals(pageParams.Termo.Ano) : true) &
                            (pageParams.Termo.DataInicial != null ? 
                                (
                                    (
                                        new DateTime(
                                            p.Ano ?? 1,
                                            p.MesDoAno ?? 1,
                                            1
                                         ) >= pageParams.Termo.DataInicial
                                     ) &
                                    (
                                        new DateTime(
                                            p.Ano ?? 1,
                                            p.MesDoAno ?? 1,
                                            1
                                        )
                                    ) <= pageParams.Termo.DataFinal
                                 )
                            : true) &
                            (pageParams.Termo.ID != null ? p.ID.Equals(pageParams.Termo.ID) : true)
                        ).OrderBy(p => true ? p.ID : p.ID)
                        .ToArray();
                }
                else
                {
                    vendasMensais = _context
                        .vendasMensal
                        .Where(p =>
                            (p.Empresa.Equals(Session.usuarioLogado.Empresa)) &
                            (pageParams.Termo.Ano != null ? p.Ano.Equals(pageParams.Termo.Ano) : true) &
                            (pageParams.Termo.Mes != null ? (p.MesDoAno.Equals(pageParams.Termo.Mes)) : true) &
                            (pageParams.Termo.ID != null ? p.ID.Equals(pageParams.Termo.ID) : true)
                        );
                }

                if (pageParams.Termo.OrdemCrescente == false)
                {
                    vendasMensais.OrderByDescending(p => p.ID).ToArray();
                }
                else
                {
                    vendasMensais.OrderBy(p => p.ID).ToArray();
                }
            }
            else
            {
                vendasMensais = _context
                        .vendasMensal
                        .Where(p => p.Empresa.Equals(Session.usuarioLogado.Empresa))
                        .ToArray();
            }

            result.resultado = PageList<VendasMensais>.CreateAsync(vendasMensais, pageParams.PageNumber, pageParams.PageSize);

            return result;
        }
    }
}
