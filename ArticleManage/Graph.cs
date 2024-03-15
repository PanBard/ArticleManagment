using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleManage
{
    internal class Graph
    {
        public List<Isotherm> Isotherms { get; set; }

        public Graph() 
        {
            this.Isotherms = new List<Isotherm>();
        }

    }
}
