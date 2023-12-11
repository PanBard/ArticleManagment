using System;
using System.Collections.Generic;
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

            //showTime();
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
                else Console.WriteLine($"Folder exist [{pdf_file}]");

                String old_path = folders.output_pdf.folderPath + pdf_file;
                String new_path = path+"\\"+pdf_file;
                if (!File.Exists(new_path))
                {
                    System.IO.File.Copy(old_path, new_path);
                }
            }
            
        }
    }
}
