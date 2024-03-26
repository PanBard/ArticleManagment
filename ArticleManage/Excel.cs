﻿using DocumentFormat.OpenXml.Spreadsheet;
using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using OfficeOpenXml.Drawing.Chart.Style;
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

            SaveExcelFileExperiment();
            SaveSummaryExcelFile("all_articles.xlsx");
        }


        private void DeleteIfExists(FileInfo file)
        {
            if (file.Exists)
            {
                file.Delete();
            }
        }

        private void SaveSummaryExcelFile( String excelFileName)
        {
            MethodsArchive method = new MethodsArchive();
            var articles = method.returnArticleWithDataFromRISfiles(folders.input_ris.filesPaths, folders.input_ris.folderPath, folders);
            //var articles2 = method.returnArticleWithAllData(folders.input_ris.filesPaths, folders.input_ris.folderPath, folders);

            //make_samples_features_files(articles); //    <------ to make csv files for sample features !!!

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
                workSheet.Column(1).Width = 5;
                workSheet.Column(2).Width = 10;
                workSheet.Column(3).Width = 50;
                workSheet.Column(4).Width = 15;
                workSheet.Column(5).Width = 45;
                workSheet.Column(6).Width = 45;
                //for (int i = 3; i <= 6; i++)
                //{
                //    workSheet.Column(i).Width = 10;
                //}
                for (int i = 7; i < 12; i++)
                {
                    workSheet.Column(i).Width = 8;
                }
                for (int i = 12; i <= 20; i++)
                {
                    workSheet.Column(i).Width = 25;
                }

                package.SaveAsync();
                Console.WriteLine($"Saved file: {excelFileName}");
            }
        }

        private void SaveExcelFileExperiment( )
        {
            int c = 0;
            foreach (var item in folders.output_folders.insideFolderPaths)
            {
                if(Directory.Exists(item + "\\graphs")) 
                {
                    DirectoryInfo dir = new DirectoryInfo(item + "\\graphs");
                    FileInfo[] Files = dir.GetFiles();
                    var folder_name = item.Replace(folders.output_folders.folderPath, "");

                    List<string> files = new List<string>();
                    foreach (var file in Files)
                    {
                        if (file.Name.Contains(".csv"))
                        {
                            files.Add(file.FullName);
                        }
                    }
                    if (files.Count > 0)
                    {
                        var t = folder_name.Split(' ');
                        var pdf_name = t[0].ToString() + t[1].ToString();
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                        String excelFilePath = item +"\\"+ pdf_name + "_all_graphs_excel.xlsx";
                        //Console.WriteLine(excelFilePath);
                        var file = new FileInfo(excelFilePath);
                        //DeleteIfExists(file); //delete file if exist
                        var excelTextFormat = new ExcelTextFormat();
                        excelTextFormat.Delimiter = ',';
                        excelTextFormat.EOL = "\n";

                        if (!File.Exists(excelFilePath))
                        {
                            using (var package = new ExcelPackage(file))
                            {
                                foreach (var csv_file_path in files)
                                {
                                    string[] name = csv_file_path.Replace(item + "\\graphs", "").Replace(".csv", "").Replace("\\", "").Replace("[", "").Replace("]", "").Split(' ');
                                    var calibrate = name[3].Split('&');
                                    if (!name[3].Contains('&'))
                                    {
                                        calibrate = name[4].Split('&');
                                    }

                                    //calibrate.ToList().ForEach(x => Console.Write(x+", "));
                                    String txt = File.ReadAllText(csv_file_path);
                                    Console.WriteLine($"Worksheet: {csv_file_path.Replace(item + "\\graphs", "").Replace(".csv", "").Replace("\\", "")}");
                                    package.Workbook.Worksheets.Add(csv_file_path.Replace(item + "\\graphs", "").Replace(".csv", "").Replace("\\", "")).Cells["A1"].LoadFromText(txt, excelTextFormat); //start from A2 cell ;true for take name of property and put it as header column
                                    var testWorksheet = package.Workbook.Worksheets[csv_file_path.Replace(item + "\\graphs", "").Replace(".csv", "").Replace("\\", "")];
                                    ExcelChart chart = testWorksheet.Drawings.AddChart("chart", eChartType.XYScatter);
                                    chart.XAxis.Title.Text = "Relative pressure"; //give label to x-axis of chart  
                                    chart.XAxis.Title.Font.Size = 12;

                                    chart.YAxis.Title.Text = "Volume of gas adsorbed"; //give label to Y-axis of chart  
                                    chart.YAxis.Title.Font.Size = 12;
                                    chart.YAxis.Title.Rotation = 270;
                                    chart.YAxis.MaxValue = float.Parse(calibrate[3]);
                                    chart.YAxis.MinValue = float.Parse(calibrate[2]);
                                    chart.XAxis.MaxValue = float.Parse(calibrate[1]);
                                    chart.XAxis.MinValue = float.Parse(calibrate[0]);
                                    chart.Legend.Remove();
                                    chart.SetSize(600, 400);
                                    chart.SetPosition(1, 0, 5, 0);
                                    //Set style 9 and Colorful Palette 3
                                    chart.StyleManager.SetChartStyle(ePresetChartStyle.Area3dChartStyle1, ePresetChartColors.ColorfulPalette1);

                                    string chart_title = $"Izoterma adsorpcji probki {name[2]} z wykresu '{name[0]} {name[1]}' ";
                                    chart.Title.Text = chart_title;
                                    chart.Title.Font.Size = 14;
                                    var series = chart.Series.Add(testWorksheet.Cells["B3:B70"], testWorksheet.Cells["A3:A70"]);
                                }
                                package.Save();

                            }
                        }
                        


                    }
                    else
                    {
                        //Console.WriteLine($"files.Count < 0 -----------------------> {folder_name}");
                    }

                    c++;
                }
                
            }


        }


        private void make_samples_features_files(List<Article> articles)
        {
            
            foreach (var article in articles)
            {
                String fileName = $"{article.FileName}_features.csv";
                String filePath = folders.input_sample_features.folderPath + fileName;                
                String data = "Figure,Sample,Total_surface[m2/g],V_total[cm3/g],V_micro[cm3/g],V_meso[cm3/g],Pore_diameter[nm],Activ_temp[stC],Activ_time[min],Activ_agent,Material" + "\n";
                var graphs = article.Graphs;
                foreach (var graph in graphs)
                {                   
                    foreach (var izo in graph.Isotherms)
                    {                            
                        data += izo.FigureNumber+","+izo.SampleName + ",,,,,,,,," + "\n";                            
                    }                    
                }
                
                if (!File.Exists(filePath))
                {
                    //Console.WriteLine($"File {fileName} was created in {filePath}");
                    Console.WriteLine(data);
                    File.WriteAllText(filePath, data);
                }
                
            }
            
        }









    }


}
