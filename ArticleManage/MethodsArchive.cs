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

                //System.Console.WriteLine(article.DOI);
                String folder_path = folders.output_folders.folderPath + article.FormalNicelyPDFName.Replace("... .pdf", "") + "\\graphs";
                if (Directory.Exists(folder_path))
                {
                    DirectoryInfo dir = new DirectoryInfo(folder_path);
                    FileInfo[] Files = dir.GetFiles();
                    List<string> csv_file_name = new List<string>();
                    List<string> bmp_file_name = new List<string>();
                    foreach (var file in Files)
                    {
                        if (file.Name.Contains(".csv"))
                        {
                            csv_file_name.Add(file.FullName);

                            //'''''''
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


                            //foreach (var coloumn1 in floatX)
                            //{

                            //    Console.WriteLine(coloumn1);
                            //}

                            //foreach (var coloumn1 in floatY)
                            //{
                            //    Console.WriteLine(coloumn1);
                            //}


                            string[] name = file.FullName.Replace(folder_path , "").Replace(".csv", "").Replace("\\", "").Replace("[", "").Replace("]", "").Split(' ');

                            var calibrate = name[3].Split('&');
                            if (!name[3].Contains('&'))
                            {
                                calibrate = name[4].Split('&');
                            }

                            //Console.WriteLine(article.FileName);
                            //Console.WriteLine($"sample name: {name[2]}");
                            //Console.WriteLine($"figure name: '{name[0]} {name[1]}'");

                            //Console.WriteLine($"MaxX: {calibrate[1]}");
                            //Console.WriteLine($"MinX: {calibrate[0]}");
                            //Console.WriteLine($"MaxY: {calibrate[3]}");
                            //Console.WriteLine($"MinY: {calibrate[2]}");

                           
                            newIsotherm.FileName = article.FileName;
                            newIsotherm.SampleName = name[2];
                            newIsotherm.FigureNumber = name[1];
                            newIsotherm.MaxX = Convert.ToSingle(calibrate[1]);
                            newIsotherm.MinX = Convert.ToSingle(calibrate[0]);
                            newIsotherm.MaxY = Convert.ToSingle(calibrate[3]);
                            newIsotherm.MinY = Convert.ToSingle(calibrate[2]);

                            //Console.WriteLine("n" + newIsotherm.FileName);
                            //Console.WriteLine($"n sample name: {newIsotherm.SampleName}");
                            //Console.WriteLine($"n figure name: Figure {newIsotherm.FigureNumber}");

                            //Console.WriteLine($"n MaxX: {newIsotherm.MaxX}");
                            //Console.WriteLine($"n MinX: {newIsotherm.MinX}");
                            //Console.WriteLine($"n MaxY: {newIsotherm.MaxY}");
                            //Console.WriteLine($"n MinY: {newIsotherm.MinY}");

                            //Console.WriteLine("n" + newIsotherm.FileName);
                            //Console.WriteLine($" data x = {newIsotherm.XAxisData.Count}");
                            //Console.WriteLine($" data y = {newIsotherm.YAxisData.Count}");

                            article.Isotherms.Add(newIsotherm);

                            //'''''''
                        }

                        if (file.Name.Contains(".bmp"))
                        {
                            bmp_file_name.Add(file.FullName);

                            
                        }
                    }
                    article.IzothermNumber = csv_file_name.Count;
                    article.ImageNumber = bmp_file_name.Count;
                    Console.WriteLine($" {article.FileName} -> csv file = {article.IzothermNumber}  bmp_name = {article.ImageNumber} isotherms={article.Isotherms.Count}");
                }

                articles.Add(article);
            }
            return articles;
        }

       

        /// <summary>
        /// identificatior = "MK" or "MKZ"
        /// </summary>
        /// <param name="text"></param>
        /// <param name="indetificator"></param>
        /// <returns></returns>
        public List<Article> insertContactAndFiguresToArticles(string[] text, List<Article> articles)
        {
            foreach(Article article in articles) 
            {
                for (int i = 0; i < text.Length; i++)
                {
                    if (text[i].Contains(article.FileName))
                    {                        
                       
                        article.ContactPerson = text[i + 1].Trim();
                        article.Email = text[i + 2].Trim();
                        article.AdsorptionIsotherms = text[i + 3].Trim().Replace("Adsorption:","");
                        article.PoreDistribution = text[i + 4].Trim().Replace("Pore:", "");
                        
                    }
                }
            }                       
            return articles;
        }


    }


}
