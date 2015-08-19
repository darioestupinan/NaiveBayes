using System.IO;
using System;

namespace FileLoader
{
    public interface IFileLoader
    {
        TextReader LoadArffFile(string path);
        string GetPath(string path);
        void SaveJsonFileToText(string json, string path);
    }
}
