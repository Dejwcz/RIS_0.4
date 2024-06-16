using System.Data;
using System.Text;
using System.Windows;
using System;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;


namespace RIS_0._4
{
    public class CWNoveHeslo : Window
    {
        public Vojaci seznam;
        public Vojak vojak;
        Label LUzivatel, LStareHeslo, LNoveHeslo, LNoveHeslo2;
        //TextBox TStareHeslo, TNoveHeslo, TNoveHeslo2;
        PasswordBox PBStareHeslo, PBNoveHeslo, PBNoveHeslo2;
        Button BUloz, BZpet;
        public CWNoveHeslo(Vojaci aseznam, string ausername)
        {
            this.seznam = aseznam;
            int poziceVSeznamu = seznam.NajdiPoziciVSeznamuUsername(ausername);
            vojak = seznam.vojaciSeznam[poziceVSeznamu];
            Width = 530; Height = 340; Title = "Změna hesla"; WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ResizeMode = ResizeMode.NoResize;
            ImageBrush myBrush = new ImageBrush();
            myBrush.ImageSource = new BitmapImage(new Uri($"{Directory.GetCurrentDirectory()}\\kruh.webp", UriKind.Absolute));
            myBrush.Stretch = Stretch.Fill; // Nastavuje způsob, jakým se obrázek roztahuje na pozadí
            Background = myBrush;

            StackPanel SPHlavniPanel = new StackPanel();
            StackPanel SPomocnyPanel1 = new StackPanel { Orientation = Orientation.Horizontal};
            StackPanel SPomocnyPanel2 = new StackPanel { Orientation = Orientation.Horizontal };
            StackPanel SPomocnyPanel3 = new StackPanel { Orientation = Orientation.Horizontal };
            StackPanel SPomocnyPanel4 = new StackPanel { Orientation = Orientation.Horizontal };
            LUzivatel = new Label { Content = seznam.NapisUzivatele(ausername), Margin = new Thickness(20), Width = 500,
                                    HorizontalContentAlignment = HorizontalAlignment.Center, FontSize = 26, FontWeight = FontWeights.Bold };
            SPHlavniPanel.Children.Add(LUzivatel);
            LStareHeslo = new Label { Content = "Zadejte staré heslo:", Margin = new Thickness(10), Width = 180, Height = 30, FontSize = 14 };
            //TStareHeslo = new TextBox { Margin = new Thickness(10), Width = 280, Height = 30 ,FontSize = 16 };
            PBStareHeslo = new PasswordBox { Margin = new Thickness(10), Width = 280, Height = 30, FontSize = 16, IsEnabled = false };
            SPomocnyPanel1.Children.Add(LStareHeslo); SPomocnyPanel1.Children.Add(PBStareHeslo);
            SPHlavniPanel.Children.Add(SPomocnyPanel1);
            LNoveHeslo = new Label { Content = "Zadejte nové heslo:", Margin = new Thickness(10), Width = 180, Height = 30, FontSize = 14 };
            //TNoveHeslo = new TextBox { Margin = new Thickness(10), Width = 280, Height = 30 , FontSize = 16 };
            PBNoveHeslo = new PasswordBox { Margin = new Thickness(10), Width = 280, Height = 30, FontSize = 16 };
            SPomocnyPanel2.Children.Add(LNoveHeslo); SPomocnyPanel2.Children.Add(PBNoveHeslo);
            SPHlavniPanel.Children.Add(SPomocnyPanel2);
            LNoveHeslo2 = new Label { Content = "Zadejte znovu nové heslo:", Margin = new Thickness(10), Width = 180, Height = 30, FontSize = 14 };
            //TNoveHeslo2 = new TextBox { Margin = new Thickness(10), Width = 280, Height = 30 , FontSize = 16 };
            PBNoveHeslo2 = new PasswordBox { Margin = new Thickness(10), Width = 280, Height = 30, FontSize = 16 };
            SPomocnyPanel3.Children.Add(LNoveHeslo2); SPomocnyPanel3.Children.Add(PBNoveHeslo2);
            SPHlavniPanel.Children.Add(SPomocnyPanel3);
            BUloz = new Button { Content = "Ulož", Margin = new Thickness(10), Width = 230, Height = 40 , FontSize = 16 };
            BUloz.Click += ZmenaHesla;
            BZpet = new Button { Content = "Zpět", Margin = new Thickness(10), Width = 230, Height = 40 , FontSize = 16 };
            BZpet.Click += (sender, e) => this.Close();
            SPomocnyPanel4.Children.Add(BUloz); SPomocnyPanel4.Children.Add(BZpet);
            SPHlavniPanel.Children.Add(SPomocnyPanel4);


            this.Content = SPHlavniPanel;
        }
        public void ZmenaHesla(object sender, EventArgs e)
        {
            string[] hashSalt = Vojaci.ZahashujHeslo(PBNoveHeslo2.Password);
            this.vojak.Salt = hashSalt[0];
            this.vojak.Password = hashSalt[1];
            MessageBox.Show("Provedeno");
            seznam.ZapisDoSouboru();
            this.Close();
        }
    }
 
    public class CWHledej : Window                                                              //okno pro vyhledání záznamu
    {
        public Vojaci seznam;
        public Button BOsobniCislo, BJmeno, BPrijmeni, BZpet;
        public TextBox THledej;
        public Label LPopis;
        public int VyhledaneCisloZaznamu;
        CWEdituj WEdituj = null;
        
        public CWHledej(Vojaci aseznam)
        {
            this.seznam = aseznam;

            Width = 260; Height = 340; Title = "Hledej"; WindowStartupLocation = WindowStartupLocation.CenterScreen; 
            ResizeMode = ResizeMode.NoResize;
            ImageBrush myBrush = new ImageBrush();
            myBrush.ImageSource = new BitmapImage(new Uri($"{Directory.GetCurrentDirectory()}\\antenky.webp", UriKind.Absolute));
            myBrush.Stretch = Stretch.Fill; // Nastavuje způsob, jakým se obrázek roztahuje na pozadí
            Background = myBrush;

            StackPanel SHlavniPanel = new StackPanel();
            StackPanel SPomocnyPanel = new StackPanel { Orientation = Orientation.Horizontal };
            this.THledej = new TextBox { Margin = new Thickness(10), Width = 200, Height = 40 };
            this.BOsobniCislo = new Button { Content = "Hledej podle osobního čísla", Margin = new Thickness(10), Width = 200, Height = 40 };
            this.BOsobniCislo.Click +=  HledejPodleOC; 
            this.BJmeno = new Button { Content = "Hledej podle jména", Margin = new Thickness(10), Width = 200, Height = 40 };
            this.BJmeno.Click += HledejPodleJmena;
            this.BPrijmeni  = new Button { Content = "Hledej podle příjmení", Margin = new Thickness(10), Width = 200, Height = 40 };
            this.BPrijmeni.Click += HledejPodlePrijmeni;
            this.BZpet = new Button { Content = "Zpět", Margin = new Thickness(10), Width = 100, Height = 40 };
            this.BZpet.Click += (sender, e ) => this.Close();
            SHlavniPanel.Children.Add(THledej);
            SHlavniPanel.Children.Add(BOsobniCislo);
            SHlavniPanel.Children.Add(BJmeno);
            SHlavniPanel.Children.Add(BPrijmeni);
            SHlavniPanel.Children.Add(BZpet);
            this.Content = SHlavniPanel;
        }

        private void HledejPodleOC(object sender, RoutedEventArgs e)
        {
            bool chybnyVstup = false;
            if (this.THledej.Text == "") { MessageBox.Show("Zadej hodnotu"); chybnyVstup = true; }
            else
            {
                try { Convert.ToInt32(THledej.Text); }
                catch (Exception ex) { MessageBox.Show("Osobní číslo musi být číslo!"); chybnyVstup = true; }
            }
            if (seznam.NajdiOC(THledej.Text) && chybnyVstup == false) 
            {                                                  
                if (WEdituj == null || !WEdituj.IsLoaded)
                {
                    WEdituj = new CWEdituj(this.seznam, THledej.Text);
                    WEdituj.Closed += (s, args) => WEdituj = null;
                    this.Close();
                    WEdituj.Show();
                }
                else { WEdituj.Focus(); }                                 
            }
            else if (!chybnyVstup){ MessageBox.Show("Nenalezen žádný záznam."); }
        }
        private void HledejPodleJmena(object sender, RoutedEventArgs e)
        {
            List<string> nalezenaOC;
            if (THledej.Text == "") { MessageBox.Show("Zadej hodnotu"); }
            else
            {
                nalezenaOC = seznam.NajdiOCPodleJmena(THledej.Text);
                if (nalezenaOC.Count == 0) { MessageBox.Show("Nenalezen žádný záznam."); }
                else
                {
                    foreach (string osobniCislo in nalezenaOC)
                    {
                        WEdituj = new CWEdituj(this.seznam, osobniCislo);
                        WEdituj.Show();
                    }
                    this.Close();
                }
            }
        }
        private void HledejPodlePrijmeni(object sender, RoutedEventArgs e)
        {
            List<string> nalezenaOC;
            if (THledej.Text == "") { MessageBox.Show("Zadej hodnotu"); }
            else
            {
                nalezenaOC = seznam.NajdiOCPodlePrijmeni(THledej.Text);
                if (nalezenaOC.Count == 0) { MessageBox.Show("Nenalezen žádný záznam."); }
                else
                {
                    foreach (string osobniCislo in nalezenaOC)
                    {
                        WEdituj = new CWEdituj(this.seznam, osobniCislo);
                        WEdituj.Show();
                    }
                    this.Close();
                }
            }
        }
    }
    public class CWEdituj : Window                                                              //okno pro editaci záznamu
    {
        public Vojaci seznam;
        public TextBox[] textboxy;
        public Label[] labely;
        public string vybraneOsCislo;
        public Button BUloz, BZrusit;
        public DatePicker[] datapickery;


        public CWEdituj(Vojaci aseznam, string avybraneOsCislo)                                 
        {
            this.seznam = aseznam;

            Width = 360; Height = 610; Title = "Zobrazaní záznamu"; WindowStartupLocation = WindowStartupLocation.CenterScreen; 
            ResizeMode = ResizeMode.NoResize;
            ImageBrush myBrush = new ImageBrush();
            myBrush.ImageSource = new BitmapImage(new Uri($"{Directory.GetCurrentDirectory()}\\antenky.webp", UriKind.Absolute));
            myBrush.Stretch = Stretch.Fill; // Nastavuje způsob, jakým se obrázek roztahuje na pozadí
            Background = myBrush;

            StackPanel SHlavniPanel = new StackPanel();
            StackPanel[] pomocnePanely = new StackPanel[seznam.PocetPolozek];
            textboxy = new TextBox[seznam.PocetPolozek];
            labely = new Label[seznam.PocetPolozek];
            datapickery = new DatePicker[seznam.PocetPolozek];
            List<string> vlastnosti = seznam.SeznamVlastnosti();
            string[] vojakVPoli = seznam.VojakDoPole(avybraneOsCislo);           
            int i = 0; 
            foreach (string vlastnost in vlastnosti)
            {   
                if (vlastnost == "Salt") { continue; }                                                                  //přeskočení soli
                this.labely[i] = new Label { Content = seznam.NazvyDoTabulky[vlastnost], Margin = new Thickness(10), Width = 100 };
                if (vlastnost == "DatumNarozeni" || vlastnost == "SluzbaOd" || vlastnost == "SluzbaDo")                 //nahrazení vybraných textboxů datepickery
                {
                    this.datapickery[i] = new DatePicker { Margin = new Thickness(10), Width = 200, Text = vojakVPoli[i] };
                    pomocnePanely[i] = new StackPanel { Orientation = Orientation.Horizontal };
                    pomocnePanely[i].Children.Add(this.labely[i]); pomocnePanely[i].Children.Add(this.datapickery[i]);
                    SHlavniPanel.Children.Add(pomocnePanely[i]);
                }
                else
                {
                    if (vlastnost == "Password" || vlastnost == "UserName" || vlastnost == "OsobniCislo")           //read only pro vybrané textboxy
                    {
                        this.textboxy[i] = new TextBox { Margin = new Thickness(10), Width = 200, Text = vojakVPoli[i], IsReadOnly = true };
                    }
                    else
                    {
                        this.textboxy[i] = new TextBox { Margin = new Thickness(10), Width = 200, Text = vojakVPoli[i] };
                    }
                    pomocnePanely[i] = new StackPanel { Orientation = Orientation.Horizontal };
                    pomocnePanely[i].Children.Add(this.labely[i]); pomocnePanely[i].Children.Add(this.textboxy[i]);
                    SHlavniPanel.Children.Add(pomocnePanely[i]);
                }
                i++;
            }
            StackPanel SPTlacitka = new StackPanel { Orientation = Orientation.Horizontal };
            this.BUloz = new Button { Content = "Ulož záznam", Margin = new Thickness(10), Width = 150, Height = 40 };
            this.BUloz.Click += UlozZaznam;
            this.BZrusit = new Button { Content = "Zrušit", Margin = new Thickness(10), Width = 150, Height = 40 };
            this.BZrusit.Click += (sender, e) => this.Close();
            SPTlacitka.Children.Add(this.BUloz); SPTlacitka.Children.Add(BZrusit);
            SHlavniPanel.Children.Add(SPTlacitka);
            this.Content = SHlavniPanel;

        }
        public void UlozZaznam(object sender, EventArgs e)
        {
            int pozice = seznam.NajdiPoziciVSeznamuOsCislo(textboxy[0].Text);
            seznam.vojaciSeznam.RemoveAt(pozice);
            seznam.PridejCelehoVojaka(textboxy[0].Text, textboxy[1].Text, textboxy[2].Text, textboxy[3].Text, DateOnly.Parse(datapickery[4].Text), 
                                      textboxy[5].Text, textboxy[6].Text, DateOnly.Parse(datapickery[7].Text), DateOnly.Parse(datapickery[8].Text), 
                                      textboxy[9].Text, textboxy[10].Text);

            this.Close();
        }
    }
    public class CWPridej : Window                                                              //okno pro přidání záznamu do tabulky
    {
        public CWEdituj WEdituj = null;
        public TextBox TOsobniCislo, THodnost, TJmeno, TPrijmeni;
        public Label LOsobniCislo, LHodnost, LJmeno, LPrijmeni;
        public Button BPridej, BZrusit;
        

        public Vojaci seznam;
        
        public CWPridej(Vojaci aseznam)
        {
            this.seznam = aseznam;
            Width = 360; Height = 300; Title = "Přidání záznam do tabulky"; WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ResizeMode = ResizeMode.NoResize;
            ImageBrush myBrush = new ImageBrush();
            myBrush.ImageSource = new BitmapImage(new Uri($"{Directory.GetCurrentDirectory()}\\antenky.webp", UriKind.Absolute));
            myBrush.Stretch = Stretch.Fill; // Nastavuje způsob, jakým se obrázek roztahuje na pozadí
            Background = myBrush;
            StackPanel SHlavniPanel = new StackPanel();
            StackPanel SHPanel1 = new StackPanel { Orientation = Orientation.Horizontal };
            StackPanel SHPanel2 = new StackPanel { Orientation = Orientation.Horizontal };
            StackPanel SHPanel3 = new StackPanel { Orientation = Orientation.Horizontal };
            StackPanel SHPanel4 = new StackPanel { Orientation = Orientation.Horizontal };
            StackPanel SHPanel5 = new StackPanel { Orientation = Orientation.Horizontal };

            this.LOsobniCislo = new Label { Content = "Osobní číslo ", Margin = new Thickness(10), Width = 100};
            this.TOsobniCislo = new TextBox { Margin = new Thickness(10), Width = 200 };      
            SHPanel1.Children.Add(this.LOsobniCislo); SHPanel1.Children.Add(this.TOsobniCislo);
            SHlavniPanel.Children.Add(SHPanel1);

            this.LHodnost = new Label { Content = "Hodnost ", Margin = new Thickness(10), Width = 100 };
            this.THodnost = new TextBox { Margin= new Thickness(10), Width = 200 };
            SHPanel2.Children.Add(this.LHodnost); ; SHPanel2.Children.Add(this.THodnost);
            SHlavniPanel.Children.Add(SHPanel2);

            this.LJmeno = new Label { Content = "Jméno ", Margin = new Thickness(10), Width = 100 };
            this.TJmeno = new TextBox { Margin = new Thickness(10), Width = 200 };
            SHPanel3.Children.Add(this.LJmeno); SHPanel3.Children.Add(TJmeno);
            SHlavniPanel.Children.Add(SHPanel3);

            this.LPrijmeni = new Label { Content = "Příjmení ", Margin = new Thickness(10), Width = 100 };
            this.TPrijmeni = new TextBox { Margin = new Thickness(10), Width = 200 };
            SHPanel4.Children.Add(this.LPrijmeni); SHPanel4.Children.Add(TPrijmeni);
            SHlavniPanel.Children.Add(SHPanel4);

            this.BPridej = new Button { Content = "Přidej záznam", Margin = new Thickness(10), Width = 150, Height = 40 };
            this.BPridej.Click += PridejZaznam;
            this.BZrusit = new Button { Content = "Zrušit", Margin = new Thickness(10), Width = 150, Height = 40 };
            this.BZrusit.Click += (sender, e) => this.Close();
            SHPanel5.Children.Add(this.BPridej); SHPanel5.Children.Add(BZrusit);
            SHlavniPanel.Children.Add(SHPanel5);

            this.Content = SHlavniPanel;
        }
        public void PridejZaznam(object sender, RoutedEventArgs e)
        {
            string defZprava = "Chyba při zpracování:\n";
            string zprava = defZprava;
            bool cisloPouzito = false;
            if (TOsobniCislo.Text == "") { zprava += "Zadejte osobní číslo.\n"; }
            if (THodnost.Text == "") { zprava += "Zadejte hodnost.\n"; }
            if (TJmeno.Text == "") { zprava += "Zadejte jméno.\n"; }
            if (TPrijmeni.Text == "") { zprava += "Zadejte příjmení.\n"; }
            try { Convert.ToInt32(TOsobniCislo.Text); }
            catch (Exception ex) { zprava += "Osobní číslo musi být číslo!"; }
            foreach (Vojak vojak in seznam.vojaciSeznam) { if (TOsobniCislo.Text == vojak.OsobniCislo) { cisloPouzito = true; break; } }
            if (cisloPouzito) { MessageBox.Show("Zadané osobní číslo už je v databázi."); }
            else
            {
                if (zprava == defZprava)
                {
                    seznam.NactiZeSouboru();
                    seznam.Pridej(TOsobniCislo.Text, TPrijmeni.Text, TJmeno.Text, THodnost.Text);
                    MessageBox.Show("Záznam byl přidán");
                    this.Close();
                    WEdituj = new CWEdituj(seznam, TOsobniCislo.Text);
                    WEdituj.Show();
                }
                else { MessageBox.Show(zprava); }
            }
        }

    }
    public partial class MainWindow : Window                                                    //hlavní okno aplikace
    {
        public string UserName { get; set; }
        public Vojaci seznam = new Vojaci();
        public List<string> SeznamVlastnostiDoTabulky { get; set; }
        public string hledaneOC;
        private CWPridej WPridej = null;
        private CWEdituj WEdituj = null;
        private CWHledej WHledej = null;
        
        public MainWindow()
        {
            InitializeComponent();
            seznam.NactiZeSouboru();
            SeznamVlastnostiDoTabulky = seznam.SeznamVlastnosti();
            NaplnDataDoTabulky();
            seznam.ZmenaSeznamu += AktualizovatDataGrid;
        }
        private void AktualizovatDataGrid()                                                     //aktualizuje data v DataGridu
        {
            DGTabulka.ItemsSource = null;
            DGTabulka.ItemsSource = seznam.vojaciSeznam;
        }
        public void NaplnDataDoTabulky()                                                        //vytvoří a naplní DataGrid DGTabulka
        {
            DGTabulka.AutoGenerateColumns = false;

            //ImageBrush myBrush = new ImageBrush();
            //myBrush.ImageSource = new BitmapImage(new Uri($"{Directory.GetCurrentDirectory()}\\001.webp", UriKind.Absolute));
            //myBrush.Stretch = Stretch.Fill;
            //DGTabulka.Background = myBrush;
            DGTabulka.Background = Brushes.Transparent;

            foreach (string vlastnost in SeznamVlastnostiDoTabulky)
            {
                DataGridTextColumn column = new DataGridTextColumn();
                column.Header = seznam.NazvyDoTabulky[vlastnost]; // Nastavení hlavičky sloupce podle seznamu
                column.Binding = new Binding(vlastnost); // Vytvoření bindingu k odpovídající vlastnosti objektu
                DGTabulka.Columns.Add(column);
            }
            DGTabulka.Columns[0].IsReadOnly = true; //nastavení osobního čísla readonly
            DGTabulka.Columns[9].IsReadOnly = true; //nastavení username čísla readonly 
            DGTabulka.Columns[10].IsReadOnly = true; //nastavení password čísla readonly
            DGTabulka.ItemsSource = seznam.vojaciSeznam;
            
        }       
        private void WHlavni_Loaded(object sender, RoutedEventArgs e)                           //vyplní label s jménem uživatele
        {
            LUzivatel.Content = seznam.NapisUzivatele(UserName);
            ImageBrush myBrush = new ImageBrush();     
            myBrush.ImageSource = new BitmapImage(new Uri($"{Directory.GetCurrentDirectory()}\\001.webp", UriKind.Absolute));
            this.Background = myBrush;           
        }   
        private void BPridej_Click(object sender, RoutedEventArgs e)                            //obsluha tlačítka přidej
        {
            // Zkontrolujte, zda instance okna již existuje a zda není zavřená
            if (WPridej == null || !WPridej.IsLoaded)
            {
                WPridej = new CWPridej(this.seznam);
                WPridej.Closed += (s, args) => WPridej = null; // Zajistěte, že se referenci znuluje po zavření okna
                WPridej.Show();
            }
            else { WPridej.Focus(); }// Pokud je okno již otevřené, přeneseme na něj fokus          
        }
        private void BVyhledej_Click(object sender, RoutedEventArgs e)                          //obsluha tlačítka hledej
        {
            if (WHledej == null || !WHledej.IsLoaded)
            {
                WHledej = new CWHledej(seznam);
                WHledej.Closed += (s, args) => WHledej = null;
                WHledej.Show();
            }
            else { WHledej.Focus();}
        }
        private void DGTabulka_MouseDoubleClick(object sender, MouseButtonEventArgs e)          //na dvojklik do tabulky otevře editaci řádku
        { 
            DataGrid grid = sender as DataGrid;
            if (grid != null && grid.SelectedItem != null)
            {
                Vojak vojak = grid.SelectedItem as Vojak;
                if (vojak != null)
                {
                    //MessageBox.Show($"Dvakrát kliknuto na řádek s jménem: {vojak.OsobniCislo} {vojak.Jmeno} {vojak.Prijmeni}");         
                    if (WEdituj == null || !WEdituj.IsLoaded)
                    {                       
                        WEdituj = new CWEdituj(this.seznam, vojak.OsobniCislo);
                        WEdituj.Closed += (s, args) => WEdituj = null;
                        WEdituj.Show();
                    }
                    else { WEdituj.Focus(); }

                }
            }
        
        }
        private void WHlavni_Closing(object sender, System.ComponentModel.CancelEventArgs e)    //zeptá se uživatele zda chce ukončit aplikaci
        {
            if (MessageBox.Show("Opravdu chcete zavřít aplikaci?\nNeuložená data budou ztracena.", "Potvrzení", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }
        private void WHlavni_Closed(object sender, EventArgs e)                                 //uloží data a zavře všechna okna
        {
            seznam.ZapisDoSouboru();
            foreach (Window window in System.Windows.Application.Current.Windows) // zajistí zavření všech oken pokud se zavře hlavní okno
            {
                if (window != this) // Pokud okno není hlavní okno
                {
                    window.Close(); // Zavřít okno
                }
            }
        }
        private void BOdeber_Click(object sender, RoutedEventArgs e)                            //odebere aktuálně vybraný záznam v DataGridu po potvrzení uživatelem
        {
            if (DGTabulka.SelectedItem != null)
            {
                Vojak vojak = DGTabulka.SelectedItem as Vojak;
                if (MessageBox.Show("Opravdu chcete odebrat záznam?", "Smazání záznamu", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    seznam.vojaciSeznam.Remove(vojak);
                    AktualizovatDataGrid();
                    seznam.ZapisDoSouboru();
                }
            }
        }
    }
}
