﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using JobSkillsDb.Entities;
using CsvHelper;
using System.Data.Entity;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.CompilerServices;

//using Parser;

namespace Apriori
{
    
    class Program
    {
        public static void ExportBasketType(List<Vacancy> vacancies, string fileName = "")
        {
            Contract.Requires<ArgumentNullException>(vacancies.Count == 0);
            
            string name = string.IsNullOrEmpty(fileName)
                ? @"session#" + Guid.NewGuid().ToString() + ".csv"
                : fileName;

            using (var writingStream = new StreamWriter(name, false, Encoding.UTF8))
            {
                var fileWriter = new CsvWriter(writingStream);
                //var recordContainer = "";

                //const string separator = ",";
                int maxSkillsetLength = 0;
                int optimalSkillsetLength = 0;

                IEnumerable<Skill> maxSet = null;
                IEnumerable<Skill> optimalMaxSet = null;

                int idOfMaxVacancy = 0;
                int idOfOptimalMaxVacancy = 0;

                Console.WriteLine("SEARCH MAX...");
                foreach (var vac in vacancies)
                {
                    if (vac.Skills.Count > maxSkillsetLength)
                    {

                        //return previous fact of condition
                        optimalSkillsetLength = maxSkillsetLength;
                        idOfOptimalMaxVacancy = idOfMaxVacancy;
                        optimalMaxSet = maxSet;

                        //return fact of condition
                        maxSkillsetLength = vac.Skills.Count;
                        maxSet = vac.Skills;
                        idOfMaxVacancy = vac.Id;
                    }
                }

                Console.WriteLine("WRITE TO FILE...");
                foreach (var item in optimalMaxSet)
                {
                    fileWriter.WriteField(item.Name);
                }
                fileWriter.NextRecord();

                foreach (var vac in vacancies.Where(x => x.Id != idOfMaxVacancy && x.Id != idOfOptimalMaxVacancy))
                {
                    if (vac.Skills.Count == 0) continue;
                    //if (!((IEnumerable<dynamic>) vac.Skills).(x => x.Name != "продажи" || x.Name != "продажи"))
                    //if (!((IEnumerable<dynamic>) vac.Skills).Where(p => p.Name.To))
                    
                    continue;

                    foreach (var skill in vac.Skills)
                    {
                        fileWriter.WriteField(skill.Name);
                    }
                    fileWriter.NextRecord();
                }
            }

        }

        public void ExportSignleType(string fileName = "")
        {

        }

        public static void WriteProgress()
        {
            Console.WriteLine(".");
        }

        static void Main(string[] args)
        {
            Console.WriteLine("WAIT");
            //var vacancies = new JobSkillsContext().Vacancies()
            var context = new JobSkillsContext();

            var minsupport = (decimal) 0.4;
           var minconfidence = (decimal) 0.4;

            //var vacancies = context.Vacancies.Include(s => s.Skills).Select(s => s.Skills).ToList();
            //var vacancies = (from x in context.Vacancies.Include(x => x.Skills)
            //    select x).ToList();

            var vacancies = context.Vacancies.Include(v => v.Skills)
                .Select(v => new  {v.Id, v.Skills}).ToList()
                .Select(v => new Vacancy()
            {
                Id = v.Id,
                Skills = v.Skills
            }).ToList();
            
            //Debug.WriteLine(DateTime.Now);
            //var vacancies = context.Vacancies.Include(x => x.Skills).Select(x => new {x.Skills}).ToList();
            //Debug.WriteLine(DateTime.Now);
            

            ExportBasketType(vacancies);
            Console.WriteLine("OK");
            Console.ReadLine();

            #region old - Contains custom apriori implementation

            //var givenSkills = context.Skills.ToList();
            //var givenVacancies = context.Vacancies.ToList();

            //var givenVacancies = context.Vacancies.Take(5).ToList();
            //foreach (var vac in givenVacancies)
            //{
            //    Console.WriteLine("name -> {0} / id -> {1}", vac.Title, vac.Id);
            //    foreach (var skill in vac.Skills)
            //    {
            //        Console.WriteLine("           -> {0}", skill.Name);
            //    }
            //}

            //var givenSkills = new List<Skill>();

            //givenSkills.AddRange(givenVacancies.ElementAt(0).Skills);
            //givenSkills.AddRange(givenVacancies.ElementAt(1).Skills);
            //givenSkills.AddRange(givenVacancies.ElementAt(2).Skills);
            //givenSkills.AddRange(givenVacancies.ElementAt(3).Skills);
            //givenSkills.AddRange(givenVacancies.ElementAt(4).Skills);

            //AprioriProcessTransactions process = new AprioriProcessTransactions(minsupport, minconfidence, givenSkills,
            //    givenVacancies);

            #endregion
        }
        
    }
}
