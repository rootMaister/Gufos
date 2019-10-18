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
    public class LocalizacaoController : ControllerBase
    {
        GufosContext _contexto = new GufosContext();

        // GET: api/Localizacao
        [HttpGet]
        public async Task<ActionResult<List<Localizacao>>> Get()
        {
            var localizacao = await _contexto.Localizacao.ToListAsync();

            if( localizacao == null )
            {
                return NotFound();
            }
            return localizacao;
        }

        // GET: api/Localizacao/2
        [HttpGet("{id}")]
        public async Task<ActionResult<Localizacao>> Get(int id)
        {
            // FindAsync = procura algo especifico no banco 
            var localizacao = await _contexto.Localizacao.FindAsync(id);

            if( localizacao == null )
            {
                return NotFound();
            }
            return localizacao;
        }

        // POST: api/Localizacao

        [HttpPost]
        public async Task<ActionResult<Localizacao>> Post(Localizacao localizacao)
        {

            try{
                // Tratamos contra ataques de SQL Injection
                await _contexto.AddAsync(localizacao);

                // Salvamos efetivamente o nosso objeto no banco de dados
                await _contexto.SaveChangesAsync();
            }catch(DbUpdateConcurrencyException)
            {
                throw;
            }

            return localizacao;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put( int id, Localizacao localizacao)
        {

            // Se o Id do objeto não existir 
            // ele retorna erro 400 (bad request)
            if(id != localizacao.LocalizacaoId)
            {
                return BadRequest();
            }
            
            // Comparamos os atributos que foram modificados através do EF
            _contexto.Entry(localizacao).State = EntityState.Modified;
           
            try{
                await _contexto.SaveChangesAsync();
            }catch(DbUpdateConcurrencyException){
                
                // Verificamos se o objeto inserido realmente existe no banco
                var localizacao_valido = await _contexto.Localizacao.FindAsync(id);

                if(localizacao_valido == null)
                {
                    return NotFound();
                }else{
                    throw;
                }
            }

            // NoContent = Retorna 204, sem nada
            return NoContent();
        }

        //  DELETE api/localizacao/id
        [HttpDelete("{id}")]
        public async Task<ActionResult<Localizacao>> Delete(int id)
        {
            var localizacao = await _contexto.Localizacao.FindAsync(id);

            if(localizacao == null)
            {
                return NotFound();
            }

            _contexto.Localizacao.Remove(localizacao);
            await _contexto.SaveChangesAsync();

            return localizacao;   
        }
    }
}