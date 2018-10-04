using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChoixResto.Models
{
    public interface IDal : IDisposable
    {
        void CreerRestaurant(string nom, string telephone);
        void ModifierRestaurant(int id, string nom, string telephone);
        bool RestaurantExiste(string nom);
        Utilisateur ObtenirUtilisateur(int id);
        Utilisateur ObtenirUtilisateur(string id);
        List<Resto> ObtientTousLesRestaurants();
        int AjouterUtilisateur(string v1, string v2);
        Utilisateur Authentifier(string v1, string v2);
        bool ADejaVote(int idSondage, string idStr);
        int CreerUnSondage();
        void AjouterVote(int idSondage, int idResto, int idUtilisateur);
        List<Resultats> ObtenirLesResultats(int idSondage);

    }


}
