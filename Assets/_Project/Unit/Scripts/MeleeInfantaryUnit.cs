using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeInfantaryUnit : BaseUnit
{
    private NavMeshAgent agent;
    private Animator animator;

    private BaseUnit target;
    private bool hasAttacked = false;

    public override void Start()
    {
        base.Start();

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        agent.speed = unitData.speed;
        agent.stoppingDistance = unitData.stopDistance;
    }

    public override void Init()
    {
        //base.Init();
        GetEnemies();
        GetNewTarget();
        canAttack = true;
    }

    private void Update()
    {
        if (!canAttack) return;
        if(isDead) return;

        Move();
        Animation();
        Attack();
    }

    private void Move()
    {
        if (target == null) return;
        agent.SetDestination(target.transform.position);
        transform.LookAt(target.transform.position);
    }

    private void Animation()
    {
        animator.SetFloat("speed", agent.velocity.magnitude);
    }

    private void Attack()
    {
        if(target == null) return;

        if (agent.remainingDistance >= unitData.stopDistance) return;

        if(Vector3.Distance(transform.position, target.transform.position) > unitData.stopDistance) return;

        if (hasAttacked) return;

        hasAttacked = true;
        animator.SetTrigger("attack");
        Invoke(nameof(ResetAttack), unitData.timeBtwAttacks);
    }

    private void ResetAttack()
    {
        hasAttacked = false;
    }

    public void AttackEvent()
    {
        if (target == null) return;

        target.TakeDamage(unitData.damage, () =>
        {
            GetNewTarget();
        });
    }

    private void GetNewTarget()
    {
        BaseUnit tempTarget = null;
        Vector3 myPosition = transform.position;
        float minDistance = Mathf.Infinity;
        foreach (var unit in unitList)
        {
            if(unit.isDead) continue;
            float distance = Vector3.Distance(unit.transform.position, myPosition);
            if (distance < minDistance)
            {
                tempTarget = unit;
                minDistance = distance;
            }
        }

        target = tempTarget;
    }

}
