using System.Collections.Concurrent;

namespace ThreadConcurrency
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Threading Demo - Bitte Option wählen:");
            Console.WriteLine("\t[1] Threads parallel abarbeiten");
            Console.WriteLine("\t[2] Threads sequentiell mit lock");
            Console.WriteLine("\t[3] Exceptions fangen (Monitor)");
            Console.WriteLine("\t[4] ManualResetEvent: Warten auf schreibenden Thread");
            Console.WriteLine("\t[5] AutoResetEvent: Sequentielle Abarbeitung");
            Console.WriteLine("\t[6] Mutex: Systemweite Sperre und sequentielle Abarbeitung");
            Console.WriteLine("\t[7] Semaphore: mehrere schreibende Threads");
            Console.WriteLine("\t[8] Concurrent Collections");
            Console.WriteLine("\t[0] Beenden");

            var input = int.TryParse(Console.ReadLine(), out int result) ? result : 0;
            switch (input)
            {
                case 1:
                    // 1. Threads werden alle parallel abgearbeitet
                    SpawnNewThreads(DoWork);
                    break;
                case 2:
                    // 2. Mit Lock werden die Threads sequentiell abgearbeitet
                    SpawnNewThreads(DoWorkUsingLock);
                    break;
                case 3:
                    // 3. Exceptions fangen um andere Threads nicht zu blockeren
                    SpawnNewThreads(DoWorkUsingMonitor);
                    break;
                case 4:
                    // 4. Wenn auf ein schreibenden Thread gewartet werden soll
                    SpawnNewThreadsWithManualReset();
                    break;
                case 5:
                    // 5. Wenn schreibende Threads sequentiell abgearbeitet werden sollen
                    SpawnNewThreadsWithAutoReset();
                    break;
                case 6:
                    // 6. Wenn schreibende Threads sequentiell abgearbeitet werden sollen
                    SpawnNewThreadsWithMutex();
                    break;
                case 7:
                    // 7. Wenn mehrere Threads schreiben duerfen, wird eine Semaphore verwendet
                    SpawnNewThreadsWithSemaphore();
                    break;
                case 8:
                    // 8. Concurrent Collections
                    ConcurrentCollectionSamples();
                    break;
                default:
                    return;
            }

            Console.ReadLine();
        }

        private static void SpawnNewThreads(Action action, int count = 10)
        {
            for (int i = 0; i < count; i++)
            {
                new Thread(() => action()).Start();
            }
        }

        private static void DoWork()
        {
            Console.WriteLine("Thread {0} started", Thread.CurrentThread.ManagedThreadId);
            Thread.Sleep(1000);
            Console.WriteLine("Thread {0} finished", Thread.CurrentThread.ManagedThreadId);
        }

        #region 2. Lock

        private static object _lock = new object();

        private static void DoWorkUsingLock()
        {
            lock (_lock)
            {
                try
                {
                    Console.WriteLine("Thread mit lock {0} started", Thread.CurrentThread.ManagedThreadId);
                    Thread.Sleep(1000);

                    if (Random.Shared.Next(0, 2) == 0)
                    {
                        throw new Exception();
                    }

                    Console.WriteLine("Thread mit lock {0} finished", Thread.CurrentThread.ManagedThreadId);
                }
                catch (Exception)
                {
                    Console.WriteLine("Exception in lock gefangen");
                }
            }
        }

        #endregion

        #region 3. Lock mit Monitor
        private static void DoWorkUsingMonitor()
        {
            // Monitor ist im Gegensatz zu lock eine .NET Klasse und bietet bessere Performance als auch Kontrolle wenn die Synchronisation komplexer wird
            try
            {
                Monitor.Enter(_lock);

                Console.WriteLine("Thread mit Monitor {0} started", Thread.CurrentThread.ManagedThreadId);
                Thread.Sleep(1000);

                if (Random.Shared.Next(0, 2) == 0)
                {
                    throw new Exception();
                }

                Console.WriteLine("Thread mit Monitor {0} finished", Thread.CurrentThread.ManagedThreadId);
            }
            catch
            {
                // Exception loggen
                Console.WriteLine("Exception mit Monitor gefangen");
            }
            finally
            {
                Monitor.Exit(_lock);
            }
        }
        #endregion

        #region 4. Manual Reset Event

        static readonly ManualResetEvent _manualResetEvent = new ManualResetEvent(false); // false == non-signaled

        private static void SpawnNewThreadsWithManualReset()
        {
            // ein schreibender Thread
            new Thread(WriteSomething).Start();

            // 10 lesende Threads
            SpawnNewThreads(ReadThatSomething);
        }

        private static void ReadThatSomething()
        {
            Console.WriteLine("Thread {0} reading", Thread.CurrentThread.ManagedThreadId);

            // Der lock wird hier sozusagen abgefragt
            _manualResetEvent.WaitOne();

            Thread.Sleep(1000);
            Console.WriteLine("Thread {0} reading completed", Thread.CurrentThread.ManagedThreadId);
        }

        private static void WriteSomething(object? obj)
        {
            Console.WriteLine("Thread {0} writing", Thread.CurrentThread.ManagedThreadId);
            _manualResetEvent.Reset();
            Thread.Sleep(5000);
            Console.WriteLine("Thread {0} writing completed", Thread.CurrentThread.ManagedThreadId);
            _manualResetEvent.Set();
        }

        #endregion

        #region 5. Auto Reset Event

        static readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(true); // true damit der erste Thread den Anfang machen kann

        private static void SpawnNewThreadsWithAutoReset()
        {
            // 5 schreibende Threads
            SpawnNewThreads(WriteSomethingDifferent, 5);

            // Auf keinen Fall sollte Set() vom Main Thread gesetzt werden
            //Thread.Sleep(5000);
            //_autoResetEvent.Set();
        }

        private static void WriteSomethingDifferent()
        {
            Console.WriteLine("Thread {0} waiting...", Thread.CurrentThread.ManagedThreadId);
            _autoResetEvent.WaitOne();

            Console.WriteLine("Thread {0} writing...", Thread.CurrentThread.ManagedThreadId);
            Thread.Sleep(2000);
            Console.WriteLine("Thread {0} writing completed", Thread.CurrentThread.ManagedThreadId);
            _autoResetEvent.Set();

        }

        #endregion

        #region 6. Mutex

        static readonly Mutex _mutex = new Mutex(true); // true damit der erste Thread den Anfang machen kann

        private static void SpawnNewThreadsWithMutex()
        {
            // Unterschied zum AutoResetEvent ist, dass Release eine ApplicationException wirft
            // "Object synchronization method was called from an unsznchronized bock of code"
            SpawnNewThreads(WriteSomethingWithMutex, 5);

            _mutex.ReleaseMutex();
        }

        private static void WriteSomethingWithMutex()
        {
            Console.WriteLine("Thread {0} waiting...", Thread.CurrentThread.ManagedThreadId);
            _mutex.WaitOne();

            Console.WriteLine("Thread {0} writing...", Thread.CurrentThread.ManagedThreadId);
            Thread.Sleep(2000);
            Console.WriteLine("Thread {0} writing completed", Thread.CurrentThread.ManagedThreadId);
            _mutex.ReleaseMutex();
        }

        #endregion

        #region 7. Semaphore

        static readonly Semaphore _semaphore = new Semaphore(initialCount: 1, maximumCount: 3);

        private static void SpawnNewThreadsWithSemaphore()
        {
            SpawnNewThreads(WriteSomethingWithSemaphore, 5);

            // Release() stoert hier nicht
            Thread.Sleep(5000);
            _semaphore.Release();
        }
        private static void WriteSomethingWithSemaphore()
        {
            Console.WriteLine("Thread {0} waiting...", Thread.CurrentThread.ManagedThreadId);
            _semaphore.WaitOne();

            Console.WriteLine("Thread {0} writing...", Thread.CurrentThread.ManagedThreadId);
            Thread.Sleep(2000);
            Console.WriteLine("Thread {0} writing completed", Thread.CurrentThread.ManagedThreadId);
            _semaphore.Release();
        }

        #endregion

        #region 8. Concurrent Collections
        private static void ConcurrentCollectionSamples()
        {
            // Sample 1
            ConcurrentBag<string> bag = ["Heiz", "Hugo", "Klara"];
            //bag[0] // Index nicht moeglich

            // Daten nur mit Linq zugaenglich
            var query = from s in bag select s;
            foreach (var item in query)
            {
                Console.WriteLine(item);
            }

            // Sample 2
            ConcurrentDictionary<string, int> dict = new();
            dict.TryAdd("Hugo", 42);
            dict.TryAdd("Heiz", 73);

            int updateClause(string key, int oldValue) => oldValue + 1;
            dict.AddOrUpdate("Klara", 8, updateClause);

            // Versuche einen Wert heraus zu holen, falls ein anderer Thread ihn nicht entfernt hat
            bool success = dict.TryGetValue("Klara", out int value);

            dict.GetOrAdd("Hugo", (key) => value);
        }
        #endregion
    }
}
