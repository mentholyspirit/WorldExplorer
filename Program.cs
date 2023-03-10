namespace Decentraland.Task1
{
    class Program
    {
        static void Main(string[] args)
        {
            using (WorldExplorer worldExplorer = new WorldExplorer())
            {
                worldExplorer.LoadScene(File.ReadAllText("scene.js"));
                worldExplorer.Start(3).Wait();
            }
        }
    }
}
