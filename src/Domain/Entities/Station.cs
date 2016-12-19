using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Range(1, Int32.MaxValue)]
        public int EcpCode { get; set; }

        [Required(ErrorMessage = "Введите название станции")]
        public string Name { get; set; }

        public string Description { get; set; }

        public virtual ICollection<OperativeSchedule> OperativeSchedulesListOfStops { get; set; }          //Многие ко многим с OperativeSchedule.ListOfStops

        public virtual ICollection<OperativeSchedule> OperativeSchedulesListWithoutStops { get; set; }     //Многие ко многим с OperativeSchedule.ListWithoutStops 

        public virtual ICollection<RegulatorySchedule> RegulatorySchedulesListOfStops { get; set; }         //Многие ко многим с RegulatorySchedule.ListOfStops

        public virtual ICollection<RegulatorySchedule> RegulatorySchedulesListWithoutStops { get; set; }   //Многие ко многим с RegulatorySchedule.ListWithoutStops 

        public virtual ICollection<RailwayStation> RailwayStations { get; set; }                            //Многие ко многим с RailwayStation (для вывода всех станций по вокзалу) 
    } 
}
