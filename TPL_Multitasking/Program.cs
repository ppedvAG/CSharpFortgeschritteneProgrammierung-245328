
namespace TPL_Multitasking
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //CreateTaskSamples();

            //CreateTaskWaitSamples();

            //CreateTaskAndCancellationToken();

            //TaskExceptionHandlingSample();

            //TaskContinuationSamples();

            ParallelSamples();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private static void ParallelSamples()
        {
            Parallel.Invoke(Count, Count, Count, () => Console.Beep());

            static void Count()
            {
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: {i}");
                }
            }
        }

        #region Tasks erstellen
        private static void CreateTaskSamples()
        {
            static void GetRandomNumber100() => GetRandomNumber(100);

            static int GetRandomNumber(object max)
            {
                // 1 sec warten
                Thread.Sleep(1000);

                int number = Random.Shared.Next(0, (int)max);
                Console.WriteLine($"Random number {number} from thread {Thread.CurrentThread.ManagedThreadId}");
                return number;
            }

            var task = new Task(GetRandomNumber100);
            task.Start();

            // ab .Net 4.0
            Task.Factory.StartNew(GetRandomNumber100);

            // ab .Net 4.5
            Task.Run(GetRandomNumber100);

            var taskWithArgs = new Task<int>(GetRandomNumber, 42);
            taskWithArgs.Start();

            // Auf Rueckgabewert warten
            var result = taskWithArgs.ConfigureAwait(false).GetAwaiter().GetResult();

            if (taskWithArgs.IsCompleted)
            {
                Console.WriteLine($"Task with args completed with {result} [id={Thread.CurrentThread.ManagedThreadId}]");
            }
        }
        #endregion

        #region Task Wait All/Any
        private static void CreateTaskWaitSamples()
        {
            var someTasks = CreateTasks((i) =>
            {
                Thread.Sleep(1000);
                Console.WriteLine($"Task {i} [id={Thread.CurrentThread.ManagedThreadId}]");
            });

            Console.WriteLine("Warten bis alle Tasks abgearbeitet wurden");
            Task.WaitAll(someTasks.ToArray());

            Console.WriteLine("Warten bis mindestens ein Tasks abgearbeitet wurde");
            Task.WaitAny(someTasks.ToArray());


        }

        static IEnumerable<Task> CreateTasks(Action<object?> action, int count = 10)
        {
            for (int i = 0; i < count; i++)
            {
                yield return Task.Factory.StartNew(action, i);
            }
        } 
        #endregion

        #region Task Cancellation
        private static void CreateTaskAndCancellationToken()
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token; // struct - Source kann beliebig viele Tokens erzeugen
            var task = new Task(RunTaskWithCancellationToken, token);

            task.Start();

            Thread.Sleep(500);

            cts.Cancel();


            static void RunTaskWithCancellationToken(object arg)
            {
                if (arg is CancellationToken token)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        if (token.IsCancellationRequested)
                        {
                            token.ThrowIfCancellationRequested();
                        }

                        Thread.Sleep(100);
                        Console.WriteLine($"Task {i} [id={Thread.CurrentThread.ManagedThreadId}]");
                    }
                }
            }
        }
        #endregion

        private static void TaskExceptionHandlingSample()
        {
            try
            {
                var tasks = CreateTasks((i) =>
                {
                    Thread.Sleep(500);
                    throw new ApplicationException("Langweilige Ausnahme aufgetreten!");
                }, 3);

                Task.WaitAll(tasks.ToArray());
            }
            catch (AggregateException aggregates)
            {
                foreach (var ex in aggregates.InnerExceptions)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        #region Tasks verketten
        private static void TaskContinuationSamples()
        {
            var task = new Task(() =>
            {
                Console.WriteLine("Task started");
                Thread.Sleep(1000);
                Console.WriteLine("Task finished");

                if (Random.Shared.Next(0, 2) == 0)
                {
                    throw new Exception("Gerade Zahl erwischt!");
                }
            });

            task.ContinueWith(t => Console.WriteLine("Okay"), 
                TaskContinuationOptions.NotOnFaulted);
            task.ContinueWith(t => Console.WriteLine("Always"));
            task.ContinueWith(t => Console.WriteLine("Faulted"), 
                TaskContinuationOptions.OnlyOnFaulted);

            task.Start();

            var taskWithResults = new Task<IEnumerable<int>>(() =>
            {
                Console.WriteLine("Task started");
                Thread.Sleep(1000);
                Console.WriteLine("Task finished");

                return [12, 23, 34];
            });

            taskWithResults.ContinueWith(t =>
            {
                foreach (var item in t.Result)
                {
                    Console.WriteLine("previous results: " + item);
                }
            });

            taskWithResults.Start();
        } 
        #endregion
    }
}
