using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimicAPI.Database;
using MimicAPI.Helpers;
using MimicAPI.Models;
using Newtonsoft.Json;
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
        public ActionResult ObterTodas([FromQuery] PalavraUrlQuery query)//puxa as propriedades de uma Query
        {
            //esse metodo pode funcionar de duas formas: recebendo o datetime como parametro ou não
            var item = _banco.Palavras.AsQueryable();
            if (query.Data.HasValue)
            {
                item = item.Where(a => a.Criado > query.Data.Value || a.Atualizado > query.Data.Value);
            }

            if (query.PagNumero.HasValue)
            {
                var quantidadeTotalRegistros = item.Count();
                //pula os proximos registros de uma página  e pega os próximos
                //ex: tem 2 páginas e cada pagina tem 5 itens
                //(2-1)*5 = pula 5 e pega os 5 próximos
                item = item.Skip((query.PagNumero.Value - 1) * query.PagRegistros.Value).Take(query.PagRegistros.Value);
                var paginacao = new Paginacao();
                paginacao.NumeroPagina = query.PagNumero.Value;
                paginacao.RegistroPorPagina = query.PagRegistros.Value;
                paginacao.TotalRegistros = quantidadeTotalRegistros;
                paginacao.TotalPaginas = (int)Math.Ceiling((double)quantidadeTotalRegistros / query.PagRegistros.Value);

                Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginacao));

                if (query.PagNumero > paginacao.TotalPaginas)
                {
                    return NotFound();
                }
            }
            return Ok(item);
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
