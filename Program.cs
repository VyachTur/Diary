using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
			Note oneNote = new Note("Первая пробная запись", new Person("Иванов", "Иван"), Mood.Bad);
			oneNote.editNote("Измененная запись");

			Note twoNote = new Note("Вторая пробная запись", new Person("Павлов", "Павел", "Павлович", new DateTime(1986, 7, 1)));

            Note threeNote = new Note("Третья пробная запись", new Person("Петров", "Петр", "Петрович", new DateTime(2000, 1, 1)));

            Notes notes = new Notes(oneNote, twoNote);

            notes.insertNotes(threeNote);   // добавляем запись в ежедневник

            notes.insertNotes(	new Note("Четвертая пробная запись", new Person("Петров", "Петр", "Петрович", new DateTime(2000, 1, 1))),
								new Note("Пятая пробная запись", new Person("Петров", "Петр", "Петрович", new DateTime(2000, 1, 1))),
								new Note("Шестая пробная запись", new Person("Петров", "Петр", "Петрович", new DateTime(2000, 1, 1)))
							  );

            notes.editNotes(2, "Дополнительно измененная запись", Mood.Great);      // редактирование записи в ежедневнике
            //Note fourNote = new Note();
            notes.deleteNotes(twoNote);   // удаление записи из ежедневника

            //notes.sortNotes(FieldsNote.MoodWr);
            notes.sortNotes(FieldsNote.Id);

            //for (int i = 1; i <= notes.Count; ++i) {
            //    Console.WriteLine(notes.getNoteForId((uint)i).prntNote());
            //}

            for (int i = 0; i < notes.Count; ++i) {
                Console.WriteLine(notes[i].prntNote());
            }

            Console.WriteLine(notes.Count);
		}
	}
}