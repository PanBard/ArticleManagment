using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleManage
{
    internal class Article
    {
        //AU Naile Karakehya,
        //T1 Effects of one-step and two-step KOH activation method on the properties and supercapacitor performance of highly porous activated carbons prepared from Lycopodium clavatum spores,
        //JO Diamond and Related Materials,
        //VL Volume 135,
        //PY 2023,
        //SP 109873,
        //DO https://doi.org/10.1016/j.diamond.2023.109873.

        public int Id { get; set; }
        public String FileName { get; set; }

        /// <summary>
        /// TI or T1
        /// </summary>
        public String PrimaryTitle { get; set; }

        /// <summary>
        /// AU
        /// </summary>
        public List<String> Autors { get; set; }

        /// <summary>
        /// DO
        /// </summary>
        public String DOI { get; set; }

        /// <summary>
        /// UR
        /// </summary>
        public String URL { get; set; }

        /// <summary>
        /// VL
        /// </summary>
        public String Volume { get; set; }

        /// <summary>
        /// IS
        /// </summary>
        public String Issue { get; set; }

        /// <summary>
        /// SP
        /// </summary>
        public String StartPage { get; set; }

        /// <summary>
        /// EP
        /// </summary>
        public String Endpage { get; set; }

        /// <summary>
        /// PY
        /// </summary>
        public String PublicationYear { get; set; }

        

        /// <summary>
        /// SN
        /// </summary>
        public String ISSN { get; set; }


        /// <summary>
        /// JO
        /// </summary>
        public String AbbreviationJournalName { get; set; }


        /// <summary>
        /// TY
        /// </summary>
        public String Type { get; set; }

   


     
        /// <summary>
        /// KW or K1
        /// </summary>
        public String Keyword { get; set; }

        /// <summary>
        /// AB
        /// </summary>
        public String Abstract { get; set; }

        /// <summary>
        /// ID
        /// </summary>
        public String ReferenceIdentifier { get; set; }

        public String Citation { get; set; }
        public String EmailText { get; set; }

        public String ContactPerson { get; set; }
        public String Email { get; set; }
        public String AdsorptionIsotherms { get; set; }
        public String PoreDistribution { get; set; }

        /// <summary>
        /// DA
        /// </summary>
        public String DateAccessed { get; set; }

        public int IzothermNumber { get; set; }
        public int ImageNumber { get; set; }
        public String FormalNicelyPDFName { get; set; }

        public Article()
        {
            this.Autors = new List<String>();
        }

    }
}
