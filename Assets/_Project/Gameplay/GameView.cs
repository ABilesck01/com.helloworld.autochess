using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{
    [SerializeField] private GameObject endRoundScreen;
    [SerializeField] private Button btnNextRound;
    [SerializeField] private TextMeshProUGUI txtEndRound;
    [Space]
    [SerializeField] private GameObject endGameScreen;
    [SerializeField] private Button btnPlayAgain;
    [SerializeField] private TextMeshProUGUI txtEndGame;
    [Space]
    [SerializeField] private Color32 blueTeamColor;
    [SerializeField] private Color32 redTeamColor;
    [Space]
    [SerializeField] private TextMeshProUGUI blueTeamPoints;
    [SerializeField] private TextMeshProUGUI redTeamPoints;

    private void Awake()
    {
        btnNextRound.onClick.AddListener(() => 
        {
            endRoundScreen.SetActive(false);
        });
        btnPlayAgain.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        });
    }

    private void OnEnable()
    {
        GameController.OnNextRound += GameController_OnNextRound;
        GameController.OnFinishGame += GameController_OnFinishGame;
    }

    private void OnDisable()
    {
        GameController.OnNextRound -= GameController_OnNextRound;
        GameController.OnFinishGame -= GameController_OnFinishGame;
    }

    private void GameController_OnFinishGame(object sender, BaseController.TeamTag e)
    {
        if (e.Equals(BaseController.TeamTag.redTeam))
        {
            txtEndGame.color = redTeamColor;
            txtEndGame.text = $"{BaseController.TeamTag.redTeam} win the game";
        }
        else
        {
            txtEndGame.color = blueTeamColor;
            txtEndGame.text = $"{BaseController.TeamTag.blueTeam} win the game";
        }

        endGameScreen.SetActive(true);
    }

    private void GameController_OnNextRound(object sender, BaseController.TeamTag e)
    {
        if (e.Equals(BaseController.TeamTag.redTeam))
        {
            txtEndRound.color = redTeamColor;
            txtEndRound.text = $"{BaseController.TeamTag.redTeam} win the match";
        }
        else
        {
            txtEndRound.color = blueTeamColor;
            txtEndRound.text = $"{BaseController.TeamTag.blueTeam} win the match";
        }

        endRoundScreen.SetActive(true);
    }

    public void UpdateScore(int blue, int red)
    {
        blueTeamPoints.text = blue.ToString();
        redTeamPoints.text = red.ToString();
    }
}
