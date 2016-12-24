using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    /// <summary>
    /// Оперативное расписание поездов.
    /// Содержит список пассажирских и пригородных поездов следующих по станции, назначенных на текущие и следующие сутки. 
    /// Данные оперативного расписания предназначены для формирования информации на  табло содержащих сведения о ближайших поездах по станции.
    /// Пересчет оперативного расписания осуществляется два раза в сутки.
    /// </summary>
    public class OperativeSchedule : IEntitie
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(10)]
        public string NumberOfTrain { get; set; }                                     //Номер поезда в расписании

        [Required]
        [MaxLength(100)]
        public string RouteName { get; set; }                                          //Станция отправления и станция назначения, а также фирменное название поезда, если есть.

        public virtual Station DispatchStation { get; set; }                           //Станция отправления

        public virtual Station DestinationStation { get; set; }                        //Станция назначения

        [Column(TypeName = "datetime2")]
        public DateTime? ArrivalTime { get; set; }                                      //Время прибытия поезда на станцию

        [Column(TypeName = "datetime2")]
        public DateTime? DepartureTime { get; set; }                                     //Время отправления поезда со станции

        public virtual ObservableCollection<Station> ListOfStops { get; set; }          //Список станций где останавливается поезд (Заполнятся только для пригородных поездов)

        public virtual ObservableCollection<Station> ListWithoutStops { get; set; }     //Список станций которые поезд


        #region FK

        [Required]
        public RailwayStation RailwayStation { get; set; }                              //Обязательно относится к вокзалу            

        #endregion
    }
}