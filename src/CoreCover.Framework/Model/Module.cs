// MIT License
// Copyright (c) 2017 Paulo Gomes (https://pjbgf.mit-license.org/)

using System;
using System.Collections.Generic;

namespace CoreCover.Framework.Model
{
    public class Module
    {
        public Module()
        {
            Types = new List<Type>();
            FileReferences = new List<FileReference>();
        }

        public string ModuleHash { get; set; }
        public string ModulePath { get; set; }
        public string ModuleName { get; set; }
        public DateTime ModuleTime { get; set; }
        public List<FileReference> FileReferences { get; }
        public List<Type> Types { get; }

        public FileReference AddFileReference(string filePath)
        {
            var fileReference = new FileReference(filePath, FileReferences.Count + 1);
            FileReferences.Add(fileReference);

            return fileReference;
        }

        public void AddType(Type type)
        {
            Types.Add(type);
        }
    }
}