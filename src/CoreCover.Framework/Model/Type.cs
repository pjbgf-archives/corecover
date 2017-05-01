// MIT License
// Copyright (c) 2017 Paulo Gomes (https://pjbgf.mit-license.org/)

using System.Collections.Generic;

namespace CoreCover.Framework.Model
{
    public class Type
    {
        public List<Method> Methods { get; }

        public Type(string fullName, int numberOfMethods)
        {
            Methods = new List<Method>(numberOfMethods);
            FullName = fullName;
        }

        public string FullName { get; }

        public void AddMethod(Method method)
        {
            Methods.Add(method);
        }
    }
}