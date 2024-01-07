using ArticleManage;

internal class Program
{
    private static void Main(string[] args)
    {
        //EnviromentCreator env = new EnviromentCreator("articleManagerFolder", "outputFolder", "txtInputFolder", "RISInputFolder");
        //Manager art = new Manager(env);


        FoldersStructure folders = new FoldersStructure();
        RenameFiles worker = new RenameFiles(folders);

        MakerFolderForEachPDF folderMaker = new MakerFolderForEachPDF(folders);
        Excel excel = new Excel(folders);


    }
}