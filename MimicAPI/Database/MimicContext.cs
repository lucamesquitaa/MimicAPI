using Microsoft.EntityFrameworkCore;
using MimicAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace MimicAPI.Database
{
    public class MimicContext : DbContext
    {
        //construtor do contexto do banco de dados
        public MimicContext(DbContextOptions<MimicContext> options) : base(options)

        {

        }
        public DbSet<Palavra> Palavras  { get; set; }
    }
}
