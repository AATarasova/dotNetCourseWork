﻿using MailingGeneratorDomain.Models;
 using MailingsGeneratorDomain.Models;
using Microsoft.EntityFrameworkCore;
namespace Mailing.MailingDal
{
    public sealed class MailingDbContext : DbContext
    {
        public DbSet<ControlEvent> ControlEvents { get; set; }
        public DbSet<Text> Texts { get; set; }
        public DbSet<MailingsGeneratorDomain.Models.Mailing> Mailings { get; set; }

        public MailingDbContext()
        {
            Database.EnsureCreated();

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ControlEvent>()
                .HasOne(ev => ev.Mail)
                .WithMany(mail => mail.Works);
            
            
            modelBuilder.Entity<Text>()
                .HasOne(ev => ev.Mail)
                .WithMany(mail => mail.MailText);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost;Database=anna;Trusted_Connection=True;");
        }

    }
}