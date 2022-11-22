using DataAccess;
using Domain;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlataformaBI.API.Services;
using System.Collections.Concurrent;

namespace PlataformaBI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class MetasController : GsoftController
    {
        private readonly GsoftDbContext _context;
        private readonly ConcurrentDictionary<string, Session> sessions;

        public MetasController(GsoftDbContext context, ConcurrentDictionary<string, Session> sessions)
            : base(sessions)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna as metas: diaria e mensal
        /// </summary>
        [HttpGet]
        public IActionResult Get()
        {
            if (!UserAuthenticated)
                return Unauthorized();

            Metas? meta = _context.metas.FirstOrDefault();

            if (meta == null)
            {
                return NoContent();
            }

            return Ok(meta);
        }


        /// <summary>
        /// Cadastra uma meta
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Metas _meta)
        {
            if (!UserAuthenticated)
                return Unauthorized();

            _meta.ID = 0;
            _meta.Empresa = Session.usuarioLogado.Empresa;

            if (!ValidaMeta(_meta))
                return BadRequest("Faltou informação");

            Metas meta = InsertAsync(_meta).Result;

            if (!ValidaMeta(meta))
            {
                return BadRequest("Meta já cadastrada. Aternativa: PUT");
            }

            return Ok(meta.ID);
        }

        /// <summary>
        /// Altera a meta cujo ID for passado (não passar null, passar zerado)
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Metas value)
        {
            if (!UserAuthenticated)
                return Unauthorized();

            if (id != value.ID)
                return BadRequest("Id passado na URL não compatível com o id passado no objeto");

            if (!ValidaMeta(value))
                return BadRequest("Faltou informação");

            var meta = await UpdateAsync(value);

            if (!ValidaMeta(meta))
                return BadRequest("Meta não encontrada");

            return Ok(meta.ID);
        }

        /// <summary>
        /// Exclui a meta cujo ID for passado
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!UserAuthenticated)
                return Unauthorized();

            var delete = await DeleteAsync(id);

            if (!delete)
                return BadRequest("Meta não encotrada");

            return Ok("Sucesso");
        }

        [NonAction]
        private bool ValidaMeta(Metas _meta)
        {
            if (
                _meta == null ||
                _meta.Empresa == 0 ||
                _meta.Vendas_Dia == null ||
                _meta.Vendas_Mes == null
            )
            {
                return false;
            }
            return true;
        }

        [NonAction]
        public async Task<bool> ExistsAsync(Metas value)
        {
            return await _context.metas.AnyAsync(x => x.ID == value.ID & x.Empresa == value.Empresa & x.Empresa == Session.usuarioLogado.Empresa);
        }

        [NonAction]
        public async Task<bool> ExistsAsync(int empresa)
        {
            return await _context.metas.AnyAsync(x => x.Empresa == empresa & x.Empresa == Session.usuarioLogado.Empresa);
        }

        [NonAction]
        public bool EmpresaAutorizada(int empresa)
        {
            return (Session.usuarioLogado.Empresa == empresa);
        }

        [NonAction]
        public async Task<Metas> InsertAsync(Metas value)
        {
            if (await ExistsAsync(value.Empresa))
                return new Metas();

            var entityEntry = await _context.metas.AddAsync(value);

            await _context.SaveChangesAsync();

            _context.ChangeTracker.Clear();

            return entityEntry.Entity;
        }

        [NonAction]
        public async Task<Metas> UpdateAsync(Metas value)
        {
            if (!await ExistsAsync(value))
                return new Metas();

            var entityEntry = _context.Update(value);

            await _context.SaveChangesAsync();

            _context.ChangeTracker.Clear();

            return entityEntry.Entity;
        }

        [NonAction]
        public async Task<bool> DeleteAsync(int value)
        {
            Metas? meta = _context.metas.FirstOrDefault(x => x.ID == value & x.Empresa == Session.usuarioLogado.Empresa);

            if (meta is null)
                return false;

            _context.metas.Remove(meta);

            await _context.SaveChangesAsync();

            _context.ChangeTracker.Clear();

            return true;
        }
    }
}
