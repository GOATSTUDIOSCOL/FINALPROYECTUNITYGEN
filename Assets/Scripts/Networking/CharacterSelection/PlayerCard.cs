using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCard : MonoBehaviour
{
    [SerializeField] private CharacterDataBase characterDataBase;
    [SerializeField] private GameObject visuals;
    [SerializeField] private Image characterIconImage;
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private TextMeshProUGUI characterNameText;

    public void UpdateDisplay(CharacterSelectState state)
    {
        if (state.CharacterId != -1)
        {
            Character character = characterDataBase.GetCharacterById(state.CharacterId);
            if (character != null)
            {
                characterIconImage.sprite = character.Icon;
                characterIconImage.enabled = true;
                characterNameText.text = character.DisplayName;
            }
        }
        else
        {
            characterIconImage.enabled = false;
        }
        playerNameText.text = $"{state.ClientId}";

        visuals.SetActive(true);
    }

    public void DisableDisplay()
    {
        visuals.SetActive(false);
    }
}