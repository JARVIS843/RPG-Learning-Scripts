using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour,IAction
{
    public float weaponRange;

    Transform target;
    private void Update()
        {
            if(target == null) return;
            if (!GetInRange())
            {
                GetComponent<Mover>().MoveTo(target.position);
            }
            else
            {
                 GetComponent<Mover>().Cancel();
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
}
}