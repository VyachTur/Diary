﻿using System;
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
			//Console.WriteLine(oneNote.getNote());

			Note twoNote = new Note("Вторая пробная запись", new Person("Павлов", "Павел", "Павлович", new DateTime(1986, 7, 1)));
			//Console.WriteLine(twoNote.getNote());

			Note threeNote = new Note("Третья пробная запись", new Person("Петров", "Петр", "Петрович", new DateTime(2000, 1, 1)));

            Notes notes = new Notes(oneNote, twoNote);

            notes.insertNotes(threeNote);   // добавляем запись в ежедневник

			notes.insertNotes(	new Note("Четвертая пробная запись", new Person("Петров", "Петр", "Петрович", new DateTime(2000, 1, 1))),
								new Note("Пятая пробная запись", new Person("Петров", "Петр", "Петрович", new DateTime(2000, 1, 1))),
								new Note("Шестая пробная запись", new Person("Петров", "Петр", "Петрович", new DateTime(2000, 1, 1)))
							  );

            //notes.deleteNotes(twoNote, threeNote);
            //notes.getNote(1).editNote("Повторно измененная запись", Mood.Great);
            notes.editNotes(1, "Дополнительно измененная запись", Mood.Great);
            


            Console.WriteLine(notes.getNote(1).prntNote());

            Console.WriteLine(notes.Count);
		}
	}
}