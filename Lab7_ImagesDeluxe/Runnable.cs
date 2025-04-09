using System.Diagnostics;

namespace TPL_ImagesDeluxe;

[DebuggerDisplay("{CurrentTask.Status}, Continue: {Continue}")]
public abstract class Runnable
{
	private bool _continue = false;

	public bool Continue
	{
		get => _continue;
		set
		{
			_continue = value;

			if (value && CurrentTask.Status == TaskStatus.Created)
				CurrentTask.Start();

			if (value && CurrentTask.Status == TaskStatus.RanToCompletion)
				CurrentTask = new Task(Run);
		}
	}

	public Task CurrentTask { get; protected set; }

	protected private abstract void Run();
}