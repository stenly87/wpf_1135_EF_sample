using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace wpf_1135_EF_sample
{
    /// <summary>
    /// Логика взаимодействия для WinSingerEditor.xaml
    /// </summary>
    public partial class WinSingerEditor : Window, INotifyPropertyChanged
    {
        public Music SelectedMusic { get; set; }
        public Singer Singer { get; set; } 
        public WinSingerEditor()
        {
            InitializeComponent();
            Singer = new Singer { Musics = new List<Music>() };
            DataContext = this;
        }

        public WinSingerEditor(Singer selectedSinger)
        {
            InitializeComponent();
            Singer = selectedSinger;
            DataContext = this;
        }

        void Signal([CallerMemberName] string prop = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public event PropertyChangedEventHandler? PropertyChanged;

        private void SaveClose(object sender, RoutedEventArgs e)
        {
            using (var db = new _1135New2024Context())
            {
                List<Music> originalMusic = new();
                if (Singer.Id == 0)
                {   // мы добавляем объект Singer (и всю иерархию вложенных в него объектов) 
                    // в индекс изменений, отслеживаемый EF
                    db.Singers.Add(Singer);
                }
                else
                {
                    // находим оригинальную запись в бд по id
                    // инклуд нужен для того, чтобы подгрузить сразу связанную
                    // музыку, и не генерировать лишние выборки при обновлении названий треков
                    var original = db.Singers.Include(s=>s.Musics).
                        FirstOrDefault(s => s.Id == Singer.Id);
                    originalMusic.AddRange(original.Musics);
                    // обновляем в ней значения с помощью объекта Singer
                    db.Entry(original).CurrentValues.SetValues(Singer);

                    // если мы попытаемся добавить уже отслеживаемый
                    // объект в бд, то он выдаст ошибку при сохранении 
                    // изменений, т.к. дублируется первичный ключ
                    //db.Singers.Add(Singer);
                }
                // для одновременного сохранения изменений в музыке
                // перебираем музыку и решаем, что с ней делать
                foreach (var music in Singer.Musics)
                    if (music.Id == 0)
                    {
                        // обязательно нужно указать внешний ключ
                        // для новых объектов
                        music.IdSinger = Singer.Id;
                        // если у нового объекта будет указана 
                        // навигация на уже отслеживаемый объект
                        // то будет ошибка в методе Add
                        //music.IdSingerNavigation = Singer;
                        db.Musics.Add(music);
                    }
                    else
                    {
                        var original = db.Musics.Find(music.Id);
                        db.Entry(original).CurrentValues.SetValues(music);
                    }
                // если у нас в текущем списке музыки нет каких-то песен,
                // которые там были, то удаляем их
                var currentMusic = Singer.Musics.Select(s => s.Id);
                var toRemove = originalMusic.Where(s => !currentMusic.Contains(s.Id));
                foreach(var r in toRemove)
                    db.Remove(r);

                // сохранение всех изменений
                db.SaveChanges();
            }
            Close();
        }

        private void DeleteMusic(object sender, RoutedEventArgs e)
        {
            if (SelectedMusic == null)
                return; 

            Singer.Musics.Remove(SelectedMusic);
            Singer.Musics = new List<Music>(Singer.Musics); // костыль
            Signal(nameof(Singer));
        }
    }
}
