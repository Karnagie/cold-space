namespace CodeBase.Core.Models.Systems
{
    public interface IDamageReceiver : ISystem
    {
        void GetDamage(int value);
    }
}