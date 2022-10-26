using DataAccess;
using Domain;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlataformaBI.API.Services;
using PlataformaBI.API.Servicos;
using PlataformaBI.API.Utils;
using System.Collections.Concurrent;

namespace PlataformaBI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class EmpresasController : GsoftController
    {
        private readonly GsoftDbContext _context;

        public EmpresasController(GsoftDbContext context, ConcurrentDictionary<string, Session> sessions)
            : base(sessions)
        {
           _context = context;
        }

        /// <summary>
        /// Retorna todos dados de todas empresas
        /// </summary>
        [HttpGet]
        public IActionResult Get([FromQuery]PageParams<string> pageParams)
        {
            if (!UserAuthenticated)
                return Unauthorized();

            IEnumerable<Empresas> query = _context.empresas.ToArray();

            if (query == null)
            {
                return NoContent();
            }

            var empresas = PageList<Empresas>.CreateAsync(query, pageParams.PageNumber, pageParams.PageSize);

            Response.AddPagination(empresas.CurrentPage, empresas.PageSize, empresas.TotalCount, empresas.TatalPages);
            
            return Ok(empresas);
        }
        
        /// <summary>
        /// Retorna os dados da empresa conforme o CNPJ do mesmo
        /// </summary>
        [HttpGet("{CNPJ}")]
        public IActionResult Get(string CNPJ)
        {
            if (!UserAuthenticated)
                return Unauthorized();

            Empresas empresa = _context.empresas.FirstOrDefault(p => p.CNPJ.Equals(Format.GetCNPJ(CNPJ)));

            if (empresa == null)
            {
                return NoContent();
            }

            return Ok(empresa);
        }

        /// <summary>
        /// Retorna os dados da empresa do usuário autenticado
        /// </summary>
        [HttpGet("Privada")]
        public IActionResult GetEmpresaPrivada()
        {
            if (!UserAuthenticated)
                return Unauthorized();

            Empresas empresa = _context.empresas.FirstOrDefault(p => p.ID.Equals(Session.usuarioLogado.Empresa)) ?? new Empresas();

            if (empresa == null || empresa.CNPJ == null)
            {
                return NoContent();
            }

            return Ok(empresa);
        }
    }
}
