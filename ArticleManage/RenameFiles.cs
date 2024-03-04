using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleManage
{
    internal class RenameFiles
    {
        FoldersStructure folders;
        public RenameFiles(FoldersStructure folders) 
        {
            this.folders = folders;


            makeFormalNameForPDFfiles();
            //makeNewNameForRISfiles();
            //makeNewNameForPDFfilesSimple();
            //makeNewNameForRISfilesSimple();

            //showTime();
        }

        void makeFormalNameForPDFfiles()
        {
            MethodsArchive method = new MethodsArchive();
            var articles = method.returnArticleWithDataFromRISfiles(folders.input_ris.filesPaths, folders.input_ris.folderPath, folders);
            List<String> new_names = new List<string>();

            foreach (var article in articles)
            {
                String old_path = folders.input_pdf.folderPath + article.FileName+".pdf";
                String new_path = folders.output_pdf.folderPath + article.FormalNicelyPDFName;
                if (!File.Exists(new_path))
                {
                    System.IO.File.Copy(old_path, new_path);
                }      
            }

        }

        void makeNewNameForRISfiles()
        {
            int counter = 51;
            foreach (var ris_name in this.folders.input_ris.filesNames)
            {
                String new_name = ris_name.Replace("MKZ","");
                new_name = "Data" + counter;
                Console.WriteLine($"name: {ris_name}   == >   new: {new_name}");           
                String old_path = this.folders.input_ris.folderPath + ris_name;
                String new_path = this.folders.output_ris.folderPath + new_name;
                System.IO.File.Copy(old_path, new_path);
                counter++;
            }

        }

        void makeNewNameForPDFfilesSimple()
        {
            int counter = 51;
            foreach (var ris_name in this.folders.input_pdf.filesNames)
            {
                String new_name = "";
                new_name = "Data" + counter+".pdf";
                Console.WriteLine($"name: {ris_name}   == >   new: {new_name}");
                String old_path = this.folders.input_pdf.folderPath + ris_name;
                String new_path = this.folders.output_pdf.folderPath + new_name;
                System.IO.File.Copy(old_path, new_path);
                counter++;
            }

        }

        void makeNewNameForRISfilesSimple()
        {
            int counter = 51;
            foreach (var ris_name in this.folders.input_ris.filesNames)
            {
                String new_name = "";
                new_name = "Data" + counter + ".ris";
                Console.WriteLine($"name: {ris_name}   == >   new: {new_name}");
                String old_path = this.folders.input_ris.folderPath + ris_name;
                String new_path = this.folders.output_ris.folderPath + new_name;
                System.IO.File.Copy(old_path, new_path);
                counter++;
            }

        }


        void showTime()
        {
            foreach (var name in folders.input_ris.filesNames)
            {
                Console.WriteLine(name);
            }

            //foreach (var path in folders.input_ris.filesPaths)
            //{
            //    Console.WriteLine(path);
            //}

            foreach (var name in folders.input_pdf.filesNames)
            {
                Console.WriteLine(name);
            }

            //foreach (var path in folders.input_pdf.filesPaths)
            //{
            //    Console.WriteLine(path);
            //}
        }




    }
}
