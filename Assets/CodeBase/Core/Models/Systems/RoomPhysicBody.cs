using CodeBase.Infrastructure.Factories;
using UnityEngine;

namespace CodeBase.Core.Models.Systems
{
    public class RoomPhysicBody : IPhysicBody
    {
        private Collider2D _upCollider;
        private Collider2D _downCollider;
        private RoomMoveDisable _roomMoveDisable;

        public RoomPhysicBody(Collider2D upCollider, Collider2D downCollider, RoomMoveDisable roomMoveDisable)
        {
            _roomMoveDisable = roomMoveDisable;
            _upCollider = upCollider;
            _downCollider = downCollider;
            if (_roomMoveDisable == RoomMoveDisable.Up)
            {
                upCollider.usedByEffector = false;
            }
        }
        
        public void Push(Vector2 force, ForceMode2D forceMode)
        {
            return;
        }

        public Vector2 ClosestPointTo(Vector2 position)
        {
            var colliders = new Collider2D[] {_upCollider, _downCollider};
            var closestPosition = colliders[0].ClosestPoint(position);
            foreach (var collider in colliders)
            {
                var point = collider.ClosestPoint(position);
                if (Vector3.Distance(position, point) 
                    < 
                    Vector3.Distance(position, closestPosition))
                {
                    closestPosition = point;
                }
            }

            return closestPosition;
        }

        public bool OverlapLine(Vector3 start, Vector3 end, out Vector3 point)
        {
            point = default;
            var colliders = new Collider2D[] {_upCollider, _downCollider};
            RaycastHit2D[] bounds = new RaycastHit2D[10];
            var size = Physics2D.LinecastNonAlloc(start, end, bounds);
            foreach (var collider in colliders)
            {
                foreach (var bound in bounds)
                {
                    if (bound.collider == collider)
                    {
                        point = bound.point;
                        return true;
                    }
                }
            }
            
            return false;
        }

        public void IgnoreCollisionWith(Collider2D collider, bool enable)
        {
            var colliders = new Collider2D[] {_upCollider, _downCollider};
            if (_roomMoveDisable == RoomMoveDisable.Down)
            {
                colliders =  new Collider2D[] {_upCollider};
            }else if (_roomMoveDisable == RoomMoveDisable.Up)
            {
                colliders =  new Collider2D[] {_downCollider};
            }
            
            foreach (var collider2D in colliders)
            {
                Physics2D.IgnoreCollision(collider2D, collider, enable);
            }
        }
    }
}