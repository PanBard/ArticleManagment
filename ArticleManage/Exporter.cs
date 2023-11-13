using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Office2013.Drawing.ChartStyle;
using Microsoft.Office.Interop.Word;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Word = Microsoft.Office.Interop.Word;

namespace ArticleManage
{
    internal class Exporter
    {
        EnviromentCreator env;

        public Exporter(EnviromentCreator env)
        {
            this.env = env;

        }


        public void ExportTXTFile(String fileName, String data)
        {
            String filePath = env.outputFolderPath + fileName;
            Console.WriteLine($"File {fileName} was exported in {filePath}");
            File.WriteAllText(filePath, data);
        }


        public void ExportJson(string fileName, List<Article> data)
        {
            String filePath = env.outputFolderPath + fileName;
            String serialized_articles = JsonConvert.SerializeObject(data);
            File.WriteAllText(filePath, serialized_articles);
        }

        public void exportInfoToTxtWORD(List<Article> articles, String fileName)
        {
            String data = "";
            foreach (Article article in articles)
            {

                data += article.Identificator + ":" + "\n";
                data += "\t" + article.Citation +"\n";
                data += "\t" + "Adsorption isotherms: " + article.AdsorptionIsotherms + "\n";
                data += "\t" + "Pore distribution: " + article.PoreDistribution + "\n";                
                data += "\t" + "Contact person: " + article.ContactPerson + ": "+article.Email + "\n\n\n";                
            }

            ExportTXTFile(fileName, data);
        }

        public void exportAllEmailsToTxt(List<Article> articles, String fileName)
        {
            String all = "";
            foreach (Article article in articles)
            {
                String data = "Email address: " + article.Email + "\n\n" + article.EmailText;
                all += article.Identificator + "\n" + data + "\n\n\f\n";
            }

            ExportTXTFile(fileName, all);
        }

        public void exportEmails(List<Article> articles, String no_yes)
        {
            String all = "";
            foreach (Article article in articles)
            {   
                String data = "Email address: " + article.Email + "\n\n" + article.EmailText;
                String fileName = article.Identificator+"_email" + ".txt";
                SaveEmaiTxtFile(fileName, data, no_yes);
            }

            
        }

        private void SaveEmaiTxtFile (String fileName, String data, String no_yes)
        {
            String filePath = "";
            if (no_yes == "yes")
            {
                filePath = env.emailOutputFolderPath_yes + fileName;
            }
            if(no_yes == "no")
            {
                filePath = env.emailOutputFolderPath_no + fileName;
            }
         
            Console.WriteLine($"File {fileName} was exported to {filePath.Replace(env.outputFolderPath,"").Replace(fileName,"")}");
            File.WriteAllText(filePath, data);
        }

        /// <summary>
        /// fileName e.g: "doc.xlsx"
        /// </summary>
        /// <param name="articles_not_readed"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public void SaveExcelFile(List<Article> not_readed_articles, List<Article> readed_articles, String fileName)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            String excelFilePath = env.makePathForSavingFile(env.outputFolderPath, fileName);
            var file = new FileInfo(excelFilePath);
            DeleteIfExists(file); //delete file if exist

            using (var package = new ExcelPackage(file))
            {
                var workSheet = package.Workbook.Worksheets.Add("artykuly_niesczytane");
                var rangeCells = workSheet.Cells["A2"].LoadFromCollection(not_readed_articles, true); //start from A2 cell ;true for take name of property and put it as header column
                rangeCells.AutoFitColumns(); //expand column if content is big

                //Formas the header
                workSheet.Cells["C1"].Value = "Artykuły niesczytane";
                workSheet.Column(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                workSheet.Row(1).Style.Font.Size = 16;

                workSheet.Row(2).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                workSheet.Row(2).Style.Font.Bold = true;
                workSheet.Column(1).Width = 3;
                workSheet.Column(2).Width = 10;
                for (int i = 3; i <= 6; i++)
                {
                    workSheet.Column(i).Width = 20;
                }
                for (int i = 7; i < 14; i++)
                {
                    workSheet.Column(i).Width = 8;
                }
                for (int i = 14; i <= 20; i++)
                {
                    workSheet.Column(i).Width = 25;
                }


                var workSheet2 = package.Workbook.Worksheets.Add("artykuly_sczytane");
                var rangeCells2 = workSheet2.Cells["A2"].LoadFromCollection(readed_articles, true); //start from A2 cell ;true for take name of property and put it as header column
                workSheet2.Cells["C1"].Value = "Artykuły sczytane";
                workSheet2.Column(1).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                workSheet2.Row(1).Style.Font.Size = 16;
                workSheet2.Row(2).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                workSheet2.Row(2).Style.Font.Bold = true;
                rangeCells2.AutoFitColumns(); //expand column if content is big
                workSheet2.Column(1).Width = 3;
                workSheet2.Column(2).Width = 10;
                for (int i = 3; i <= 6; i++)
                {
                    workSheet2.Column(i).Width = 20;
                }
                for (int i = 7; i <= 14; i++)
                {
                    workSheet2.Column(i).Width = 8;
                }
                for (int i = 14; i <= 20; i++)
                {
                    workSheet2.Column(i).Width = 25;
                }

                package.SaveAsync();
                Console.WriteLine("end");
            }
        }

        private void DeleteIfExists(FileInfo file)
        {
            if (file.Exists)
            {
                file.Delete();
            }
        }




















    }
}
