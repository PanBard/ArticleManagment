using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArticleManage
{
    internal class Email
    {
        public Email() 
        {
        
        }

        public List<Article> generateEmailsForArticels(List<Article> articles)
        {
            foreach (var article in articles)
            {   
                String email = generateEmail(article);
                article.EmailText = email;
            }
            return articles;
        }

        private String generateEmail(Article article)
        {
            if (article.Email != "?" && article.ContactPerson != "?")
            {

                String fig = handleFiguresNumeration(article);
 
                String email = $"Dear {article.ContactPerson}," +
                    $"\r\nI am writing to kindly request access to the data presented in your article: [{article.Citation}], specifically featured in {fig} " +
                    "I am particularly interested in obtaining the adsorption isotherms." +
                    " I am seeking your permission to use this data for testing a new tool for analysing the porous structure of carbonous adsorbents. " +
                    "The results of these tests will be used for publication in international scientific journals." +
                    "\r\nI would like to assure you that I will diligently cite your mentioned article and include appropriate acknowledgments for granting access to the data." +
                    " Your kind consideration of this request would be greatly appreciated." +
                    " Additionally, I kindly ask for the provision of the adsorption isotherms in " +
                    "numerical format, preferably in a text file (txt) or spreadsheet (xls), which will facilitate effective analysis." +
                    "\r\nI am thankful in advance for your understanding and support." +
                    " I look forward to a positive response to my request." +
                    "\r\nSincerely,\r\n";                

                return email;
            }
            else return null;
        }

        private String handleFiguresNumeration(Article article)
        {
            String figures = article.AdsorptionIsotherms;
            List<String> list = new List<String>();
           
            //Console.WriteLine($"    before: {article.AdsorptionIsotherms} | {article.PoreDistribution}");
            //Console.Write("    after: ");
            int leng = article.AdsorptionIsotherms.Length;
            for (int i = 0; i < leng; i++)
            {
                if (article.PoreDistribution == "?")
                {
                    //Console.WriteLine(article.AdsorptionIsotherms);
                    return article.AdsorptionIsotherms;
                 
                }

                if (figures[i] == ',')
                {
                    Console.WriteLine(figures.Substring(i + 1).Trim());
                    //String letter = autor.Substring(i + 1).Trim();
                    //allAutors += autor.Replace(letter, "") + letter[0] + "." + "; ";
                    break;
                }
                else
                {
                    if(article.AdsorptionIsotherms == article.PoreDistribution)
                    {
                        //Console.WriteLine(article.AdsorptionIsotherms);
                        return article.AdsorptionIsotherms;
                    }
                    else
                    {
                        
                        //Console.WriteLine(article.AdsorptionIsotherms + "," + article.PoreDistribution);
                        return article.AdsorptionIsotherms +","+article.PoreDistribution;
                    }
                }
            }

            return figures;
        }
    }
}
