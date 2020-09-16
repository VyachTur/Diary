using System;
using System.Diagnostics;
using System.Linq.Expressions;
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

        // Идентификаторы записей (Id_Note) должны быть различным (в рамках одной программы).
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
                    Console.WriteLine("6 - Упорядочить записи в ежедневнике по выбранному полю;");
                    Console.WriteLine("========================================================");
                    Console.WriteLine("7 - Выйти из программы.");
                    Console.WriteLine("========================================================");
                    Console.WriteLine();

                    Console.Write("Выберите пункт: ");
                    int.TryParse(Console.ReadLine(), out choice);
                } while (choice < 1 || choice > 7);

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
                            else whatMood = Mood.GOOD;
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

                        Console.Write("Введите путь к файлу для загрузки данных в ежедневник: ");
                        string path = Console.ReadLine();

                        Console.Write("Загрузить данные по диапазону дат? (д/н): ");
                        string ch = Console.ReadLine();
                        Console.WriteLine();

                        if (ch == "д") {
                            DateTime from, to;  // для обозначения промежутка дат
                            Console.Write("Загружаем с (дата в формате дд.мм.гггг чч:ММ:сс):  ");
                            DateTime.TryParse(Console.ReadLine(), out from);
                            Console.Write("Загружаем по (дата в формате дд.мм.гггг чч:ММ:сс): ");
                            DateTime.TryParse(Console.ReadLine(), out to);

                            notes.loadNotes(path, from, to);

                        } else {
                            notes.loadNotes(path);
                        }

                        continue;

                    case 5: // выбран пункт "Выгрузить данные в файл"

                        // если запсей в ежедневнике нет, то переходим к следующей итерации цикла
                        if (!notes.prntNotes()) {
                            continue;
                        }

                        Console.WriteLine();
                        Console.Write("Введите путь к файлу ежедневника (по умолчанию создастся файл Diary.diary в директории программы): ");
                        path = Console.ReadLine();
                        notes.unloadNotes(path);

                        continue;

                    case 6: // выбран пункт "Упорядочить записи"

                        int chFie;  // выбор поля сортировки

                        do {
                            Console.Clear();
                            Console.WriteLine("=============================");
                            Console.WriteLine("Поля записи:");
                            Console.WriteLine("=============================");
                            Console.WriteLine("1 - Идентификатор записи;");
                            Console.WriteLine("2 - Дата создания записи;");
                            Console.WriteLine("3 - Текст записи;");
                            Console.WriteLine("4 - Создатель записи;");
                            Console.WriteLine("5 - Настроение;");
                            Console.WriteLine("=============================");
                            Console.WriteLine();

                            Console.Write("Выберите поле по которому будем сортировать: ");
                            int.TryParse(Console.ReadLine(), out chFie);
                        } while (chFie < 1 || chFie > 5);

                        Console.Write("Отсортировать в обратном порядке? (д/н): ");
                        string chOrd = Console.ReadLine();

                        FieldsNote field = FieldsNote.DATE_NOTE;   // поле по которому будем сортировать (по умолчанию - дата создания записи)
                        Order order;

                        if (chOrd == "д") order = Order.DESC;
                        else order = Order.ASC;

                        switch (chFie) {
                            case 1: // сортировка по Id
                                field = FieldsNote.ID_NOTE;
                                break;

                            case 2: // сортировка по дате создания
                                field = FieldsNote.DATE_NOTE;
                                break;
                            case 3: // сортировка по тексту записи
                                field = FieldsNote.NOTATION_NOTE;
                                break;
                            case 4: // сортировка по создателю записи
                                field = FieldsNote.WRITER_NOTE;
                                break;
                            case 5: // сортировка по настроению
                                field = FieldsNote.MOOD_NOTE;
                                break;

                            case 6: // вернуться в главное меню
                                continue;
                        }

                        notes.sortNotes(field, order);

                        continue;

                    case 7: // выбран пункт "Выйти"
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


        /// <summary>
        /// ГЛАВНЫЙ МЕТОД (точка входа программы)
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args) {
            //Console.ReadLine();
            titleAnimation("<<<ЕЖЕДНЕВНИК>>>", ConsoleColor.Cyan);

            Menu();

        }
    }
}