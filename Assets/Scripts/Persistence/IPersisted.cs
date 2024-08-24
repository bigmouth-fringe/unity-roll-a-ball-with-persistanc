namespace Persistence
{
    public interface IPersisted
    {
        void Load(GameState state);

        void Save(ref GameState state);
    }
}
