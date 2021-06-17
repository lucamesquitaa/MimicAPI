using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimicAPI.Helpers;
using MimicAPI.Models;
using MimicAPI.Models.DTO;
using MimicAPI.Repositories.Contracts;
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

        private readonly IPalavraRepository _repository;
        private readonly IMapper _mapper;
        public PalavrasController(IPalavraRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        //app -- api/palavras (mostra a lista de palavras)
        [Route("")]
        [HttpGet]
        public ActionResult ObterPalavras([FromQuery] PalavraUrlQuery query)//puxa as propriedades de uma Query
        {
            var item = _repository.ObterPalavras(query);
            if (query.PagNumero > item.Paginacao.TotalPaginas)
            {
                return NotFound();
            }
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(item.Paginacao));
            return Ok(item);
        }
        //recuperar uma palavra -- api/palavras/2 (mostra)
        [HttpGet("{id}", Name = "ObterPalavra")]
        public ActionResult Obter(int id)
        {
            var obj = _repository.Obter(id);
            if (obj == null)
                return NotFound(); //muda o retorno pra 404 ( erro )

            PalavraDTO palavraDTO = _mapper.Map<Palavra, PalavraDTO>(obj);
            palavraDTO.Links = new List<LinkDTO>();
            palavraDTO.Links.Add(new LinkDTO("self", Url.Link("ObterPalavra", new { id = palavraDTO.Id }), "GET"));
            palavraDTO.Links.Add(new LinkDTO("update", Url.Link("AtualizarPalavra", new { id = palavraDTO.Id }), "PUT"));
            palavraDTO.Links.Add(new LinkDTO("delete", Url.Link("ExcluirPalavra", new { id = palavraDTO.Id }), "DELETE"));

            return Ok(palavraDTO);
        }
        //cadastro de palavras -- api/palavras (post: dados)
        [Route("")]
        [HttpPost]
        public ActionResult Cadastrar([FromBody] Palavra palavra)
        {
            _repository.Cadastrar(palavra);
            return Created($"api/palavras/{palavra.Id}", palavra);
        }
        //modificar palavra -- api/palavras (put: dados)

        [HttpPut("{id}", Name = "AtualizarPalavra")]
        public ActionResult Atualizar(int id, [FromBody] Palavra palavra)
        {
            var obj = _repository.Obter(id);
            if (obj == null)
                return NotFound(); //muda o retorno pra 404 ( erro )

            palavra.Id = id;
            _repository.Atualizar(palavra);
            return Ok();
        }
        //deletar palavra -- api/palavras/2 (deleta)
       
        [HttpDelete("{id}", Name = "ExcluirPalavra")]
        public ActionResult Deletar(int id)
        {
            var palavra = _repository.Obter(id);
            if (palavra == null)
                return NotFound(); //muda o retorno pra 404 ( erro )

            _repository.Deletar(id);

            return NoContent();
        }
    }
}
