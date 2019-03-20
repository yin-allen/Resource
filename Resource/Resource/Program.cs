using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Resource
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Resource> englishResources = new List<Resource>();
            List<Resource> chineseResources = new List<Resource>();
            string target = "iBank.TermDeposit";
            string chineseVersionFilePath = @"C:\Users\allen\Desktop\Team\中文\" + target + @"\resources.txt";
            string englishVersionFilePath = @"C:\Users\allen\Desktop\Team\英文\" + target + @"\resources.txt";
            string exportPath = @"C:\Users\allen\Desktop\Team\結果\" + target + @"\";
            string exportString = "";

            //將新光提供的翻譯轉到原本資料 string exportString = test(englishResources, chineseResources, chineseVersionFilePath, englishVersionFilePath); 
            //從中文找出英文沒有的結果 exportString = test2(englishResources, chineseResources, chineseVersionFilePath, englishVersionFilePath, exportString);
            //找出英文KEY重複            
            exportString = test3(englishResources, englishVersionFilePath, exportString);

            //從英文中找出未翻譯的 exportString = test4(englishResources, englishVersionFilePath, exportString);



            using (StreamWriter outputFile = new StreamWriter(Path.Combine(exportPath, "重複KEY.txt")))
            {
                outputFile.WriteLine(exportString);
            }
        }

        //從英文中找出未翻譯的 
        private static string test4(List<Resource> englishResources, string englishVersionFilePath, string exportString)
        {
            using (var streamReader = new StreamReader(englishVersionFilePath))
            {
                while (!streamReader.EndOfStream)
                {
                    var readLine = streamReader.ReadLine().Trim();
                    var split = readLine.Split(":");
                    Resource resource = new Resource();
                    resource.key = split[0];
                    if (split.Length >= 2)
                        resource.value = split[1].Replace(",", "");
                    englishResources.Add(resource);
                }
            }

            foreach (var item in englishResources)
            {
                System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"^[A-Za-z]+$");
                if (!string.IsNullOrEmpty(item.value))
                {
                    var split = item.value.Split("\"");
                    var charFist = split[1].ToCharArray();
                    if (!regex.IsMatch(charFist[0].ToString()))
                        exportString += item.key + ":" + item.value + ", \n";
                }
            }

            return exportString;
        }

        /// <summary>
        /// 找出英文KEY重複
        /// </summary>
        /// <param name="englishResources"></param>
        /// <param name="englishVersionFilePath"></param>
        /// <param name="exportString"></param>
        /// <returns></returns>
        private static string test3(List<Resource> englishResources, string englishVersionFilePath, string exportString)
        {
            using (var streamReader = new StreamReader(englishVersionFilePath))
            {
                while (!streamReader.EndOfStream)
                {
                    var readLine = streamReader.ReadLine().Trim();
                    var split = readLine.Split(":");
                    Resource resource = new Resource();
                    resource.key = split[0];
                    if (split.Length >= 2)
                        resource.value = split[1].Replace(",", "");
                    englishResources.Add(resource);
                }
            }

            foreach (var item in englishResources)
            {
                var result = englishResources.Where(resource => resource.key == item.key).Count();
                if (result >= 2 && !string.IsNullOrEmpty(item.key))
                    exportString += item.key + "\n";
            }

            return exportString;
        }


        /// <summary>
        /// 從中文找出英文沒有的結果 
        /// </summary>
        /// <param name="englishResources"></param>
        /// <param name="chineseResources"></param>
        /// <param name="chineseVersionFilePath"></param>
        /// <param name="englishVersionFilePath"></param>
        /// <param name="exportString"></param>
        /// <returns></returns>
        private static string test2(List<Resource> englishResources, List<Resource> chineseResources, string chineseVersionFilePath, string englishVersionFilePath, string exportString)
        {
            using (var streamReader = new StreamReader(englishVersionFilePath))
            {
                while (!streamReader.EndOfStream)
                {
                    var readLine = streamReader.ReadLine().Trim();
                    var split = readLine.Split(":");
                    Resource resource = new Resource();
                    resource.key = split[0];
                    if (split.Length >= 2)
                        resource.value = split[1].Replace(",", "");
                    englishResources.Add(resource);
                }
            }

            using (var streamReader = new StreamReader(chineseVersionFilePath))
            {
                while (!streamReader.EndOfStream)
                {
                    var readLine = streamReader.ReadLine().Trim();
                    var split = readLine.Split(":");
                    Resource resource = new Resource();
                    resource.key = split[0];

                    if (split.Count() >= 2)
                        resource.value = split[1].Replace(",", "");

                    chineseResources.Add(resource);
                }
            }

            List<Resource> ListTemp = new List<Resource>();

            for (int i = 0; i < chineseResources.Count; i++)
            {
                var resource = englishResources.Where(item => item.key.Contains(chineseResources[i].key)).FirstOrDefault();
                if (chineseResources[i].key != string.Empty)
                {
                    if (resource == null)
                    {
                        Resource resourceTemp = new Resource();
                        resourceTemp.key = chineseResources[i].key;
                        resourceTemp.value = chineseResources[i].value;
                        exportString += chineseResources[i].key + ":" + chineseResources[i].value + ", \n";
                        ListTemp.Add(resourceTemp);
                    }
                }
            }

            return exportString;
        }

        /// <summary>
        /// 將新光提供的翻譯轉到原本資料
        /// </summary>
        /// <param name="englishResources"></param>
        /// <param name="chineseResources"></param>
        /// <param name="chineseVersionFilePath"></param>
        /// <param name="englishVersionFilePath"></param>
        /// <param name="ttt"></param>
        /// <returns></returns>
        private static string test(List<Resource> englishResources, List<Resource> chineseResources, string chineseVersionFilePath, string englishVersionFilePath)
        {
            using (var streamReader = new StreamReader(englishVersionFilePath))
            {
                while (!streamReader.EndOfStream)
                {
                    var readLine = streamReader.ReadLine().Trim();
                    var split = readLine.Split(":");
                    Resource resource = new Resource();
                    resource.key = split[0];
                    resource.value = split[1].Replace(",", "");
                    englishResources.Add(resource);
                }
            }

            using (var streamReader = new StreamReader(chineseVersionFilePath))
            {
                while (!streamReader.EndOfStream)
                {
                    var readLine = streamReader.ReadLine().Trim();
                    var split = readLine.Split(":");
                    Resource resource = new Resource();
                    resource.key = split[0];

                    if (split.Count() >= 2)
                        resource.value = split[1].Replace(",", "");

                    chineseResources.Add(resource);
                }
            }


            var exportString = "";
            for (int i = 0; i < chineseResources.Count(); i++)
            {
                var resource = englishResources.Where(item => item.key.Contains(chineseResources[i].key)).FirstOrDefault();

                if (chineseResources[i].key == string.Empty)
                {
                    chineseResources[i].key = "";
                    chineseResources[i].value = " \n";
                    exportString += chineseResources[i].key + chineseResources[i].value;
                }
                else
                {
                    if (resource != null)
                        chineseResources[i].value = resource.value;
                    else
                        chineseResources[i].value = chineseResources[i].value;
                    exportString += chineseResources[i].key + ":" + chineseResources[i].value + ", \n";
                }
            }

            return exportString;
        }
    }

    class Resource
    {
        public string key { get; set; }
        public string value { get; set; }
    }
}
