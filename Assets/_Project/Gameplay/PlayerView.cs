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

    private PlayerController playerController;

    private void Awake()
    {
        btnPlay.onClick.AddListener(() =>
        {
            playerController.PlayGame();
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
                playerController.SetCharacterToBuild(d);
            });
        }
    }
}
