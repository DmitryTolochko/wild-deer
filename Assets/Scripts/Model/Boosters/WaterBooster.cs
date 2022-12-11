using ServiceInstances;


namespace Model.Boosters
{
    public class WaterBooster : IBooster
    {
        public BoosterType Type => BoosterType.Water;


        public void Use()
        {
            throw new System.NotImplementedException();
        }
    }
}