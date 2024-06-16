using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Reflection;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Security.Cryptography;


namespace RIS_0._4
{    public partial class Vojak // pokud přidáváš vlastnosti(sloupce do tabulky) NEZAPOMEŇ aktualizovat slovník názvů!!!
    {
        public string OsobniCislo { get; set; }
        public string Hodnost { get; set; }
        public string Jmeno { get; set; }
        public string Prijmeni { get; set; }
        public DateOnly DatumNarozeni { get; set; }
        public string Bydliste { get; set; }
        public string RodneCislo { get; set; }                    
        public DateOnly SluzbaOd {  get; set; }
        public DateOnly SluzbaDo {  get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; } 
        public Vojak(string osobniCislo, string hodnost, string jmeno, string prijmeni)
        {
            this.OsobniCislo = osobniCislo;
            this.Hodnost = hodnost;
            this.Jmeno = jmeno;
            this.Prijmeni = prijmeni;
        }
    }

    public partial class Vojaci
    {
        public ObservableCollection<Vojak> vojaciSeznam { get; set; } = new ObservableCollection<Vojak>();
        public Dictionary<string, string> NazvyDoTabulky { get; set; } = new Dictionary<string, string>() { //jména sloupců do DGTabulky, jinak si bere názvy vlastnosti
        {"OsobniCislo", "Osobní číslo"}, {"Hodnost", "Hodnost"}, {"Jmeno", "Jméno"}, {"Prijmeni", "Příjmení"}, 
        {"DatumNarozeni", "Datum narození"}, {"Bydliste", "Bydliště" }, {"RodneCislo", "Rodné číslo"}, 
        {"SluzbaOd", "Slouží od"}, {"SluzbaDo", "Závazek do"}, {"UserName", "Uživatelské jméno"}, {"Password", "Heslo"},
        {"Salt", "Sůl" }};
        public int PocetPolozek { get; set; }       // při každém zápisu do souboru se aktualizuje
        public int PocetRadku { get; set; }         // při každém zápisu do souboru se aktualizuje
        public event Action ZmenaSeznamu;
        public void Pridej(string osobniCislo, string prijmeni, string jmeno, string hodnost)      //přidá vojáka. Jen povinné pole a unikátní username ve tvaru
        {                                                                                          // příjmení + první písmeno jména a index existuje-li duplicita
            string userName;
            Vojak vojak = new Vojak(osobniCislo, hodnost, jmeno, prijmeni);           
            userName = OdstranDiakritiku(prijmeni.Trim()).ToLower() + OdstranDiakritiku(jmeno.Trim().Substring(0,1)).ToLower();
            userName = ZkontrolujVyskytUsername(userName);
            vojak.UserName = userName;
            this.vojaciSeznam.Add(vojak);
            ZapisDoSouboru();
            ZmenaSeznamu?.Invoke();
        }
        public string ZkontrolujVyskytUsername(string ausername)                                   //zkontroluje duplicitu username. Pokud je již v seznamu
        {                                                                                          //přidá index nebo ho zvetší o jedna
            string username = ausername;
            int i = 1;                              
            bool jeJenJedno = false;
            while (!jeJenJedno)                              
            {
                bool duplicitaNalezena = false;
                foreach (Vojak vojak in this.vojaciSeznam)
                {
                    if (username == vojak.UserName)
                    {
                        duplicitaNalezena = true;
                        i++;
                        username = ausername + i;
                        break;
                    }
                }
                if (!duplicitaNalezena)
                {
                    jeJenJedno = true; 
                }
            }
            return username;
        }
        public string OdstranDiakritiku(string text)                                               //odstraní diakritiku ze stringu(pro vytvoření username)
        {
            var normalizovanyText = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();
            foreach (var c in normalizovanyText)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }
            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
        public void PridejCelehoVojaka(string osobniCislo, string hodnost, string jmeno, string prijmeni, 
                                       DateOnly datumNarozeni, string bydliste, string rodneCislo, DateOnly sluzbaOd, 
                                       DateOnly sluzbaDo, string userName, string password)
        {
            Vojak vojak = new Vojak(osobniCislo, hodnost, jmeno, prijmeni);
            vojak.DatumNarozeni = datumNarozeni; vojak.Bydliste = bydliste; vojak.RodneCislo = rodneCislo;
            vojak.SluzbaOd = sluzbaOd; vojak.SluzbaDo = sluzbaDo; vojak.UserName = userName; vojak.Password = password;
            this.vojaciSeznam.Add(vojak);
            ZapisDoSouboru();
            ZmenaSeznamu?.Invoke();
        }
        public bool NajdiOC(string aOsCislo)                                    //zjistí zda je dané osobní číslo v seznamu
        {
            bool jeVSeznamu = false;
            foreach (Vojak vojak in vojaciSeznam)
            {
                if (vojak.OsobniCislo == aOsCislo) { jeVSeznamu = true; break; }
            }
            return jeVSeznamu;
        }                   
        public List<string> NajdiOCPodleJmena(string aJmeno)                    //najde všechna osobní čísla podle jména
        {
            List <string> OCisla = new List<string>();
            foreach (Vojak vojak in vojaciSeznam)
            {
                if (vojak.Jmeno == aJmeno) { OCisla.Add(vojak.OsobniCislo); }               
            }
            return OCisla;
        }                  
        public List<string> NajdiOCPodlePrijmeni(string aPrijmeni)              //najde všechna osobní čísla podle příjmení
        {
            List<string> OCisla = new List<string>();
            foreach (Vojak vojak in vojaciSeznam)
            {
                if (vojak.Prijmeni == aPrijmeni) { OCisla.Add(vojak.OsobniCislo); }
            }
            return OCisla;
        }           
        public int NajdiPoziciVSeznamuOsCislo(string aOsCislo)                  //najde pozici osobního čísla v seznamu
        {
            int i = 0;
            foreach (Vojak vojak in vojaciSeznam)
            {
                if (vojak.OsobniCislo == aOsCislo) { break; }
                i++;
            }
            return i;
        }               
        public int NajdiPoziciVSeznamuUsername(string ausername)                //najde pozici username v seznamu, (!vrátí -1 když ne)
        {
            int i = 0;
            foreach (Vojak vojak in vojaciSeznam)
            {
                if (vojak.UserName == ausername) { break; }
                i++;
            }
            //if (i > this.PocetRadku) { i = -1; }
            return i;
        }
        public string[] VojakDoPole(string aOsCislo)                            //převede záznam podle osobního čísla do pole 
        {
            int pozice;
            string[] vojak = new string[this.PocetPolozek];
            pozice = NajdiPoziciVSeznamuOsCislo(aOsCislo);

            vojak[0] = this.vojaciSeznam[pozice].OsobniCislo; vojak[1] = this.vojaciSeznam[pozice].Hodnost;
            vojak[2] = this.vojaciSeznam[pozice].Jmeno; vojak[3] = this.vojaciSeznam[pozice].Prijmeni;
            vojak[4] = this.vojaciSeznam[pozice].DatumNarozeni.ToString("dd.MM.yyyy"); vojak[5] = this.vojaciSeznam[pozice].Bydliste;
            vojak[6] = this.vojaciSeznam[pozice].RodneCislo; vojak[7] = this.vojaciSeznam[pozice].SluzbaOd.ToString("dd.MM.yyyy");
            vojak[8] = this.vojaciSeznam[pozice].SluzbaDo.ToString("dd.MM.yyyy"); vojak[9] = this.vojaciSeznam[pozice].UserName;
            vojak[10] = this.vojaciSeznam[pozice].Password;
            return vojak;        
        }       
        public void ZapisDoSouboru()                                            //zápis do souboru vojaci.dat ve formátu json
        {
            this.SpoctiPocetRadku();
            this.SpoctiPocetSloupcu();
            File.WriteAllText("vojaci.dat", JsonSerializer.Serialize(this));
        }
        public void NactiZeSouboru()                                            //načtení dat ze souboru vojaci.dat
        {
            string serializovane = File.ReadAllText("vojaci.dat");
            Vojaci pomocna = JsonSerializer.Deserialize<Vojaci>(serializovane);
            this.vojaciSeznam = pomocna.vojaciSeznam;
        }           
        public List<string> SeznamVlastnosti()                                  //spočítá a aktualizuje počet položek třídy voják, vráti seznam vlastností
        {
            {
                this.PocetPolozek = 0;
                Type autoType = typeof(Vojak);
                List<string> seznamVlastnosti = new List<string>();
                PropertyInfo[] properites = autoType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (PropertyInfo property in properites)
                {
                    this.PocetPolozek++;
                    seznamVlastnosti.Add(property.Name);
                }
                return seznamVlastnosti;
            }
        }
        public void SpoctiPocetSloupcu()                                        //spočítá a aktualizuje počet položek třídy voják
        {
            {
                this.PocetPolozek = 0;
                Type autoType = typeof(Vojak);
                List<string> seznamVlastnosti = new List<string>();
                PropertyInfo[] properites = autoType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (PropertyInfo property in properites)
                {
                    this.PocetPolozek++;
                }
            }
        }
        public void SpoctiPocetRadku()                                          //spočítá a aktualizuje počet řádků
        {
            this.PocetRadku = 0;
            foreach (Vojak vojak in this.vojaciSeznam) { this.PocetRadku++; }
        }
        public bool OverUzivatele(string username, string password)             //demo přihlašovací funkce
        {
            bool status = false;
            int pozice = NajdiPoziciVSeznamuUsername(username);
           
            if (pozice == -1) { MessageBox.Show("Uživatelské jméno neexistuje!"); } // tato varianta nenastane
            else { Vojak vojak = this.vojaciSeznam[pozice];

                if (vojak.UserName == username && Vojaci.OverHeslo(password,vojak.Password,vojak.Salt )) { status = true; }
            }
            return status;
        }                                               
        public static string[] ZahashujHeslo(string apassword)                  //Zahashuje heslo a vrátí sůl a hash
        {
            byte[] sul = new byte[16];
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetBytes(sul);
            }
            using (var pbkdf2 = new Rfc2898DeriveBytes(apassword, sul, 10000))
            {
                byte[] hash = pbkdf2.GetBytes(20);
                string sulBase64 = Convert.ToBase64String(sul);
                string hashBase64 = Convert.ToBase64String(hash);
                return [sulBase64, hashBase64];
            }
        }
        public static bool OverHeslo(string azadaneHeslo, string ahash, string asul)// Ověří heslo podle uloženého hashe a soli
        {
            byte[] sul = Convert.FromBase64String(asul);                            // Dekoduj sůl z Base64

                                                                                    // Zahashuj zadané heslo s uloženou solí
            using (var pbkdf2 = new Rfc2898DeriveBytes(azadaneHeslo, sul, 10000))
            {
                byte[] hash = pbkdf2.GetBytes(20);                                  // Vygeneruj hash
                string novyHashBase64 = Convert.ToBase64String(hash);               // Převeď nový hash na Base64
                
                return novyHashBase64 == ahash;                                     // Porovnej nový hash s uloženým hashem
            }
        }
        public string NapisUzivatele(string username)                               //vrátí string hodnotu pro label na úvodní stránce
        {
            string uvitani = "Uživatel: ";
            foreach (Vojak vojak in this.vojaciSeznam)
                { 
                    if (vojak.UserName == username) 
                    {
                        uvitani += $"{vojak.Hodnost} {vojak.Jmeno} {vojak.Prijmeni}";
                    }
                }
            return uvitani;
        }
    }
}
