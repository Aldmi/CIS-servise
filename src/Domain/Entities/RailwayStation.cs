using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        public List<OperativeSchedule> OperativeSchedules { get; set; }
        public List<RegulatorySchedule> RegulatorySchedules { get; set; }
        public Info Info { get; set; }
        public Diagnostic Diagnostic { get; set; }
    }
}