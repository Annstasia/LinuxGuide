
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;



namespace LinuxGuide
{


    class MainClass
    {
        //Dictionary<string, List<Dictionary<string, List<string>>>> table =
        //            new Dictionary<string, List<Dictionary<string, List<string>>>>();
        static Dictionary<string, Dictionary<string, List<string>>> table =
                    new Dictionary<string, Dictionary<string, List<string>>>();

        static string Input_ans(string quest, bool valid_empty=false)
        {
            Console.WriteLine(quest);
            string inputed = Console.ReadLine().Replace("\n", "");
            if (!valid_empty)
            {
                while (inputed == "")
                {
                    Console.WriteLine(quest);
                    inputed = Console.ReadLine();
                }
            }
            if (inputed == "Выйти")
            {
                Exit();
            }
            if (inputed == "Просмотр")
            {
                Looking();
                inputed = Input_ans(quest, valid_empty);
            }
            if (inputed == "Справка")
            {
                Reference();
                inputed = Input_ans(quest, valid_empty);
            }

            return inputed;

        }

        static void Start()
        {
            while (true)
            {
                //Console.WriteLine("Введите режим (Игра, Добавление, Удаление, Просмотр)");
                // string command = Console.ReadLine();

                Header();

                string command = Input_ans("Введите режим (Игра, Добавление, Удаление)");
                // if (command == "Выйти") Exit();
                while (!(command == "Игра" || command == "Добавление" || command == "Удаление"))
                {
                    command = Input_ans("Введите режим (Игра, Добавление, Удаление, Просмотр)");
                }
                Console.Clear();

                if (command == "Игра")
                {
                    Play();
                }
                if (command == "Добавление")
                {
                    Adding();
                }
                if (command == "Удаление")
                {
                    Delete();
                }

                Console.Clear();
            }
        }



        static void Play() {
            if (table.Count == 0)
            {
                Console.WriteLine("Нет данных");
                Console.ReadKey();
                return;
            }
            string category = ChooseCategory();
            if (category == "Главная") return;
            string category1 = "" + category;
            string ans = "";
            string quest = "";
            bool without_help = true;
            Random random = new Random();

            while (true)
            {
                Header();
                without_help = true;
                Console.WriteLine("Введите команду, которая выполняет указанное действие:");
                if (category == "Все")
                {
                    int i1 = random.Next(table.Count);
                    foreach (string s in table.Keys)
                    {
                        if (i1 == 0)
                        {
                            category1 = s;
                            break;
                        }
                        i1 -= 1;
                    }
                }
                int i = random.Next(table[category1].Count);
                foreach (string s in table[category1].Keys)
                {
                    if (i == 0)
                    {
                        quest = s;
                        break;
                    }
                    i -= 1;
                }
                ans = Input_ans(quest);
                if (ans == "Помощь")
                {
                    Show_answer(category1, quest);
                    without_help = false;
                    Console.ReadKey();
                    Console.Clear();
                    continue;
                }
                if (ans == "Главная")return;

                while (!(table[category1][quest].Contains(ans)))
                {
                    Console.Write("Неверно!");
                    ans = Input_ans("Попробуйте еще раз или посмотрите праильные ответы, введя 'Помощь'");
                    if (ans == "Помощь")
                    {
                        without_help = false;
                        Show_answer(category1, quest);
                        break;
                    }
                    if (ans == "Главная")return;
                }
                if (without_help){
                    Console.WriteLine("Верно!");
                }
                Console.ReadKey();
                Console.Clear();
            }
        }


        static void Adding()
        {
            while (true)
            {
                Header();
                string category = Input_ans("Введите категорию");
                if (category == "Главная") return;
                string quest = Input_ans("Введите описание команды");
                if (quest == "Главная") return;
                string ans = Input_ans("Введите команду");
                if (ans == "Главная") return;


                if (!table.ContainsKey(category))
                {
                    table[category] = new Dictionary<string, List<string>>();
                }
                if (!table[category].ContainsKey(quest))
                {
                    table[category][quest] = new List<string>();
                }
                table[category][quest].Add(ans);
                Console.WriteLine("Добавлено!");
                Console.ReadKey();
                Console.Clear();
            }




        }

        static void Delete()
        {
            while (true)
            {
                Header();
                if (table.Count == 0)
                {
                    Console.WriteLine("Нет данных");
                    Console.ReadKey();
                    return;
                }
                string category = Input_ans("Введите категорию (Если хотите удалить все категории, нажмите Enter)", true);
                if (category == "Главная") return;
                if (category == "")
                {
                    table.Clear();
                    Console.WriteLine("Удалено");
                    Console.ReadKey();
                    Console.Clear();
                    continue;
                }
                string quest = Input_ans("Введите описание (Если хотите удалить все описания в категории, нажмите Enter)", true);
                if (quest == "Главная") return;
                if (quest == "")
                {
                    table.Remove(category);
                    Console.WriteLine("Удалено");
                    Console.ReadKey();
                    Console.Clear();
                    continue;
                }
                string ans = Input_ans("Введите команду (Если хотите удалить все команды, выполняющие данное описание , нажмите Enter)", true);
                if (ans == "Главная") return;
                if (ans == "")
                {
                    table[category].Remove(quest);
                    if (table[category].Count == 0)
                    {
                        table.Remove(category);
                    }
                    Console.WriteLine("Удалено");
                    Console.ReadKey();
                    Console.Clear();
                    continue;
                }
                else
                {
                    table[category][quest].Remove(ans);
                    if (table[category][quest].Count == 0)
                    {
                        table[category].Remove(quest);
                        if (table[category].Count == 0)
                        {
                            table.Remove(category);
                        }
                    }
                    Console.WriteLine("Удалено");
                }
                Console.ReadKey();
                Console.Clear();
            }


        }


        static void Looking()
        {
            // Console.Clear();
            foreach (string category in table.Keys)
            {
                Console.WriteLine("Категория \"" + category + "\":");
                bool first;
                foreach (string quest in table[category].Keys)
                {
                    Console.Write("\t");
                    first = true;
                    foreach (string ans in table[category][quest])
                    {
                        if (first)
                        {
                            Console.Write(ans);
                            first = false;
                        }
                        else Console.Write("; " + ans);
                    }
                    Console.WriteLine(" - " + quest);
                }
                Console.WriteLine();
            }
        }

        static void Reference()
        {
            Console.WriteLine();
            Console.WriteLine("\tДанная игра предназначена для запоминания основных команд Linux");
            Console.WriteLine("\tВаша задача -  вводить команды, которые выполняют указанные действия");
            Console.WriteLine("\tВсе команды разделены на категории");
            Console.WriteLine();
            Console.WriteLine("\tЧтобы начать игру, вам необходимо ввести слово \"Игра\" на главной странице, " +
            	"после чего выбрать категорию, команды которой вы хотите угадывать. Если вы хотите угадывать команды всех категорий, выберите \"Все\"");
            Console.WriteLine("\tВам будут представлены описания команд в случайном порядке, вы должны ввести любую команду, удовлетворяющую данному описанию.");
            Console.WriteLine("\tЕсли вы не помните команду, то введите \"Помощь\"");
            Console.WriteLine("\tИгра бесконечна, возможны повторы команд");
            Console.WriteLine("\tКогда вы захотите закончить игру введите \"Главная\", чтобы вернуться в основное меню, или \"Выйти\", чтобы выйти из игры");
            Console.WriteLine();
            Console.WriteLine("\tТакже у вас есть возможность добавить свои команды");
            Console.WriteLine("\tДля этого введите в основном меню \"Добавление\"");
            Console.WriteLine("\tВы можете создавать новые категории, команды и их описания");
            Console.WriteLine("\tДопустимо использовать несколько команд с одинаковым описанием. При этом во время игры достаточно ввести любую команду, удовлетворяющую описанию");
            Console.WriteLine("\tНе допустимо создание пустых категорий, команд без описаний");
            Console.WriteLine("\tНе допустимо использовать в названиях категорий команд и описаний служебные слова: Просмотр, Справка, Главная, Выйти, Помощь");
            Console.WriteLine("\tДля сохранения изменений после выхода из программы необходимо осуществлять выход только командой \"Выйти\"");
            Console.WriteLine(); 
            Console.WriteLine("\tВы можете удалять команды");
            Console.WriteLine("\tДля этого введите в основном меню \"Удаление\"");
            Console.WriteLine("\tДля сохранения изменений после выхода из программы необходимо осуществлять выход только командой \"Выйти\"");
            Console.WriteLine();
            Console.WriteLine("\tИспользуйте следующие служебные слова:");
            Console.WriteLine("\t\tВыйти - для завершения программы с сохранением изменений");
            Console.WriteLine("\t\tПросмотр - для вывода всех команд с описанием и категориями");
            Console.WriteLine("\t\tСправка - для вывода инструкции");
            Console.WriteLine("\t\tГлавная - для перехода в основное меню");
            Console.WriteLine("\tКонец справки");
            Console.WriteLine();













        }

        static void Header()
        {
            Console.WriteLine("### Добро пожаловать в LinuxGuide. Ниже представлена небольшая шпаргалка");
            Console.WriteLine("### Для выхода с сохранением данным введите \"Выйти\"");
            Console.WriteLine("### Для перехода в начальное меню введите \"Главная\"");
            Console.WriteLine("### Для просмотра всех данных введите \"Просмотр\"");
            Console.WriteLine("### Для просмотра краткого руководства введите \"Справка\"");
        }

        static string ChooseCategory()
        {
            Header();
            Console.WriteLine("Выберите категорию:");
            Console.WriteLine("Все;");
            foreach (string s in table.Keys)
            {
                Console.WriteLine(s + ";");
            }
            string category = Console.ReadLine();
            while (!(category == "Все" || table.ContainsKey(category)))
            {
                category = Input_ans("Выберите категорию из заданных!");
                if (category == "Главная") return "Главная";
            }
            Console.Clear();
            return category;
        }

        static void Show_answer(string category1, string quest)
        {
            Console.Write("Правильными ответами являются: ");
            foreach (string s in table[category1][quest])
            {
                Console.Write(s + "; ");
            }
            Console.WriteLine();

        }

        static void FillingOut()
        {

            using (FileStream fstream = new FileStream("table.json", FileMode.OpenOrCreate))
            {
                byte[] array = new byte[fstream.Length];
                // считываем данные
                fstream.Read(array, 0, array.Length);
                string textFromFile = System.Text.Encoding.Default.GetString(array);
                // Console.WriteLine($"Текст из файла: {textFromFile}");
                table = JsonConvert.DeserializeObject<Dictionary <string, Dictionary<string, List<string>>>>(textFromFile);
            }
      
        }

        static void Exit()
        {
            using (FileStream fstream = new FileStream("table.json", FileMode.OpenOrCreate))
            {
                // преобразуем строку в байты
                byte[] array = System.Text.Encoding.Default.GetBytes(JsonConvert.SerializeObject(table));
                // запись массива байтов в файл
                fstream.Write(array, 0, array.Length);
                Console.WriteLine("Текст записан в файл");
            }
            Environment.Exit(0);
        }


        public static void Main(string[] args)
        {


            FillingOut();
            Start();

        }
    }



}
