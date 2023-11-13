using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleManage
{
    internal class Importer
    {

        public  List<Article> deseriaizeFile(string serializeFilePath)
        {
            string seralizedArticles = File.ReadAllText(serializeFilePath);
            List<Article> articles = JsonConvert.DeserializeObject<List<Article>>(seralizedArticles);
            return articles;
        }

        public String[] readTxtFile(string FilePath)
        {
            String[] txt = File.ReadAllLines(FilePath);
            return txt;
        }

        public List<String[]> readRiSfiles(List<string> RISfilePath)
        {
            List<String[]> risFiles = new List<String[]>();
            foreach (var item in RISfilePath)
            {
                risFiles.Add(File.ReadAllLines(item));
            }
            return risFiles;
        }
    }
}
