namespace CoreCover.Framework.Model
{
    public class FileReference
    {
        public string FilePath { get; }
        public int UniqueId { get; set; }

        public FileReference(string filePath, int uniqueId)
        {
            FilePath = filePath;
            UniqueId = uniqueId;
        }
    }
}