﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour,IAction
    {
        
        [SerializeField] float maxSpeed = 6f;
        NavMeshAgent agent;

        Health health;
        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            health = GetComponent<Health>();
        }
        // Update is called once per frame
        void Update()
        {
            agent.enabled = !health.IsDead();
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination,speedFraction);
        }

        public void MoveTo(Vector3 destination, float speedFractoin)
        {
            agent.destination = destination;
            agent.speed = maxSpeed * Mathf.Clamp01(speedFractoin);
            agent.isStopped = false;
        }

        public void Cancel()
        {
            agent.isStopped = true;
        }

        public void UpdateAnimator()
        {
            Vector3 velocity = agent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("forwardSpeed", speed);
        }
    }
}
