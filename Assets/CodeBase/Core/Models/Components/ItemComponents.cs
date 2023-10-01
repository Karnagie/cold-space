using UnityEngine;

namespace CodeBase.Core.Models.Components
{
    public class ItemComponents
    {
        public Collider2D Collider { get; }

        public ItemComponents(Collider2D collider)
        {
            Collider = collider;
        }
    }
}