using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimicAPI.Database;
using MimicAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicAPI.Controllers
{
    [Route("api/palavras")]
    public class PalavrasController : ControllerBase
    {

        private readonly MimicContext _banco;
        public PalavrasController(MimicContext banco)
        {
            _banco = banco;

        }
        //app -- api/palavras (mostra a lista de palavras)
        [Route("")]
        [HttpGet]
        public ActionResult ObterTodas()
        {
            return Ok(_banco.Palavras);
        }
        //recuperar uma palavra -- api/palavras/2 (mostra)
        [Route("{id}")]
        [HttpGet]
        public ActionResult Obter(int id)
        {
            var obj = _banco.Palavras.Find(id);
            if (obj == null)
                return NotFound(); //muda o retorno pra 404 ( erro )

            return Ok();
        }
        //cadastro de palavras -- api/palavras (post: dados)
        [Route("")]
        [HttpPost]
        public ActionResult Cadastrar([FromBody] Palavra palavra)
        {
            _banco.Palavras.Add(palavra);
            _banco.SaveChanges();
            return Created($"api/palavras/{palavra.Id}",palavra);
        }
        //modificar palavra -- api/palavras (put: dados)
        [Route("{id}")]
        [HttpPut]
        public ActionResult Atualizar(int id, [FromBody] Palavra palavra)
        {
            var obj = _banco.Palavras.AsNoTracking().FirstOrDefault(async=>async.Id == id);
            if (obj == null)
                return NotFound(); //muda o retorno pra 404 ( erro )

            palavra.Id = id;
            _banco.Palavras.Update(palavra);
            _banco.SaveChanges();
            return Ok();
        }
        //deletar palavra -- api/palavras/2 (deleta)
        [Route("{id}")]
        [HttpDelete]
        public ActionResult Deletar(int id)
        {
            var palavra = _banco.Palavras.Find(id);
            if (palavra == null)
                return NotFound(); //muda o retorno pra 404 ( erro )

            palavra.Ativo = false;
            _banco.Palavras.Update(palavra); //não exclui a palavra, apenas deixa ela "inativa" nas propriedades da palavra
            _banco.SaveChanges();
            return NoContent();
        }
    }
}
