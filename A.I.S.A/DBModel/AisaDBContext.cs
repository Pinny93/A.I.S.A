using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A.I.S.A_.DBModel
{
    public class AisaDBContext : DbContext
    {
        public AisaDBContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            this.DBPath = System.IO.Path.Join(path, "AISA.db");
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={this.DBPath}");

        public DbSet<Queries> Queries { get; set; }

        public string DBPath { get; }
    }
}