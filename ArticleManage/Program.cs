using ArticleManage;
using System.Reflection.Metadata;

internal class Program
{
    private static void Main(string[] args)
    {
        //EnviromentCreator env = new EnviromentCreator("articleManagerFolder", "outputFolder", "txtInputFolder", "RISInputFolder");
        //Manager art = new Manager(env);

        Spinner spinner = new Spinner();
        Logo logo = new Logo();

        spinner.Start("folders");
        FoldersStructure folders = new FoldersStructure();
        spinner.Stop();

        spinner.Start("worker");
        RenameFiles worker = new RenameFiles(folders);
        spinner.Stop();

        spinner.Start("folderMaker");
        MakerFolderForEachPDF folderMaker = new MakerFolderForEachPDF(folders);
        spinner.Stop();

        spinner.Start("excel");
        Excel excel = new Excel(folders);
        spinner.Stop();

        spinner.Start("json");
        jsonExporter jSON = new jsonExporter(folders);
        spinner.Stop();

        //MethodsArchive methodsArchive = new MethodsArchive();
        //methodsArchive.readCSVFile(folders);
        //methodsArchive.changeCSVFile(folders);

        spinner.Dispose();
    }
}