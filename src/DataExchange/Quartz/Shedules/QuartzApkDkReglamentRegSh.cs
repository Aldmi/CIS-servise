using System;
using System.Collections.Generic;
using Castle.Facilities.Startable;
using DataExchange.Quartz.Jobs;
using DataExchange.WebClient;
using Domain.Entities;
using Quartz;
using Quartz.Impl;

namespace DataExchange.Quartz.Shedules
{
    public class QuartzApkDkReglamentRegSh
    {
        public static void Start(ApkDkWebClient apkDkWebClient, IEnumerable<Station> stationsOwner )
        {
            //Планировщик
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();

            //Заполнение словаря пользовательских данных
            JobDataMap dataMap = new JobDataMap
            {
                ["ApkDkWebClient"] = apkDkWebClient,
                ["stationsOwner"] = stationsOwner
            };

            //Создание объекта работы и установка данных для метода Execute
            IJobDetail job = JobBuilder.Create<QuartzJobGetRegSh>()
                .WithIdentity("getRegShJob", "group1")                 //идентификатор работы (по нему можно найти работу)
                .SetJobData(dataMap)
                .Build();


            //Создание первого условия сработки
            ITrigger triggerReg1 = TriggerBuilder.Create()             // создаем триггер
                .WithIdentity("triggerReg1", "group1")                 // идентифицируем триггер с именем и группой
                .StartAt(DateTimeOffset.Now.AddSeconds(5))             //старт тригера и первый вызов через 5 сек
                .WithSimpleSchedule(x => x                             // далее 5 вызовов с интервалом 5 сек
                    .WithIntervalInSeconds(20)                         // 
                    .WithRepeatCount(500))
               .ForJob(job)
               .Build(); // создаем триггер 


            //Создание второго условия сработки
            ITrigger triggerReg2 = TriggerBuilder.Create()
                .WithIdentity("triggerReg2", "group1")
                .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(13, 19))          //1 раз в сутки в 13:19
                .ForJob(job)
                .Build();

            //Связывание объекта работы с тригером внутри планировщика
            scheduler.ScheduleJob(job, triggerReg1);
            scheduler.ScheduleJob(triggerReg2);

            //запуск планировщика
            scheduler.Start();
        }


        public static void Stop()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            JobKey job = new JobKey("getRegShJob", "group1");
            scheduler.DeleteJob(job);
        }


        public static void Shutdown()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Shutdown(true);
        }
    }
}