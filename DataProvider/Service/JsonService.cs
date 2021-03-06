﻿
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using System.IO;
using Newtonsoft.Json.Linq;
using DataProvider.ServiceAbstraction;
using DataProvider;
using DataProvider.Model;
using System;

namespace DataProvider.Service
{
    public class JsonService : IJsonService
    {
        private readonly string _jsonDataFilePath;
        public JsonService(IOptions<AppSettings> appsettings)
        {
            _jsonDataFilePath = appsettings.Value.JsonDataFile;
        }

        public List<Horse> GetHorsesFromJsonService()
        {
            List<Horse> horseList = new List<Horse>();
            try
            {
                var jsonData = JObject.Parse(File.ReadAllText(_jsonDataFilePath));

                if (string.IsNullOrEmpty(jsonData.ToString())) return null;

                var markets = jsonData["RawData"]["Markets"];

                var selections = markets[0]["Selections"];

                foreach (var selection in selections)
                {
                    var horse = new Horse
                    {
                        Price = (double)selection["Price"],
                        Name = selection["Tags"]["name"].ToString()
                    };
                    horseList.Add(horse);

                }
                return horseList;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception getting horses from json service: {e}");
                throw;
            }
        }
    }

} 
