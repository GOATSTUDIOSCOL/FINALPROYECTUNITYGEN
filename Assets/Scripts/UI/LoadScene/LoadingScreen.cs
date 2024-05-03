using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class LoadingScreen : MonoBehaviour
{
    public Slider loadingSlider;
    public TextMeshProUGUI pressToPlayText;
    public float loadingTime = 8f;
    public List<Sprite> loadingImages;
    public Image loadingImageObject;
    public GameObject audioBegin;

    private bool loadingComplete = false;

    void Start()
    {
        loadingSlider.value = 0f;
        pressToPlayText.gameObject.SetActive(false);
        StartCoroutine(LoadLevel());
        SetRandomImage();

        if (audioBegin == null ) {
            LobbyManager.Instance.OnJoinedLobby += LobbyManager_OnJoinedLobby;
        }
    }

    private void LobbyManager_OnJoinedLobby(object sender, LobbyManager.LobbyEventArgs e)
    {
        gameObject.SetActive(true);
    }


    private void OnEnable()
    {
        loadingSlider.value = 0f;
        pressToPlayText.gameObject.SetActive(false);
        StartCoroutine(LoadLevel());
        SetRandomImage();
    }

    void Update()
    {
        if (loadingComplete) {
            if (audioBegin != null && Input.GetKeyDown(KeyCode.E)) {
                audioBegin.gameObject.SetActive(true);
                gameObject.SetActive(false);
            }  else if (audioBegin == null) {
                gameObject.SetActive(false);
            }
        }
    }

    IEnumerator LoadLevel()
    {
        float timer = 0f;
        while (timer < loadingTime)
        {
            timer += Time.deltaTime;
            loadingSlider.value = timer / loadingTime;
            yield return null;
        }

        loadingComplete = true;
        if (audioBegin) {
            pressToPlayText.gameObject.SetActive(true);
            pressToPlayText.text = "Press E to play";
        }
       
    }

    void SetRandomImage()
    {
        if (loadingImages.Count > 0 && loadingImageObject != null)
        {
            int randomIndex = Random.Range(0, loadingImages.Count);
            loadingImageObject.sprite = loadingImages[randomIndex];
        }
        else
        {
            Debug.LogWarning("No se han asignado im�genes a la lista de im�genes de carga o el objeto de imagen no est� asignado.");
        }
    }
}