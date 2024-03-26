using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleManage
{
    internal class Logo
    {
        public Logo() 
        {
            DisplayLogo();
        }

        public void DisplayLogo()
        {
            string workingDirectory = Environment.CurrentDirectory; // This will get the current WORKING directory (i.e. \bin\Debug)            
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName; // This will get the current PROJECT directory
            string path = projectDirectory + "\\" + "logo\\autobot_logo.txt";
            string[] seralizedArticles = File.ReadAllLines(path);
            foreach (var item in seralizedArticles)
            {
                Console.WriteLine(item);
            }
        }
    }
}
