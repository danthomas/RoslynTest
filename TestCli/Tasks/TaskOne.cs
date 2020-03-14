using CommandLineInterface;

namespace TestCli.Tasks
{
    public class TaskOne : ITask
    {
        private readonly IConsole _console;

        public TaskOne(IConsole console)
        {
            _console = console;
        }

        public void Run()
        {
            _console.WriteLine("Running TaskOne");
        }
    }


    public class ClearConsole : ITask
    {
        private readonly IConsole _console;

        public ClearConsole(IConsole console)
        {
            _console = console;
        }

        public void Run()
        {
            _console.Clear();
        }
    }

    public class Explore : ITask
    {
        private readonly IConsole _console;

        public Explore(IConsole console)
        {
            _console = console;
        }

        public void Run()
        {
            _console.WriteLine("Running Explore");
        }
    }

    public class EditCode : ITask
    {
        private readonly IConsole _console;

        public EditCode(IConsole console)
        {
            _console = console;
        }

        public void Run()
        {
            _console.WriteLine("Running EditCode");
        }
    }

    public class EditVisualStudio : ITask
    {
        private readonly IConsole _console;

        public EditVisualStudio(IConsole console)
        {
            _console = console;
        }

        public void Run()
        {
            _console.WriteLine("Running EditVisualStudio");
        }
    }

    public class GitResetHardHead : ITask
    {
        private readonly IConsole _console;

        public GitResetHardHead(IConsole console)
        {
            _console = console;
        }

        public void Run()
        {
            _console.WriteLine("Running GitResetHardHead");
        }
    }

    public class GitCommitWip : ITask
    {
        private readonly IConsole _console;

        public GitCommitWip(IConsole console)
        {
            _console = console;
        }

        public void Run()
        {
            _console.WriteLine("Running GitCommitWip");
        }
    }
    public class GitCommit : ITask
    {
        private readonly IConsole _console;

        public GitCommit(IConsole console)
        {
            _console = console;
        }

        public void Run()
        {
            _console.WriteLine("Running GitCommit");
        }
    }
    public class GitCheckoutMaster : ITask
    {
        private readonly IConsole _console;

        public GitCheckoutMaster(IConsole console)
        {
            _console = console;
        }

        public void Run()
        {
            _console.WriteLine("Running GitCheckoutMaster");
        }
    }
    public class GitCheckout : ITask
    {
        private readonly IConsole _console;

        public GitCheckout(IConsole console)
        {
            _console = console;
        }

        public void Run()
        {
            _console.WriteLine("Running GitCheckout");
        }
    }
}