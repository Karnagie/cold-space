using UnityEngine;

namespace CodeBase.Core.Models.Systems
{
    public interface IPhysicBody : ISystem
    {
        void Push(Vector2 force, ForceMode2D forceMode);
        Vector2 ClosestPointTo(Vector2 position);
        bool OverlapLine(Vector3 start, Vector3 end, out Vector3 point);
        void IgnoreCollisionWith(Collider2D collider, bool enable);
    }
}