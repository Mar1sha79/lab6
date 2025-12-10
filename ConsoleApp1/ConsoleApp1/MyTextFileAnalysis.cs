using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public static class MyTextFileAnalysis
{
    private const string DefaultFileName = "text_analysis.txt";

    private static void CreateDefaultFile()
    {
        File.WriteAllText(DefaultFileName, "", Encoding.UTF8);
    }

    public static HashSet<char> GetCommonCharactersFromFile()
    {
        if (!File.Exists(DefaultFileName))
        {
            CreateDefaultFile();
            throw new InvalidOperationException("Файл " + DefaultFileName + " создан. Добавьте текст в файл и запустите программу снова.");
        }

        string text = File.ReadAllText(DefaultFileName, Encoding.UTF8);

        if (string.IsNullOrWhiteSpace(text))
            throw new InvalidOperationException("Файл " + DefaultFileName + " пуст. Добавьте текст в файл.");

        var words = text.Split(new[] { ' ', ',', '.', '!', '?', ';', ':', '\t', '\n', '\r',
                                      '(', ')', '[', ']', '{', '}', '"', '\'', '-', '—' },
                              StringSplitOptions.RemoveEmptyEntries);

        var validWords = words.Where(word => word.Any(char.IsLetter)).ToArray();

        if (validWords.Length == 0)
            return new HashSet<char>();

        var result = new HashSet<char>(validWords[0]);
        foreach (var word in validWords.Skip(1))
        {
            if (word.Length > 0)
            {
                result.IntersectWith(word);
            }
        }
        return result;
    }

    public static (string fileName, long fileSize, DateTime createdTime) GetFileInfo()
    {
        if (!File.Exists(DefaultFileName))
        {
            CreateDefaultFile();
        }

        FileInfo fileInfo = new FileInfo(DefaultFileName);
        return (fileInfo.Name, fileInfo.Length, fileInfo.CreationTime);
    }
}