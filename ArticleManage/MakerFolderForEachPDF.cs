﻿using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Formats.Tar;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleManage
{
    internal class MakerFolderForEachPDF
    {
        FoldersStructure folders;
        public MakerFolderForEachPDF(FoldersStructure folders)
        {
            this.folders = folders;


            makeFolderForPDFfilesAndCopyToIt();
            //extractTarFile();
            //showTime();
        }

        void copyGraphsToFolders()
        {                                       
            List<String> files_names = new List<string>();
            foreach (var name in folders.input_graph.filesNames)
            {
                var e = name.Split(' ');
                //Console.WriteLine(e[0]);
                files_names.Add(e[0]);
                
            }
            
            foreach (var folder in folders.output_folders.insideFolderPaths)
            {
                Console.WriteLine(folder);
            }

            List<String> orderedList = files_names
            .OrderBy(x => new string(x.Where(char.IsLetter).ToArray()))
            .ThenBy(x =>
            {
                int number;
                if (int.TryParse(new string(x.Where(char.IsDigit).ToArray()), out number))
                    return number;
                return -1;
            }).ToList();

            foreach (var name in orderedList)
            {
                Console.WriteLine($"{name}");
            }

        }

        void copyGraph(String pdfFileName)
        {
            foreach (var graph_name in folders.input_graph.filesNames)
            {
                var e = graph_name.Split(' ');                
                var t = pdfFileName.Split(' ');
                var r = t[0].ToString() + t[1].ToString();
                if ( r == e[0])
                {
                    String old_path = folders.input_graph.folderPath + graph_name; 
                    String new_path = folders.output_folders.folderPath + pdfFileName.Replace("... .pdf", "")+"\\"+ graph_name;
                    //Console.WriteLine($"{graph_name}       -->          {pdfFileName.Replace("... .pdf", "")}");
                    if (!File.Exists(new_path))
                    {
                        System.IO.File.Copy(old_path, new_path);
                    }
                }
            }
        }

        void showTime()
        {
            foreach (var name in folders.output_pdf.filesNames)
            {
                Console.WriteLine(name);
            }

            foreach (var path in folders.output_pdf.filesPaths)
            {
                Console.WriteLine(path);
            }

        }

        private void makeFolderForPDFfilesAndCopyToIt()
        {
            foreach (var pdf_file in this.folders.output_pdf.filesNames)
            {
                String path = this.folders.output_folders.folderPath + pdf_file.Replace("... .pdf", "");
                bool exists = System.IO.Directory.Exists(path);

                if (!exists)
                {
                    System.IO.Directory.CreateDirectory(path);
                    Console.WriteLine($"Create new folder [{pdf_file}]");
                }
                //else Console.WriteLine($"Folder exist [{pdf_file}]");

                String old_path = folders.output_pdf.folderPath + pdf_file;
                String new_path = path+"\\"+pdf_file;
                if (!File.Exists(new_path))
                {
                    System.IO.File.Copy(old_path, new_path);
                }
                copyGraph(pdf_file);
            }
            
        }

        private void extractTarFile()
        {

            using (StreamReader file = File.OpenText(folders.temp.insideFolderPaths.First() + "\\wpd.json"))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                JObject o2 = (JObject)JToken.ReadFrom(reader);
                var o = o2["datasetColl"];
                String data = "";
                foreach (var item in o)
                {
                    Console.WriteLine(item["name"]);
                    data += item["name"]+"\n";
                    foreach (var item1 in item["data"])
                    {
                        Console.WriteLine($"x: {item1["value"][0]} y: {item1["value"][1]}");
                        data += $"{item1["value"][0]} , {item1["value"][1]}" + "\n";
                    }
                    String nameTxt = folders.temp.folderPath + item["name"] +".txt";
                    File.WriteAllText(nameTxt, data);
                    data = "";
                }
                //Console.WriteLine(o[0]);
            }

            


            //try
            //{
            //    TarFile.ExtractToDirectory(folders.plot_digitizer_projects.filesPaths.First(), folders.temp.folderPath, false);
            //}
            //catch (IOException e)
            //{

            //    Console.WriteLine("failed to extraxt archive");
            //    Console.WriteLine($"{e.Message}");
            //}

        }


    }
}
