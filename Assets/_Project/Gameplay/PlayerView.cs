using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
    [System.Serializable]
    public struct CharacterView
    {
        public Transform container;
        public PlayerViewItem viewItemTemplate;
    }

    [SerializeField] private CharacterView characterView;
    [SerializeField] private TextMeshProUGUI txtMoney;
    [SerializeField] private Button btnPlay;
    [Space]
    [SerializeField] private GameObject hidable;

    private PlayerController playerController;

    private void Awake()
    {
        btnPlay.onClick.AddListener(() =>
        {
            playerController.PlayGame();
            hidable.SetActive(false);
        });
    }

    public void SetPlayerController(PlayerController playerController)
    {
        this.playerController = playerController;
    }

    public void UpdateCharatersList(List<CharacterData> data)
    {
        foreach (Transform t in characterView.container)
        {
            Destroy(t.gameObject);
        }

        foreach (CharacterData d in data)
        {
            var item = Instantiate(characterView.viewItemTemplate, characterView.container);
            item.FillData(d.characterName, d.cost.ToString(), () =>
            {
                if(!playerController.CheckMoney(d.cost))
                {
                    Debug.Log("Dont have money!");
                    return;
                }
                playerController.SetCharacterToBuild(d);
            });
        }
    }

    private void OnEnable()
    {
        GameController.OnNextRound += GameController_OnNextRound;
    }

    private void OnDisable()
    {
        GameController.OnNextRound -= GameController_OnNextRound;
    }

    private void GameController_OnNextRound(object sender, BaseController.TeamTag e)
    {
        hidable.SetActive(true);
    }

    internal void UpdateMoney(int currentMoney)
    {
        txtMoney.text = currentMoney.ToString();
    }
}
