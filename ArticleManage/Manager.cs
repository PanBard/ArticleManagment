using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ArticleManage
{
    internal class Manager
    {
        public List<Article> articles_no;
        public List<Article> articles_yes;
        public Manager(EnviromentCreator env) 
        {
            MethodsArchive method = new MethodsArchive();
            Exporter export = new Exporter(env);
            Importer import = new Importer();
            Email email = new Email();

            //String[] txt_no = import.readTxtFile(env.txtInputFolderPath + "\\niesczytane_contacts.txt");
            //this.articles_no = method.returnArticleWithDataFromRISfiles(env.riscFilesPaths_no, env.RISInputFolderPath_no);
            //this.articles_no = method.insertContactAndFiguresToArticles(txt_no, this.articles_no);
            //this.articles_no = method.returnArticleWithGeneratedCitation(this.articles_no);
            //this.articles_no = email.generateEmailsForArticels(articles_no);
            //export.exportEmails(articles_no, "no");
            //export.exportAllEmailsToTxt(this.articles_no, "niesczytane_all.txt");
            //export.exportInfoToTxtWORD(this.articles_no, "niezczytane_WORD.txt");

            //String[] txt_yes = import.readTxtFile(env.txtInputFolderPath+ "\\sczytane_contacts.txt");            
            //this.articles_yes = method.returnArticleWithDataFromRISfiles(env.riscFilesPaths_yes, env.RISInputFolderPath_yes);            
            //this.articles_yes = method.insertContactAndFiguresToArticles(txt_yes, this.articles_yes);
            //this.articles_yes = method.returnArticleWithGeneratedCitation(this.articles_yes);
            //this.articles_yes = email.generateEmailsForArticels(articles_yes);
            //export.exportEmails(articles_yes, "yes");
            //export.exportAllEmailsToTxt(this.articles_yes, "sczytane_all.txt");
            //export.exportInfoToTxtWORD(this.articles_yes, "zczytane_WORD.txt");

            //export.ExportJson("niesczytane.json", articles_no);
            //export.ExportJson("sczytane.json", articles_yes);
            //export.SaveSummaryExcelFile(articles_no, articles_yes, "12.xlsx");








            //this.articles = method.convertTxtFile_toObject_niesczytane(txt);
            //export.ExportJson("article.json", this.articles);
            //readSerializedFile(this.serializeFilePath);
            //export.SaveSummaryExcelFile(articles,"");
            //method.updateArticle_sczytane(articles);
            //method.updateArticle_niesczytane(articles);
            //SaveSummaryExcelFile(articles, "excel_niesczytane.xlsx");
            //SaveSummaryExcelFile(articles, "excel_sczytane.xlsx");
            //niesczytane_get_contact_adreses_and_fig_numbers();
            //method.sczytane_get_contact_adreses_and_fig_numbers(articles);       
        }

        //AU Naile Karakehya,
        //T1 Effects of one-step and two-step KOH activation method on the properties and supercapacitor performance of highly porous activated carbons prepared from Lycopodium clavatum spores,
        //JO Diamond and Related Materials,
        //VL Volume 135,
        //PY 2023,
        //SP 109873,
        //DO https://doi.org/10.1016/j.diamond.2023.109873.

        private void display()
        {
            foreach (var article in articles_no)
            {   
                
                Console.Write(article.Id+" - ");
                Console.WriteLine(article.FileName);

                foreach(var item in article.Autors) 
                {
                    Console.Write(item);
                }
                Console.WriteLine("\n"+article.PrimaryTitle);
                Console.WriteLine(article.AbbreviationJournalName);
                Console.WriteLine(article.Volume);
                Console.WriteLine(article.PublicationYear);
                Console.WriteLine(article.StartPage);
                Console.WriteLine(article.DOI);

                Console.WriteLine(article.ContactPerson);
                Console.WriteLine(article.Email);
                Console.WriteLine(article.AdsorptionIsotherms);
                Console.WriteLine(article.PoreDistribution+"\n");

            }

            foreach (var article in articles_yes)
            {

                Console.Write(article.Id + " - ");
                Console.WriteLine(article.FileName);

                foreach (var item in article.Autors)
                {
                    Console.Write(item);
                }
                Console.WriteLine("\n" + article.PrimaryTitle);
                Console.WriteLine(article.AbbreviationJournalName);
                Console.WriteLine(article.Volume);
                Console.WriteLine(article.PublicationYear);
                Console.WriteLine(article.StartPage);
                Console.WriteLine(article.DOI);

                Console.WriteLine(article.ContactPerson);
                Console.WriteLine(article.Email);
                Console.WriteLine(article.AdsorptionIsotherms);
                Console.WriteLine(article.PoreDistribution + "\n");

            }

        }



        

        


   

      



       

       

     
    }
}
