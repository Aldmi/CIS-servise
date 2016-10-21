using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Migrations.Model;

namespace Domain.Entities
{
    /// <summary>
    /// Номер пути прибытия и время опоздания.
    /// Данные информационного потока предназначены для формирования информации на  табло содержащих сведения о  ближайших поездах, а также платформенных указателях. 
    /// Пересчет данных номера пути(платформы) прибытия и времени опоздания производится непрерывно в режиме реального времени для поездов из списка оперативного расписанияиз списка оперативного расписания.
    /// Не содержит поезда уже прибывшие или отправившиеся  со станции. 
    /// </summary>
    public class Info
    {
        [Key]
        public int Id { get; set; }

        public Station DispatchStation { get; set; }       //Станция отправления

        public Station StationOfDestination { get; set; }  //Станция назначения

        public DateTime ArrivalTime { get; set; }          //Время прибытия поезда на станцию

        public DateTime DepartureTime { get; set; }        //Время отправления поезда со станции

        public int Platform { get; set; }                 //Номер платформы прибытия поезда, если еще неизвестен, то равен 0.

        public int Way { get; set; }                      //Номер пути прибытия поезда, если еще неизвестен, то равен 0.

        public string RouteName { get; set; }              //Станция отправления и станция назначения, а также фирменное название поезда, если есть.

        public int Lateness { get; set; }          //Количество минут опоздания от графика движения
    }
}