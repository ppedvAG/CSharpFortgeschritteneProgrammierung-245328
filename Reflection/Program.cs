using BusinessLogic.Attributes;
using BusinessLogic.Contracts;
using BusinessLogic.Services;
using Serialization.Data;
using System.Reflection;
using System.Text;

namespace Reflection
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Reflection arbeitet mit Typen
            // d.h. zwei Moeglichkeiten
            Car vehicle = Car.Generate(1).First();
            Type carType = vehicle.GetType();

            // oder besser, weil keine potentielle NullReferenceException
            carType = typeof(Car);
            string vehicleInfo = PrintProperties(vehicle, p => $"{p.Name,-12} {p.GetValue(vehicle)}");
            Console.WriteLine(vehicleInfo);

            Console.WriteLine("Member-Name\tMember-Type\tTyp zur Laufzeit\tTyp zur Compilezeit");
            var memberInfo = PrintMembers(new VehicleService(), m => $"{m.Name,-20} {m.MemberType}\t{m.ReflectedType.Name}   {m.DeclaringType.Name,-8}");
            Console.WriteLine(memberInfo);

            // aktuelles Projekt
            _ = Assembly.GetExecutingAssembly();

            var serializationAssembly = Assembly.GetAssembly(typeof(Car));

            // Alternativ DLL aus bin Verzeichnis laden
            var currentPath = Environment.CurrentDirectory;
            var businessLogicPath = Path.Combine(currentPath.Replace(nameof(Reflection), "BusinessLogic"), "BusinessLogic.dll");
            if (!File.Exists(businessLogicPath))
                throw new FileNotFoundException(businessLogicPath);

            // Im Debugger anschauen
            var businessLogicAssemblyFromDll = Assembly.LoadFrom(businessLogicPath);

            Console.WriteLine("\nService anhand von einem Attribut erstellen");

            var serviceTypes = businessLogicAssemblyFromDll.GetTypes()
                .Where(t => t.GetCustomAttribute<ServiceAttribute>() != null)
                .ToList();

            var service = serviceTypes.Select(type =>
            {
                var attr = type.GetCustomAttribute<ServiceAttribute>();
                Console.WriteLine($"{attr.Order} {type.Name}: {attr.Description}");

                var service = (IVehicleService)Activator.CreateInstance(type);
                service.ShowInfo();

                Console.WriteLine("\n\nPrivate readonly Fields von VehicleService setzen");
                var privateFields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

                privateFields[0].SetValue(service, "___Infiltrated___");

                service.ShowInfo();

                return service;

            }).First();

            EventHandler<DataChangedEventArgs<Car>> customHandler = (sender, args) => Console.WriteLine("Custom Handler: {0}", args.NewValue);

            typeof(VehicleService).GetEvent(nameof(VehicleService.DataChanged))
                .AddEventHandler(service!, customHandler);

            Console.WriteLine($"\tDavor:\t {service.Data[2]}");
            var updateMethod = typeof(VehicleService).GetMethod(nameof(VehicleService.Update));
            updateMethod.Invoke(service, [2, vehicle]);

            Console.WriteLine($"\tDanach:\t {service.Data[2]}");

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        private static string PrintProperties<T>(T obj, Func<PropertyInfo, string> formatter)
        {
            return typeof(T)
                .GetProperties()
                .Aggregate(new StringBuilder(), (sb, p) => sb.AppendLine(formatter(p)))
                .ToString();
        }

        private static string PrintMembers<T>(T obj, Func<MemberInfo, string> formatter)
        {
            return typeof(T)
                .GetMembers()
                .Aggregate(new StringBuilder(), (sb, p) => sb.AppendLine(formatter(p)))
                .ToString();
        }
    }
}
