using CodeBase.Data;

namespace CodeBase.Infrastructure.Services.PersistentProgress
{
    public interface IPersistentProgressService
    {
        public IPlayerProgress Progress { get; set; }
    }
}