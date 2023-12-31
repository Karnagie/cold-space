﻿using UnityEngine;

namespace CodeBase.Core.Behaviours
{
    public class SpiderLegBehaviour : MonoBehaviour
    {
        public float Length = 1;
        public Transform Transform;
        public Transform Pole;
        public Transform TargetPivot;
        public GameObject[] Ragdoll;
        public Transform DefaultPivot;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(gameObject.transform.position, gameObject.transform.position + Vector3.right*Length);
        }
    }
}