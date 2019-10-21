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
    public class UsuarioController : ControllerBase
    {
        GufosContext _contexto = new GufosContext();

        
        // GET: api/Usuario

        /// <summary>
        /// Listar os Usuarios
        /// </summary>
        /// <returns>Lista de Usuarios</returns>
        [HttpGet]
        public async Task<ActionResult<List<Usuario>>> Get()
        {
            var usuarios = await _contexto.Usuario.Include("TipoUsuario").Include("Presenca").ToListAsync();

            if(usuarios == null) {
                return NotFound();
            }

            return usuarios;
        }
        
        // GET: api/Usuario/2
        /// <summary>
        /// Chamar Usuario pelo ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ID do Usuario</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> Get(int id)
        {
            var usuario = await _contexto.Usuario.Include("TipoUsuario").Include("Presenca").FirstOrDefaultAsync(e => e.UsuarioId == id);

            if(usuario == null) {
                return NotFound();
            }

            return usuario;
        }

        // POST: api/Usuario

        /// <summary>
        /// Cadastrar Usuario
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Usuario>> Post(Usuario usuario)
        {
            try
            {
                // Tratamento contra SQL Injection
                await _contexto.AddAsync(usuario);
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            
            return usuario;
        }

        // PUT

        /// <summary>
        /// Editar Usuario Listado pelo ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, Usuario usuario)
        {
            if(id != usuario.UsuarioId){
                return BadRequest();
            }

            // Comparamos os atributos que foram modificados através do EF
            _contexto.Entry(usuario).State = EntityState.Modified;
            
            try {
                await _contexto.SaveChangesAsync(); 
            } catch (DbUpdateConcurrencyException) {
                // Verfica se o objeto inserido existe no banco
                var usuario_valido = await _contexto.Usuario.FindAsync(id);

                if(usuario_valido == null) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE api/usuario/id

        /// <summary>
        /// Deletar Usuario pelo ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Usuario>> Delete(int id){
            var usuario = await _contexto.Usuario.FindAsync(id);

            if(usuario == null) {
                return NotFound();
            }

            _contexto.Usuario.Remove(usuario);
            await _contexto.SaveChangesAsync();
            
            return usuario;
        }
    }
}