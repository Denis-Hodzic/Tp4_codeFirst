using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tp4_codeFirst.Models.EntityFramework
{
    [Table("t_e_film_flm")]
    public partial class Film
    {
        [Key]
        [Column("flm_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FilmId { get; set; }

        [Required]
        [Column("flm_titre")]
        [StringLength(100)]
        public string Titre { get; set; } = null!;

        [Column("flm_resume", TypeName = "text")]
        public string? Resume { get; set; }

        [Column("flm_datesortie", TypeName = "date")]
        public DateTime DateSortie { get; set; }

        [Column("flm_duree", TypeName = "numeric(3,0)")]
        public decimal Duree { get; set; }

        [Required]
        [Column("flm_genre")]
        [StringLength(30)]
        public string Genre { get; set; } = null!;

        // Navigation demandée : NotesFilm
        [InverseProperty(nameof(Notation.FilmNote))]
        public virtual ICollection<Notation> NotesFilm { get; set; } = new List<Notation>();
    }
}