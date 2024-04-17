using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleManage
{
    internal class jsonExporter
    {
        public FoldersStructure folders { get; }

        public jsonExporter(FoldersStructure folders) 
        {
            this.folders = folders;
            ExportArticlesToJson("all_artcles.json");
        }



        private void DeleteIfExists(FileInfo file)
        {
            if (file.Exists)
            {
                file.Delete();
            }
        }

        public void ExportArticlesToJson(string fileName)
        {
            MethodsArchive method = new MethodsArchive();
            var articles = method.returnArticleWithDataFromRISfiles(folders.input_ris.filesPaths, folders.input_ris.folderPath, folders);
            String filePath = folders.output_json.folderPath + fileName;
            var file = new FileInfo(filePath);
            DeleteIfExists(file);
            String serialized_articles = JsonConvert.SerializeObject(articles);
            File.WriteAllText(filePath, serialized_articles);           
            Console.WriteLine($"Saved all {articles.Count} articles data in file: {fileName}");

            SaveCSVFile(articles);



        }



        private void SaveCSVFile(List<Article> articles)
        {
            List<string> files = new List<string>();
            String csvText_for_all_data = "FileName,Graph Number,Sample Name, S_BET, Total pore volume, Micropore volume"+"\n";
            String csvText_for_complete_data = "FileName,Graph Number,Sample Name, S_BET, Total pore volume, Micropore volume" + "\n";
            foreach (var article in articles)
            {
                foreach (var graph in article.Graphs) 
                {
                    foreach(var isotherm in graph.Isotherms)
                    {
                        //Console.WriteLine(isotherm.SampleFeatures.Total_surface_area.GetType());
                        if (isotherm.SampleFeatures.Total_surface_area != "" && isotherm.SampleFeatures.Total_pore_volume != "" && isotherm.SampleFeatures.Micropore_volume != "")
                        {
                            //Console.WriteLine(isotherm.SampleFeatures.Sample_name);
                            files.Add(isotherm.SampleFeatures.Sample_name);
                            csvText_for_complete_data += article.FileName + "," + isotherm.FigureNumber.Split('_').First() + "," + isotherm.SampleFeatures.Sample_name + "," + isotherm.SampleFeatures.Total_surface_area + "," + isotherm.SampleFeatures.Total_pore_volume + "," + isotherm.SampleFeatures.Micropore_volume + "\n";
                        }
                        files.Add(isotherm.SampleFeatures.Sample_name);
                        csvText_for_all_data += article.FileName + "," + isotherm.FigureNumber.Split('_').First() + "," + isotherm.SampleFeatures.Sample_name + "," + isotherm.SampleFeatures.Total_surface_area + "," + isotherm.SampleFeatures.Total_pore_volume + "," + isotherm.SampleFeatures.Micropore_volume + "\n";


                    }
                }
            }
            //Console.WriteLine(csvText_for_all_data);
            File.WriteAllText(folders.output_excel.folderPath + "BET_all_samples_data.csv", csvText_for_all_data);
            File.WriteAllText(folders.output_excel.folderPath + "BET_complete_samples_data.csv", csvText_for_complete_data);
        }

    }

    
  


}
