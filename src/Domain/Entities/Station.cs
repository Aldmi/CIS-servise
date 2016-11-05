using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    /// <summary>
    /// ЖД Станция.
    /// </summary>
    public class Station : IEntitie
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите Ecp код станции")]
        public int EcpCode { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public virtual ICollection<OperativeSchedule> OperativeSchedulesListOfStops { get; set; }        //Многие ко многим с OperativeSchedule.ListOfStops

        public virtual ICollection<OperativeSchedule> OperativeSchedulesListWithoutStops { get; set; }   //Многие ко многим с OperativeSchedule.ListWithoutStops 

        public virtual ICollection<RailwayStation> RailwayStations { get; set; }                         //Многие ко многим с RailwayStation (для вывода всех станций по вокзалу) 
    }
}
