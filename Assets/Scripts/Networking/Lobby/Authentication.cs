using UnityEngine;
using UnityEngine.UI;

public class Authentication : MonoBehaviour
{

    [SerializeField] private Button authenticateButton;


    private void Awake()
    {
        authenticateButton.onClick.AddListener(() =>
        {
            LobbyManager.Instance.Authentication(EditPlayerPreferences.Instance.GetPlayerName());
            LobbyListUI.Instance.Show();
            Hide();
        });
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
