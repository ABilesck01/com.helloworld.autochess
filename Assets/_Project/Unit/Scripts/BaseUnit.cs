using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseUnit : MonoBehaviour
{
    [SerializeField] protected UnitData unitData;

    [SerializeField] protected SpriteRenderer teamViewer;

    [SerializeField] private BaseController controller;

    public bool canAttack = false;
    
    [SerializeField] protected List<BaseUnit> unitList = new List<BaseUnit>();

    public bool isDead = false;
    private int currentHealth;

    public BaseController Controller { get => controller; set => controller = value; }

    public virtual void Start()
    {
        currentHealth = unitData.health;
        
    }

    public void SetTeam(BaseController.TeamTag team)
    {
        gameObject.tag = team.ToString();
        if (team.Equals(BaseController.TeamTag.redTeam))
            teamViewer.color = Color.red;
        else
            teamViewer.color = Color.blue;
    }

    protected void GetEnemies()
    {
        unitList.Clear();
        var enemyUnits = GameObject.FindGameObjectsWithTag(controller.GetEnemyTeamTag().ToString());
        foreach (var unit in enemyUnits)
        {
            if(unit.TryGetComponent<BaseUnit>(out BaseUnit baseUnit))
            {
                if (baseUnit.isDead) continue;
                unitList.Add(baseUnit);
            }
        }
    }

    public void TakeDamage(int amount, Action OnDeath = null)
    {
        if (isDead)
        {
            OnDeath?.Invoke();
            return;
        }

        var rand = UnityEngine.Random.value;

        if(rand > unitData.avoidAttack)
        {
            return;
        }

        currentHealth -= amount;
        if(currentHealth <= 0)
        {
            isDead = true;
            OnDeath?.Invoke();
            controller.CheckUnitsDeath();
            GetComponent<Animator>().SetTrigger("isDead");
        }
    }

    public virtual void Init()
    {
        GetEnemies();
        canAttack = true;
    }


}
