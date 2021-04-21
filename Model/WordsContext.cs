using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class WordsContext : DbContext
    {
        public WordsContext([NotNullAttribute] DbContextOptions options) : base(options)
        {
        }

        public DbSet<Word> Words { get; set; }
    }
}
