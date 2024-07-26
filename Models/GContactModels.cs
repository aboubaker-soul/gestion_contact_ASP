using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Projet_gestionContacts.Models
{
    public class GContactModels
    {
        public class Utilisateurs
        {
            [Key]
            public int Id { get; set; }

            [Required(ErrorMessage = "Le champ Nom est requis.")]
            public string Nom { get; set; } = string.Empty;

            [Required(ErrorMessage = "Le champ Email est requis.")]
            [EmailAddress(ErrorMessage = "Le format de l'Email est invalide.")]
            public string Email { get; set; } = string.Empty;

            [Required(ErrorMessage = "Le champ Mot de passe est requis.")]
            public string MotDePasse { get; set; } = string.Empty;

            // Relation avec Contact
            public ICollection<Contact> Contacts { get; set; } = new List<Contact>();
        }

        public class Contact
        {
            [Key]
            public int IdContact { get; set; }

            [Required(ErrorMessage = "Le champ Nom est requis.")]
            public string NomContact { get; set; } = string.Empty;

            [Required(ErrorMessage = "Le champ Email est requis.")]
            [EmailAddress(ErrorMessage = "Le format de l'Email est invalide.")]
            public string EmailContact { get; set; } = string.Empty;

            public bool? Favorie { get; set; }

            public string? NumeroFix { get; set; }

            public string? NumeroPersonnel { get; set; }

            [Required(ErrorMessage = "Le champ Numéro Mobile est requis.")]
            public string NumeroMobile { get; set; } = string.Empty;

            [Required(ErrorMessage = "Le champ Utilisateur est requis.")]
            [ForeignKey("Utilisateur")]
            public int IdUtilisateur { get; set; }

            public Utilisateurs? Utilisateur { get; set; } // Marqué comme nullable
        }
    }
}
