using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ChoixResto.Models
{
    public class Utilisateur
    {
        public int Id { get; set; }
        [Required]
        public string Prenom { get; set; }
    }

    [Table("Restos")]
    public class Resto
    {
        public int Id { get; set; }
        [Required]
        public string Nom { get; set; }
        public string Telephone { get; set; }
    }

    public class Vote
    {
        public int Id { get; set; }
        public virtual Resto Resto { get; set; }
        public virtual Utilisateur Utilisateur { get; set; }
    }

    public class Sondage
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public virtual List<Vote> Votes { get; set; }
    }
    
    public class BddContext : DbContext
    {
        public DbSet<Sondage> Sondages { get; set; }
        public DbSet<Resto> Restos { get; set; }
    }

    public class Dal : IDal
    {
        private BddContext bdd;

        public Dal()
        {
            bdd = new BddContext();
        }

        public List<Resto> ObtientTousLesRestaurants()
        {
            return bdd.Restos.ToList();
        }

        public void Dispose()
        {
            bdd.Dispose();
        }

        public void CreerRestaurant(string nom, string telephone)
        {
            bdd.Restos.Add(new Resto { Nom = nom, Telephone = telephone });
            bdd.SaveChanges();
        }

        public void ModifierRestaurant(int id, string nom, string telephone)
        {
            Resto restoTrouve = bdd.Restos.FirstOrDefault(resto => resto.Id == id);
            if (restoTrouve != null)
            {
                restoTrouve.Nom = nom;
                restoTrouve.Telephone = telephone;
                bdd.SaveChanges();
            }
        }
    }
}