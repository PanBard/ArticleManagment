using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleManage
{
    internal class EnviromentCreator
    {
        public String projectFolderName {  get; set; }
        public String serializeFolderName { get; set; }
        public String txtInputFolderName { get; set; }
        public String RISInputFolderName { get; set; }
        public String RISInputFolderName_no { get; set; }
        public String RISInputFolderName_yes { get; set; }
        public String projectFolderPath { get; set; }
        public String outputFolderPath { get; set; }
        public String txtInputFolderPath { get; set; }
        public String RISInputFolderPath_main { get; set; }
        public String RISInputFolderPath_no { get; set; }
        public String RISInputFolderPath_yes { get; set; }

        public String emailOutputFolderPath_yes { get; set; }
        public String emailOutputFolderPath_no { get; set; }
        public String txtFilePath { get; set; }

        public List<String> riscFilesPaths_no;
        public List<String> riscFilesPaths_yes;

        public EnviromentCreator(String thisProjectFolderName, String serializeFolderName, String txtInputFolderName, String RISInputFolderName) 
        {
            this.projectFolderName = thisProjectFolderName;
            this.serializeFolderName = serializeFolderName;
            this.txtInputFolderName = txtInputFolderName;
            this.RISInputFolderName = RISInputFolderName;
            this.RISInputFolderName_no = "no";
            this.RISInputFolderName_yes = "yes";
            makePaths();
            makeDirIfNotExist(this.projectFolderPath);
            makeDirIfNotExist(this.outputFolderPath);
            makeDirIfNotExist(this.txtInputFolderPath);
            makeDirIfNotExist(this.RISInputFolderPath_main);
            makeDirIfNotExist(this.RISInputFolderPath_no);
            makeDirIfNotExist(this.RISInputFolderPath_yes);
            makeDirIfNotExist(this.emailOutputFolderPath_yes);
            makeDirIfNotExist(this.emailOutputFolderPath_no);
            //this.txtFilePath = getFilePath(this.txtInputFolderPath);

            this.riscFilesPaths_no =  getRISFilePath(this.RISInputFolderPath_no);
            this.riscFilesPaths_yes = getRISFilePath(this.RISInputFolderPath_yes);


        }

        private void makePaths()
        {
            this.projectFolderPath = makePathToDir(this.projectFolderName+"\\");
            this.outputFolderPath = this.projectFolderPath + this.serializeFolderName+ "\\";
            this.txtInputFolderPath = this.projectFolderPath + this.txtInputFolderName + "\\";
            this.RISInputFolderPath_main = this.projectFolderPath + this.RISInputFolderName + "\\";
            this.RISInputFolderPath_no =  this.RISInputFolderPath_main + this.RISInputFolderName_no + "\\";
            this.RISInputFolderPath_yes = this.RISInputFolderPath_main + this.RISInputFolderName_yes + "\\";
            this.emailOutputFolderPath_yes = this.outputFolderPath + "\\" +"emaile_yes\\";
            this.emailOutputFolderPath_no = this.outputFolderPath + "\\" + "emaile_no\\";
        }

        private String makePathToDir(String folderName)
        {   
            // This will get the current WORKING directory (i.e. \bin\Debug)
            string workingDirectory = Environment.CurrentDirectory;
            // This will get the current PROJECT directory
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;

            String main_folder_name = "\\C_sharp_programs_folders\\";
            return projectDirectory +  main_folder_name + folderName;
        }

        private void makeDirIfNotExist(string path)
        {
            bool exists = System.IO.Directory.Exists(path);

            if (!exists)
            {
                System.IO.Directory.CreateDirectory(path);
                Console.WriteLine($"Create new folder [{path}]");
            }
            else Console.WriteLine($"Folder exist [{path}]");
        }

        private String getFilePath(String path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] Files = dir.GetFiles();            
            if (Files.Length == 0)
            {
                Console.WriteLine($"\n!!! Copy txt file to  {this.txtInputFolderPath}  !!! (and run program again)");
                Console.Read();
            }
            
            return Files[0].FullName; //return only first file
        }

        public String makePathForSavingFile (String saveDirpath, String fileName)
        {
            return saveDirpath + fileName;
        }

        private List<String> getRISFilePath(String path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] Files = dir.GetFiles();
            String[] files_paths = new String[Files.Length];
            if (Files.Length == 0)
            {
                Console.WriteLine($"\n!!! Copy .ris file to  {path}  !!! (and run program again)");
                Console.Read();
            }
            
            for (int i = 0; i < Files.Length; i++)
            {
                files_paths[i] = Files[i].FullName;
            }               
            //sorting paths with numerical order
            List<String> orderedList = files_paths
           .OrderBy(x => new string(x.Where(char.IsLetter).ToArray()))
           .ThenBy(x =>
           {
               int number;
               if (int.TryParse(new string(x.Where(char.IsDigit).ToArray()), out number))
                   return number;
               return -1;
           }).ToList();

            return orderedList; //return only first file
        }
    }
}
