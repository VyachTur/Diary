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
		Great = 1,		// отличное
		Good,			// хорошее
		Bad				// плохое
	}

	/// <summary>
	/// Структура реализующая одну запись в ежедневнике
	/// </summary>
	struct Note {

		#region Fields

		// Поля
		private uint id_Note;					// id-записи
		private static uint countNotes = 0;     // количество созданных записей (объектов структуры Note)

		private Mood whatMood;                  // настроение во время создания или редактирования записи
		private string notation;				// текст записи

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
		public DateTime Datetime_Note { get; private set; } // дата и время создания записи

		/// <summary>
		/// Текст записи
		/// </summary>
		public string Notation { 
			get { return this.notation; } 
			set { this.notation = value; }	// если запись помечена как законченная (финализирована), то правит ее нельзя
		}
		/// <summary>
		/// Пользователь создавший запись
		/// </summary>
		public Person Writer { get; private set; }

		/// <summary>
		/// Настроение
		/// </summary>
		public Mood WhatMood { 
			get { return this.whatMood; } 
			set { this.whatMood = value; }	// если запись финализирована, то править ее нельзя
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
		public Note(string notation, Person Writer, Mood whatMood) {
			this.id_Note = ++countNotes;
			this.Datetime_Note = DateTime.Now;
			this.Writer = Writer;
            this.whatMood = whatMood;
            this.notation = notation;
        }
		/// <summary>
		/// Конструктор объекта "запись" (2)
		/// </summary>
		/// <param name="Notation">Текст записи</param>
		/// <param name="Writer">Пользователь создающий запись</param>
		public Note(string Notation, Person Writer) : this(Notation, Writer, Mood.Great) { }

        /// <summary>
        /// Редактирование записи (в записи после создания можно изменять только текст записи (Notation) и настроение (WhatMood)
        /// </summary>
        /// <param name="newNotation">Новая измененная запись</param>
        /// <param name="newWhatMood">Новое настроение (по умолчанию хорошее(Great))</param>
        public void editNote(string newNotation, Mood newWhatMood = Mood.Good) {
            // Если запись финализирована, то поменять ее не получится
            this.Notation = newNotation;
            this.WhatMood = newWhatMood;
        }

        /// <summary>
        /// Возвращает информацию о записи в виде строки
        /// </summary>
        /// <returns>Строка с данными о записи (ID, Дата и время, Текст записи, Кто сделал запись, Настроение пишущего, Финализирована ли запись?</returns>
        public string prntNote() {
			// Возвращаем данные в записи, если они есть
			if (!String.IsNullOrEmpty(notation)) {
				return $"ID: {this.id_Note}\n" +
						$"DateTime Note: {this.Datetime_Note}\n" +
						$"Notation: {this.notation}\n" +
						$"Writer: {this.Writer.prntPerson()}\n" +
						$"Mood: {this.whatMood}\n";
			}

			return String.Empty;	// если данных в записи нет, то возвращаем пустую строку
        }


		#endregion // Constructors and Methods

	}
}
