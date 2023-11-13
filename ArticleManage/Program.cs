using ArticleManage;

internal class Program
{
    private static void Main(string[] args)
    {       
        EnviromentCreator env = new EnviromentCreator("articleManagerFolder", "outputFolder", "txtInputFolder", "RISInputFolder");
        Manager art = new Manager(env);
        

    }
}