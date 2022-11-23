using ServiceInstances;

namespace Model.Boosters
{
    public class PinkTrapBooster : IBooster
    {
        public BoosterType Type => BoosterType.PinkTrap;
        

        public void Use()
        {
            throw new System.NotImplementedException();
        }
    }
}