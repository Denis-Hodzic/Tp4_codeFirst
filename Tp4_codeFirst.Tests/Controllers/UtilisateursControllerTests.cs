using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore;
using Tp4_codeFirst.Controllers;
using Tp4_codeFirst.Models.EntityFramework;
using System;
using System.Linq;
using Tp4_codeFirst.Models.DataManager;
using Tp4_codeFirst.Models.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Tp4_codeFirst.Tests
{
    [TestClass]
    public class UtilisateursControllerTests
    {
        private readonly FilmRatingsDbContext context;
        private readonly IDataRepository<Utilisateur> dataRepository;
        private readonly UtilisateursController controller;

        public UtilisateursControllerTests()
        {
            var options = new DbContextOptionsBuilder<FilmRatingsDbContext>()
                .UseNpgsql("Host=localhost;Port=5432;Database=FilmRatings;Username=postgres;Password=Dragon10")
                .Options;

            context = new FilmRatingsDbContext(options);
            dataRepository = new UtilisateurManager(context);
            controller = new UtilisateursController(dataRepository);
        }

        [TestMethod]
        public void GetAllUtilisateurs_OK()
        {
            var attendu = context.Utilisateurs.ToList();

            var actionResult = controller.GetUtilisateurs().Result;

            var recu =
                actionResult.Value
                ?? (actionResult.Result as OkObjectResult)?.Value as System.Collections.Generic.IEnumerable<Utilisateur>;

            Assert.IsNotNull(recu);
            Assert.AreEqual(attendu.Count, recu.Count());
        }

        [TestMethod]
        public void GetUtilisateurById_OK()
        {
            int id = context.Utilisateurs.Select(u => u.UtilisateurId).First();

            var result = controller.GetUtilisateurById(id).Result;

            Assert.IsNotNull(result.Value);
            Assert.AreEqual(id, result.Value.UtilisateurId);
        }

        [TestMethod]
        public void GetUtilisateurById_NotFound()
        {
            int id = -1;

            var result = controller.GetUtilisateurById(id).Result;

            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void GetUtilisateurByEmail_OK()
        {
            string mail = context.Utilisateurs.Select(u => u.Mail).First();

            var result = controller.GetUtilisateurByEmail(mail).Result;

            Assert.IsNotNull(result.Value);
            Assert.AreEqual(mail.ToUpper(), result.Value.Mail.ToUpper());
        }

        [TestMethod]
        public void GetUtilisateurByEmail_NotFound()
        {
            string mail = "inexistant_" + Guid.NewGuid() + "@gmail.com";

            var result = controller.GetUtilisateurByEmail(mail).Result;

            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void PostUtilisateur_ModelValidated_CreationOK()
        {
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

            _ = controller.PostUtilisateur(user).Result;

            var userRecupere = context.Utilisateurs
                .FirstOrDefault(u => u.Mail.ToUpper() == user.Mail.ToUpper());

            Assert.IsNotNull(userRecupere);
            Assert.AreEqual(userRecupere.Mail.ToUpper(), user.Mail.ToUpper());
        }

        [TestMethod]
        public void PostUtilisateur_MailDuplique_Exception()
        {
            string mailExistant = context.Utilisateurs.Select(u => u.Mail).First();

            Utilisateur user = new Utilisateur
            {
                Nom = "DUPLIQUE",
                Prenom = "Test",
                Mobile = "0606070809",
                Mail = mailExistant,
                Pwd = "Toto1234!"
            };

            Assert.ThrowsException<AggregateException>(() =>
            {
                _ = controller.PostUtilisateur(user).Result;
            });
        }

        [TestMethod]
        public void DeleteUtilisateur_OK()
        {
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

            context.Utilisateurs.Add(user);
            context.SaveChanges();

            int id = user.UtilisateurId;

            _ = controller.DeleteUtilisateur(id).Result;

            var userSupprime = context.Utilisateurs.FirstOrDefault(u => u.UtilisateurId == id);
            Assert.IsNull(userSupprime);
        }
    }
}