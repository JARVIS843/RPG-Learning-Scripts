using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour,IAction
{
    [SerializeField] 
    float weaponRange = 2;
    [SerializeField]
    float timebetweenAttacks = 1f;
    [SerializeField]
    float weaponDamage = 5f;

    float timeSinceLastAttack = 0;

    Transform target;
    private void Update()
        {
            timeSinceLastAttack +=Time.deltaTime;

            if(target == null) return;
            if (!GetInRange())
            {
                GetComponent<Mover>().MoveTo(target.position);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehavior();
            }
        }

        private void AttackBehavior()
        {
            if(timeSinceLastAttack > timebetweenAttacks)
            {
                //This will trigger the Hit() event
                GetComponent<Animator>().SetTrigger("attack");
                timeSinceLastAttack = 0;
            }
        }

        private bool GetInRange()
        {
            return Vector3.Distance(target.position, transform.position) < weaponRange;
        }

        public void Attack(CombatTarget combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.transform;
        }

        public void Cancel()
        {
            target = null;
        }

        //Animation Event
        void Hit()
        {
            if(target !=null)
            {
                Health healthComponent = target.GetComponent<Health>();
                healthComponent.TakeDamage(weaponDamage);
            }
        }
    }
}