using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Tp4_codeFirst.Models.EntityFramework
{
    [PrimaryKey("FilmId", "UtilisateurId")]
    [Table("avis")]
    public partial class Notation
    {
        [Key]
        [Column("flm_id")]
        [ForeignKey(nameof(flm_id))]
        public int FilmId { get; set; }

        [Key]
        [Column("utl_id")]
        public int UtilisateurId { get; set; }

        [Column("not_note")]
        public decimal Note { get; set; }

        [ForeignKey(nameof(Idfilm))]
        [InverseProperty(nameof(Film.Avis))]
        public virtual Film IdfilmNavigation { get; set; } = null!;

        [ForeignKey(nameof(Idutilisateur))]
        [InverseProperty(nameof(Utilisateur.Avis))]
        public virtual Utilisateur IdutilisateurNavigation { get; set; } = null!;
    }
}
