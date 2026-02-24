using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
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

        [Column("utl_mobile")]
        [StringLength(50)]
        public string? Mobile { get; set; }

        [Column("utl_mail")]
        [StringLength(100)]
        public string? Mail { get; set; } = null!;

        [Column("utl_pwd")]
        [StringLength(64)]
        public string Pwd { get; set; } = null!;

        [Column("utl_cp")]
        [StringLength(5)]
        public string? CodePostal { get; set; }

        [Column("utl_ville")]
        [StringLength(50)]
        public string? Ville { get; set; }

        [Column("utl_pays")]
        [StringLength(50)]
        public string? Pays { get; set; }

        [Column("utl_latitude")]
        public float? Latitude { get; set; }

        [Column("utl_longitude")]
        public float? Longitude { get; set; }

        [Column("utl_datecreation")]
        public DateTime DateCreation { get; set; } 

        [InverseProperty(nameof(Notation.Note))]
        public virtual ICollection<Notation> NotesUtilisateur { get; set; } = new List<Notation>();
    }
}
