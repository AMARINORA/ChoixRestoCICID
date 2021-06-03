using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChoixResto.Models;
using ChoixResto.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChoixResto.Controllers
{
	public class VoteController : Controller
	{
        private IDal dal;
        public VoteController()
        {
            dal = new Dal();
        }
        public ActionResult Index(int id)
        {
            RestaurantVoteViewModel viewModel = new RestaurantVoteViewModel
            {
                ListeDesResto = dal.ObtientTousLesRestaurants().Select(r => new RestaurantCheckBoxViewModel { Id = r.Id, NomEtTelephone = string.Format("{0} ({1})", r.Nom, r.Telephone) }).ToList()
            };
            if (dal.ADejaVote(id, HttpContext.User.Identity.Name))
            {
                return RedirectToAction("AfficheResultat", new { id = id });
            }
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Index(RestaurantVoteViewModel viewModel, int id)
        {
            if (!ModelState.IsValid)
                return View(viewModel);
            Utilisateur utilisateur = dal.ObtenirUtilisateur(HttpContext.User.Identity.Name);
            if (utilisateur == null)
                return new NotFoundResult();
            foreach (RestaurantCheckBoxViewModel restaurantCheckBoxViewModel in viewModel.ListeDesResto.Where(r => r.EstSelectionne))
            {
                dal.AjouterVote(id, restaurantCheckBoxViewModel.Id, utilisateur.Id);
            }
            return RedirectToAction("AfficheResultat", new { id = id });
        }

        public ActionResult AfficheResultat(int id)
        {
            if (!dal.ADejaVote(id, HttpContext.User.Identity.Name))
            {
                return RedirectToAction("Index", new { id = id });
            }
            List<Resultats> resultats = dal.ObtenirLesResultats(id);
            return View(resultats.OrderByDescending(r => r.NombreDeVotes).ToList());
        }

        public ActionResult ListeSondages()
        {
            List<Sondage> listSondages = dal.ObtenirLesSondages();
            return View(listSondages);
        }
    }
}
