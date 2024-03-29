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
    public class EventoController : ControllerBase
    {
        GufosContext _contexto = new GufosContext();

        // GET: api/Evento

        /// <summary>
        /// Listar Eventos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<Evento>>> Get()
        {
            var eventos = await _contexto.Evento.Include("Categoria").Include("Localizacao").ToListAsync();

            if(eventos == null) {
                return NotFound();
            }

            return eventos;
        }
        
        // GET: api/Evento/2

        /// <summary>
        /// Chamar Evento pelo ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Evento>> Get(int id)
        {
            var evento = await _contexto.Evento.Include("Categoria").Include("Localizacao").FirstOrDefaultAsync(e => e.EventoId == id);

            if(evento == null) {
                return NotFound();
            }

            return evento;
        }

        // POST: api/Evento

        /// <summary>
        /// Cadastrar Evento
        /// </summary>
        /// <param name="evento"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Evento>> Post(Evento evento)
        {
            try
            {
                // Tratamento contra SQL Injection
                await _contexto.AddAsync(evento);
                await _contexto.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            
            return evento;
        }

        // PUT

        /// <summary>
        /// Editar Evento pelo ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="evento"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, Evento evento)
        {
            if(id != evento.EventoId){
                return BadRequest();
            }

            // Comparamos os atributos que foram modificados através do EF
            _contexto.Entry(evento).State = EntityState.Modified;
            
            try {
                await _contexto.SaveChangesAsync(); 
            } catch (DbUpdateConcurrencyException) {
                // Verfica se o objeto inserido existe no banco
                var evento_valido = await _contexto.Evento.FindAsync(id);

                if(evento_valido == null) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE api/evento/id

        /// <summary>
        /// Deletar Evento
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Evento>> Delete(int id){
            var evento = await _contexto.Evento.FindAsync(id);

            if(evento == null) {
                return NotFound();
            }

            _contexto.Evento.Remove(evento);
            await _contexto.SaveChangesAsync();
            
            return evento;
        }
    }
}