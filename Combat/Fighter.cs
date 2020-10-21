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

    float timeSinceLastAttack = Mathf.Infinity;

    Health target;
    private void Update()
        {
            timeSinceLastAttack +=Time.deltaTime;

            if(target == null) return;

            if(target.IsDead()) return;

            if (!GetInRange())
            {
                GetComponent<Mover>().MoveTo(target.transform.position,1f);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehavior();
            }
        }

        private void AttackBehavior()
        {
            transform.LookAt(target.transform);
            if(timeSinceLastAttack > timebetweenAttacks)
            {
                //This will trigger the Hit() event
                TriggerAttack();
                timeSinceLastAttack = 0;
            }
        }

        private void TriggerAttack()
        {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }

        private bool GetInRange()
        {
            return Vector3.Distance(target.transform.position, transform.position) < weaponRange;
        }
        public bool CanAttack(GameObject combatTarget)
        {
            if(combatTarget == null) return false;
            Health targetToTest = combatTarget.GetComponent<Health>();
            return (targetToTest != null) && !(targetToTest.IsDead());
        }
        public void Attack(GameObject combatTarget)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel()
        {
            StopAttack();
            target = null;
        }

        private void StopAttack()
        {
            //GetComponent<Animator>().SetTrigger("attack");
            GetComponent<Animator>().ResetTrigger("stopAttack");
        }

        //Animation Event
        void Hit()
        {
            if(target !=null)
            {
                target.TakeDamage(weaponDamage);
            }
        }
    }
}