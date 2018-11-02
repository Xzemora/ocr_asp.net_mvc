using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ChoixResto.Models
{
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
        public Utilisateur ObtenirUtilisateur(string idStr)
        {
            switch (idStr)
            {
                case "Chrome":
                    return CreeOuRecupere("Nico", "1234");
                case "IE":
                    return CreeOuRecupere("Jérémie", "1234");
                case "Firefox":
                    return CreeOuRecupere("Delphine", "1234");
                case "Edge":
                    return CreeOuRecupere("Antony", "1234");
                default:
                    return CreeOuRecupere("Timéo", "1234");
            }
        }

        private Utilisateur CreeOuRecupere(string nom, string motDePasse)
        {
            Utilisateur utilisateur = Authentifier(nom, motDePasse);
            if (utilisateur == null)
            {
                int id = AjouterUtilisateur(nom, motDePasse);
                return ObtenirUtilisateur(id);
            }
            return utilisateur;
        }

        public int AjouterUtilisateur(string nom, string mdp)
        {
            string motDePasseEncode = EncodeMD5(mdp);
            Utilisateur utilisateur = new Utilisateur { Prenom = nom, MotDePasse = motDePasseEncode };
            bdd.Utilisateurs.Add(utilisateur);
            bdd.SaveChanges();
            return utilisateur.Id;
        }

        public Utilisateur Authentifier(string prenom, string mdp)
        {
            string motDePasseEncode = EncodeMD5(mdp);
            return bdd.Utilisateurs.FirstOrDefault(u => u.Prenom == prenom && u.MotDePasse == motDePasseEncode);
        }

        public bool ADejaVote(int idSondage, string idStr)
        {
            Utilisateur utilisateur = ObtenirUtilisateur(idStr);
            if (utilisateur != null)
            {
                Sondage sondage = bdd.Sondages.First(s => s.Id == idSondage);
                if (sondage.Votes == null)
                    return false;
                return sondage.Votes.Any(v => v.Utilisateur != null && v.Utilisateur.Id == utilisateur.Id);
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