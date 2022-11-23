using ServiceInstances;

namespace Model.Boosters
{
    public class ProtectiveCapBooster : IBooster
    {
        public BoosterType Type => BoosterType.ProtectiveCap;


        public void Use()
        {
            throw new System.NotImplementedException();
        }
    }
}