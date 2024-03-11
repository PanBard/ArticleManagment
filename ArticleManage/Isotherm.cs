using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleManage
{
    internal class Isotherm
    {
        public String FileName { get; set; }
        public String SampleName { get; set; }
        public String FigureNumber { get; set; }

        public float MaxX { get; set; }
        public float MinX { get; set; }
        public float MaxY { get; set; }
        public float MinY { get; set; }

        public List<float> XAxisData { get; set; }
        public List<float> YAxisData { get; set; }

        public Isotherm()
        {
            XAxisData = new List<float>();
            YAxisData = new List<float>();
        }

    }
}
