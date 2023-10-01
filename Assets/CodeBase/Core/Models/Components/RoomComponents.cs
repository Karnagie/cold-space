using UnityEngine;

namespace CodeBase.Core.Models.Components
{
    public class RoomComponents
    {
        public readonly Collider2D DownCollider;
        public readonly Collider2D UpCollider;
        public readonly Transform[] SpawnPoints;

        public readonly Transform Transform;

        public readonly Transform Center;
        public readonly SpriteRenderer Lamp;


        public RoomComponents(Collider2D downCollider, Transform transform, Transform center, Collider2D upCollider,
            Transform[] spawnPoints, SpriteRenderer lamp)
        {
            DownCollider = downCollider;
            UpCollider = upCollider;
            SpawnPoints = spawnPoints;
            Transform = transform;
            Center = center;
            Lamp = lamp;
        }
    }
}