using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleManage
{
    internal class MethodsArchive
    {
        private EnviromentCreator env;

        public MethodsArchive(EnviromentCreator env) 
        {
            this.env = env;
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
        /// filePaths = for no or for yes ris files
        /// </summary>
        /// <param name="filePaths"></param>
        /// <returns></returns>
        public List<Article> returnArticleWithDataFromRISfiles(List<String> filePaths, String dirPath)
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
                article.Identificator = risFileName;
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
                    if (text[i].Contains(article.Identificator))
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
