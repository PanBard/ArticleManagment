using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleManage
{
    internal class Folder
    {
        private String mainFolderName = "main_working_folder";
        public String folderName { get; set; }
        public String folderPath { get; set; }

        public List<String> filesNames { get; set; }
        public List<String> filesPaths {  get; set; }
        public List<String> insideFolderPaths { get; set; }


        public Folder(string folderName)
        {

            this.filesNames = new List<String>();
            this.filesPaths = new List<String>();
            this.insideFolderPaths = new List<String>();
            this.folderName = folderName;
            this.makePaths();
            this.makeDirIfNotExist(this.folderPath);
            this.makeFilesNamesAndPaths();

            this.sort_names();
            this.sort_paths();
        }

        private void makePaths()
        {          
            string workingDirectory = Environment.CurrentDirectory; // This will get the current WORKING directory (i.e. \bin\Debug)            
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName; // This will get the current PROJECT directory
            this.folderPath =  projectDirectory + "\\" + this.mainFolderName + "\\" + this.folderName + "\\";
        }

        private void makeDirIfNotExist(string path)
        {
            bool exists = System.IO.Directory.Exists(path);

            if (!exists)
            {
                System.IO.Directory.CreateDirectory(path);
                Console.Write($"Create new folder [{this.folderName}]");
            }
            else Console.Write($"Folder exist [{this.folderName}]");
        }

        private void makeFilesNamesAndPaths()
        {
            DirectoryInfo dir = new DirectoryInfo(this.folderPath);
            FileInfo[] Files = dir.GetFiles();
            DirectoryInfo[] dicts = dir.GetDirectories();
            if (Files.Length == 0 && dicts.Length == 0)
            {
                Console.Write($" Files inside: {Files.Length}|");
                Console.WriteLine($" Dir inside: {dicts.Length}");
            }
            else
            {
                Console.Write($" Files inside: {Files.Length}|");
                Console.WriteLine($" Dir inside: {dicts.Length}");
                for (int i = 0; i < Files.Length; i++)
                {
                    filesNames.Add(Files[i].Name); 
                }

                for (int i = 0; i < Files.Length; i++)
                {
                    filesPaths.Add(Files[i].FullName);
                }

                for (int i = 0; i < dicts.Length; i++)
                {
                    insideFolderPaths.Add(dicts[i].FullName);
                }

            }

        }

        private void sort_names()
        {
            if(this.filesNames.Count > 0)
            {
                String[] files_names = new String[this.filesNames.Count];
                files_names = this.filesNames.ToArray();
                List<String> orderedList = files_names
               .OrderBy(x => new string(x.Where(char.IsLetter).ToArray()))
               .ThenBy(x =>
               {
                   int number;
                   if (int.TryParse(new string(x.Where(char.IsDigit).ToArray()), out number))
                       return number;
                   return -1;
               }).ToList();

                this.filesNames = orderedList;

            }
           
        }

        private void sort_paths()
        {
            if (this.filesNames.Count > 0)
            {
                String[] files_paths = new String[this.filesPaths.Count];
                files_paths = this.filesPaths.ToArray();
                List<String> orderedList = files_paths
               .OrderBy(x => new string(x.Where(char.IsLetter).ToArray()))
               .ThenBy(x =>
               {
                   int number;
                   if (int.TryParse(new string(x.Where(char.IsDigit).ToArray()), out number))
                       return number;
                   return -1;
               }).ToList();

                this.filesPaths = orderedList;

            }

        }

    }


}
