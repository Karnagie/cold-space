namespace CodeBase.Core.Models.Systems
{
    public interface IDamager : ISystem
    {
        public void TryDamage();
    }
}