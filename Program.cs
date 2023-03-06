using System.Data.SQLite;
using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        List<Personne> personnes = new List<Personne>();

        for (int i = 0; i < 77; i++)
        {
            string prenom = GenerateRandomPrenom();
            int age = GenerateRandomAge();
            string ville = GenerateRandomVille();

            Personne p = new Personne(prenom, age, ville);
            personnes.Add(p);

            Console.WriteLine($"Personne {i + 1} : {prenom}, {age} ans, {ville}");
        }

        string connectionString = "Data Source = C:\\Users\\tibo9\\db_tinder.db;Version=3;";
        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
        {
            connection.Open();

            // Création de la table "Personne" si elle n'existe pas déjà
            string createTableSql = "CREATE TABLE IF NOT EXISTS cluster (id INTEGER PRIMARY KEY AUTOINCREMENT, prenom TEXT, age INTEGER, ville TEXT)";
            using (SQLiteCommand createTableCommand = new SQLiteCommand(createTableSql, connection))
            {
                createTableCommand.ExecuteNonQuery();
            }

            // Insertion des données de la liste "personnes" dans la table "Personne"
            string insertSql = "INSERT INTO cluster (prenom, age, ville) VALUES (@prenom, @age, @ville)";
            using (SQLiteCommand insertCommand = new SQLiteCommand(insertSql, connection))
            {
                // Utilisation de paramètres pour éviter les attaques par injection SQL
                insertCommand.Parameters.Add(new SQLiteParameter("@prenom"));
                insertCommand.Parameters.Add(new SQLiteParameter("@age"));
                insertCommand.Parameters.Add(new SQLiteParameter("@ville"));

                foreach (Personne personne in personnes)
                {
                    insertCommand.Parameters["@prenom"].Value = personne.Prenom;
                    insertCommand.Parameters["@age"].Value = personne.Age;
                    insertCommand.Parameters["@ville"].Value = personne.Ville;

                    insertCommand.ExecuteNonQuery();
                }
                Console.WriteLine("Base mise à jour");

            }

            connection.Close();
        }
    }

    private static string GenerateRandomPrenom()
    {
        string[] prenoms = { "Alice", "Bob", "Charlie", "David", "Emma", "Frank", "Grace", "Henry", "Isabelle", "Jack", "Kate", "Liam", "Mia", "Noah", "Olivia", "Parker", "Quinn", "Ryan", "Sofia", "Thomas", "Uma", "Victoria", "William", "Xavier", "Yara", "Zoe" };
        Random rand = new Random();
        return prenoms[rand.Next(prenoms.Length)];
    }

    private static int GenerateRandomAge()
    {
        Random rand = new Random();
        return rand.Next(18, 81);
    }

    private static string GenerateRandomVille()
    {
        string[] villes = { "Paris", "Lyon", "Marseille", "Bordeaux", "Toulouse", "Nice", "Nantes", "Lille", "Rennes", "Strasbourg", "Montpellier", "Rouen", "Brest", "Grenoble", "Caen", "Reims", "Amiens", "Metz", "Tours", "Nancy", "Orléans", "Besançon", "Clermont-Ferrand", "Dijon", "Limoges", "Angers", "Poitiers", "Le Havre", "Saint-Étienne", "Mulhouse", "Perpignan", "Béziers", "Avignon", "Valence", "Toulon", "La Rochelle", "Chambéry", "Annecy", "Aix-en-Provence", "Saint-Malo" };
        Random rand = new Random();
        return villes[rand.Next(villes.Length)];
    }
}

public class Personne
{
    public string Prenom { get; set; }
    public int Age { get; set; }
    public string Ville { get; set; }

    public Personne(string prenom, int age, string ville)
    {
        Prenom = prenom;
        Age = age;
        Ville = ville;
    }
}

