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

          
        }

    }

    
  


}
