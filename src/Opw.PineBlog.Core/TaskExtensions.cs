using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Opw.PineBlog
{
    public static class TaskExtensions
    {
        public static TaskAwaiter<(T1, T2)> GetAwaiter<T1, T2>(this (Task<T1> t1, Task<T2> t2) t)
        {
            return Task.Run(async () => (await t.t1, await t.t2)).GetAwaiter();
        }
    }
}
