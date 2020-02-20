using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Arvauspeli
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> arvaukset = new List<int>();
            int arvattava, arvaus;
            Boolean jatka = true;
            DateTime tanaan = DateTime.Now;
            string tiedosto = "D:\\arvaukset.txt";                                  // Huom! Muista vaihtaa oletus!

            Console.ForegroundColor = ConsoleColor.Black;                           // Versiotietojen ja päivämäärän taustan ja fontin värit
            Console.BackgroundColor = ConsoleColor.DarkRed;

            Console.WriteLine("Arvauspeli, versio 1.0 \n" + tanaan.ToString());

            Console.ResetColor();                                                   // Värien palautus oletusarvoiksi

            while (jatka)
            {
                Console.WriteLine("\nTervetuloa pelaamaan arvauspeliä!");
                Console.WriteLine("Tietokone arpoo sinulle luvun väliltä 0 - 100. Sinun tulee arvata oikea luku.");

                arvattava = satunnaisGeneraattori();                                // Annetaan satunnaisluku muuttujalle arvattava

                do
                {
                    arvaus = arvaaLuku(arvattava);                                  // Luvun arvaamisen ja vertaamisen metodikutsu
                    arvaukset.Add(arvaus);                                          // Jokainen arvattu luku talletetaan listalle
                }
                while (arvattava != arvaus);

                tulosta(arvaukset, tiedosto);                                       // Listalle lisättyjen arvausten tulostus
                jatka = pelaaUudelleen();
                arvaukset.Clear();
            }
        }

        public static int satunnaisGeneraattori()                                   // Tietokoneen arpoman satunnaisluvun luontimetodi
        {
            Random generaattori = new Random();                                     // Luodaan satunnaislukugeneraattori

            int satunnaisluku = generaattori.Next(1, 100);                          // Arvotaan luku väliltä 1 - 100 ja annetaan se muuttujalle satunnaisluku

            return satunnaisluku;                                                   // Palautetaan satunnaisluku
        }

        public static int arvaaLuku(int arvattava)                                  // Arvausten syöttämiseen ja vertaamiseen satunnaislukumuuttujaan muodostettu metodi
        {
            Alku:                                                                   // Goto käskyn alkukohta. Estää väärän syötteen tallentumisen listalle.
            Console.Write("\nAnna luku: ");
            int arvaus = 0;

            try
            {
                arvaus = Convert.ToInt16(Console.ReadLine());

                if (arvaus > arvattava && arvaus <= 100)
                {
                    Console.WriteLine("Luku on pienempi kuin arvaus");
                }
                else if (arvaus < arvattava && arvaus >= 0)
                {
                    Console.WriteLine("Luku on suurempi kuin arvaus");
                }
                else if (arvaus < 0 || arvaus > 100)
                {
                    Console.WriteLine("Sinun tulee antaa luku väliltä 0 - 100");        // Jos syöte ei ole väliltä 0 - 100, hypätään metodin alkuun
                    goto Alku;
                }
            }
            catch(Exception)
            {
                Console.WriteLine("En ymmärtänyt. Varmista, että syötät vain lukuja väliltä 1-100.");
                goto Alku;                                                              // Jos syöte on muu kuin kokonaisluku, annetaan virheilmoitus ja hypätään alkuun
            }

            return arvaus;
        }

        public static void tulosta(List<int> arvaukset, string tiedosto)                // Metodi listan arvojen tulostamiseen
        {
            string edellinenTulos = "0";
            int nykyinenTulos = arvaukset.Count;

            Console.WriteLine("\nOnneksi olkoon! Arvasit oikein!");
            Console.WriteLine("Arvasit yhteensä " + arvaukset.Count + " kertaa.");

            Console.WriteLine("\nArvauksesi:");
            for(int i = 0; i < arvaukset.Count; i++)
            {
                Console.WriteLine((i + 1) + ". arvaus: " + arvaukset[i]);
            }

            if (File.Exists(tiedosto))
            {
                edellinenTulos = File.ReadAllText(tiedosto);                            // Luetaan tiedostosta edellisen pelikerran tulos
                Console.WriteLine("\nAiemmalla pelikerralla arvasit {0} kertaa.", edellinenTulos);

                if (nykyinenTulos > int.Parse(edellinenTulos))                          // Verrataan edellisen pelin tulosta nykyiseen peliin ja tulostetaan sen mukaisesti
                {
                    Console.WriteLine("Harmillista. Pärjäsit viimeksi paremmin.");
                }
                else if(nykyinenTulos < int.Parse(edellinenTulos))
                {
                    Console.WriteLine("Hienoa! Onnistuit paremmin kuin viime kerralla!");
                    
                    //Voittopiippaus :D                                           
                    Console.Beep(659, 125);
                    Console.Beep(659, 125);
                    Thread.Sleep(125);
                    Console.Beep(659, 125);
                    Thread.Sleep(167);
                    Console.Beep(523, 125);
                    Console.Beep(659, 125);
                    Thread.Sleep(125);
                    Console.Beep(784, 125);
                    Thread.Sleep(375);
                    Console.Beep(392, 125);
                }
                else if(nykyinenTulos == int.Parse(edellinenTulos))
                {
                    Console.WriteLine("Oho, suoritit pelin yhtä monella arvauksella kuin viimeksi!");
                }

                File.WriteAllText(tiedosto, arvaukset.Count.ToString());                 // Tallennetaan tämän pelikerran tulos
            }
            else
                Console.WriteLine("Edellisen pelin tulostietoja ei ole saatavilla.");
        }

        public static Boolean pelaaUudelleen()
        {
            Console.Write("Haluatko pelata uudelleen (K/E)?");
            string valinta = Console.ReadLine();

            if(valinta.ToUpper().Equals("K"))
            {
                return true;
            }
            else
                return false;
        }
    }
}
