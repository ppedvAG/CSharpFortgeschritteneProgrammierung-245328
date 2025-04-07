namespace HelloEvents
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var course = new Course();
            course.CourseStarted += (sender, e) => Console.WriteLine("Kurs gestartet");
            course.CourseFinished += (sender, e) =>
            {
                Console.WriteLine("Kurs beendet");
                Console.WriteLine(e.contents);
            };

            Console.WriteLine("Press any key to start the course");
            Console.ReadKey();
            course.Start();
        }
    }

    public class Course
    {
        public record CourseFinishedEventArgs(string contents);

        public event EventHandler CourseStarted;

        public event EventHandler<CourseFinishedEventArgs> CourseFinished;

        public void Start()
        {
            // Wenn wir ein Event aufrufen, uebergeben wir den "sender"
            // welches der this Kontext der Klasse ist (Best Practice)
            CourseStarted?.Invoke(this, EventArgs.Empty);

            // 2 Sekunden warten
            Thread.Sleep(2000);

            var args = new CourseFinishedEventArgs("Jetzt ist der Kurs fertig");
            CourseFinished?.Invoke(this, args);
        }

        // Vsliznz@ppedv.onmicrosoft.com
        // (Only!4VS]
    }
}
