using backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using backend.Controllers;

// Adiciona a arvore de objetos 
// dotnet add package Microsoft.AspNetCore.Mvc.NewtonsoftJson


namespace backend.Controllers
{
    // Define a rota do controller, e diz que é um controller de API
    [Route("api/[controller]")] 
    [ApiController]
    public class TipoUsuarioController : ControllerBase
    {
        GufosContext _contexto = new GufosContext();

        // GET: api/TipoUsuario

        /// <summary>
        /// Listar Tipos de Usuario
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<TipoUsuario>>> Get()
        {
            var tipos = await _contexto.TipoUsuario.Include("Usuario").ToListAsync();

            if(tipos == null) {
                return NotFound();
            }

            return tipos;
        }
        
        // GET: api/TipoUsuario/2

        /// <summary>
        /// Chamar Tipo de Usuario pelo ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoUsuario>> Get(int id)
        {
            var tipo_usuario = await _contexto.TipoUsuario.Include("Usuario").FirstOrDefaultAsync(e => e.TipoUsuarioId == id);

            if(tipo_usuario == null) {
                return NotFound();
            }

            return tipo_usuario;
        }

        // POST: api/TipoUsuario

        /// <summary>
        /// Cadastrar Tipo de Usuario
        /// </summary>
        /// <param name="tipo_usuario"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<TipoUsuario>> Post(TipoUsuario tipo_usuario)
        {
            try
            {
                // Tratamento contra SQL Injection
                await _contexto.AddAsync(tipo_usuario);
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            
            return tipo_usuario;
        }

        // PUT

        /// <summary>
        /// Editar Tipo de Usuario 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tipo_usuario"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, TipoUsuario tipo_usuario)
        {
            if(id != tipo_usuario.TipoUsuarioId){
                return BadRequest();
            }

            // Comparamos os atributos que foram modificados através do EF
            _contexto.Entry(tipo_usuario).State = EntityState.Modified;
            
            try {
                await _contexto.SaveChangesAsync(); 
            } catch (DbUpdateConcurrencyException) {
                // Verfica se o objeto inserido existe no banco
                var tipo_usuario_valido = await _contexto.TipoUsuario.FindAsync(id);

                if(tipo_usuario_valido == null) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE api/tipo_usuario/id

        /// <summary>
        /// Deletar Tipo de Usuario
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<TipoUsuario>> Delete(int id){
            var tipo_usuario = await _contexto.TipoUsuario.FindAsync(id);

            if(tipo_usuario == null) {
                return NotFound();
            }

            _contexto.TipoUsuario.Remove(tipo_usuario);
            await _contexto.SaveChangesAsync();
            
            return tipo_usuario;
        }
    }
}