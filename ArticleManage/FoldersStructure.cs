using DocumentFormat.OpenXml.Office.CustomUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleManage
{
    internal class FoldersStructure
    {
        public Folder input_pdf {  get; set; }
        public Folder input_ris { get; set; }
        public Folder output_pdf { get; set; }
        public Folder output_ris { get; set; }
        public Folder output_folders { get; set; }
        public Folder output_excel { get; set; }
        public Folder input_graph {  get; set; }
        public Folder plot_digitizer_projects { get; set; }
        public Folder temp { get; set; }

        public FoldersStructure()
        {
            this.input_pdf = new Folder("input_pdf");
            this.input_ris = new Folder("input_ris");
            this.output_pdf = new Folder("output_pdf");
            //this.output_ris = new Folder("output_ris");
            this.output_folders = new Folder("output_folders");
            this.output_excel = new Folder("output_excel");
            this.input_graph = new Folder("input_graph");
            //this.plot_digitizer_projects = new Folder("plot_digitizer_projects");
            this.temp = new Folder("temp");
        }







    }
}
