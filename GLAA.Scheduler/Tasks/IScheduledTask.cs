using System.Threading;
using System.Threading.Tasks;

namespace GLAA.Scheduler.Tasks
{
    public interface IScheduledTask
    {
        string Schedule { get; }
        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}
