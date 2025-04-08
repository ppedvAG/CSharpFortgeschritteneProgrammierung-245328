using System.Reflection;
using System.Runtime.Loader;
using CalcUI = Calculator.App.Calculator;

string PluginPath = Path.Combine(Environment.CurrentDirectory, "Plugins");

CopyAssembliesToPluginDirectory(PluginPath, "Calculator.ArithmeticPlugins");

var plugins = LoadPlugins<IOperation>(PluginPath);

if (!plugins.Any())
    throw new ApplicationException($"Keine PlugIns mit {nameof(IOperation)} gefunden");

CalcUI.Run(plugins.ToArray());

static void CopyAssembliesToPluginDirectory(string pluginPath, params string[] locations)
{
    string assemblyName = Assembly.GetExecutingAssembly().GetName().Name;

    if (!Directory.Exists(pluginPath))
        Directory.CreateDirectory(pluginPath);

    foreach (string location in locations)
    {
        string sourcePath = Environment.CurrentDirectory.Replace(assemblyName, location);
        if (Directory.Exists(sourcePath))
        {
            foreach (var dll in Directory.GetFiles(sourcePath, "*.dll"))
            {
                string target = Path.Combine(pluginPath, Path.GetFileName(dll));
                File.Copy(dll, target, true);
            }
        }
        else
        {
            throw new DirectoryNotFoundException(location);
        }
    }
}

static IEnumerable<T> LoadPlugins<T>(string pluginPath) where T : class
{
    var context = new AssemblyLoadContext(null);
    return Directory.GetFiles(pluginPath, "*.dll")
        .Select(context.LoadFromAssemblyPath)
        .SelectMany(a => a.ExportedTypes)
        .Where(t => typeof(T).IsAssignableFrom(t))
        .Select(t => (T)Activator.CreateInstance(t));
}