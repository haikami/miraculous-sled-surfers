using SledSurfers.Scripts.Core;
using SledSurfers.Scripts.Data.Models;

namespace SledSurfers.Scripts.Events
{
    public class GameStateChannel : Channel<GameState> { }
    public class RunChannel : Channel<RunResultData> { }
    public class CurrencyChannel : Channel<int> { }
    public class LevelLoadedChannel : Channel { }  
}