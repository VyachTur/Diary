using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diary {
    enum FieldsNote {
        Id = 1,     // Id
        DateT,      // Дата и время
        Notat,      // Текст записи
        Wr,         // Кто сделал запись
        MoodWr      // Настроение
    }

    /// <summary>
    /// Структура реализующая ежедневник
    /// </summary>
    struct Notes {
        #region Fields

        // Поля
        private Note[] notes;       // массив-ежедневник
        private int count;          // количество элементов в массиве (записей)

        #endregion // Fields

        /// <summary>
        /// Показывает количество элементов в массиве-ежедневнике
        /// </summary>
        public int Count { 
            get { return count; }
        }

        // Индексатор
        public Note this[int index] {
            get { return notes[index]; }
        }

        #region Constructors and Methods

        // Конструкторы и Методы

        /// <summary>
        /// Конструктор объекта "ежедневник"
        /// </summary>
        /// <param name="args">Записи</param>
        public Notes(params Note[] args) {
            notes = args;
            count = args.Length;    // меняем значение количества элементов в массиве-ежедневнике
        }

        public void editNotes(uint id, string newNotation, Mood newWhatMood = Mood.Good) {
            int i;

            for (i = 0; i < count; ++i) {
                if (this.notes[i].Id_Note == id) {
                    this.notes[i].Notation = newNotation;
                    this.notes[i].WhatMood = newWhatMood;
                    return;
                }
            }

        }

        /// <summary>
        /// Возвращает запись по ее id
        /// </summary>
        /// <param name="id">Идентификатор записи в ежедневнике</param>
        /// <returns></returns>
        public Note getNoteForId(uint id) {
            int i;

            for (i = 0; i < this.count; ++i) {
                if (this.notes[i].Id_Note == id) break;
            }

            return i == this.count ? new Note() : this.notes[i];      // если индекс нашелся в цикле, то возвращаем запись с совпавшим индексом
                                                            // иначе возвращаем "пустую" запись
        }

        //public Note getNote(uint counter) {
        //    if (counter >= 0 && counter < this.count) return notes[counter];

        //    return new Note();  // если индекс за пределами массива notes, то возвращаем Note()
        //}

        /// <summary>
        /// Добавляет запись(-и) в ежедневник
        /// </summary>
        /// <param name="args">Добавляемая(-ые) запись(-и)</param>
        public void insertNotes(params Note[] args) {
            Array.Resize(ref this.notes, this.count + args.Length);

            foreach (Note note in args) {
                this.notes[count++] = note;
            }
        }

        /// <summary>
        /// Удаляет запись из ежедневника по ее ID
        /// </summary>
        /// <param name="id">ID записи (свойство Id_Note, начинаются с 1)</param>
        public void deleteNotes(uint id) {
            // id записи не может быть < 1
            if (id > 0 && id < this.count) {
                Note[] notesTmp = new Note[this.count - 1];
                int i = 0;  // счетчик для индекса массива notesTmp

                foreach (Note note in this.notes) {
                    if (note.Id_Note != id) {
                        notesTmp[i++] = note;
                    }
                }

                --this.count;  // уменьшаем количество записей
                this.notes = notesTmp;
            }
        }

        /// <summary>
        /// Перегруженный метод, удаляет записи из ежедневника по названиям объектов записей (если они были ранне отдельно созданы)
        /// </summary>
        /// <param name="args">Массив записей (либо одна запись)</param>
        public void deleteNotes(params Note[] args) {
            int i, j;   // счетчики циклов

            for (i = 0; i < args.Length; ++i) {
                for (j = 0; j < this.count; ++j) {
                   if (args[i].Id_Note == notes[j].Id_Note) {
                        // Если Id двух записей равны, то это одна и та же запись и ее надо удалить.
                        // Используем этот же метод, но в исходной интерпретации.
                        this.deleteNotes(notes[j].Id_Note);
                    } 
                }
            }

        }



        ///////////////////////////////////////СОРТИРОВКА//////////////////////////////////////////

        /// <summary>
        /// Меняет два объекта Note местами
        /// </summary>
        /// <param name="n1">Первое значение</param>
        /// <param name="n2">Второе значение</param>
        private void swapNotes(ref Note n1, ref Note n2) {
            Note tmpNote = n1;
            n1 = n2;
            n2 = tmpNote;
        }

        public void sortNotes(FieldsNote fn = FieldsNote.DateT) {

            switch (fn) {
                case FieldsNote.Id:
                    for (int i = 0; i < this.count; ++i) {
                        for (int j = 0; j < this.count - 1; ++j) {
                            if (notes[j].Id_Note > notes[j + 1].Id_Note) {
                                Note tmpNote = notes[j];
                                notes[j] = notes[j + 1];
                                notes[j + 1] = tmpNote;
                            }
                        }
                    }

                    break;

                case FieldsNote.MoodWr:
                    for (int i = 0; i < this.count; ++i) {
                        for (int j = 0; j < this.count - 1; ++j) {
                            if (notes[j].WhatMood > notes[j + 1].WhatMood) {
                                Note tmpNote = notes[j];
                                notes[j] = notes[j + 1];
                                notes[j + 1] = tmpNote;
                            }
                        }
                    }

                    break;

                case FieldsNote.DateT:
                    for (int i = 0; i < this.count; ++i) {
                        for (int j = 0; j < this.count - 1; ++j) {
                            if (notes[j].Datetime_Note > notes[j + 1].Datetime_Note) {
                                Note tmpNote = notes[j];
                                notes[j] = notes[j + 1];
                                notes[j + 1] = tmpNote;
                            }
                        }
                    }

                    break;

                case FieldsNote.Notat:
                    for (int i = 0; i < this.count; ++i) {
                        for (int j = 0; j < this.count - 1; ++j) {
                            if (String.Compare( notes[j].Notation, 
                                                notes[j + 1].Notation, true) == 1) {

                                Note tmpNote = notes[j];
                                notes[j] = notes[j + 1];
                                notes[j + 1] = tmpNote;
                            }
                        }
                    }

                    break;

                case FieldsNote.Wr:
                    for (int i = 0; i < this.count; ++i) {
                        for (int j = 0; j < this.count - 1; ++j) {
                            if (String.Compare( notes[j].Writer.stringForSort(), 
                                                notes[j + 1].Writer.stringForSort(),
                                                true) == 1) {

                                Note tmpNote = notes[j];
                                notes[j] = notes[j + 1];
                                notes[j + 1] = tmpNote;
                            }
                        }
                    }

                    break;

            }
        }

        ////////////////////////////////////СОРТИРОВКА_КОНЕЦ///////////////////////////////////////


        // МЕТОДЫ ДЛЯ РАБОТЫ С ФАЙЛАМИ

        // - Загрузка данных из файла
        // - Выгрузка данных в файл
        // - Добавления данных в текущий ежедневник из выбранного файла
        // - Импорт записей по выбранному диапазону дат


        /// <summary>
        /// Загружает данные в ежедневник из файла
        /// (если в ежедневнике есть файлы, то происходит добавление)
        /// </summary>
        /// <param name="path">Путь к файлу-ежедневнику</param>
        public void loadNotes(string path) {

        }

        /// <summary>
        /// Перегруженный метод, загружает данные в ежедневник из файла 
        /// (если в ежедневнике есть файлы, то происходит добавление, добавление происходит в диапазоне дат)
        /// </summary>
        /// <param name="path">Путь к файлу-ежедневнику</param>
        public void loadNotes(string path, DateTime from, DateTime to) {

        }

        /// <summary>
        /// Выгружает данные из ежедневника в файл
        /// </summary>
        /// <param name="path">Путь к файлу-ежедневнику</param>
        public void unloadNotes(string path) {

        }

        #endregion  // Constructors and Methods

    }
}
