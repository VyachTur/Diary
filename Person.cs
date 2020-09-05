using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diary {
	/// <summary>
	/// Структура реализующая персону, а именно хранящая информацию о ФИО и ДР какой-то персоны
	/// </summary>
	struct Person {
		#region Properties

		// Автосвойства

		/// <summary>
		/// Фамилия
		/// </summary>
		public string Family { get; set; }	// Фамилия

		/// <summary>
		/// Имя
		/// </summary>
		public string Name { get; set; }		// Имя

		/// <summary>
		/// Отчество
		/// </summary>
		public string Sirname { get; set; }	// Отчество

		/// <summary>
		/// Дата рождения
		/// </summary>
		public DateTime BirthDate { get; set; }	// Дата рождения

		#endregion // Properties

		#region Constructors and Methods

		// Конструкторы и методы

		/// <summary>
		/// Конструктор объекта "персона" (1)
		/// </summary>
		/// <param name="Family">Фамилия</param>
		/// <param name="Name">Имя</param>
		/// <param name="Sirname">Отчество</param>
		/// <param name="BirthDate">Дата рождения</param>
		public Person(string Family, string Name, string Sirname, DateTime BirthDate) {
			this.Family = Family;
			this.Name = Name;
			this.Sirname = Sirname;
			this.BirthDate = new DateTime(BirthDate.Year, BirthDate.Month, BirthDate.Day); // Нужны только год, месяц и день рождения
		}

		/// <summary>
		/// Конструктор объекта "персона" (2)
		/// </summary>
		/// <param name="Family">Фамилия</param>
		/// <param name="Name">Имя</param>
		/// <param name="Sirname">Отчество</param>
		public Person(string Family, string Name, string Sirname) :
			this(Family, Name, Sirname, new DateTime(1900, 1, 1)) { }

		/// <summary>
		/// Конструктор объекта "персона" (3)
		/// </summary>
		/// <param name="Family">Фамилия</param>
		/// <param name="Name">Имя</param>
		public Person(string Family, string Name) :
			this(Family, Name, String.Empty, new DateTime(1900, 1, 1)) { }

		/// <summary>
		/// Возвращает информацию о персоне
		/// </summary>
		/// <returns>Строка с инфой о ФИО и ДР персоны</returns>
		public string getPerson() {
			return $"{this.Family} {this.Name} {this.Sirname}, {this.BirthDate.ToShortDateString()}";
        }

		#endregion // Constructors and Methods
	}
}
