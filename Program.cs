using System;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Diary {

    #region Задание
    /// Разработать ежедневник.
    /// В ежедневнике реализовать возможность 
    /// - создания
    /// - удаления
    /// - реактирования 
    /// записей
    /// 
    /// В отдельной записи должно быть не менее пяти полей
    /// 
    /// Реализовать возможность 
    /// - Загрузки даннах из файла
    /// - Выгрузки даннах в файл
    /// - Добавления данных в текущий ежедневник из выбранного файла
    /// - Импорт записей по выбранному диапазону дат
    /// - Упорядочивания записей ежедневника по выбранному полю
    #endregion

    class Program {

        // Идентификаторы записей (Id_Note) должны быть различны (в рамках одной программы).
        // Для этого при загрузке записей из файла ежедневника с совпадающими идентификаторами
        // они будут меняться (для исключения совпадения идентификаторов).

        /// <summary>
        /// Вывод главного меню программы
        /// </summary>
        static public void Menu() {
            int choice = 0;
            Notes notes = new Notes();   // создание пустого ежедневника

            do {    // бесконечный цикл (до выхода пользователя из программы)

                do {
                    Console.Clear();
                    Console.WriteLine("========================================================");
                    Console.WriteLine("                    ГЛАВНОЕ МЕНЮ:");
                    Console.WriteLine("========================================================");
                    Console.WriteLine("1 - Добавить запись в ежедневник;");
                    Console.WriteLine("2 - Редактировать запись в ежедневнике;");
                    Console.WriteLine("3 - Удалить запись из ежедневника;");
                    Console.WriteLine("========================================================");
                    Console.WriteLine("4 - Загрузить данные из файла;");
                    Console.WriteLine("5 - Выгрузить данные в файл;");
                    Console.WriteLine("6 - Добавление данных в текущий ежедневник из файла;");
                    Console.WriteLine("7 - Импорт записей из файла по выбранному диапазону дат;");
                    Console.WriteLine("8 - Упорядочить записи в ежедневнике по выбранному полю;");
                    Console.WriteLine("========================================================");
                    Console.WriteLine("9 - Выйти из программы.");
                    Console.WriteLine("========================================================");
                    Console.WriteLine();

                    Console.Write("Выберите пункт: ");
                    int.TryParse(Console.ReadLine(), out choice);
                } while (choice < 1 || choice > 9);

                Console.Clear();

                switch (choice) {
                    case 1: // выбран пункт "Добавить запись"

                        notes.prntNotes();  // выводим ежедневник (если есть записи)
                        Console.Clear();

                        string whatFamily, whatName, whatSirname;
                        DateTime whatBirthDate;
                        string whatNotation;
                        Mood whatMood = Mood.GOOD;      // по умолчанию настроение хорошее

                        Console.Write("Фамилия: ");
                        whatFamily = Console.ReadLine();
                        Console.Write("Имя: ");
                        whatName = Console.ReadLine();
                        Console.Write("Отчество: ");
                        whatSirname = Console.ReadLine();

                        string sBDay;   // вспомогательная переменная для конвертации в дату
                        do {
                            Console.Write("Дата рождения (формат - дд.мм.гггг): ");
                            sBDay = Console.ReadLine();
                            if (!DateTime.TryParse(sBDay, out whatBirthDate)) {
                                continue; // если дата рождения не является датой, то повторяем ввод
                            }
                            else {
                                break;  // если дата рождения корректна (является датой) то выходим из цикла
                            }
                        } while (true);

                        // Создатель записи
                        Person whatPerson = new Person(whatFamily, whatName, whatSirname, whatBirthDate);

                        Console.Clear();

                        Console.Write("Напишите о настроении(плохое, хорошее, отличное): ");
                        string strMood = Console.ReadLine();
                        if (strMood == "плохое") whatMood = Mood.BAD;
                        else if (strMood == "отличное") whatMood = Mood.GREAT;

                        Console.Clear();

                        Console.Write("Сделайте запись: ");
                        whatNotation = Console.ReadLine();

                        Note note = new Note(whatNotation, whatPerson, whatMood);
                        notes.insertNotes(note);

                        continue;

                    case 2: // выбран пункт "Редактировать запись"

                        // если запсей в ежедневнике нет, то переходим к следующей итерации цикла
                        if (!notes.prntNotes()) {
                            continue;
                        }

                        uint id;    // хранит идентификатор записи
                        do {
                            Console.WriteLine();
                            Console.Write("Введите номер идентификатора редактируемой записи (буква, или 0 - выход в меню): ");
                            uint.TryParse(Console.ReadLine(), out id);
                            if (id == 0) break;

                            if (!notes.isNoteId(id)) {
                                Console.WriteLine("Записи с таким идентификатором не существует!");
                                continue;
                            }

                            break;
                        } while (true);

                        if (id == 0) continue;  // пользователь выбрал выход, выводим меню

                        Console.Clear();
                        whatMood = notes.getNoteForId(id).Mood_Note;
                        Console.Write("Настроение(плохое, хорошее, отличное): ");
                        strMood = Console.ReadLine();
                        if (!String.IsNullOrEmpty(strMood)) {
                            if (strMood == "плохое") whatMood = Mood.BAD;
                            else if (strMood == "отличное") whatMood = Mood.GREAT;
                        }

                        Console.Write("Сделайте запись: ");
                        whatNotation = Console.ReadLine();
                        // Если пользователь ввел пустое поле, то не меняем текст записи
                        if (String.IsNullOrEmpty(whatNotation)) whatNotation = notes.getNoteForId(id).Notation_Note;
                       
                        notes.editNotes(id, whatNotation, whatMood);    // редактируем запись

                        Console.Clear();
                        notes.prntNotes();

                        Console.ReadKey();

                        continue;

                    case 3: // выбран пункт "Удалить запись"

                        // если запсей в ежедневнике нет, то переходим к следующей итерации цикла
                        if (!notes.prntNotes()) {
                            continue;
                        }

                        do {
                            Console.WriteLine();
                            Console.Write("Введите номер идентификатора удаляемой записи (буква, или 0 - выход в меню): ");
                            uint.TryParse(Console.ReadLine(), out id);
                            if (id == 0) break;

                            if (!notes.isNoteId(id)) {
                                Console.WriteLine("Записи с таким идентификатором не существует!");
                                continue;
                            }

                            break;
                        } while (true);

                        if (id == 0) continue;  // пользователь выбрал выход, выводим меню

                        notes.deleteNotes(id);  // удаляем запись

                        Console.Clear();
                        notes.prntNotes();

                        Console.ReadKey();

                        continue;

                    case 4: // выбран пункт "Загрузить данные из файла"


                        break;

                    case 5: // выбран пункт "Выгрузить данные в файл"

                        break;

                    case 6: // выбран пункт "Добавление данных из файла"

                        break;

                    case 7: // выбран пункт "Импорт из файла по диапазону дат"

                        break;

                    case 8: // выбран пункт "Упорядочить записи"

                        break;

                    case 9: // выбран пункт "Выйти"
                        return;
                }

            } while (true);
        }

        /// <summary>
        /// Метод отображающий в консоли анимацию заголовка
        /// </summary>
        /// <param name="title">Текст заголовка</param>
        static public void titleAnimation(string title, ConsoleColor color = ConsoleColor.Green) {
            string logo = title;
            Console.SetCursorPosition(Console.WindowWidth / 2 - logo.Length/2, Console.WindowHeight / 2 - 1);
            Console.ForegroundColor = color;

            foreach (char ch in logo) {
                Console.Write(ch);
                System.Threading.Thread.Sleep(90);
            }
            System.Threading.Thread.Sleep(400);

            for (int i = 0; i < 100; i += 10) {
                
                Console.SetCursorPosition(Console.WindowWidth / 2 - logo.Length / 2, Console.WindowHeight / 2 - 1);
                
                Console.Write(logo);
                System.Threading.Thread.Sleep(100 - i);
                Console.Clear();
            }

            Console.ResetColor();
            Console.Clear();
        }

        static void Main(string[] args) {
            //Console.ReadKey();
            //titleAnimation("<<<ЕЖЕДНЕВНИК>>>", ConsoleColor.Cyan);

            Menu();



            //Note oneNote = new Note("Первая пробная запись", new Person("Иванов", "Иван"), Mood.BAD);

            //Console.WriteLine("Нажмите клавишу...");
            //Console.ReadKey();  // задержка для формирования промежутка времени создания
            //                    // для проверки импорта по диапазону дат в записях

            //Note twoNote = new Note("Вторая пробная запись", new Person("Павлов", "Павел", "Павлович", new DateTime(1986, 7, 1)));

            //Console.WriteLine("Нажмите клавишу...");
            //Console.ReadKey();  // задержка для формирования промежутка времени создания
            //                    // для проверки импорта по диапазону дат в записях

            //Note threeNote = new Note("Третья пробная запись", new Person("Петров", "Петр", "Петрович", new DateTime(2000, 1, 1)));

            //Console.WriteLine("Нажмите клавишу...");
            //Console.ReadKey();  // задержка для формирования промежутка времени создания
            //                    // для проверки импорта по диапазону дат в записях

            //// СОЗДАНИЕ ЕЖЕДНЕВНИКА
            //////////////////////////////////////////////////////////////////////////////
            //Notes notes1 = new Notes(oneNote, twoNote);
            //////////////////////////////////////////////////////////////////////////////
            /////


            //notes1.insertNotes(threeNote);   // добавляем запись в ежедневник

            //// Пример добавления массива записей (можно раскомментировать для тестирования)
            ////Console.WriteLine("Нажмите клавишу...");
            ////Console.ReadKey();  // задержка для формирования промежутка времени создания в записях

            ////notes1.insertNotes(new Note("Четвертая пробная запись", new Person("Петров", "Петр", "Петрович", new DateTime(2000, 1, 1))),
            ////                    new Note("Пятая пробная запись", new Person("Петров", "Петр", "Петрович", new DateTime(2000, 1, 1))),
            ////                    new Note("Шестая пробная запись", new Person("Петров", "Петр", "Петрович", new DateTime(2000, 1, 1)))
            ////                  );

            //// РЕДАКТИРОВАНИЕ ЗАПИСИ В ЕЖЕДНЕВНИКЕ
            //////////////////////////////////////////////////////////////////////////////
            //notes1.editNotes(3, "Дополнительно измененная запись", Mood.GREAT);
            //////////////////////////////////////////////////////////////////////////////
            /////

            //// УДАЛЕНИЕ ЗАПИСИ twoNote ИЗ ЕЖЕДНЕВНИКА
            //////////////////////////////////////////////////////////////////////////////
            //notes1.deleteNotes(twoNote);
            //////////////////////////////////////////////////////////////////////////////
            /////

            //// СОРТИРОВКА ЕЖЕДНЕВНИКА ПО ПОЛЮ Id_Note ПО УБЫВАНИЮ
            //////////////////////////////////////////////////////////////////////////////
            //notes1.sortNotes(FieldsNote.ID_NOTE, Order.DESC);
            //////////////////////////////////////////////////////////////////////////////
            /////

            //Console.WriteLine($"Количество записей в созданном ежедневнике - {notes1.Count}");

            //// ВЫГРУЗКА ЕЖЕДНЕВНИКА В ФАЙЛ
            //////////////////////////////////////////////////////////////////////////////
            //notes1.unloadNotes(@"C:\1\out.diary");
            //////////////////////////////////////////////////////////////////////////////
            /////

            //// ДОБАВЛЕНИЕ ДАННЫХ В ЕЖЕДНЕВНИК ИЗ ФАЙЛА (+ ЗАГРУЗКА ДАННЫХ ИЗ ФАЙЛА)
            //////////////////////////////////////////////////////////////////////////////
            //notes1.loadNotes(@"C:\1\out.diary");    // количество записей удвоится, т.к. файл
            //                                        // загружается тот в который ранее выгружали
            //////////////////////////////////////////////////////////////////////////////
            /////

            //// ЗАГРУЗКА ДАННЫХ ПО ВЫБРАННОМУ ДИАПАЗОНУ ДАТ
            //////////////////////////////////////////////////////////////////////////////
            //notes1.loadNotes(@"C:\1\out.diary", DateTime.Now.AddSeconds(-4), DateTime.Now);
            //////////////////////////////////////////////////////////////////////////////
            /////

            //notes1.unloadNotes(@"C:\1\out_SECOND.diary");


            //Console.WriteLine($"Количество записей в ежедневнике после добавления из файла - {notes1.Count}");


            //Console.WriteLine();

        }
    }
}