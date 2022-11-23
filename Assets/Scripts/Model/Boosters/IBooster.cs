using ServiceInstances;

namespace Model.Boosters
{
    public interface IBooster
    {
        BoosterType Type { get; }
        void Use();
    }
}