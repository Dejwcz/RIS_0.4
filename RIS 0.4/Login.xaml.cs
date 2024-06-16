using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;


namespace RIS_0._4
{
    /// <summary>
    /// Interakční logika pro Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Vojaci seznam = new Vojaci();
        CWNoveHeslo WNoveHeslo = null;
        public Login()
        {
            InitializeComponent();
            seznam.NactiZeSouboru();
            ResizeMode = ResizeMode.NoResize;
            ImageBrush myBrush = new ImageBrush();
            myBrush.ImageSource = new BitmapImage(new Uri($"{Directory.GetCurrentDirectory()}\\kruh.webp", UriKind.Absolute));
            myBrush.Stretch = Stretch.Fill; // Nastavuje způsob, jakým se obrázek roztahuje na pozadí
            Background = myBrush;
        }
        private void TLogin_Click(object sender, RoutedEventArgs e) 
        {
            int pozice;
            seznam.SpoctiPocetRadku();
            if (seznam.NajdiPoziciVSeznamuUsername(TJmeno.Text) == seznam.PocetRadku)
            {
                MessageBox.Show("Uživatelské jméno nenalezeno.");
            }
            else
            {
                pozice = seznam.NajdiPoziciVSeznamuUsername(TJmeno.Text);
                Vojak vojak = seznam.vojaciSeznam[pozice];
                if (vojak.Password != "" && seznam.OverUzivatele(TJmeno.Text, PBHeslo.Password) == true)
                {
                    MainWindow main = new MainWindow();
                    main.UserName = TJmeno.Text;
                    App.Current.MainWindow = main;
                    this.Close();
                    main.Show();
                }
                else
                {    
                    if (TJmeno.Text == vojak.UserName && vojak.Password == "" )
                    {
                        if (WNoveHeslo == null || !WNoveHeslo.IsLoaded)
                        {
                            MessageBox.Show("Inicializace hesla nového uživatele");
                            WNoveHeslo = new CWNoveHeslo(this.seznam, TJmeno.Text);
                            WNoveHeslo.Show();
                        }
                    }
                    else { MessageBox.Show("Přihlášení se nezdařilo"); }
                }
            }
        }

    }  
}

