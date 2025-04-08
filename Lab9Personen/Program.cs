using Lab9_Personen.Extensions;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace Lab9Personen
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            var personen = JsonSerializer.Deserialize<List<Person>>(File.ReadAllText("Personen.json"))!;

            // Hier den Code schreiben

            static string DefaultFormatter(Person p)
                => $"{($"    {p.Vorname} {p.Nachname}")} ({p.Alter}), {p.Job.Titel}, {p.Job.Gehalt} €";

            // 1. Finde alle Personen, die mindestens 60 Jahre alt sind.
            personen.Where(e => e.Alter >= 60)
                .PrintTop10((p) => $"    {($"{p.Vorname} {p.Nachname}")} ist {p.Alter} Jahre alt.")
                .ToConsole("Personen ueber 60 Jahre alt\n");

            // 2. Finde alle Personen, die im Jahr 1977 geboren wurden.
            personen.Where(e => e.Geburtsdatum.Year == 1977)
                .PrintTop10(p => $"    {p.Vorname} {p.Nachname}: {p.Geburtsdatum.ToString("dd. MMMM")}")
                .ToConsole("Personen geboren im Jahr 1977\n");

            // 3. Finde alle Personen, die mehr als 5000€/Monat verdienen.
            personen.Where(e => e.Job.Gehalt > 5000)
                .PrintTop10(DefaultFormatter)
                .ToConsole("Personen mit mehr als 5000€ Gehalt\n");

            // 4. Sortiere alle Personen nach Jobtitel, danach nach Gehalt.
            personen.OrderBy(e => e.Job.Titel)
                .ThenBy(e => e.Job.Gehalt)
                .PrintTop10(DefaultFormatter)
                .ToConsole("Personen sortiert nach Jobtitel und Gehalt\n");

            // 5. Wieviele Personen haben einen Vornamen mit mindestens 10 Buchstaben?
            personen.Count(e => e.Vorname.Length >= 10)
                .ToConsole("Personen mit mindestens 10 Buchstaben: \t");

            // 6. Wieviel verdienen Softwareentwickler im Durchschnitt?
            personen.Where(e => e.Job.Titel == "Softwareentwickler")
                .Average(e => e.Job.Gehalt)
                .ToConsole("\nSoftwareentwickler im Durchschnitt: \t");

            // 7. Wie viele Personen haben genau zwei Hobbies?
            personen.Count(e => e.Hobbies.Count == 2)
                .ToConsole("\nPersonen mit genau zwei Hobbies: \t");

            // 8. Finde alle Personen, die Radfahren und Laufen als Hobbies haben.
            personen.Where(e => e.Hobbies.Contains("Radfahren") && e.Hobbies.Contains("Laufen"))
                .PrintTop10(p => $"    {p.Vorname} {p.Nachname} hat Radfahren und Laufen als Hobby.")
                .ToConsole("\nPersonen mit Radfahren und Laufen als Hobby\n");

            // 9. Finde alle Personen, deren Vorname mit M beginnt und deren Nachname mit S beginnt.
            personen.Where(e => e.Vorname[0] == 'M' && e.Nachname[0] == 'S')
                .PrintTop10(DefaultFormatter)
                .ToConsole("Personen mit M-Vorname und S-Nachname\n");

            // 10. Finde alle Personen, bei der der Vorname und der Nachname den selben Anfangsbuchstaben haben.
            personen.Where(e => e.Vorname[0] == e.Nachname[0])
                .PrintTop10(DefaultFormatter)
                .ToConsole("Personen mit gleichem Anfangsbuchstaben\n");

            // 11. Finde alle Personen, die überdurchschnittlich alt sind in Relation zu allen Personen.
            personen.Where(e => e.Alter > personen.Average(x => x.Alter))
                .PrintTop10(DefaultFormatter)
                .ToConsole("Personen ueber Durchschnitt alt\n");

            // 12. Wieviele Personen die über 60 Jahre alt sind betreiben noch Sport (eine Tätigkeit von: Laufen, Radfahren, Ballsport, Fitness)?
            var desiredHobbies = new string[] { "Laufen", "Radfahren", "Ballsport", "Fitness" };
            personen.Where(e => e.Alter > 60 && e.Hobbies.Any(h => desiredHobbies.Contains(h)))
                .ToString(DefaultFormatter)
                .ToConsole("Personen ueber 60 Jahre alt mit Sport-Hobby\n");
            personen.Where(e => e.Alter > 60 && e.Hobbies.Intersect(desiredHobbies).Any())
                .ToString(DefaultFormatter)
                .ToConsole("Personen ueber 60 Jahre alt mit Sport-Hobby (Intersect)\n");

            // 13. Wieviele verschiedene Jobs gibt es?
            personen.Select(e => e.Job.Titel)
                .Distinct()
                .Count()
                .ToConsole("Anzahl unterschiedlicher Jobs:\t");

            // 14. Finde das höchste Gehalt aller Tischler.
            personen.Where(e => e.Job.Titel == "Tischler")
                .Max(e => e.Job.Gehalt)
                .ToConsole("Hoechster Gehalt eines Tischlers: \t");

            // 15. Verdienen alle Personen die über 50 Jahre alt sind über 2000€?
            personen.Where(e => e.Alter > 50 && e.Job.Gehalt > 2000)
                .ToString(DefaultFormatter)
                .ToConsole("Alle ueber 50 und mehr als 2000€\n");

            // 16. Gib alle Vor- und Nachnamekombinationen aus, sortiert nach Länge.
            personen.Select(e => e.Vorname + " " + e.Nachname)
                .OrderBy(e => e.Length)
                .ToConsole("Vor- und Nachnamekombinationen\n");

            // 17. Finde die Top 5 Höchstverdiener.
            personen.OrderByDescending(e => e.Job.Gehalt)
                .Take(5).ToString(DefaultFormatter)
                .ToConsole("Top 5 Hoechstverdiener\n");

            // 18. Finde alle Personen, die seit mindestens 20 Jahren in ihrem Job angestellt sind.
            personen.Count(e => (int)(DateTime.Today.Subtract(e.Job.Einstellungsdatum).TotalDays / 365.25) >= 20)
                .ToConsole("Personen seit mindestens 20 Jahren ihren Job haben:\t");

            // 19. Welche Vornamen kommen in der Liste am häufigsten vor?
            personen.GroupBy(e => e.Vorname)
                .ToDictionary(e => e.Key, e => e.Count())
                .OrderByDescending(e => e.Value)
                .Where(c => c.Value > 1)
                .GroupBy(e => e.Value)
                .ToString(e => $"    {e.Key} mal: {string.Join(", ", e.Select(x => x.Key))}")
                .ToConsole("haufigste Vornamen\n");

            // 20. Finde pro Beruf das höchste Gehalt, sortiert nach Jobtitel.
            personen.GroupBy(e => e.Job.Titel)
                .ToDictionary(e => e.Key, e => e.Max(x => x.Job.Gehalt))
                .OrderBy(e => e.Key)
                .ToString(e => $"    {e.Key}: {e.Value} €")
                .ToConsole("pro Beruf das hoechste Gehalt\n");

            // 21. Finde das am häufigsten vorkommende Hobby.
            personen.Select(e => e.Hobbies)
                .SelectMany(e => e)
                .GroupBy(e => e)
                .ToDictionary(e => e.Key, e => e.Count())
                .OrderByDescending(e => e.Value)
                .ToString(e => $"    {e.Key}: {e.Value} Personen")
                .ToConsole("am haufigsten vorkommendes Hobby\n");

            // 22. Finde pro Berufsgruppe die Top 3 Höchstverdiener.
            personen.GroupBy(e => e.Job.Titel)
                .ToDictionary(e => e.Key, e => e.OrderByDescending(x => x.Job.Gehalt).Take(3))
                .SelectMany(e => e.Value)
                .ToString(p => $"    {p.Job.Titel}: {p.Vorname} {p.Nachname} {p.Job.Gehalt} €")
                .ToConsole("pro Berufsgruppe die Top 3 Hoechstverdiener\n");
        }
    }

    [DebuggerDisplay("Person - ID: {ID}, Vorname: {Vorname}, Nachname: {Nachname}, GebDat: {Geburtsdatum.ToString(\"yyyy.MM.dd\")}, Alter: {Alter}, " +
"Jobtitel: {Job.Titel}, Gehalt: {Job.Gehalt}, Einstellungsdatum: {Job.Einstellungsdatum.ToString(\"yyyy.MM.dd\")}")]
    public record Person(int ID, string Vorname, string Nachname, DateTime Geburtsdatum, int Alter, Beruf Job, List<string> Hobbies);

    public record Beruf(string Titel, int Gehalt, DateTime Einstellungsdatum);
}
