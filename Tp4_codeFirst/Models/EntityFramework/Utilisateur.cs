using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Intrinsics.X86;

namespace Tp4_codeFirst.Models.EntityFramework
{
    [Table("t_e_film_flm")]
    public partial class Utilisateur
    {
        [Key]
        [Column("utl_id")]
        public int UtilisateurId { get; set; }

        [Column("utl_nom")]
        [StringLength(50)]
        public string? Nom { get; set; }

        [Column("utl_prenom")]
        [StringLength(50)]
        public string? Prenom { get; set; }

        [Column("utl_mobile"), TypeName = "char(10)")]
        public string? Mobile { get; set; }

        [Column("flm_datesortie")]
        public DateTime DateSortie { get; set; }

        [Column("flm_duree")]
        public decimal Duree { get; set; }

        [Column("flm_genre")]
        [StringLength(30)]
        public string Genre { get; set; }

        [InverseProperty(nameof(Notation.Note))]
        public virtual ICollection<Notation> Notations { get; set; } = new List<Notation>();
    }
}
