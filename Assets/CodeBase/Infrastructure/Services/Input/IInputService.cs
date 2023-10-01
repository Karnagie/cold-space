using CodeBase.Infrastructure.Helpers;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.Input
{
    public interface IInputService
    {
        Vector2 Moving();
        bool Picking { get; }
        bool BombPlacing { get; }
        bool RoomDestroyerPlacing { get; }
        bool LampPlacing { get; }
    }
}