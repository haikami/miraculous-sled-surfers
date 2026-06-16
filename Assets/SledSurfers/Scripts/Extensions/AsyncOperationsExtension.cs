using System.Threading.Tasks;
using UnityEngine;

namespace SledSurfers.Scripts.Extensions
{
    public static class AsyncOperationExtensions
    {
        public static Task AsTask(this AsyncOperation op)
        {
            var tcs = new TaskCompletionSource<bool>();
            op.completed += _ => tcs.SetResult(true);
            return tcs.Task;
        }
    }
}