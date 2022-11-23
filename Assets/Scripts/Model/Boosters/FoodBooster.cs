using ServiceInstances;


namespace Model.Boosters
{
    public class FoodBooster : IBooster
    {
        public BoosterType Type => BoosterType.Food;
        

        public void Use()
        {
            throw new System.NotImplementedException();
        }
    }
}