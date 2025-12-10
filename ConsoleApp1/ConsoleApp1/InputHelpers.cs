using System;
using System.Collections.Generic;

public static class InputHelpers
{
    public static int ReadInt(string prompt, int minValue = int.MinValue, int maxValue = int.MaxValue)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = Console.ReadLine();
            if (input == null) input = "";
            if (int.TryParse(input.Trim(), out int result) && result >= minValue && result <= maxValue)
                return result;
            Console.WriteLine($"Ошибка! Введите целое число от {minValue} до {maxValue}");
        }
    }

    public static string ReadString(string prompt, bool allowEmpty = false)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = Console.ReadLine();
            if (input == null) input = "";
            input = input.Trim();
            if (!allowEmpty && string.IsNullOrEmpty(input))
                Console.WriteLine("Ошибка! Ввод не может быть пустым");
            else
                return input;
        }
    }

    public static List<int> ReadIntList(string prompt)
    {
        Console.WriteLine(prompt);
        var list = new List<int>();
        while (true)
        {
            Console.Write("Введите число (или stop для завершения): ");
            string input = Console.ReadLine();
            if (input == null) input = "";
            input = input.Trim().ToLower();
            if (input == "stop")
                break;
            if (int.TryParse(input, out int number))
                list.Add(number);
            else
                Console.WriteLine("Ошибка! Введите целое число или stop");
        }
        return list;
    }

    public static LinkedList<int> ReadLinkedList(string prompt)
    {
        Console.WriteLine(prompt);
        var list = new LinkedList<int>();
        while (true)
        {
            Console.Write("Введите число (или stop для завершения): ");
            string input = Console.ReadLine();
            if (input == null) input = "";
            input = input.Trim().ToLower();
            if (input == "stop")
                break;
            if (int.TryParse(input, out int number))
                list.AddLast(number);
            else
                Console.WriteLine("Ошибка! Введите целое число или stop");
        }
        return list;
    }
}