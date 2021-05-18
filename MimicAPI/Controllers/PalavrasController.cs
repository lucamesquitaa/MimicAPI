using Microsoft.AspNetCore.Mvc;
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
            return Ok(_banco.Palavras.Find(id));
        }
        //cadastro de palavras -- api/palavras (post: dados)
        [Route("")]
        [HttpPost]
        public ActionResult Cadastrar([FromBody] Palavra Palavra)
        {
            _banco.Palavras.Add(Palavra);
            return Ok();
        }
        //modificar palavra -- api/palavras (put: dados)
        [Route("{id}")]
        [HttpPut]
        public ActionResult Atualizar(int id, [FromBody] Palavra palavra)
        {
            _banco.Palavras.Update(palavra);
            return Ok();
        }
        //deletar palavra -- api/palavras/2 (deleta)
        [Route("{id}")]
        [HttpDelete]
        public ActionResult Deletar(int id)
        {
            _banco.Palavras.Remove(_banco.Palavras.Find(id));
            return Ok();
        }
    }
}
