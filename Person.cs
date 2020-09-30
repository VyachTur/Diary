using System;

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
		/// Вспомогательный метод для сортировки в массиве из объектов Person
		/// </summary>
		/// <returns>Строка для сортировки</returns>
		public string personToString() {
			return this.Family + this.Name + this.Sirname + this.BirthDate.ToShortDateString();
        }

		/// <summary>
		/// Преобразование СТРОКИ формата возвращаемого методом prntPerson в объект Person
		/// </summary>
		/// <param name="person">Строка в формате метода prntPerson</param>
		/// <returns></returns>
		public static Person stringToPerson(string person) {
			Person personTmp = new Person();
			personTmp.Family = person.Substring(0, person.IndexOf(' ', 0));
			personTmp.Name = person.Substring(personTmp.Family.Length + 1,
												person.IndexOf(' ', personTmp.Family.Length + 1) -
												personTmp.Family.Length - 1);
			if (person.Substring(personTmp.Family.Length + personTmp.Name.Length + 2, 1) == ",") {
				personTmp.Sirname = String.Empty;
				personTmp.BirthDate = DateTime.Parse(person.Substring(	personTmp.Family.Length +
																		personTmp.Name.Length + 4));
            } else {
				personTmp.Sirname = person.Substring(	personTmp.Family.Length + personTmp.Name.Length + 2,
														person.Length - person.IndexOf(',', 0) - 4);
				personTmp.BirthDate = DateTime.Parse(person.Substring(	personTmp.Family.Length +
																		personTmp.Name.Length +
																		personTmp.Sirname.Length + 4));
			}

			return personTmp;
        }

		/// <summary>
		/// Возвращает информацию о персоне в виде строки
		/// </summary>
		/// <returns>Строка с инфой о ФИО и ДР персоны</returns>
		public string prntPerson() {
			return $"{this.Family} {this.Name} {this.Sirname}, {this.BirthDate.ToShortDateString()}";
        }

		#endregion // Constructors and Methods
	}
}
