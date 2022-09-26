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
        private readonly ConcurrentDictionary<string, Session> sessions;

        public EmpresasController(GsoftDbContext context, ConcurrentDictionary<string, Session> sessions)
            : base(sessions)
        {
           _context = context;
           this.sessions = sessions;
        }

        /// <summary>
        /// Retorna todos dados de todas empresas
        /// </summary>
        [HttpGet]
        public IActionResult Get()
        {
            if (!this.UserAuthenticated)
                return Unauthorized();

            IEnumerable<Empresas> empresas = _context.empresas.ToArray();

            if (empresas == null)
            {
                return NoContent();
            }

            return Ok(empresas);
        }
        
        /// <summary>
        /// Retorna os dados do empresas conforme o CNPJ do mesmo
        /// </summary>
        [HttpGet("{CNPJ}")]
        public IActionResult Get(string CNPJ)
        {
            Empresas empresas = _context
                        .empresas
                        .FirstOrDefault(p =>
                            p.CNPJ.Equals(Format.GetCNPJ(CNPJ))
                         );

            if (empresas == null)
            {
                return NoContent();
            }

            return Ok(empresas);
        }
/*
        [HttpGet("Email/{CNPJ}")]
        public IActionResult EnviarEmail(string CNPJ)
        {
            Empresas empresas = _context.empresas.FirstOrDefault(p => p.CNPJ.Equals(GetCNPJ(CNPJ)));


            if (empresas == null)
            {
                return NoContent();
            }
            empresas.Senha = CriptoSenha.MD5Senha(empresas.CNPJ);
            Email email = _context.email.FirstOrDefault(p => p.ativo);

            EnvioEmail envioEmail = new EnvioEmail(email);
            var a = envioEmail.enviarEmail(empresas);
            return Ok(empresas);
        }

        [HttpPost]
        public IActionResult Login(string cnpj, string senha)
        {
            Empresas empresas = _context.empresas.FirstOrDefault(p => p.CNPJ == GetCNPJ(cnpj));
            if (empresas == null || senha == null)
            {
                return NoContent();
            }

            var Senha = CriptoSenha.MD5Senha(empresas.CNPJ);
            if (empresas.Senha.Equals(senha.ToUpper()))
            {
                return Ok(empresas);
            }
            else
            {
                return NoContent();
            }

        }
*/

        [HttpPost("Login")]
        public async Task<IActionResult> Login(string email, string senha)
        {
            if (email == null || senha == null)
                return BadRequest();

            //senha = CriptoSenha.MD5Senha(senha);
            Usuarios usuarioLogado = await _context.usuarios.FirstOrDefaultAsync(p => p.Email.Equals(email) && p.Senha.Equals(senha));
            
            if (usuarioLogado == null)
                return BadRequest();

            var sessionExists = this.sessions.Values.FirstOrDefault(x => x.usuarioLogado.ID == usuarioLogado.ID);

            if (sessionExists is null)
            {
                sessionExists = new Session(this.sessions, usuarioLogado);
            }
            else
            {
                sessionExists.UpdateLastRequest();
            }

            HttpContext.Response.Headers.Add("gsoft-wd-token", sessionExists.Token);

            usuarioLogado.Senha = "";

            return Ok(usuarioLogado);
        }


    }
}
