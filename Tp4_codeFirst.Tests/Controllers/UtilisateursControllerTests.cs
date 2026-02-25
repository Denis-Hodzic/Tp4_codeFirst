using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Tp4_codeFirst.Controllers;
using Tp4_codeFirst.Models.EntityFramework;
using Tp4_codeFirst.Models.Repository;

namespace Tp4_codeFirst.Tests
{
    [TestClass]
    public class UtilisateursControllerMoqTests
    {
        // -------- POST --------
        [TestMethod]
        public void PostUtilisateur_ModelValidated_CreationOK_AvecMoq()
        {
            // Arrange
            var mockRepository = new Mock<IDataRepository<Utilisateur>>();
            var controller = new UtilisateursController(mockRepository.Object);

            Utilisateur user = new Utilisateur
            {
                Nom = "POISSON",
                Prenom = "Pascal",
                Mobile = "0606070809",
                Mail = "poisson@gmail.com",
                Pwd = "Toto12345678!",
                Rue = "Chemin de Bellevue",
                CodePostal = "74940",
                Ville = "Annecy-le-Vieux",
                Pays = "France",
                Latitude = null,
                Longitude = null
            };

            // Optionnel: vérifier que Add est bien appelé
            mockRepository.Setup(r => r.Add(user));

            // Act
            var actionResult = controller.PostUtilisateur(user).Result;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsInstanceOfType(actionResult.Result, typeof(CreatedAtActionResult));

            var created = actionResult.Result as CreatedAtActionResult;
            Assert.IsNotNull(created);
            Assert.IsInstanceOfType(created.Value, typeof(Utilisateur));

            var returnedUser = created.Value as Utilisateur;
            Assert.IsNotNull(returnedUser);

            // Vérifie que l’appel à Add a été fait 1 fois
            mockRepository.Verify(r => r.Add(It.IsAny<Utilisateur>()), Times.Once);
        }

        // -------- GET BY ID (OK) --------
        [TestMethod]
        public void GetUtilisateurById_ExistingIdPassed_ReturnsRightItem_AvecMoq()
        {
            // Arrange
            Utilisateur user = new Utilisateur
            {
                UtilisateurId = 1,
                Nom = "Calida",
                Prenom = "Lilley",
                Mobile = "0653930778",
                Mail = "clilleymd@last.fm",
                Pwd = "Toto12345678!",
                Rue = "Impasse des bergeronnettes",
                CodePostal = "74200",
                Ville = "Allinges",
                Pays = "France",
                Latitude = 46.344795F,
                Longitude = 6.4885845F
            };

            var mockRepository = new Mock<IDataRepository<Utilisateur>>();
            mockRepository.Setup(r => r.GetById(1)).Returns(user);

            var controller = new UtilisateursController(mockRepository.Object);

            // Act
            var actionResult = controller.GetUtilisateurById(1).Result;

            // Assert
            Assert.IsNotNull(actionResult);
            Assert.IsNotNull(actionResult.Value);
            Assert.AreEqual(user, actionResult.Value);
        }

        // -------- GET BY ID (NOT FOUND) --------
        [TestMethod]
        public void GetUtilisateurById_UnknownIdPassed_ReturnsNotFoundResult_AvecMoq()
        {
            // Arrange
            var mockRepository = new Mock<IDataRepository<Utilisateur>>();
            mockRepository.Setup(r => r.GetById(It.IsAny<int>())).Returns((Utilisateur)null);

            var controller = new UtilisateursController(mockRepository.Object);

            // Act
            var actionResult = controller.GetUtilisateurById(0).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundResult));
        }

        // -------- GET BY EMAIL (OK) --------
        [TestMethod]
        public void GetUtilisateurByEmail_ExistingEmailPassed_ReturnsRightItem_AvecMoq()
        {
            // Arrange
            string email = "clilleymd@last.fm";
            Utilisateur user = new Utilisateur
            {
                UtilisateurId = 1,
                Nom = "Calida",
                Prenom = "Lilley",
                Mobile = "0653930778",
                Mail = email,
                Pwd = "Toto12345678!",
                Rue = "Impasse des bergeronnettes",
                CodePostal = "74200",
                Ville = "Allinges",
                Pays = "France",
                Latitude = 46.344795F,
                Longitude = 6.4885845F
            };

            var mockRepository = new Mock<IDataRepository<Utilisateur>>();
            mockRepository.Setup(r => r.GetByString(email)).Returns(user);

            var controller = new UtilisateursController(mockRepository.Object);

            // Act
            var actionResult = controller.GetUtilisateurByEmail(email).Result;

            // Assert
            Assert.IsNotNull(actionResult.Value);
            Assert.AreEqual(user, actionResult.Value);
        }

        // -------- GET BY EMAIL (NOT FOUND) --------
        [TestMethod]
        public void GetUtilisateurByEmail_UnknownEmailPassed_ReturnsNotFoundResult_AvecMoq()
        {
            // Arrange
            var mockRepository = new Mock<IDataRepository<Utilisateur>>();
            mockRepository.Setup(r => r.GetByString(It.IsAny<string>())).Returns((Utilisateur)null);

            var controller = new UtilisateursController(mockRepository.Object);

            // Act
            var actionResult = controller.GetUtilisateurByEmail("inexistant@gmail.com").Result;

            // Assert
            Assert.IsInstanceOfType(actionResult.Result, typeof(NotFoundResult));
        }

        // -------- DELETE (OK) --------
        [TestMethod]
        public void DeleteUtilisateurTest_AvecMoq()
        {
            // Arrange
            Utilisateur user = new Utilisateur
            {
                UtilisateurId = 1,
                Nom = "Calida",
                Prenom = "Lilley",
                Mobile = "0653930778",
                Mail = "clilleymd@last.fm",
                Pwd = "Toto12345678!",
                Rue = "Impasse des bergeronnettes",
                CodePostal = "74200",
                Ville = "Allinges",
                Pays = "France",
                Latitude = 46.344795F,
                Longitude = 6.4885845F
            };

            var mockRepository = new Mock<IDataRepository<Utilisateur>>();
            mockRepository.Setup(r => r.GetById(1)).Returns(user);

            var controller = new UtilisateursController(mockRepository.Object);

            // Act
            var actionResult = controller.DeleteUtilisateur(1).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult));
            mockRepository.Verify(r => r.Delete(It.IsAny<Utilisateur>()), Times.Once);
        }

        // -------- PUT (OK) --------
        [TestMethod]
        public void PutUtilisateur_ExistingIdPassed_ReturnsNoContent_AvecMoq()
        {
            // Arrange
            int id = 1;

            Utilisateur userEnBase = new Utilisateur
            {
                UtilisateurId = id,
                Nom = "Calida",
                Prenom = "Lilley",
                Mobile = "0653930778",
                Mail = "clilleymd@last.fm",
                Pwd = "Toto12345678!"
            };

            Utilisateur userModifie = new Utilisateur
            {
                UtilisateurId = id,
                Nom = "Calida_MODIF",
                Prenom = "Lilley",
                Mobile = "0600000000",
                Mail = "clilleymd@last.fm",
                Pwd = "Toto12345678!"
            };

            var mockRepository = new Mock<IDataRepository<Utilisateur>>();
            mockRepository.Setup(r => r.GetById(id)).Returns(userEnBase);

            var controller = new UtilisateursController(mockRepository.Object);

            // Act
            var actionResult = controller.PutUtilisateur(id, userModifie).Result;

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(NoContentResult));
            mockRepository.Verify(r => r.Update(It.IsAny<Utilisateur>(), It.IsAny<Utilisateur>()), Times.Once);
        }
    }
}