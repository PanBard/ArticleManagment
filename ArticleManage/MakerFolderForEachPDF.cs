﻿using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Formats.Tar;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aspose.Zip.Rar;

namespace ArticleManage
{
    internal class MakerFolderForEachPDF
    {
        FoldersStructure folders;
        public MakerFolderForEachPDF(FoldersStructure folders)
        {
            this.folders = folders;


            makeFolderForPDFfilesAndCopyToIt();
            //readDataFromProject();
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
                var e = graph_name.Split('_');                
                var t = pdfFileName.Split(' ');
                var r = t[0].ToString() + t[1].ToString();
                if ( r == e[0] && !graph_name.Contains(".rar"))
                {
                    String old_path = folders.input_graph.folderPath + graph_name; 
                    String new_path = folders.output_folders.folderPath + pdfFileName.Replace("... .pdf", "")+ "\\graphs\\" + graph_name;
                    //Console.WriteLine($"{graph_name}       -->          {pdfFileName.Replace("... .pdf", "")}");
                    if (!File.Exists(new_path))
                    {
                        System.IO.File.Copy(old_path, new_path);
                    }
                }
            }
        }

        void copyTarPackage(String pdfFileName)
        {
            foreach (var file_name in folders.input_graph.filesNames)
            {

                if(file_name.Contains(".tar") )
                {
                    
                    var tar_name = file_name.Split('_');
                    var t = pdfFileName.Split(' ');
                    var pdf_name = t[0].ToString() + t[1].ToString();
                    if (pdf_name == tar_name[0])
                    {                                              
                            String old_path = folders.input_graph.folderPath + file_name;
                            String new_path = folders.output_folders.folderPath + pdfFileName.Replace("... .pdf", "") + "\\graphs\\" + file_name;
                            //Console.WriteLine($"{file_name}       -->          {pdfFileName.Replace("... .pdf", "")}");
                            if (!File.Exists(new_path))
                            {
                                System.IO.File.Copy(old_path, new_path);
                            }

                            extractTarPackagesToTempFolder(new_path);                                               
                    }
                }

                if (file_name.Contains(".rar"))
                {
                    var t = pdfFileName.Split(' ');
                    var pdf_name = t[0].ToString() + t[1].ToString();
                    var rar_name = file_name.Split('_');
                    if (pdf_name == rar_name[0])
                    {
                        String rar_file_path = folders.input_graph.folderPath + file_name;
                        String path_to_extract_rar_file = folders.output_folders.folderPath + pdfFileName.Replace("... .pdf", "") + "\\graphs";
                        extractReadedDataTarPackagesToOutputFolder(path_to_extract_rar_file, rar_file_path);
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

                String graph_path = path + "\\" + "graphs";
                bool exists2 = System.IO.Directory.Exists(graph_path);
                if (!exists2)
                {
                    System.IO.Directory.CreateDirectory(graph_path);
                    Console.WriteLine($"Create new folder [{graph_path}]");
                }
                //else Console.WriteLine($"Folder exist [{pdf_file}]");

                String old_path = folders.output_pdf.folderPath + pdf_file;
                String new_path = path+"\\"+pdf_file;
                if (!File.Exists(new_path))
                {
                    System.IO.File.Copy(old_path, new_path);
                }

                //]]]]]]]]]
                String[] only_pdf_Data_number_name = pdf_file.Split(' ');
                String path_graph_folder = this.folders.temp_graph.folderPath + only_pdf_Data_number_name[0]+ only_pdf_Data_number_name[1];
                bool graph_folder_exists = System.IO.Directory.Exists(path_graph_folder);

                if (!graph_folder_exists)
                {
                    System.IO.Directory.CreateDirectory(path_graph_folder);
                    Console.WriteLine($"Create new folder [{path_graph_folder}]");
                }
                //]]]]]]]]]

                copyGraph(pdf_file);
                copyTarPackage(pdf_file);
                readDataFromProject(pdf_file);
                
            }
            Console.WriteLine($"Making folders for each {folders.input_pdf.filesNames.Count} articles");
        }

        private void readDataFromProject(String pdfFileName)
        {
            var t = pdfFileName.Split(' ');
            var pdf_name = t[0].ToString() + t[1].ToString();

            foreach (var path in folders.temp.insideFolderPaths)
            {
                String elo = path + " ";
                if(elo.Contains(pdf_name + " ") || path.Contains(pdf_name + "_"))
                {

                    //[[[[[[[[[[[[[[[[
                    DirectoryInfo dir = new DirectoryInfo(path);
                    FileInfo[] Files = dir.GetFiles();
                    string graph_number = "";
                    String path_new_graph_folder = "";
                    foreach (var item in Files)
                    {
                        if(item.Name.Contains(".bmp"))
                        {
                            //string[] temp = item.Name.Replace(".bmp","").Replace("f","").Split(' ');
                            //string[] temp2 = item.Name.Replace(".bmp", "").Replace("f", "").Split('_');
                            //if(temp.Length > 1) 
                            //{ 
                            //    graph_number = temp[1];  
                            //}
                            //else 
                            //{ 
                            //    graph_number = temp2[1]; 
                            //}

                            //''''''''''
                            string[] temp = item.Name.Replace(".bmp", "").Replace("f", "").Split(' ');
                            string[] temp2 = item.Name.Replace(".bmp", "").Replace("f", "").Split('_');
                            if (temp.Length > 1)
                            {
                                graph_number = temp[1];
                            }
                            else
                            {
                                if(temp2.Count() == 3)
                                {
                                    graph_number = temp2[1] +"_"+ temp2[2];
                                }
                                else
                                {
                                    graph_number = temp2[1];
                                }
                                
                            }
                            //''''''''''

                            //============
                            //]]]]]]]]]
                                //String[] only_pdf_Data_number_name = path.Replace("folders.temp.folderPath","").Split('_');
                             
                            path_new_graph_folder = this.folders.temp_graph.folderPath +pdf_name+"\\"+ $"{pdf_name} Figure {graph_number}";
                            //Console.WriteLine(path_graph_folder);
                            bool graph_folder_exists = System.IO.Directory.Exists(path_new_graph_folder);

                            if (!graph_folder_exists)
                            {
                                System.IO.Directory.CreateDirectory(path_new_graph_folder);
                                Console.WriteLine($"Create new folder [{path_new_graph_folder}]");
                            }
                            //]]]]]]]]]
                            //============

                            //graph_number = temp[1];
                            //Console.WriteLine(graph_number);
                        }
                    }
                    //[[[[[[[[[[[[[[[[
                    using (StreamReader file = File.OpenText(path + "\\wpd.json"))

                    using (JsonTextReader reader = new JsonTextReader(file))
                    {
                        JObject o2 = (JObject)JToken.ReadFrom(reader);
                        var o = o2["datasetColl"];
                        var Calibration_y1 = o2["axesColl"][0]["calibrationPoints"][0]["dy"];
                        var Calibration_y2 = o2["axesColl"][0]["calibrationPoints"][3]["dy"];
                        var Calibration_x1 = o2["axesColl"][0]["calibrationPoints"][0]["dx"];
                        var Calibration_x2 = o2["axesColl"][0]["calibrationPoints"][3]["dx"];
                        Console.WriteLine($"{pdf_name} in Figure {graph_number} ->  x1:{Calibration_x1}, x2: {Calibration_x2}, y1: {Calibration_y1}, y2: {Calibration_y2}");
                        String data = "";
                        foreach (var item in o)
                        {
                            //Console.WriteLine(item["name"]);
                            data += item["name"]+"," + "\n" +"X,Y" + "\n";
                            
                            foreach (var item1 in item["data"])
                            {
                                //Console.WriteLine($"x: {item1["value"][0]} y: {item1["value"][1]}");
                                data += $"{String.Format("{0:0.0000}",item1["value"][0])} , {String.Format("{0:0.0000}", item1["value"][1])}" + "\n";
                            }

                            String csv_folder_path = folders.output_folders.folderPath + pdfFileName.Replace("... .pdf", "") + "\\graphs" ;                            
                            String nameTxt = csv_folder_path + "\\"+"Figure "+ graph_number+" " + item["name"] +" " + $"[{Calibration_x1}&{Calibration_x2}&{Calibration_y1}&{Calibration_y2}]" + ".csv";
                            String csv_in_temp_graph_folder = path_new_graph_folder + "\\" + "Figure " + graph_number + " " + item["name"] + " " + $"[{Calibration_x1}&{Calibration_x2}&{Calibration_y1}&{Calibration_y2}]" + ".csv";
                            File.WriteAllText(csv_in_temp_graph_folder, data);
                            File.WriteAllText(nameTxt, data);
                            data = "";
                        }
                        //Console.WriteLine(o[0]);
                    }

                }
               

            }
           
                           
        }

        private void extractTarPackagesToTempFolder(String path)
        {
            if (!Directory.Exists(path))
            {
                try
                {
                    //TarFile.ExtractToDirectory(folders.plot_digitizer_projects.filesPaths.First(), folders.temp.folderPath, false);
                    TarFile.ExtractToDirectory(path, folders.temp.folderPath, false);
                }
                catch (IOException e)
                {
                    //Console.WriteLine($"failed to extraxt archive -> {e.Message}");
                }
            }
        }

        private void extractReadedDataTarPackagesToOutputFolder(String path_to_extract_rar_file, String rar_file_path)
        {
            if (!Directory.Exists(rar_file_path))
            {
              using (RarArchive archive = new RarArchive(rar_file_path))
                {
                    archive.ExtractToDirectory(path_to_extract_rar_file);
                }
            }                        
        }


    }
}
