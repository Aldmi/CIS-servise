using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    /// <summary>
    /// Вокзал.
    /// </summary>
    public class RailwayStation
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите Ecp код станции")]
        public string Name { get; set; }

        public virtual ICollection<Station> Stations { get; set; }                              // многие ко многим с Station. (список возможных станций этого вокзала)

        public virtual ICollection<OperativeSchedule> OperativeSchedules { get; set; }         // один ко многим с  OperativeSchedule. (одна запись в расписании принаджежит только 1 вокзалу)

        //public List<RegulatorySchedule> RegulatorySchedules { get; set; }                    // один ко многим с RegulatorySchedules. (одна запись в расписании принаджежит только 1 вокзалу)

       //public virtual ICollection<Info> Infos { get; set; }

       // public virtual ICollection<Diagnostic> Diagnostics { get; set; }
    }
}