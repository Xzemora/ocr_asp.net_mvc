﻿using System;
using System.Collections.Generic;
using System.Linq;

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
            bdd.Utilisateurs.Add(new Utilisateur { Prenom = nom, Mdp = mdp });
            bdd.SaveChanges();
            int idUtilisateur = bdd.Utilisateurs.Count();
            return idUtilisateur;
        }

        public Utilisateur Authentifier(string prenom, string mdp)
        {
            List<Utilisateur> utilisateurs = bdd.Utilisateurs.ToList();
            for (int i = 0; i < utilisateurs.Count; i++)
            {
                if (utilisateurs[i].Prenom == prenom && utilisateurs[i].Mdp == mdp)
                {
                    return utilisateurs[i];
                }
            }
            return null;
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
            Sondage sondage = ObtientSondage(idSondage);
            List<Vote> votes = sondage.Votes;
            for (int i=0; i<restos.Count; i++)
            {
                Resultats resultat = new Resultats { Nom = restos[i].Nom, Telephone = restos[i].Telephone };
                int count = 0;
                for(int j =0;j<votes.Count; j++)
                {
                    if(votes[j].Resto.Nom == resultat.Nom)
                    {
                        count++;
                    }
                }
                resultat.NombreDeVotes = count;
                resultats.Add(resultat);
            }
            
            return resultats;
        }
    }
}