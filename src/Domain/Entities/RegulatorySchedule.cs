using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    /// <summary>
    /// Нормативное расписание поездов.
    /// Нормативное расписание движения поездов содержит сведения обо всех пассажирских и пригородных поездах следующих по станции с учетом вводимых изменений.
    /// Глубина предоставления нормативного  расписания по умолчанию составляет:
    ///	для пассажирских поездов – 60 суток;
    ///	для пригородных поездов – 21 суток.
    /// Данные нормативного расписания предназначены для формирования информации на табло с общим расписанием движения поездов по станции
    /// Пересчет нормативного расписания осуществляется один раз в сутки.
    /// </summary>
    public class RegulatorySchedule : IEntitie
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int NumberOfTrain { get; set; }                      //Номер поезда в расписании

        [MaxLength(100)]
        public string RouteName { get; set; }                       //Станция отправления и станция назначения, а также фирменное название поезда, если есть.

        public virtual Station DispatchStation { get; set; }        //Станция отправления

        public virtual Station StationOfDestination { get; set; }   //Станция назначения

        [Column(TypeName = "datetime2")]
        [Required]
        public DateTime ArrivalTime { get; set; }                  //Время прибытия поезда на станцию

        [Column(TypeName = "datetime2")]
        [Required]
        public DateTime DepartureTime { get; set; }                //Время отправления поезда со станции

        public string DaysFollowings { get; set; }                 //Дни следования поезда(ежедневно, четные, по рабочим и т.п.)

        public virtual ObservableCollection<Station> ListOfStops { get; set; }      //Список станций где останавливается поезд (Заполнятся только для пригородных поездов)

        public virtual ObservableCollection<Station> ListWithoutStops { get; set; } //Список станций которые поезд проезжает без остановки (Заполнятся только для пригородных поездов)
    }
}