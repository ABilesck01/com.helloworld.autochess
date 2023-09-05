using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private BotController botController;
    [Space]
    [SerializeField] private GameView gameView;
    [Space]
    [SerializeField] private int maxRounds = 3;

    private int redTeamPoints = 0;
    private int blueTeamPoints = 0;

    private int completedRoundCount = 0;

    public static event EventHandler<BaseController.TeamTag> OnFinishGame;
    public static event EventHandler<BaseController.TeamTag> OnNextRound;

    private void OnEnable()
    {
        PlayerController.OnPlayerReady += PlayerController_OnPlayerReady;
        BaseController.OnAllUnitsDeath += BaseController_OnAllUnitsDeath;
        
    }

    private void OnDisable()
    {
        PlayerController.OnPlayerReady -= PlayerController_OnPlayerReady;
        BaseController.OnAllUnitsDeath -= BaseController_OnAllUnitsDeath;
    }

    private void PlayerController_OnPlayerReady(object sender, EventArgs e)
    {
        botController.Play();
    }

    private void BaseController_OnAllUnitsDeath(object sender, BaseController.TeamTag e)
    {
        BaseController.TeamTag winner;

        if (e.Equals(BaseController.TeamTag.redTeam))
        {
            winner = BaseController.TeamTag.redTeam;
            redTeamPoints++;

        }
        else
        {
            winner = BaseController.TeamTag.blueTeam;
            blueTeamPoints++;
        }

        completedRoundCount++;
        if(completedRoundCount >= maxRounds) 
        {
            OnFinishGame?.Invoke(this, winner);
            return;
        }

        OnNextRound?.Invoke(this, winner);
        botController.Respawn();
    }
}
