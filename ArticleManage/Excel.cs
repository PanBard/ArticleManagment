using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleManage
{
    internal class Excel
    {
        public FoldersStructure folders { get; }
        public Excel(FoldersStructure folders) 
        {
            this.folders = folders;


            SaveExcelFile("all_article.xlsx");
        }

 


        private void SaveExcelFile( String excelFileName)
        {
            MethodsArchive method = new MethodsArchive();
            var articles = method.returnArticleWithDataFromRISfiles(folders.input_ris.filesPaths, folders.input_ris.folderPath);

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            String excelFilePath = folders.output_excel.folderPath + excelFileName;
            var file = new FileInfo(excelFilePath);
            DeleteIfExists(file); //delete file if exist

            using (var package = new ExcelPackage(file))
            {
                var workSheet = package.Workbook.Worksheets.Add("artykuly");
                var rangeCells = workSheet.Cells["A2"].LoadFromCollection(articles, true); //start from A2 cell ;true for take name of property and put it as header column
                rangeCells.AutoFitColumns(); //expand column if content is big

                //Formas the header
                workSheet.Cells["C1"].Value = "Artykuły ";
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

                package.SaveAsync();
                Console.WriteLine($"Saved file: {excelFileName}");
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
