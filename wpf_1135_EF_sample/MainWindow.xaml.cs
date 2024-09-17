using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace wpf_1135_EF_sample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private List<Singer> singers;

        public List<Singer> Singers
        {
            get => singers;
            set
            {
                singers = value;
                Signal();
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            using (var db = new _1135New2024Context())
            {
                Singers = db.Singers.
                    Include(s => s.Musics).
                    Include(s => s.YellowPresses).
                    Where(Check).ToList();
            }
        }

        //s => s.Firstname.Contains("Алла")
        bool Check(Singer s) =>
            s.Firstname.Contains("Алла");

        public event PropertyChangedEventHandler? PropertyChanged;
        void Signal([CallerMemberName] string prop = null) 
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}

// вместо того, чтобы писать запросы руками
// или разрабатывать систему, которая будет 
// эти запросы генерировать, можно воспользоваться
// существующими ORM-системами, среди которых
// нас интересует EF - Entity Framework core

// для использования EF - необходимо установить
// нугет-пакеты, конкретный пакет зависит от 
// используемый субд
// Pomelo.EntityFrameworkCore.MySql - для mysql/mariadb
// Microsoft.EntityFrameworkCore.SqlServer - для MS SQL server
// Npgsql.EntityFrameworkCore.PostgreSQL - для postgres
// дополнительно ставим еще один пакет:
// Microsoft.EntityFrameworkCore.Tools - этот пакет нужен для создания кода взаимодействия с субд

// есть несколько подходов для работы в EF
// самый простой - подход database first - сначала создается бд в субд, потом с помощью пакета tools и соответсвтующего пакета для работы с EF создается код для работы с субд
// чуть посложнее - подход code first - сначала описываются классы, создаются ссылки между классами, необходимые атрибуты, тогда с помощью EF можно из кода создать бд в субд

// чтобы подключиться к бд по database first нужно открыть
// диспетчер пакетов и выполнить команду scaffold-dbcontext
// синтаксис:
// scaffold-dbcontext "строка подключения" название пакета (напр. Pomelo.EntityFrameworkCore.MySql)
// если мы хотим пересоздать все, что было создано этой командой ранее
// то выполняем ее же, но в конце добавляем -f

// возможные проблемы при выполнении команды scaffold-dbcontext:
// 1. Команда scaffold-dbcontext не найдена - решение - установить Microsoft.EntityFrameworkCore.Tools 
// 2. Может быть какая-нибудь странная ошибка, намекающая названием на разницу в систаксисе в субд и генераторе - решение - надо проверить версии пакетов, надо чтоб пакет к субд был той же версии, что и пакет tools
// 3. Ошибка подключения к бд - решение - проверить правильность логина, пароля, адреса, названия бд и для версий выше 6 - может потребоваться доп параметр в строке подключения: TrustServerCertificate=true
// 4-99. остальные ошибки - дважды или трижды проверить, что проект компилируется без ошибок перед выполнением команды scaffold

// для выполнения запроса к бд нужен экзепляр класса,
// наследуемого от DbContext (_1135New2024Context)
// на нем есть свойства типа DbSet для каждой таблицы
// через это свойство мы можем обратиться к таблице с тем, чтобы 
// получить или удалить какие данные из нее
// при этом мы можем использовать стандартные лямбда-выражения
// для фильтрации получаемых записей
// !!! для получения связанных данных в свойствах
// навигации (помечены virtual) используется метод расширения
// Include (доступен с using Microsoft.EntityFrameworkCore;)
// в Include указывается название свойства навигации - в кавычках либо лямбдой