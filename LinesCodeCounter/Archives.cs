namespace LinesCodeCounter
{
    public abstract class ArchiveBase
    {
        public string Name { get; protected set; }
        public string Path { get; protected set; }
        public ArchiveBase Parent { get; protected set; }
        public List<ArchiveBase> Children { get; protected set; }

        protected ArchiveBase(string path, ArchiveBase parent = null)
        {
            this.Path = path;
            this.Name = System.IO.Path.GetFileName(path);
            this.Parent = parent;
            this.Children = new List<ArchiveBase>();
        }

        public abstract void Open();
        public abstract void Delete();

        public string GetName() => Name;
        public string GetPath() => Path;
    }

    public class Folder : ArchiveBase
    {
        private DirectoryInfo directory;

        public Folder(string path, ArchiveBase parent = null) : base(path, parent)
        {
            directory = new DirectoryInfo(path);
        }

        public override void Open()
        {
            Children.Clear();

            foreach (var dir in directory.GetDirectories())
            {
                var childFolder = new Folder(dir.FullName, this);
                childFolder.Open();
                Children.Add(childFolder);
            }

            foreach (var file in directory.GetFiles())
            {
                var childFile = new FileArchive(file.FullName, this);
                Children.Add(childFile);
            }
        }

        public override void Delete()
        {
            foreach (var child in Children)
            {
                child.Delete();
            }
            directory.Delete(true);
        }

        public int CountLines(List<string> extensions)
        {
            int totalLines = 0;
            foreach (var child in Children)
            {
                if (child is FileArchive file)
                {
                    totalLines += file.CountLines(extensions);
                }
                else if (child is Folder folder)
                {
                    totalLines += folder.CountLines(extensions);
                }
            }
            return totalLines;
        }
    }

    public class FileArchive : ArchiveBase
    {
        private FileInfo fileInfo;

        public FileArchive(string path, ArchiveBase parent = null) : base(path, parent)
        {
            fileInfo = new FileInfo(path);
        }

        public override void Open()
        {
            Console.WriteLine($"Open archive: {Path}");
        }

        public override void Delete()
        {
            fileInfo.Delete();
        }

        public int CountLines(List<string> extensions)
        {
            if (!extensions.Contains(fileInfo.Extension))
            {
                return 0;
            }

            int lineCount = 0;
            try
            {
                using (StreamReader reader = new StreamReader(fileInfo.FullName))
                {
                    while (reader.ReadLine() != null)
                    {
                        lineCount++;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file {fileInfo.FullName}: {ex.Message}");
            }
            return lineCount;
        }
    }
}
