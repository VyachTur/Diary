using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diary {
	/// <summary>
	/// Перечисление реализующее настроение
	/// </summary>
	enum Mood {
		GREAT = 1,		// ОТЛИЧНОЕ
		GOOD,			// ХОРОШЕЕ
		BAD				// ПЛОХОЕ
	}

	/// <summary>
	/// Структура реализующая одну запись в ежедневнике
	/// </summary>
	struct Note {

		#region Fields

		// Поля
		private uint id_Note;					// id-записи
		private DateTime date_Note;             // Дата и время создания записи
		private string notation_Note;           // текст записи
		private Person writer_Note;				// создатель записи
		private Mood mood_Note;                 // настроение
		

		private static uint countNotes = 0;     // количество созданных записей (объектов структуры Note)

		#endregion  // Fields


		#region Properties

		// Свойства и автосвойства

		/// <summary>
		/// Id записи
		/// </summary>
		public uint Id_Note {
			get { return this.id_Note; }
        }
		/// <summary>
		/// Дата и время создания записи
		/// </summary>
		public DateTime Date_Note {
			get { return this.date_Note; }
		}

		/// <summary>
		/// Текст записи
		/// </summary>
		public string Notation_Note { 
			get { return this.notation_Note; } 
			set { this.notation_Note = value; }	// если запись помечена как законченная (финализирована), то правит ее нельзя
		}
		/// <summary>
		/// Пользователь создавший запись
		/// </summary>
		public Person Writer_Note {
			get { return this.writer_Note; }
		}

		/// <summary>
		/// Настроение
		/// </summary>
		public Mood Mood_Note { 
			get { return this.mood_Note; } 
			set { this.mood_Note = value; }	// если запись финализирована, то править ее нельзя
		}


		#endregion  // Properties


		#region Constructors and Methods

		// Конструкторы и методы

		/// <summary>
		/// Конструктор объекта "запись" (1)
		/// </summary>
		/// <param name="notation">Текст записи</param>
		/// <param name="Writer">Пользователь создающий запись</param>
		/// <param name="whatMood">Настроение</param>
		public Note(string notation, Person writer, Mood mood) {
			this.id_Note = ++countNotes;
			this.date_Note = DateTime.Now;
			this.writer_Note = writer;
            this.mood_Note = mood;
            this.notation_Note = notation;
        }
		/// <summary>
		/// Конструктор объекта "запись" (2)
		/// </summary>
		/// <param name="Notation">Текст записи</param>
		/// <param name="Writer">Пользователь создающий запись</param>
		public Note(string notation, Person writer) : this(notation, writer, Mood.GREAT) { }

        /// <summary>
        /// Редактирование записи (в записи после создания можно изменять только текст записи (Notation) и настроение (WhatMood)
        /// </summary>
        /// <param name="newNotation">Новая измененная запись</param>
        /// <param name="newWhatMood">Новое настроение (по умолчанию хорошее(GREAT))</param>
        public void editNote(string newNotation, Mood newMood = Mood.GOOD) {
            // Если запись финализирована, то поменять ее не получится
            this.notation_Note = newNotation;
            this.mood_Note = newMood;
        }

        /// <summary>
        /// Возвращает информацию о записи в виде строки
        /// </summary>
        /// <returns>Строка с данными о записи (ID, Дата и время, Текст записи, Кто сделал запись, Настроение пишущего, Финализирована ли запись?</returns>
        public string prntNote() {
			// Возвращаем данные в записи, если они есть
			if (!String.IsNullOrEmpty(notation_Note)) {
				return  $"ID:            {this.id_Note}\n" +
						$"DateTime Note: {this.date_Note}\n" +
						$"Notation:      {this.notation_Note}\n" +
						$"Writer:        {this.writer_Note.prntPerson()}\n" +
						$"Mood:          {this.mood_Note}\n";
			}

			return String.Empty;	// если данных в записи нет, то возвращаем пустую строку
        }


		#endregion // Constructors and Methods

	}
}
