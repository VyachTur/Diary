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

        private static bool isGreaterOneNote(ref Note oneNote, ref Note twoNote, string fieldNote) {
            // Переменные для сравнения строковых значений полей
            string fieldStrVal1 = String.Empty;
            string fieldStrVal2 = String.Empty;

            // Получение значений свойств объектов
            if (fieldNote == "Notation_Note") {
                //fieldVal1 = oneNote.GetType().GetProperty(fildNote).GetValue(oneNote).ToString();
                //fieldVal2 = twoNote.GetType().GetProperty(fildNote).GetValue(twoNote).ToString();
                fieldStrVal1 = oneNote.Notation_Note;
                fieldStrVal2 = twoNote.Notation_Note;

            } else if (fieldNote == "Writer_Note") {
                fieldStrVal1 = oneNote.Writer_Note.personToString();
                fieldStrVal2 = twoNote.Writer_Note.personToString();

            }
            // Если значение свойства первого объекта больше, то возвращаем true
            if (String.Compare(fieldStrVal1, fieldStrVal2, true) == 1) return true;


            ////////////////////////////СРАВНЕНИЕ СВОЙСТВ Mood_Note, Id_Note, Date_Note///////////////////////////
            if (fieldNote == "Mood_Note") {
                uint fieldMoodVal1 = (uint)oneNote.Mood_Note;
                uint fieldMoodVal2 = (uint)twoNote.Mood_Note;

                return fieldMoodVal1 > fieldMoodVal2;
            }
            
            if (fieldNote == "Id_Note") {
                uint fieldIdVal1 = oneNote.Id_Note;
                uint fieldIdVal2 = twoNote.Id_Note;

                return fieldIdVal1 > fieldIdVal2;
            }

            if (fieldNote == "Date_Note") {
                DateTime fieldDTVal1 = oneNote.Date_Note;
                DateTime fieldDTVal2 = twoNote.Date_Note;

                return fieldDTVal1 > fieldDTVal2;
            }
            //////////////////////КОНЕЦ_СРАВНЕНИЕ СВОЙСТВ Mood_Note, Id_Note, Date_Note//////////////////////////////
            

            return false;   // в остальных случаях возвращаем false
        }

        /// <summary>
        /// Сортирует массив notes по значению одного из свойств класса Note
        /// </summary>
        /// <param name="piNote">Свойство класса Note</param>
        private void sortByPropValue(FieldsNote fNote = FieldsNote.DATE_NOTE) {
            // Если сортируем по свойствам Notation_Note, Writer_Note
            if (fNote == FieldsNote.NOTATION_NOTE || fNote == FieldsNote.WRITER_NOTE) {
                for (int i = 0; i < this.count; ++i) {
                    for (int j = 0; j < this.count - 1; ++j) {
                        if (isGreaterOneNote(ref notes[j], ref notes[j + 1], fNote.ToString())) {
                            swapNotes(ref notes[j], ref notes[j + 1]);
                        }

                        //if (String.Compare( piNote.GetValue(notes[j]),
                        //                    piNote.GetValue(notes[j + 1]),
                        //                    true)) {
                        //    Note tmpNote = notes[j];
                        //    notes[j] = notes[j + 1];
                        //    notes[j + 1] = tmpNote;
                        //}

                        //var propNote = piNote.GetValue(notes[j]);
                    }
                }
            }
            else {    // если сортируем по свойствам Id_Note, Date_Note, mood_Note
                for (int i = 0; i < this.count; ++i) {
                    for (int j = 0; j < this.count - 1; ++j) {
                        var propNote = notes[j].GetType().GetProperty(fNote.ToString()).GetValue(notes[j]);
                    
                    
                    }
                }
            }
        }

        /// <summary>
        /// Сортирует ежедневник
        /// </summary>
        /// <param name="fn">Поле по которому сортировать</param>
        public void sortNotes(FieldsNote fn = FieldsNote.DATE_NOTE) {
            sortByPropValue(fn);
            if (count > 1) {
                switch (fn) {
                    case FieldsNote.ID_NOTE:
                        
                        //for (int i = 0; i < this.count; ++i) {
                        //    for (int j = 0; j < this.count - 1; ++j) {
                        //        //var propNote = notes[j].GetType().GetProperty("Id_Note").GetValue(notes[j]);
                        //        var propNote = 
                        //        if (notes[j].Id_Note > notes[j + 1].Id_Note) {
                        //            Note tmpNote = notes[j];
                        //            notes[j] = notes[j + 1];
                        //            notes[j + 1] = tmpNote;
                        //        }
                        //    }
                        //}

                        break;

                    //case FieldsNote.MoodWr:
                    //    for (int i = 0; i < this.count; ++i) {
                    //        for (int j = 0; j < this.count - 1; ++j) {
                    //            if (notes[j].Mood_Note > notes[j + 1].Mood_Note) {
                    //                Note tmpNote = notes[j];
                    //                notes[j] = notes[j + 1];
                    //                notes[j + 1] = tmpNote;
                    //            }
                    //        }
                    //    }

                    //    break;

                    //case FieldsNote.DateT:
                    //    for (int i = 0; i < this.count; ++i) {
                    //        for (int j = 0; j < this.count - 1; ++j) {
                    //            if (notes[j].Date_Note > notes[j + 1].Date_Note) {
                    //                Note tmpNote = notes[j];
                    //                notes[j] = notes[j + 1];
                    //                notes[j + 1] = tmpNote;
                    //            }
                    //        }
                    //    }

                    //    break;

                    //case FieldsNote.Notat:
                    //    for (int i = 0; i < this.count; ++i) {
                    //        for (int j = 0; j < this.count - 1; ++j) {
                    //            if (String.Compare(notes[j].Notation_Note,
                    //                                notes[j + 1].Notation_Note, true) == 1) {

                    //                Note tmpNote = notes[j];
                    //                notes[j] = notes[j + 1];
                    //                notes[j + 1] = tmpNote;
                    //            }
                    //        }
                    //    }

                    //    break;

                    //case FieldsNote.Wr:
                    //    for (int i = 0; i < this.count; ++i) {
                    //        for (int j = 0; j < this.count - 1; ++j) {
                    //            if (String.Compare(notes[j].Writer_Note.personToString(),
                    //                                notes[j + 1].Writer_Note.personToString(),
                    //                                true) == 1) {

                    //                Note tmpNote = notes[j];
                    //                notes[j] = notes[j + 1];
                    //                notes[j + 1] = tmpNote;
                    //            }
                    //        }
                    //    }

                    //    break;

                }
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
