using System;

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
        static void Main(string[] args) {
            Note oneNote = new Note("Первая пробная запись", new Person("Иванов", "Иван"), Mood.BAD);

            Note twoNote = new Note("Вторая пробная запись", new Person("Павлов", "Павел", "Павлович", new DateTime(1986, 7, 1)));

            Note threeNote = new Note("Третья пробная запись", new Person("Петров", "Петр", "Петрович", new DateTime(2000, 1, 1)));

            Notes notes1 = new Notes(oneNote, twoNote);  // ежедневник

            notes1.insertNotes(threeNote);   // добавляем запись в ежедневник

            notes1.insertNotes(new Note("Четвертая пробная запись", new Person("Петров", "Петр", "Петрович", new DateTime(2000, 1, 1))),
                                new Note("Пятая пробная запись", new Person("Петров", "Петр", "Петрович", new DateTime(2000, 1, 1))),
                                new Note("Шестая пробная запись", new Person("Петров", "Петр", "Петрович", new DateTime(2000, 1, 1)))
                              );

            notes1.editNotes(3, "Дополнительно измененная запись", Mood.GREAT);      // редактирование записи в ежедневнике

            notes1.deleteNotes(twoNote);   // удаление записи из ежедневника

            notes1.sortNotes(FieldsNote.ID_NOTE, Order.DESC);    // сортировка ежедневника по полю Id_Note в по убыванию

            notes1.unloadNotes(@"C:\1\out.diary");    // Вывод ежедневника в файл

            //Console.WriteLine($"Количество записей в первом ежедневнике - {notes1.Count}");
            //Console.WriteLine();

            Notes notes2 = new Notes();
            notes2.loadNotes(@"C:\1\out.diary");

            notes2.unloadNotes(@"C:\1\out_SECOND.diary");

            //Person prsn = Person.stringToPerson("Петров Петр Петрович, 01.01.2000");

            //Console.WriteLine($"Количество записей во втором ежедневнике - {notes2.Count}");
            Console.WriteLine();
        }
    }
}