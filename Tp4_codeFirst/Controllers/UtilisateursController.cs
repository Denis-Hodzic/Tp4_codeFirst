using Microsoft.AspNetCore.Mvc;
using Tp4_codeFirst.Models.EntityFramework;
using Tp4_codeFirst.Models.Repository;

namespace Tp4_codeFirst.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilisateursController : ControllerBase
    {
        private readonly IDataRepository<Utilisateur> dataRepository;

        public UtilisateursController(IDataRepository<Utilisateur> dataRepo)
        {
            dataRepository = dataRepo;
        }

        // GET: api/Utilisateurs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Utilisateur>>> GetUtilisateurs()
        {
            return dataRepository.GetAll();
        }

        // GET: api/Utilisateurs/GetById/5
        [HttpGet]
        [Route("[action]/{id}")]
        [ActionName("GetById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Utilisateur>> GetUtilisateurById(int id)
        {
            var utilisateur = dataRepository.GetById(id);
            if (utilisateur.Value == null) return NotFound();
            return utilisateur;
        }

        // GET: api/Utilisateurs/GetByEmail/toto@titi.fr
        [HttpGet]
        [Route("[action]/{email}")]
        [ActionName("GetByEmail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Utilisateur>> GetUtilisateurByEmail(string email)
        {
            var utilisateur = dataRepository.GetByString(email);
            if (utilisateur.Value == null) return NotFound();
            return utilisateur;
        }

        // PUT: api/Utilisateurs/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutUtilisateur(int id, Utilisateur utilisateur)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (id != utilisateur.UtilisateurId) return BadRequest();

            var userToUpdate = dataRepository.GetById(id);
            if (userToUpdate.Value == null) return NotFound();

            dataRepository.Update(userToUpdate.Value, utilisateur);
            return NoContent();
        }

        // POST: api/Utilisateurs
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Utilisateur>> PostUtilisateur(Utilisateur utilisateur)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            dataRepository.Add(utilisateur);

            // IMPORTANT: actionName = "GetById" (cf poly)
            return CreatedAtAction("GetById", new { id = utilisateur.UtilisateurId }, utilisateur);
        }

        // DELETE: api/Utilisateurs/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUtilisateur(int id)
        {
            var utilisateur = dataRepository.GetById(id);
            if (utilisateur.Value == null) return NotFound();

            dataRepository.Delete(utilisateur.Value);
            return NoContent();
        }
    }
}