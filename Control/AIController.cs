using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspiciontime = 3f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1.0f;
    
        Fighter fighter;

        Health health;
        GameObject player;

        Vector3 guardPosition;

        float timeSinceLastSawPlayer = Mathf.Infinity;
        private void Start()
        {
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            player = GameObject.FindWithTag("Player");
            guardPosition = transform.position;
        }
        private void Update()
        {
            if(health.IsDead()) return;

            if (InAttackRangeOfPlayer(player) && fighter.CanAttack(player))
            {
                timeSinceLastSawPlayer = 0;
                AttackBehaviour();
            }
            else if(timeSinceLastSawPlayer < suspiciontime)
            {
                //Suspicion state
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }
            timeSinceLastSawPlayer +=Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition;

            if(patrolPath != null)
            {
                if(AtWayPoint())
                {
                    CycleWayPoint();
                }
                nextPosition = GetCurrentWayPoint();
            }

            GetComponent<Mover>().StartMoveAction(nextPosition);
        }

        private Vector3 GetCurrentWayPoint()
        {
            throw new NotImplementedException();
        }

        private void CycleWayPoint()
        {
            throw new NotImplementedException();
        }

        private bool AtWayPoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position,GetCurrentWayPoint());
            return distanceToWaypoint < waypointTolerance;
        }

        private void SuspicionBehaviour()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            fighter.Attack(player);
        }

        private bool InAttackRangeOfPlayer(GameObject player)
        {
            return Vector3.Distance(transform.position, player.transform.position)<=chaseDistance;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position,chaseDistance);
        }
    }

}