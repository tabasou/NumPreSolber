using AppKit;

namespace NumPreSolber
{
    static class MainClass
    {
        static void Main(string[] args)
        {
            NSApplication.Init();

            Question q = new Question();

            NSApplication.Main(args);
        }
    }
}
