using SledSurfers.Scripts.Data.Models;

namespace SledSurfers.Scripts.Managers
{
    public class RunResultManager
    {
        public RunResultData LastRunResultData { get; private set; }

        public void Clear() => LastRunResultData = new RunResultData();
        
        public void SetLastRunResultData(RunResultData runResultData)
        {
            LastRunResultData = runResultData;
        }
    }
}