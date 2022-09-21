using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AsyncBoekOpdracht
{
    class Boek
    {
        public string Titel { get; set; } 
        public string Auteur { get; set; } 

        public Boek(string titel, string auteur){
            this.Titel = titel;
            this.Auteur = auteur;
        }
        
        public float AIScore {
            get {
                // Deze 'berekening' is eigenlijk een ingewikkeld AI algoritme.
                // Pas de volgende vier regels niet aan.
                double ret = 1.0f;
                for (int i = 0; i < 10000000; i++)
                    for (int j = 0; j < 10; j++)
                        ret = (ret + Willekeurig.Random.NextDouble()) % 1.0;
                return (float)ret;
            }
        }
    }
    static class Willekeurig
    {
        public static Random Random = new Random();
        public static async Task Vertraging(int MinAantalMs = 500, int MaxAantalMs = 1000)
        {
            await Task.Delay(Random.Next(MinAantalMs, MaxAantalMs));
        }
    }
    static class Database
    {
        private static List<Boek> lijst = new List<Boek>();
        public static async void VoegToe(Boek b)
        {
            await Willekeurig.Vertraging(); // INSERT INTO ...
            lijst.Add(b);
        }
        public static async Task<List<Boek>> HaalLijstOp()
        {
            await Willekeurig.Vertraging(); // SELECT * FROM ...
            return lijst;
        }
        public static async void Logboek(string melding)
        {
            await Willekeurig.Vertraging(); // schrijf naar logbestand
        }
    }
    class Program
    {
        static async Task VoegBoekToe() {
            Console.WriteLine("Geef de titel op: ");
            var titel = Console.ReadLine();
            Console.WriteLine("Geef de auteur op: ");
            var auteur = Console.ReadLine();
            Database.VoegToe(new Boek(titel,auteur));
            Database.Logboek("Er is een nieuw boek!");
            Console.WriteLine("De huidige lijst met boeken is: ");
            foreach (var boek in await(Database.HaalLijstOp())) {
                Console.WriteLine(boek.Titel);
            }
        }
        static async Task ZoekBoek() {
            Console.WriteLine("Waar gaat het boek over?");
            var beschrijving = Console.ReadLine();
            Boek beste = null;
            foreach (var boek in await(Database.HaalLijstOp()))
                if (beste == null || boek.AIScore > beste.AIScore)
                    beste = boek;
            Console.WriteLine("Het boek dat het beste overeenkomt met de beschrijving is: ");
            Console.WriteLine(beste.Titel);
        }
        static bool Backupping = false;
        // "Backup" kan lang duren. We willen dat de gebruiker niet hoeft te wachten,
        // en direct daarna boeken kan toevoegen en zoeken.
        static async void Backup() {
            if (Backupping)
                return;
            Backupping = true;
            await Willekeurig.Vertraging(2000, 3000);
            Backupping = false;
        }
        static async Task Main(string[] args)
        {
            seedDB(10);
            Console.WriteLine("Welkom bij de boeken administratie!");
            string key = "";
            while (key != "0") {
                Console.WriteLine("1) Boek toevoegen");
                Console.WriteLine("2) Boek zoeken");
                Console.WriteLine("3) Backup maken van de boeken");
                Console.WriteLine("0) Quit");
                key = Console.ReadLine();
                if (key == "1")
                    await VoegBoekToe();
                else if (key == "2")
                    await ZoekBoek();
                else if (key == "3")
                    Backup();
                else Console.WriteLine("Ongeldige invoer!");
                Console.Clear();
                
            }
        }


        static void seedDB(int amount){
            for(int i =0; i<amount;i++){
                Database.VoegToe(new Boek(i.ToString(), i.ToString()));
            }
        }

        // static void Main(String[] args){
        //     MainAsync(args).GetAwaiter().GetResult();
        // }
    }
}
