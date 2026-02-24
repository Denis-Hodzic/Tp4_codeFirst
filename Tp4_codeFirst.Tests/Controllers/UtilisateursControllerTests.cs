using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using Tp4_codeFirst.Controllers;
using Tp4_codeFirst.Models.EntityFramework;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Tp4_codeFirst.Tests
{
    [TestClass]
    public class UtilisateursControllerTests
    {
        private readonly FilmRatingsDbContext _context;
        private readonly UtilisateursController _controller;

        public UtilisateursControllerTests()
        {
            var options = new DbContextOptionsBuilder<FilmRatingsDbContext>()
                .UseNpgsql("Host=localhost;Port=5432;Database=FilmRatings;Username=postgres;Password=Dragon10")
                .Options;

            _context = new FilmRatingsDbContext(options);
            _controller = new UtilisateursController(_context);
        }

        [TestMethod]
        public void GetAllUtilisateurs_OK()
        {
            // Arrange
            var attendu = _context.Utilisateurs.ToList();

            // Act
            var actionResult = _controller.GetUtilisateurs().Result; // async -> sync uniquement dans tests
            var ok = actionResult.Result as OkObjectResult; // selon ton controller scaffolé
            Assert.IsNotNull(ok);

            var recu = ok.Value as System.Collections.Generic.IEnumerable<Utilisateur>;
            Assert.IsNotNull(recu);

            // Assert
            Assert.AreEqual(attendu.Count, recu.Count(), "Le nombre d'utilisateurs ne correspond pas.");
        }

        [TestMethod]
        public void GetUtilisateurById_OK()
        {
            // Arrange : on prend un id existant
            int id = _context.Utilisateurs.Select(u => u.UtilisateurId).First();

            // Act
            var result = _controller.GetUtilisateurById(id).Result;

            // Assert
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(id, result.Value.UtilisateurId);
        }

        [TestMethod]
        public void GetUtilisateurById_NotFound()
        {
            // Arrange : id impossible
            int id = -1;

            // Act
            var result = _controller.GetUtilisateurById(id).Result;

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void GetUtilisateurByEmail_OK()
        {
            // Arrange : email existant
            string mail = _context.Utilisateurs.Select(u => u.Mail).First();

            // Act
            var result = _controller.GetUtilisateurByEmail(mail).Result;

            // Assert
            Assert.IsNotNull(result.Value);
            Assert.AreEqual(mail.ToUpper(), result.Value.Mail.ToUpper());
        }

        [TestMethod]
        public void GetUtilisateurByEmail_NotFound()
        {
            // Arrange
            string mail = "inexistant_" + Guid.NewGuid() + "@gmail.com";

            // Act
            var result = _controller.GetUtilisateurByEmail(mail).Result;

            // Assert
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void PostUtilisateur_ModelValidated_CreationOK()
        {
            // Arrange (poly : mail unique via random/timestamp) :contentReference[oaicite:5]{index=5}
            var rnd = new Random();
            int chiffre = rnd.Next(1, 1000000000);

            Utilisateur user = new Utilisateur
            {
                Nom = "MACHIN",
                Prenom = "Luc",
                Mobile = "0606070809",
                Mail = "machin" + chiffre + "@gmail.com",
                Pwd = "Toto1234!",
                Rue = "Chemin de Bellevue",
                CodePostal = "74940",
                Ville = "Annecy-le-Vieux",
                Pays = "France",
                Latitude = null,
                Longitude = null
            };

            // Act
            var result = _controller.PostUtilisateur(user).Result;

            // Assert : on récupère depuis la BD via mail unique (poly) :contentReference[oaicite:6]{index=6}
            var userRecupere = _context.Utilisateurs
                .FirstOrDefault(u => u.Mail.ToUpper() == user.Mail.ToUpper());

            Assert.IsNotNull(userRecupere);

            user.UtilisateurId = userRecupere.UtilisateurId;
            Assert.AreEqual(userRecupere.Mail.ToUpper(), user.Mail.ToUpper());
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void PostUtilisateur_MailDuplique_Exception()
        {
            // Arrange : prendre un mail existant -> viole contrainte unique (base)
            string mailExistant = _context.Utilisateurs.Select(u => u.Mail).First();

            Utilisateur user = new Utilisateur
            {
                Nom = "DUPLIQUE",
                Prenom = "Test",
                Mobile = "0606070809",
                Mail = mailExistant,
                Pwd = "Toto1234!"
            };

            // Act -> SaveChangesAsync dans le controller va lever une exception en base
            _ = _controller.PostUtilisateur(user).Result;
        }

        [TestMethod]
        public void DeleteUtilisateur_OK()
        {
            // Arrange : ajouter un user via DbSet puis SaveChanges (poly) :contentReference[oaicite:7]{index=7}
            var rnd = new Random();
            int chiffre = rnd.Next(1, 1000000000);

            Utilisateur user = new Utilisateur
            {
                Nom = "A_SUPPRIMER",
                Prenom = "Test",
                Mobile = "0606070809",
                Mail = "delete" + chiffre + "@gmail.com",
                Pwd = "Toto1234!"
            };

            _context.Utilisateurs.Add(user);
            _context.SaveChanges();

            int id = user.UtilisateurId;

            // Act
            var result = _controller.DeleteUtilisateur(id).Result;

            // Assert : vérifier qu’il n’existe plus
            var userSupprime = _context.Utilisateurs.FirstOrDefault(u => u.UtilisateurId == id);
            Assert.IsNull(userSupprime);
        }
    }
}