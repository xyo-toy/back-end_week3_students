using back_end_students.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace back_end_students.Datas
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) { }
        public DbSet<Students> students { get; set; }
        public DbSet<Students_description> students_descriptions { get; set; }

    }
}
