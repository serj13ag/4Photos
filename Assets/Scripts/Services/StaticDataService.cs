using ScriptableObjects;

namespace Services
{
    public class StaticDataService
    {
        public LevelStaticData[] Levels { get; }

        public StaticDataService(LevelStaticData[] levelsStaticData)
        {
            Levels = levelsStaticData;
        }
    }
}