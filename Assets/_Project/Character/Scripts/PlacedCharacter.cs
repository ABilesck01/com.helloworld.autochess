using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedCharacter : MonoBehaviour
{
    public BaseUnit unit;

    private void Awake()
    {
        unit = GetComponentInChildren<BaseUnit>();
    }

    public void SetController(BaseController controller)
    {
        unit.Controller = controller;
        unit.SetTeam(controller.GetTeamTag());
    }

    public void ReadyUnit()
    {
        unit.Init();
    }

    public bool UnitIsDead()
    {
        return unit.isDead;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
