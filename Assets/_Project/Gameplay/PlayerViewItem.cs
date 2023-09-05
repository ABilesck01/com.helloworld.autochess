using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerViewItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtName;
    [SerializeField] private TextMeshProUGUI txtCost;
    [SerializeField] private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void FillData(string characterName, string characterCost, UnityAction OnClick)
    {
        txtName.text = characterName;
        txtCost.text = characterCost;
        button.onClick.AddListener(OnClick);
    }
}
