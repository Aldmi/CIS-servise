using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataExchange.WebClient;
using Domain.Entities;
using Quartz;

namespace DataExchange.Quartz.Jobs
{
    public class QuartzJobGetRegSh : IJob
    {
        public async void Execute(IJobExecutionContext context)
        {
            //Получение данных установленных в dataMap для данного ключа job
            JobDataMap dataMap = context.JobDetail.JobDataMap;
            var apkDk = dataMap["ApkDkWebClient"] as ApkDkWebClient;
            var stationsOwner = dataMap["stationsOwner"] as IEnumerable<Station>;

            if(apkDk == null || stationsOwner == null)
                return;

            //foreach (var stOwner in stationsOwner)
            //{
            //    apkDk.LoadHttpDataInDb("regular", stOwner);
            //}


            //var tasks = stationsOwner.Select(st => apkDk.LoadHttpDataInDb("regular", st)).ToArray();
            //Task.WaitAll(tasks);

            //DEBUG
           

            string pathShed = @"G:\XML файлы\Нормативное\doc_19155_Курская.xml";
            string pathStat = @"G:\XML файлы\ESRList.xml";
            var stat = stationsOwner.FirstOrDefault(st => st.Name == "Курский");
            await apkDk.LoadXmlDataInDb(pathShed, pathStat,"regular", stat);


            pathShed = @"G:\XML файлы\Нормативное\doc_19612_Рижская.xml";
            stat = stationsOwner.FirstOrDefault(st => st.Name == "Рижский");
            await apkDk.LoadXmlDataInDb(pathShed, pathStat, "regular", stat);


            pathShed = @"G:\XML файлы\Нормативное\doc_19351_Павелецкая.xml";
            stat = stationsOwner.FirstOrDefault(st => st.Name == "Павелецкий");
            await apkDk.LoadXmlDataInDb(pathShed, pathStat, "regular", stat);


            pathShed = @"G:\XML файлы\Нормативное\doc_19600_Савельевский.xml";
            stat = stationsOwner.FirstOrDefault(st => st.Name == "Савеловский");
            await apkDk.LoadXmlDataInDb(pathShed, pathStat, "regular", stat);




            //new Station { Name = "Курский", EcpCode = 19155 },
            //new Station { Name = "Павелецкий", EcpCode = 19351 },
            //new Station { Name = "Казанский", EcpCode = 19390 },
            //new Station { Name = "Ярославский", EcpCode = 19550 },
            //new Station { Name = "Савеловский", EcpCode = 19600 },
            //new Station { Name = "Рижский", EcpCode = 19612 },
            //new Station { Name = "Киевский", EcpCode = 19810 },
            //new Station { Name = "Смоленский", EcpCode = 19823 }
        }
    }
}