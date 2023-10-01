using UnityEngine;

namespace CodeBase.Core.Behaviours
{
    public class RoomBehaviour : MonoBehaviour
    {
        public Transform Transform;
        public Collider2D DownCollider;
        public Collider2D UpCollider;
        public Collider2D AllRoomCollider;
        public Transform Center;
        public Transform[] SpawnPoints;
        
        public SpriteRenderer WindowSprite;
        public SpriteRenderer Body;
        public SpriteRenderer Lamp;


        public void DestroyWindow()
        {
            WindowSprite.gameObject.SetActive(true);
            Body.gameObject.SetActive(false);
        }

        public void RoomDestroy()
        {
            WindowSprite.gameObject.SetActive(false);
            Body.gameObject.SetActive(false);
            AllRoomCollider.gameObject.SetActive(true);
        }
    }
}