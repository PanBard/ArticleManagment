using DocumentFormat.OpenXml.Vml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleManage
{
    internal class MethodsArchive
    {
       

        public MethodsArchive() 
        {
         
        }

        public void changeCSVFile(FoldersStructure folders) //method for change csv file for samples features
        {
            var saving_folder_path = folders.output_folders.folderPath.Replace("output_folders\\", "") + "output_new_csv_files_v2";
            bool exists = System.IO.Directory.Exists(saving_folder_path);
            if (!exists)
            {
                System.IO.Directory.CreateDirectory(saving_folder_path);
                Console.Write($"Create new folder");
            }

            var csv_files_names = folders.input_sample_features.filesNames;

            foreach (var name in csv_files_names)
            {
                String first_row = "Figure_number,Sample_name,Total_surface_area[m2/g],Total_pore_volume[cm3/g],Micropore_volume[cm3/g],Mesopore_volume[cm3/g],Average_pore_diameter[nm],Activation_temperature[stC],Activation_time[min],Impregnation_ratio[agent/char],Activation_type,Activation_agent,Material_type" + "\n";
                var data = File.ReadAllLines(folders.input_sample_features.folderPath + name).Select(a => a.Split(",")); //skip(1) for 1 row
                var elo = data.ToArray();
                String new_data = "";
                Console.WriteLine("count: " + data.Count());
                for (int i = 0; i < data.Count(); i++)
                {
                    if(i==0)
                    {
                        new_data += first_row;
                    }
                    else
                    {
                        var line = elo[i];
                        for (int j = 0; j < line.Count(); j++)
                        {

                            if (j == 9)
                            {
                                new_data += ",,";
                                Console.Write($",");
                            }
  
                            new_data += $"{line[j]}";
                            Console.Write($"{line[j]}");
                            if (j != line.Count() - 1)
                            {
                                new_data += $",";
                                Console.Write($",");
                            }
                        }
                        new_data += "\n";
                        Console.WriteLine();
                    }
                   
                }
                Console.WriteLine();

                var new_file_path_to_save = saving_folder_path + "\\" + name;

                if (!File.Exists(new_file_path_to_save))
                {
                    //Console.WriteLine($"File {fileName} was created in {filePath}");
                    File.WriteAllText(new_file_path_to_save, new_data);
                }

            }
        }


        public void readCSVFile(FoldersStructure folders)
        {
            var data = File.ReadAllLines(folders.input_sample_features.folderPath + "Data54_features.csv").Select(a => a.Split(",")); //skip(1) for 1 row
            var elo = data.ToList();
            foreach (var line in data)
            {
                for(int i = 0; i<line.Count(); i++ ) 
                {
                    Console.Write($"{elo[0][i]}={line[i]} | ");
                }
                //foreach (var field in line)
                //{
                //    Console.Write($"{field},     ");
                //}                
                Console.WriteLine();
            }
        }

        //AU Naile Karakehya,
        //T1 Effects of one-step and two-step KOH activation method on the properties and supercapacitor performance of highly porous activated carbons prepared from Lycopodium clavatum spores,
        //JO Diamond and Related Materials,
        //VL Volume 135,
        //PY 2023,
        //SP 109873,
        //DO https://doi.org/10.1016/j.diamond.2023.109873.

        //AU Xin Cui, Yuchen Jiang, Zhifeng He, Zeyi Liu, Xiaoyang Yang, Jiafeng Wan, Yifu Liu, Fangwei Ma,
        //T1 Preparation of tank-like resin-derived porous carbon sphere for supercapacitor: The influence of KOH activator and activation temperature on structure and performance,
        //JO Diamond and Related Materials,
        //VL Volume 136,
        //PY 2023,
        //SP 110054,
        //DO https://doi.org/10.1016/j.diamond.2023.110054.

        

        public List<Article> returnArticleWithGeneratedCitation(List<Article> articles)
        {
            foreach(Article article in articles)
            {
               article.Citation = generateCitation(article);
            }

         return articles;
        }


        public String generateCitation(Article article)
        {
            String citation = "";
            citation += generateNiceAutors(article);
            citation += article.PrimaryTitle.Trim() + ", ";
            citation += article.AbbreviationJournalName + ", ";
            citation += article.Volume + ", ";
            citation += article.PublicationYear + ", ";
            citation += article.StartPage + ", ";
            citation += article.DOI ;
            return citation;
        }

        private String generateNiceAutors(Article article)
        {
            List<String> autors = new List<string>();
            String allAutors = "";

            foreach (String autor in article.Autors)
            {
                autors.Add(autor.Replace("|","").Trim());
            }
            foreach (String autor in autors)
            {
                
                int leng = autor.Length;
                for (int i = 0; i < leng; i++)
                {

                    if (autor[i] == ',')
                    {
                        String letter = autor.Substring(i + 1).Trim();
                        allAutors += autor.Replace(letter, "") + letter[0] + "." + "; ";
                        break;
                    }
                }
                
            }
            allAutors = allAutors.Substring(0,(allAutors.Length-2)) + " ";
            return allAutors;
        }


        /// <summary>
        /// filePaths = for no or for yes ris csv_file_name
        /// </summary>
        /// <param name="filePaths"></param>
        /// <returns></returns>
        public List<Article> returnArticleWithDataFromRISfiles(List<String> filePaths, String dirPath, FoldersStructure folders)
        {
            List<Article> articles = new List<Article>();
            int id = 0;
            foreach (String path in filePaths)
            {
                String risFileName = path.Replace(dirPath, "").Replace(".ris", "");
                String[] ris = File.ReadAllLines(path);

                id++;
                Article article = new Article();
                article.Id = id;
                article.FileName = risFileName;
                foreach (String line in ris)
                {
                    if (line.Length > 1)
                    {
                        String code = line[0].ToString() + line[1].ToString();

                        if (code == "AU" || code == "A1")
                        {                           
                            article.Autors.Add(line.Replace("AU  - ","").Replace("A1  - ", "") + "|");
                        }

                        if (code == "T1" || code =="TI")
                        {                          
                            article.PrimaryTitle = line.Replace("T1  - ", "").Replace("TI  - ", "");
                        }

                        if (code == "JO" || code == "T2")
                        {
                            article.AbbreviationJournalName = line.Replace("JO  - ", "").Replace("T2  - ", "").Trim();
                        }

                        if (code == "VL")
                        {
                            article.Volume = line.Replace("VL  - ", "");
                        }

                        if (code == "IS")
                        {
                            article.Issue = line.Replace("IS  - ", "");
                        }

                        if (code == "PY" || code == "Y1")
                        {
                            article.PublicationYear = line.Replace("PY  - ", "").Replace("Y1  - ", "");
                        }

                        if (code == "SP")
                        {
                            article.StartPage = line.Replace("SP  - ", "");
                        }

                        if (code == "EP")
                        {
                            article.Endpage = line.Replace("EP  - ", "");
                        }

                        if (code == "SN")
                        {
                            article.ISSN = line.Replace("SN  - ", "");
                        }

                        if (code == "DO")
                        {
                            String w = line.Replace("DO  - ", "");
                            if (!w.Contains("http"))
                            {
                                article.DOI = "https://doi.org/" + line.Replace("DO  - ", "");
                            }
                            else article.DOI = line.Replace("DO  - ", "");
                            
                        }

                        if (code == "TY")
                        {
                            String type = line.Replace("TY  - ", "");
                            switch (type)
                            {
                                case "JOUR":
                                    article.Type = "Journal Article";
                                    break;

                                case "EJOUR":
                                    article.Type = "Electronic Article";
                                    break;
                            }                                                       
                        }

                        if (code == "UR")
                        {
                            article.URL = line.Replace("UR  - ", "");
                        }
                    }
                }

                //[[[[[[[[[[[[            
                string[] title = article.PrimaryTitle.Split(null);
                String new_title = $"Data {article.Id} {title[0]} {title[1]} {title[2]} {title[3]}... .pdf";
                article.FormalNicelyPDFName = new_title;
                //[[[[[[[[[[[[
          
                //;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;
                String temp_graph_folder_path = folders.temp_graph.folderPath + article.FileName;
                if (Directory.Exists(temp_graph_folder_path) )
                {
                    DirectoryInfo dir = new DirectoryInfo(temp_graph_folder_path);
                    DirectoryInfo[] inside_main_graph_dir = dir.GetDirectories();

                    var allSamplesFeatures = GetSampleFeaturesFromOneCSVFile(folders, article.FileName);
                  
                    foreach (var graph_directory in inside_main_graph_dir)
                    {                        
                        FileInfo[] Files = graph_directory.GetFiles();

                        List<string> csv_file_name = new List<string>();                       
                        Graph new_graph = new Graph();

                        foreach (var file in Files)
                        {                            
                            csv_file_name.Add(file.FullName);
                             
                            var reader = new StreamReader(File.OpenRead(file.FullName));
                            List<string> listX = new List<string>();
                            List<string> listY = new List<string>();
                            List<float> floatX = new List<float>();
                            List<float> floatY = new List<float>();
                            while (!reader.EndOfStream)
                            {
                                var line = reader.ReadLine();
                                var values = line.Split(',');
                                listX.Add(values[0]);
                                listY.Add(values[1]);
                            }
                            listX.RemoveRange(0, 2);
                            listY.RemoveRange(0, 2);

                            Isotherm newIsotherm = new Isotherm();

                            foreach (var coloumn1 in listX) newIsotherm.XAxisData.Add(Convert.ToSingle(coloumn1));
                            foreach (var coloumn1 in listY) newIsotherm.YAxisData.Add(Convert.ToSingle(coloumn1));

                            string[] name = file.FullName.Replace(graph_directory.FullName, "").Replace(".csv", "").Replace("\\", "").Replace("[", "").Replace("]", "").Split(' ');

                            var calibrate = name[3].Split('&');
                            if (!name[3].Contains('&'))
                            {
                                calibrate = name[4].Split('&');
                            }

                            newIsotherm.FileName = article.FileName;
                            newIsotherm.SampleName = name[2];
                            newIsotherm.FigureNumber = name[1];
                            newIsotherm.MaxX = Convert.ToSingle(calibrate[1]);
                            newIsotherm.MinX = Convert.ToSingle(calibrate[0]);
                            newIsotherm.MaxY = Convert.ToSingle(calibrate[3]);
                            newIsotherm.MinY = Convert.ToSingle(calibrate[2]);

                            //-------------11-04-24
                            foreach (var feature in allSamplesFeatures)
                            {
                                if(feature.Sample_name == newIsotherm.SampleName && feature.Figure_number == newIsotherm.FigureNumber) 
                                {
                                    newIsotherm.SampleFeatures = feature;
                                }
                            }
                            //-------------11-04-24

                            new_graph.Isotherms.Add(newIsotherm);
                            article.IzothermNumber += 1;
                                                                                
                        }
                        article.Graphs.Add(new_graph);                   
                        article.GraphNumber = article.Graphs.Count;
                        //Console.WriteLine($" {article.FileName} -> csv file = {article.IzothermNumber}  graphs={article.Graphs.Count} ");
                    }

                }
                //;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;


                articles.Add(article);
            }
            return articles;
        }



       
        List<SampleFeatures> GetSampleFeaturesFromOneCSVFile(FoldersStructure folders, String articleFileName)
        {
            List<SampleFeatures> sampleFeatures = new List<SampleFeatures>();
            
            String samleFeatures_folder_path = folders.input_sample_features.folderPath + articleFileName + "_features.csv";
            if (File.Exists(samleFeatures_folder_path))
            {
                //Console.WriteLine(samleFeatures_folder_path);
                var sampleCSVReader = new StreamReader(File.OpenRead(samleFeatures_folder_path));
                List<String[]> samples = new List<string[]>();
                while (!sampleCSVReader.EndOfStream)
                {
                    var lines = sampleCSVReader.ReadLine().Split(',');
                    samples.Add(lines);

                }
                samples.RemoveAt(0);
                foreach (var line in samples)
                {
                    
                    SampleFeatures oneSampleFeature = new SampleFeatures();
                    oneSampleFeature.Figure_number = line[0];
                    oneSampleFeature.Sample_name = line[1];
                    oneSampleFeature.Total_surface_area = line[2];
                    oneSampleFeature.Total_pore_volume   = line[3];
                    oneSampleFeature.Micropore_volume = line[4];
                    oneSampleFeature.Mesopore_volume = line[5];
                    oneSampleFeature.Average_pore_diameter = line[6];
                    oneSampleFeature.Activation_temperature = line[7];
                    oneSampleFeature.Activation_time    = line[8];
                    oneSampleFeature.Impregnation_ratio = line[9];
                    oneSampleFeature.Activation_type = line[10];
                    oneSampleFeature.Activation_agent = line[11];
                    oneSampleFeature.Material_type = line[12];
                    sampleFeatures.Add(oneSampleFeature);
                }
            }

            //foreach (var item in sampleFeatures)
            //{
            //    Console.WriteLine( $"{item.Sample_name} - BET[{item.Total_surface_area}]");
            //}

            return sampleFeatures;
        }




        ///// <summary>
        ///// identificatior = "MK" or "MKZ"
        ///// </summary>
        ///// <param name="text"></param>
        ///// <param name="indetificator"></param>
        ///// <returns></returns>
        //public List<Article> insertContactAndFiguresToArticles(string[] text, List<Article> articles)
        //{
        //    foreach(Article article in articles) 
        //    {
        //        for (int i = 0; i < text.Length; i++)
        //        {
        //            if (text[i].Contains(article.FileName))
        //            {                        
                       
        //                article.ContactPerson = text[i + 1].Trim();
        //                article.Email = text[i + 2].Trim();
        //                article.AdsorptionIsotherms = text[i + 3].Trim().Replace("Adsorption:","");
        //                article.PoreDistribution = text[i + 4].Trim().Replace("Pore:", "");
                        
        //            }
        //        }
        //    }                       
        //    return articles;
        //}

        

    }


}
