using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.InputEncoding = System.Text.Encoding.UTF8;

        while (true)
        {
            Console.Clear();
            Console.WriteLine("ЛАБОРАТОРНАЯ РАБОТА №4");
            Console.WriteLine("1. Задание 1 - Количество различных элементов списка");
            Console.WriteLine("2. Задание 2 - Замена соседей в LinkedList");
            Console.WriteLine("3. Задание 3 - Сельскохозяйственные культуры");
            Console.WriteLine("4. Задание 4 - Общие символы в словах");
            Console.WriteLine("5. Задание 5 - Анализ школ");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите задание: ");

            string choice = Console.ReadLine();
            if (choice == null) choice = "";

            switch (choice)
            {
                case "1":
                    Task1();
                    break;
                case "2":
                    Task2();
                    break;
                case "3":
                    Task3();
                    break;
                case "4":
                    Task4();
                    break;
                case "5":
                    Task5();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Неверный выбор!");
                    break;
            }

            Console.WriteLine("\nНажмите любую клавишу для продолжения...");
            Console.ReadKey();
        }
    }

    static void Task1()
    {
        Console.WriteLine("\nЗАДАНИЕ 1");
        Console.WriteLine("Определение количества различных элементов списка");

        var list = InputHelpers.ReadIntList("Введите элементы списка (целые числа):");

        int distinctCount = MyCollectionOperations.CountDistinctElements(list);
        Console.WriteLine("\nСписок: [" + string.Join(", ", list) + "]");
        Console.WriteLine("Количество различных элементов: " + distinctCount);
    }

    static void Task2()
    {
        Console.WriteLine("\nЗАДАНИЕ 2");
        Console.WriteLine("Замена соседей элемента на позиции E на элемент F в LinkedList");

        var list = InputHelpers.ReadLinkedList("Введите элементы LinkedList:");

        if (list.Count < 3)
        {
            Console.WriteLine("Ошибка! LinkedList должен содержать минимум 3 элемента");
            return;
        }

        Console.WriteLine("\nТекущий список: [" + string.Join(" -> ", list) + "]");

        int E = InputHelpers.ReadInt("Введите позицию элемента E (0-" + (list.Count - 1) + "): ", 0, list.Count - 1);
        int F = InputHelpers.ReadInt("Введите элемент F: ", int.MinValue, int.MaxValue);

        var originalList = new LinkedList<int>(list);

        try
        {
            MyCollectionOperations.ReplaceNeighborsByPosition(list, E, F);

            Console.WriteLine("\nИсходный список: [" + string.Join(" -> ", originalList) + "]");
            Console.WriteLine("Результат: [" + string.Join(" -> ", list) + "]");

            var elementNode = list.First;
            for (int i = 0; i < E; i++)
            {
                elementNode = elementNode.Next;
            }
            Console.WriteLine("\nПояснение: Соседи элемента на позиции " + E + " (значение: " + elementNode.Value + ") заменены на " + F);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка: " + ex.Message);
        }
    }

    static void Task3()
    {
        Console.WriteLine("\nЗАДАНИЕ 3");
        Console.WriteLine("Анализ сельскохозяйственных культур");

        var cooperatives = new Dictionary<int, HashSet<string>>();

        int n = InputHelpers.ReadInt("Введите количество кооперативов: ", 1, 100);

        for (int i = 1; i <= n; i++)
        {
            Console.WriteLine("\nКООПЕРАТИВ №" + i + "");
            var cultures = new HashSet<string>();

            Console.WriteLine("Вводите названия культур по одной в строке.");
            Console.WriteLine("Для завершения ввода введите пустую строку.");

            int cultureCount = 0;
            while (true)
            {
                cultureCount++;
                Console.Write("Введите культуру " + cultureCount + ": ");
                string culture = Console.ReadLine();
                if (culture == null) culture = "";
                culture = culture.Trim();

                if (string.IsNullOrEmpty(culture))
                {
                    if (cultures.Count == 0)
                    {
                        Console.WriteLine("Ошибка! Нужно ввести хотя бы одну культуру.");
                        cultureCount--;
                        continue;
                    }
                    break;
                }

                if (cultures.Contains(culture))
                {
                    Console.WriteLine("Культура '" + culture + "' уже есть в списке!");
                    cultureCount--;
                }
                else
                {
                    cultures.Add(culture);
                    Console.WriteLine("Добавлена культура: '" + culture + "'");
                }

                Console.WriteLine("Текущий список: " + string.Join(", ", cultures));
                Console.WriteLine();
            }

            cooperatives[i] = cultures;
            Console.WriteLine("Кооператив №" + i + " завершен");
            Console.WriteLine("Итоговый список культур: " + string.Join(", ", cultures));
            Console.WriteLine();
        }

        bool hasCultures = false;
        foreach (var coop in cooperatives)
        {
            if (coop.Value.Count > 0)
            {
                hasCultures = true;
                break;
            }
        }

        if (!hasCultures)
        {
            Console.WriteLine("Ошибка! Ни в одном кооперативе не введены культуры.");
            return;
        }

        Console.WriteLine("\n" + new string('=', 50));
        Console.WriteLine("ВЫПОЛНЯЕМ АНАЛИЗ...");
        Console.WriteLine(new string('=', 50));

        var result = MySetAnalysis.AnalyzeSets(cooperatives);

        // Выводим результаты
        Console.WriteLine("\n" + new string('=', 60));
        Console.WriteLine("РЕЗУЛЬТАТЫ АНАЛИЗА");
        Console.WriteLine(new string('=', 60));

        Console.WriteLine("Всего кооперативов: " + n);

        int totalCultures = 0;
        foreach (var coop in cooperatives)
        {
            totalCultures += coop.Value.Count;
        }
        Console.WriteLine("Всего введено культур: " + totalCultures);

        Console.WriteLine("\n1. КУЛЬТУРЫ ВО ВСЕХ КООПЕРАТИВАХ:");
        if (result.inAll.Count > 0)
        {
            int counter = 1;
            foreach (var culture in result.inAll.OrderBy(c => c))
            {
                Console.WriteLine("   " + counter + ". " + culture);
                counter++;
            }
        }
        else
        {
            Console.WriteLine("   Нет культур, которые выращиваются во всех кооперативах");
        }

        Console.WriteLine("\n2. КУЛЬТУРЫ ТОЛЬКО В НЕКОТОРЫХ КООПЕРАТИВАХ:");
        if (result.inSome.Count > 0)
        {
            int counter = 1;
            foreach (var culture in result.inSome.OrderBy(c => c))
            {
                int count = 0;
                foreach (var coop in cooperatives)
                {
                    if (coop.Value.Contains(culture))
                    {
                        count++;
                    }
                }
                Console.WriteLine("   " + counter + ". " + culture + " (в " + count + " кооперативах)");
                counter++;
            }
        }
        else
        {
            Console.WriteLine("   Нет культур, которые выращиваются только в некоторых кооперативах");
        }

        Console.WriteLine("\n3. КУЛЬТУРЫ РОВНО В ОДНОМ КООПЕРАТИВЕ:");
        if (result.inOne.Count > 0)
        {
            int counter = 1;
            foreach (var culture in result.inOne.OrderBy(c => c))
            {
                int coopNumber = 0;
                foreach (var coop in cooperatives)
                {
                    if (coop.Value.Contains(culture))
                    {
                        coopNumber = coop.Key;
                        break;
                    }
                }
                Console.WriteLine("   " + counter + ". " + culture + " (только в кооперативе №" + coopNumber + ")");
                counter++;
            }
        }
        else
        {
            Console.WriteLine("   Нет культур, которые выращиваются ровно в одном кооперативе");
        }

        // Дополнительная информация
        Console.WriteLine("\n" + new string('-', 40));
        Console.WriteLine("СВОДКА ПО КООПЕРАТИВАМ:");
        foreach (var coop in cooperatives.OrderBy(kv => kv.Key))
        {
            Console.WriteLine("Кооператив №" + coop.Key + ": " + coop.Value.Count + " культур - " + string.Join(", ", coop.Value.OrderBy(c => c)));
        }
    }

    static void Task4()
    {
        Console.WriteLine("\nЗАДАНИЕ 4");
        Console.WriteLine("Определение символов, которые есть в каждом слове текста из файла");

        try
        {
            var fileInfo = MyTextFileAnalysis.GetFileInfo();
            Console.WriteLine("Используемый файл: " + fileInfo.fileName);

            var commonChars = MyTextFileAnalysis.GetCommonCharactersFromFile();

            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("РЕЗУЛЬТАТЫ АНАЛИЗА");
            Console.WriteLine(new string('=', 60));

            Console.WriteLine("\nСимволы, которые есть в КАЖДОМ слове:");
            if (commonChars.Count > 0)
            {
                Console.WriteLine("Найдено " + commonChars.Count + " символов: " + string.Join(", ", commonChars.OrderBy(c => c)));
            }
            else
            {
                Console.WriteLine("Нет символов, которые присутствуют в каждом слове");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка: " + ex.Message);
        }
    }

    static void Task5()
    {
        Console.WriteLine("\nЗАДАНИЕ 5");
        Console.WriteLine("Анализ школ с баллом выше среднего по району");

        while (true)
        {
            Console.WriteLine("\nВыберите действие:");
            Console.WriteLine("1 - Показать текущие данные");
            Console.WriteLine("2 - Добавить нового ученика");
            Console.WriteLine("3 - Очистить файл");
            Console.WriteLine("4 - Выполнить анализ");
            Console.WriteLine("0 - Вернуться в меню");
            Console.Write("Ваш выбор: ");

            string choice = Console.ReadLine();
            if (choice == null) choice = "";

            switch (choice)
            {
                case "1":
                    MyStudentAnalysis.ShowFileContent();
                    break;
                case "2":
                    MyStudentAnalysis.AddStudentThroughConsole();
                    break;
                case "3":
                    MyStudentAnalysis.ClearFile();
                    break;
                case "4":
                    ExecuteSchoolAnalysis();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Неверный выбор!");
                    break;
            }
        }
    }

    static void ExecuteSchoolAnalysis()
    {
        try
        {
            var fileInfo = MyStudentAnalysis.GetStudentsFileInfo();
            Console.WriteLine("Используемый файл: " + fileInfo.fileName);

            var students = MyStudentAnalysis.ReadStudentsFromFile();
            var (schools, singleAverage) = MyStudentAnalysis.FindSchoolsAboveAverage(students);

            Console.WriteLine("\n" + new string('=', 60));
            Console.WriteLine("РЕЗУЛЬТАТЫ АНАЛИЗА");
            Console.WriteLine(new string('=', 60));

            if (schools.Count > 0)
            {
                Console.WriteLine("Школы с баллом выше среднего: " + string.Join(" ", schools));

                if (schools.Count == 1 && singleAverage.HasValue)
                {
                    Console.WriteLine("Средний балл = " + singleAverage.Value.ToString("F1"));
                }
            }
            else
            {
                Console.WriteLine("Нет школ с баллом выше среднего по району");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка: " + ex.Message);
        }
    }
}