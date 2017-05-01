// MIT License
// Copyright (c) 2017 Paulo Gomes (https://pjbgf.mit-license.org/)

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