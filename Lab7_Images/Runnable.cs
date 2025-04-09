using System.Diagnostics;

namespace TPL_Uebung;

[DebuggerDisplay("{CurrentTask.Status}, Continue: {Continue}")]
public abstract class Runnable
{
    public bool Continue { get; set; } = true;

    public void Start()
    {
        if (Continue)
        {
            if (CurrentTask.Status == TaskStatus.Created)
                CurrentTask.Start();

            if (CurrentTask.Status == TaskStatus.RanToCompletion)
                CurrentTask = new Task(Run);
        }
    }

    public Task CurrentTask { get; protected set; }

	protected abstract void Run();
}