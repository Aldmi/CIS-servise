﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        public uint NumberOfTrain { get; set; }             //Номер поезда в расписании

        public string RouteName { get; set; }               //Станция отправления и станция назначения, а также фирменное название поезда, если есть.

        public Station DispatchStation { get; set; }        //Станция отправления

        public Station StationOfDestination { get; set; }   //Станция назначения

        public DateTime ArrivalTime { get; set; }           //Время прибытия поезда на станцию

        public DateTime DepartureTime { get; set; }         //Время отправления поезда со станции

        public string DaysFollowing { get; set; }           //Дни следования поезда(ежедневно, четные, по рабочим и т.п.)

        public List<Station> ListOfStops { get; set; }      //Список станций где останавливается поезд (Заполнятся только для пригородных поездов)

        public List<Station> ListWithoutStops { get; set; } //Список станций которые поезд проезжает без остановки (Заполнятся только для пригородных поездов)
    }
}