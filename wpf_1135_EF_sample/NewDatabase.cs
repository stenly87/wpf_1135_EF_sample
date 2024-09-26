using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpf_1135_EF_sample
{
    public class NewDatabase : DbContext
    {
        public NewDatabase()
        {
            // выполнять на проде только в пятницу
            //Database.EnsureDeleted();
            // удостовериться, что бд создана (создать, если ее нет)
            Database.EnsureCreated(); 
        }

        public DbSet<Cat> Cats { get; set; }
        public DbSet<Mouse> Mouse { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseMySql("server=192.168.200.13;userid=student;password=student;database=1135_☻new_2024", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.3.39-mariadb"));
        }
    }


    public class Cat
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }

    public class Mouse
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public int IdCat { get; set; }
        public Cat Cat { get; set; }
    }
}
