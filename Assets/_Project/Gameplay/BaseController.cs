using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    public enum TeamTag
    {
        blueTeam,
        redTeam
    }

    [SerializeField] private TeamTag teamTag;

    public TeamTag GetTeamTag() { return teamTag; }

    public TeamTag GetEnemyTeamTag()
    {
        if (teamTag == TeamTag.blueTeam)
            return TeamTag.redTeam;

        return TeamTag.blueTeam;
    }

    [SerializeField] protected List<CharacterData> characterDataDeck;

    protected List<CharacterData> GetCharacterDataDeck() => characterDataDeck;

    [SerializeField] protected List<PlacedCharacter> placedUnits;

    public static event EventHandler<TeamTag> OnAllUnitsDeath;

    protected int currentMoney;

    private void OnEnable()
    {
        GameController.OnNextRound += GameController_OnNextRound;
    }

    private void OnDisable()
    {
        GameController.OnNextRound -= GameController_OnNextRound;
    }

    private void GameController_OnNextRound(object sender, TeamTag e)
    {
        placedUnits.Clear();
    }

    public void AddUnit(PlacedCharacter built)
    {
        if(placedUnits == null)
            placedUnits = new List<PlacedCharacter>();

        placedUnits.Add(built);
    }

    protected GridBuilder gridBuilder;

    public void ReadyUnits()
    {
        foreach (PlacedCharacter item in placedUnits)
        {
            item.ReadyUnit();
        }
    }

    public void CheckUnitsDeath()
    {
        bool hasUnitAlive = false;
        foreach (PlacedCharacter item in placedUnits)
        {
            if(!item.UnitIsDead())
            {
                hasUnitAlive = true;
            }
        }

        if (hasUnitAlive) return;

        OnAllUnitsDeath?.Invoke(this, teamTag);
        return;
    }

    public virtual void AddMoney(int amount)
    {
        currentMoney += amount;
    }

    public virtual bool CheckMoney(int amount)
    {
        return currentMoney >= amount;
    }

    public virtual void SpendMoney(int amount)
    {
        if(!CheckMoney(amount)) return;

        currentMoney -= amount;
    }
}
