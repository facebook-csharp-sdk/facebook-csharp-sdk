namespace Facebook
{
    using System;
    using System.IO;
    using System.Reflection;

    public class TestHelpers
    {
        public static string GetPathRelativeToExecutable(string fileName)
        {
            string executable = new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath;
            return Path.GetFullPath(Path.Combine(Path.GetDirectoryName(executable), fileName));
        }
    }
}