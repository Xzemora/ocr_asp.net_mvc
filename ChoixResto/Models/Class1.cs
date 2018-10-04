using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace ChoixResto.Models
{
    public class Utilisateur
    {
        public int Id { get; set; }
        [Required]
        public string Prenom { get; set; }
        [Required]
        public string Mdp { get; set; }
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
        public Sondage()
        {
            Date = DateTime.Now;
            Votes = new List<Vote>();
        }
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public virtual List<Vote> Votes { get; set; }
    }

    public class Resultats
    {
        public string Nom { get; set; }
        public string Telephone { get; set; }
        public int NombreDeVotes { get; set; }
    }

    public class BddContext : DbContext
    {
        public DbSet<Sondage> Sondages { get; set; }
        public DbSet<Resto> Restos { get; set; }
        public DbSet<Utilisateur> Utilisateurs { get; set; }
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

        public Resto ObtientResto(int idResto)
        {
            List<Resto> restos = ObtientTousLesRestaurants();
            
            for (int j = 0; j < restos.Count; j++)
            {
                if (restos[j].Id == idResto)
                {
                    return restos[j];
                }
            }
            return null;
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

        public bool RestaurantExiste(string nom)
        {
            List<Resto> restos = ObtientTousLesRestaurants();
            for(int i=0; i<restos.Count; i++)
            {

                if(restos[i].Nom == nom)
                {
                    return true;
                }                
            }
            return false;
        }

        
        public Utilisateur ObtenirUtilisateur(int id)
        {
            List<Utilisateur> utilisateurs = bdd.Utilisateurs.ToList();
            for(int i=0; i<utilisateurs.Count; i++)
            {
                if(utilisateurs[i].Id == id)
                {
                    return utilisateurs[i];
                }                
            }
            return null;
        }
        public Utilisateur ObtenirUtilisateur(string id)
        {
            if (Int32.TryParse(id, out int intId))
            {
                return ObtenirUtilisateur(intId);
            }
            return null;           
            
        }

        public int AjouterUtilisateur(string nom, string mdp)
        {
            string motDePasseEncode = EncodeMD5(mdp);
            Utilisateur utilisateur = new Utilisateur { Prenom = nom, Mdp = motDePasseEncode };
            bdd.Utilisateurs.Add(utilisateur);
            bdd.SaveChanges();
            return utilisateur.Id;
        }

        public Utilisateur Authentifier(string prenom, string mdp)
        {
            string motDePasseEncode = EncodeMD5(mdp);
            return bdd.Utilisateurs.FirstOrDefault(u => u.Prenom == prenom && u.Mdp == motDePasseEncode);
        }

        public bool ADejaVote(int idSondage, string idUtilisateur)
        {
            if (ObtientSondage(idSondage) != null)
            {
                List<Vote> votes = ObtientSondage(idSondage).Votes;
                for (int j = 0; j < votes.Count; j++)
                {
                    if (votes[j].Utilisateur == ObtenirUtilisateur(idUtilisateur) && votes[j].Utilisateur != null)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public int CreerUnSondage()
        {
            bdd.Sondages.Add(new Sondage());
            bdd.SaveChanges();
            int idSondage = bdd.Sondages.Count();
            return idSondage;
        }

        public Sondage ObtientSondage(int idSondage)
        {
            List<Sondage> sondages = bdd.Sondages.ToList();
            for (int i = 0; i < sondages.Count; i++)
            {
                if (sondages[i].Id == idSondage)
                {
                    return sondages[i];
                }
            }
            return null;
        }

        public void AjouterVote(int idSondage, int idResto, int idUtilisateur)
        {
            
            ObtientSondage(idSondage).Votes.Add(new Vote { Resto = ObtientResto(idResto), Utilisateur = ObtenirUtilisateur(idUtilisateur) });
                
            
        }

        public List<Resultats> ObtenirLesResultats(int idSondage)
        {
            List<Resultats> resultats = new List<Resultats>();
            List<Resto> restos = ObtientTousLesRestaurants();
            //Sondage sondage = ObtientSondage(idSondage);
            //List<Vote> votes = sondage.Votes;
            //for (int i=0; i<restos.Count; i++)
            //{
            //    Resultats resultat = new Resultats { Nom = restos[i].Nom, Telephone = restos[i].Telephone };
            //    int count = 0;
            //    for(int j =0;j<votes.Count; j++)
            //    {
            //        if(votes[j].Resto.Nom == resultat.Nom)
            //        {
            //            count++;
            //        }
            //    }
            //    resultat.NombreDeVotes = count;
            //    resultats.Add(resultat);
            //}
            //resultats.Sort((a, b) => (a.NombreDeVotes.CompareTo(b.NombreDeVotes)));
            //resultats.Reverse();
            Sondage sondage = bdd.Sondages.First(s => s.Id == idSondage);
            foreach (IGrouping<int, Vote> grouping in sondage.Votes.GroupBy(v => v.Resto.Id))
            {
                int idRestaurant = grouping.Key;
                Resto resto = restos.First(r => r.Id == idRestaurant);
                int nombreDeVotes = grouping.Count();
                resultats.Add(new Resultats { Nom = resto.Nom, Telephone = resto.Telephone, NombreDeVotes = nombreDeVotes });
            }
            return resultats;
        }

        private string EncodeMD5(string motDePasse)
        {
            string motDePasseSel = "ChoixResto" + motDePasse + "ASP.NET MVC";
            return BitConverter.ToString(new MD5CryptoServiceProvider().ComputeHash(ASCIIEncoding.Default.GetBytes(motDePasseSel)));
        }
    }
}