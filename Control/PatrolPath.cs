using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
        public class PatrolPath : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            const float waypointGizmosRadius = 0.3f;
            //Gizmos.color = Color.gray;
            for (int i = 0; i < transform.childCount; i++)
            {
                int j = GetNextIndex(i);
                Gizmos.DrawSphere(GetWayPoint(i), waypointGizmosRadius);
                Gizmos.DrawLine(GetWayPoint(i),GetWayPoint(j));
            }
        }

        private int GetNextIndex(int i)
        {
            if((i+1)== transform.childCount)
            {
                return 0;
            }
            return i + 1;
        }

        private Vector3 GetWayPoint(int i)
        {
            return transform.GetChild(i).position;
        }
    }

}