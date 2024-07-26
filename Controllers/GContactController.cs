using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Projet_gestionContacts.Models;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Projet_gestionContacts.Services;
using static Projet_gestionContacts.Models.GContactModels;
using Projet_gestionContacts.Migrations;

namespace Projet_gestionContacts.Controllers
{

    public class GContactController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Constructeur qui initialise le contexte de la base de données
        public GContactController(ApplicationDbContext context)
        {
            _context = context;
        }



        // GET: Contact/Modifier/5
        public async Task<IActionResult> Modifier(int id) // Déclaration de la méthode asynchrone "Modifier" qui accepte un identifiant de contact
        {
            // Récupérer l'identifiant de l'utilisateur de la session
            var utilisateurId = HttpContext.Session.GetInt32("UtilisateurId");

            // Vérifier si l'utilisateur est connecté
            if (utilisateurId == null) // Si l'identifiant de l'utilisateur n'est pas trouvé dans la session
            {
                // Rediriger vers la page de connexion
                return RedirectToAction("Connexion", "GContact");
            }
            else // Si l'utilisateur est connecté
            {
                // Rechercher le contact dans la base de données en utilisant l'identifiant fourni
                var contact = await _context.Contacts.FindAsync(id);

                // Vérifier si le contact existe
                if (contact == null) // Si le contact n'est pas trouvé
                {
                    // Retourner une réponse NotFound (404)
                    return NotFound();
                }

                // Retourner la vue avec le contact trouvé pour l'afficher ou le modifier
                return View(contact);
            }
        }


        [HttpPost] // Indique que cette méthode gère les requêtes POST
        public async Task<IActionResult> Modifier(int id, Contact contact) // Déclaration de la méthode asynchrone "Modifier" qui accepte un identifiant de contact et un objet contact
        {
            // Recherche le contact dans la base de données en utilisant l'identifiant fourni de manière asynchrone
            var contact1 = await _context.Contacts.FindAsync(id);

            // Vérifie si le contact existe
            if (contact1 == null) // Si le contact n'est pas trouvé
            {
                // Redirige vers la page d'index des contacts
                return RedirectToAction("Index", "GContact");
            }

            // Mise à jour des champs modifiables uniquement
            contact1.NomContact = contact.NomContact; // Met à jour le nom du contact
            contact1.NumeroMobile = contact.NumeroMobile; // Met à jour le numéro de mobile
            contact1.NumeroPersonnel = contact.NumeroPersonnel; // Met à jour le numéro personnel
            contact1.NumeroFix = contact.NumeroFix; // Met à jour le numéro fixe
            contact1.EmailContact = contact.EmailContact; // Met à jour l'email du contact

            // Met à jour le champ Favorie, utilise false si null
            contact1.Favorie = contact.Favorie ?? false;

            // Vérifie si le modèle est valide
            if (ModelState.IsValid) // Si le modèle est valide
            {
                _context.Update(contact1); // Met à jour le contact dans le contexte de la base de données
                await _context.SaveChangesAsync(); // Enregistre les modifications dans la base de données de manière asynchrone
                return RedirectToAction(nameof(Index)); // Redirige vers la méthode Index
            }

            // Si le modèle n'est pas valide, retourne la vue de modification avec les erreurs de validation
            return View(contact);
        }



        public IActionResult Supprime(int id) // Déclaration de la méthode "Supprime" qui accepte un identifiant de contact
        {
            // Recherche le contact dans la base de données en utilisant l'identifiant fourni
            var contact = _context.Contacts.Find(id);

            // Vérifie si le contact existe
            if (contact == null) // Si le contact n'est pas trouvé
            {
                // Redirige vers la page "Ajouter" si le contact n'existe pas
                return RedirectToAction(nameof(Ajouter));
            }

            // Supprime le contact de la base de données
            _context.Contacts.Remove(contact);

            // Enregistre les modifications dans la base de données
            _context.SaveChanges();

            // Redirige vers la méthode "Index" après la suppression
            return RedirectToAction(nameof(Index));
        }



        public async Task<IActionResult> Index() // Déclaration de la méthode asynchrone "Index"
        {
            // Récupère l'identifiant de l'utilisateur de la session
            var utilisateurId = HttpContext.Session.GetInt32("UtilisateurId");

            // Vérifie si l'utilisateur est connecté
            if (utilisateurId == null) // Si l'identifiant de l'utilisateur n'est pas trouvé dans la session
            {
                // Redirige vers la page de connexion
                return RedirectToAction("Connexion", "GContact");
            }
            else // Si l'utilisateur est connecté
            {
                // Recherche les contacts dans la base de données pour l'utilisateur connecté de manière asynchrone
                var contacts = await _context.Contacts
                    .Where(c => c.IdUtilisateur == utilisateurId) // Filtre les contacts par l'identifiant de l'utilisateur
                    .ToListAsync(); // Convertit les résultats en liste de manière asynchrone

                // Retourne la vue avec la liste des contacts
                return View(contacts);
            }
        }


        public async Task<IActionResult> Favorie() // Déclaration de la méthode asynchrone "Favorie"
        {
            // Récupère l'identifiant de l'utilisateur de la session
            var utilisateurIdS = HttpContext.Session.GetInt32("UtilisateurId");

            // Vérifie si l'utilisateur est connecté
            if (utilisateurIdS == null) // Si l'identifiant de l'utilisateur n'est pas trouvé dans la session
            {
                // Redirige vers la page de connexion
                return RedirectToAction("Connexion", "GContact");
            }
            else // Si l'utilisateur est connecté
            {
                // Récupère à nouveau l'identifiant de l'utilisateur de la session (redondant, peut être simplifié)
                var utilisateurId = HttpContext.Session.GetInt32("UtilisateurId");

                // Recherche les contacts favoris dans la base de données pour l'utilisateur connecté de manière asynchrone
                var contacts = await _context.Contacts
                    .Where(c => c.IdUtilisateur == utilisateurId && c.Favorie == true) // Filtre les contacts par l'identifiant de l'utilisateur et les contacts favoris
                    .ToListAsync(); // Convertit les résultats en liste de manière asynchrone

                // Retourne la vue avec la liste des contacts favoris
                return View(contacts);
            }
        }


        // Action pour afficher le formulaire d'inscription
        public IActionResult Inscription()
        {
            return View();
        }

        // Action pour gérer la soumission du formulaire d'inscription
        [HttpPost]
        public async Task<IActionResult> Inscription(Utilisateurs utilisateur)
        {
            if (ModelState.IsValid) // Vérifie si le modèle de données est valide
            {
                // Vérifier si l'email existe déjà dans la base de données
                var utilisateurExistant = await _context.Utilisateurs
                    .FirstOrDefaultAsync(u => u.Email == utilisateur.Email);

                if (utilisateurExistant != null) // Si l'email existe déjà
                {
                    // Ajoute une erreur de modèle pour l'email, avec un message personnalisé
                    ModelState.AddModelError("Email", "Cet email est déjà utilisé.");
                    return View(utilisateur); // Retourne la vue d'inscription avec l'utilisateur pour afficher l'erreur
                }

                // Ajoute l'utilisateur à la base de données
                _context.Utilisateurs.Add(utilisateur);
                await _context.SaveChangesAsync(); // Enregistre les modifications dans la base de données de manière asynchrone

                TempData["SuccessMessage"] = "Inscription réussie ! Connectez-vous maintenant."; // Message de succès temporaire
                return RedirectToAction("Connexion", "GContact"); // Redirige vers la page de connexion
            }

            return View(utilisateur); // Retourne la vue d'inscription avec l'utilisateur pour afficher les erreurs de validation
        }



        // Action pour afficher le formulaire de connexion
        public IActionResult Connexion()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Connexion(string email, string pass)
        {
            if (ModelState.IsValid)
            {
                // Vérifier les informations de connexion
                var utilisateur = _context.Utilisateurs
                    .FirstOrDefault(u => u.Email == email && u.MotDePasse == pass);

                if (utilisateur != null)
                {
                    // Ajouter l'ID utilisateur à la session
                    HttpContext.Session.SetInt32("UtilisateurId", utilisateur.Id);

                    // Rediriger vers la page d'accueil
                    return RedirectToAction("Index", "GContact");
                }
                else
                {
                    ViewBag.Message = "Nom d'utilisateur ou mot de passe incorrect.";
                }
            }

            // Si les informations de connexion sont invalides ou si la validation du modèle échoue,
            // retourner à la vue avec le message d'erreur
            return View();
        }



        // Action pour déconnecter l'utilisateur
        public IActionResult Deconnexion()
        {
            HttpContext.Session.Clear(); // Effacer toutes les données de session
            return RedirectToAction("Connexion");
        }



        // Action pour afficher le formulaire d'ajout d'un contact
        public IActionResult Ajouter()
        {

            var utilisateurId = HttpContext.Session.GetInt32("UtilisateurId");

            if (utilisateurId == null)
            {
                return RedirectToAction("Connexion", "GContact");
            }
            else
            {

                // Créer une instance vide de Contact pour le formulaire
                var contact = new Contact();
                return View(contact);
            }
        }

        // Action pour gérer la soumission du formulaire d'ajout d'un contact
        [HttpPost]
        public async Task<IActionResult> Ajouter(Contact contact)
        {
            var utilisateurId = HttpContext.Session.GetInt32("UtilisateurId");

            if (utilisateurId == null)
            {
                return RedirectToAction("Connexion", "GContact");
            }

            if (ModelState.IsValid)
            {
                contact.IdUtilisateur = utilisateurId.Value;

                _context.Contacts.Add(contact);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(contact);
        }




    }
}
