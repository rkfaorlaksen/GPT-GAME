namespace MilkyWayStation.Core.Save
{
    public interface ISaveService
    {
        GameState LoadGameState();
        void SaveGameState(GameState state);
    }
}
