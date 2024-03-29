using Microsoft.AspNetCore.Mvc;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace backend.Controllers
{   
    // Definimos nossa rota de controller e dizemos que é o um controller de API 
    [Route("api/[controller]")]
    [ApiController] 
    public class CategoriaController : ControllerBase
    {
        GufosContext _contexto = new GufosContext();

        // GET: api/Categoria

        /// <summary>
        /// Lista de Categorias
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<Categoria>>> Get()
        {
            var categorias = await _contexto.Categoria.ToListAsync();

            if( categorias == null )
            {
                return NotFound();
            }
            return categorias;
        }

        // GET: api/Categoria/2

        /// <summary>
        /// Chamar Categoria pelo ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Categoria>> Get(int id)
        {
            // FindAsync = procura algo especifico no banco 
            var categoria = await _contexto.Categoria.FindAsync(id);

            if( categoria == null )
            {
                return NotFound();
            }
            return categoria;
        }

        // POST: api/Categoria

        /// <summary>
        /// Cadastrar Nova Categoria
        /// </summary>
        /// <param name="categoria"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Categoria>> Post(Categoria categoria)
        {

            try{
                // Tratamos contra ataques de SQL Injection
                await _contexto.AddAsync(categoria);

                // Salvamos efetivamente o nosso objeto no banco de dados
                await _contexto.SaveChangesAsync();
            }catch(DbUpdateConcurrencyException)
            {
                throw;
            }

            return categoria;
        }

        /// <summary>
        /// Editar Categorias Listadas
        /// </summary>
        /// <param name="id"></param>
        /// <param name="categoria"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> Put( int id, Categoria categoria)
        {

            // Se o Id do objeto não existir 
            // ele retorna erro 400 (bad request)
            if(id != categoria.CategoriaId)
            {
                return BadRequest();
            }
            
            // Comparamos os atributos que foram modificados através do EF
            _contexto.Entry(categoria).State = EntityState.Modified;
           
            try{
                await _contexto.SaveChangesAsync();
            }catch(DbUpdateConcurrencyException){
                
                // Verificamos se o objeto inserido realmente existe no banco
                var categoria_valido = await _contexto.Categoria.FindAsync(id);

                if(categoria_valido == null)
                {
                    return NotFound();
                }else{
                    throw;
                }
            }

            // NoContent = Retorna 204, sem nada
            return NoContent();
        }

        //  DELETE api/categoria/id

        /// <summary>
        /// Deletar Categoria pelo ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<Categoria>> Delete(int id)
        {
            var categoria = await _contexto.Categoria.FindAsync(id);

            if(categoria == null)
            {
                return NotFound();
            }

            _contexto.Categoria.Remove(categoria);
            await _contexto.SaveChangesAsync();

            return categoria;   
        }
    }
}