using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Diary {
    enum FieldsNote {
        ID_NOTE = 1,    // ID
        DATE_NOTE,      // ДАТА И ВРЕМЯ
        NOTATION_NOTE,  // ТЕКСТ ЗАПИСИ
        WRITER_NOTE,    // КТО СДЕЛАЛ ЗАПИСЬ
        MOOD_NOTE       // НАСТРОЕНИЕ
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

        // Индексатор get-only
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

        public void editNotes(uint id, string newNotation, Mood newWhatMood = Mood.GOOD) {
            int i;

            for (i = 0; i < count; ++i) {
                if (this.notes[i].Id_Note == id) {
                    this.notes[i].Notation_Note = newNotation;
                    this.notes[i].Mood_Note = newWhatMood;
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
        /// Меняет местами объекты Note
        /// </summary>
        /// <param name="n1">Первый объект Note</param>
        /// <param name="n2">Второй объект Note</param>
        private static void swapNotes(ref Note oneNote, ref Note twoNote) {
            Note tmpNote = oneNote;
            oneNote = twoNote;
            twoNote = tmpNote;
        }

        /// <summary>
        /// Определяет больше ли первый объект второго
        /// </summary>
        /// <param name="oneNote">Первый объект сравнения</param>
        /// <param name="twoNote">Второй объект сравнения</param>
        /// <param name="fn">Поле по которому происходит сравнение</param>
        /// <returns>true - если oneNote > twoNote</returns>
        private static bool isGreaterOneNote(ref Note oneNote, ref Note twoNote, FieldsNote fn) {
            // Переменные для сравнения строковых значений полей
            string fieldStrVal1 = String.Empty;
            string fieldStrVal2 = String.Empty;

            // Получение значений свойств объектов
            if (fn == FieldsNote.NOTATION_NOTE) {
                //fieldVal1 = oneNote.GetType().GetProperty(fildNote).GetValue(oneNote).ToString();
                //fieldVal2 = twoNote.GetType().GetProperty(fildNote).GetValue(twoNote).ToString();
                fieldStrVal1 = oneNote.Notation_Note;
                fieldStrVal2 = twoNote.Notation_Note;

            } else if (fn == FieldsNote.WRITER_NOTE) {
                fieldStrVal1 = oneNote.Writer_Note.personToString();
                fieldStrVal2 = twoNote.Writer_Note.personToString();

            }
            // Если значение свойства первого объекта больше, то возвращаем true
            if (String.Compare(fieldStrVal1, fieldStrVal2, true) == 1) return true;


            ////////////////////////////СРАВНЕНИЕ СВОЙСТВ Mood_Note, Id_Note, Date_Note///////////////////////////
            if (fn == FieldsNote.MOOD_NOTE) {
                uint fieldMoodVal1 = (uint)oneNote.Mood_Note;
                uint fieldMoodVal2 = (uint)twoNote.Mood_Note;

                return fieldMoodVal1 > fieldMoodVal2;
            }
            
            if (fn == FieldsNote.ID_NOTE) {
                uint fieldIdVal1 = oneNote.Id_Note;
                uint fieldIdVal2 = twoNote.Id_Note;

                return fieldIdVal1 > fieldIdVal2;
            }

            if (fn == FieldsNote.DATE_NOTE) {
                DateTime fieldDTVal1 = oneNote.Date_Note;
                DateTime fieldDTVal2 = twoNote.Date_Note;

                return fieldDTVal1 < fieldDTVal2;
            }
            //////////////////////КОНЕЦ_СРАВНЕНИЕ СВОЙСТВ Mood_Note, Id_Note, Date_Note//////////////////////////////
            

            return false;   // в остальных случаях возвращаем false
        }

        /// <summary>
        /// Сортирует массив notes по значению одного из свойств класса Note
        /// </summary>
        /// <param name="piNote">Свойство класса Note</param>
        public void sortNotes(FieldsNote fNote = FieldsNote.DATE_NOTE) {
            for (int i = 0; i < this.count; ++i) {
                for (int j = 0; j < this.count - 1; ++j) {
                    if (isGreaterOneNote(ref notes[j], ref notes[j + 1], fNote)) {
                        swapNotes(ref notes[j], ref notes[j + 1]);
                    }
                }
            }
        }

        ////////////////////////////////////КОНЕЦ_СОРТИРОВКА///////////////////////////////////////


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
