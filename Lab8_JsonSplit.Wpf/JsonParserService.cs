using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Lab8_JsonSplit.Wpf
{
    public class JsonParserService
    {
        public JsonParserService(Action<string> updateProgress, Action<double> setMaxProgress)
        {
            UpdateProgress = updateProgress;
            SetMaxProgress = setMaxProgress;
        }

        public Action<string> UpdateProgress { get; }
        public Action<double> SetMaxProgress { get; }

        /// <summary>
        /// Diese Methode soll die originale Json Datei laden, aufteilen und in die einzelnen Dateien speichern.
        /// Diese Methode ist mit dem linken Button (Split Json) in der UI verbunden
        /// </summary>
        public async Task SaveSplitJson()
        {
            using StreamReader sr = new StreamReader("history.city.list.min.json");
            JsonDocument jd = await JsonDocument.ParseAsync(sr.BaseStream);
            //StreamReader erstellen und BaseStream davon als Stream-Parameter hier benutzen
            //Json File einlesen mit await um UI-Freeze zu verhindern

            if (Directory.Exists("Lab"))
                Directory.Delete("Lab", true); //Übung zurücksetzen
            Directory.CreateDirectory("Lab");

            ConcurrentDictionary<string, List<JsonElement>> cities = new();
            foreach (JsonElement je in jd.RootElement.EnumerateArray()) //Json Datei iterieren
            {
                string countryCode = je.GetProperty("city").GetProperty("country").GetString(); //Auf den CountryCode zugreifen (AT, DE, IT, ...)
                if (!cities.ContainsKey(countryCode))
                    cities.TryAdd(countryCode, new());
                cities[countryCode].Add(je);
            }

            SetMaxProgress(cities.Count);

            //await ForEachAsync um UI nicht freezen zu lassen
            //await Parallel.ForEachAsync(cities, (kv, x) => //Json Files schreiben
            //{
            //	File.WriteAllText(Path.Combine("Lab", $"{kv.Key}.json"), JsonListToJson(kv.Value)); //Methode weiter unten
            //  UpdateProgress($"{kv.Key} in die Datei geschrieben");
            //	return ValueTask.CompletedTask; //Hier einfach "leeres" Objekt zurückgeben
            //});

            //Ohne Parallel, langsamer
            List<Task> writeTasks = new();
            foreach (KeyValuePair<string, List<JsonElement>> kv in cities) //Json Files schreiben
                writeTasks.Add(WriteFileAndUpdateUI(kv.Key, JsonListToJson(kv.Value)));
            await Task.WhenAll(writeTasks);
        }

        private async Task WriteFileAndUpdateUI(string path, string content)
        {
            File.WriteAllTextAsync(Path.Combine("Lab", $"{path}.json"), content); //Methode weiter unten

            UpdateProgress($"{path} in die Datei geschrieben");
        }

        /// <summary>
        /// Diese Methode soll die aufgeteilten Json Dateien wieder einlesen und danach in einem Dictionary bereit stellen.
        /// Diese Methode ist mit dem rechten Button (Load Json) in der UI verbunden
        /// </summary>
        public async Task LoadSplitJsonFiles()
        {
            ConcurrentDictionary<string, List<JsonElement>> jsons = new(); //Parallel-sicheres Dictionary
            List<string> countryCodes = Directory.GetFiles("Lab").Select(path => Path.GetFileNameWithoutExtension(path)).ToList();
            //Alle Dateinamen ohne Pfad und Erweiterung

            SetMaxProgress(countryCodes.Count);

            //await ForEachAsync um UI nicht freezen zu lassen
            await Parallel.ForEachAsync(countryCodes, (code, ct) =>
            {
                string path = Path.Combine("Lab", $"{code}.json"); //Einzelnes File angreifen
                string file = File.ReadAllText(path); //File einlesen
                var array = JsonDocument.Parse(file).RootElement;
                jsons.TryAdd(code, array.EnumerateArray().ToList()); //Element hinzufügen (hier mit TryAdd)

                UpdateProgress($"{code} geladen");

                return ValueTask.CompletedTask; //Hier einfach "leeres" Objekt zurückgeben
            });
        }

        //Es gibt keine Methode um aus einer Liste von JsonElements ein JsonArray zu generieren
        private string JsonListToJson(List<JsonElement> jsons)
        {
            return jsons.Aggregate(new StringBuilder("[\n"), (sb, je) =>
                sb.Append('\t')
                  .Append(je.GetRawText())
                  .Append(",\n"))
                  .ToString()
                  .TrimEnd(',', '\n') + "\n]";
        }
    }
}
