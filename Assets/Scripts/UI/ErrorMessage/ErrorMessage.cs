using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ErrorMessage : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI message;
    private float timeToClose = 5f;
    private Animation animation;
    
    private void Awake() {
        animation = GetComponent<Animation>();
    }

    private void Start()
    {
        LobbyManager.Instance.OnLobbyUserError += LobbyManager_OnLobbyUserError;
        Hide();
    }

    public void LobbyManager_OnLobbyUserError(object sender, LobbyManager.UserErrorEventArgs e) {
        Hide();
        this.message.text = e.errorMessage;
        Show();
    } 

    private void Show() => gameObject.SetActive(true);
    private void Hide() => gameObject.SetActive(false);


    public void Update() {
        if (gameObject.activeSelf) {
            this.timeToClose -= Time.deltaTime;
            if (timeToClose < 0) Hide(); 
            if (timeToClose < 1) animation.Play("FadeOutErrorPanel");
        }
        
    }

    private void OnEnable() {       
        timeToClose = 5f;
        animation.Play("FadeInErrorPanel");
    }
}