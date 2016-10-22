using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    /// <summary>
    /// ЖД Станция.
    /// </summary>
    public class Station
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите Ecp код станции")]
        public int EcpCode { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<OperativeSchedule> OperativeSchedulesListOfStops { get; set; }        //Многие ко многим с OperativeSchedule.ListOfStops

        public List<OperativeSchedule> OperativeSchedulesListWithoutStops { get; set; }   //Многие ко многим с OperativeSchedule.ListWithoutStops 

        public List<RailwayStation> RailwayStations { get; set; }                         //Многие ко многим с RailwayStation (для вывода всех станций по вокзалу) 
    }
}
