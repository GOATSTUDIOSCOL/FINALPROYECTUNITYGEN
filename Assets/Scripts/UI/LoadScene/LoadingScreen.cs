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

    private bool loadingComplete = false;

    void Start()
    {
        LobbyManager.Instance.OnJoinedLobby += LobbyManager_OnJoinedLobby;
        loadingSlider.value = 0f;
        pressToPlayText.gameObject.SetActive(false);
        StartCoroutine(LoadLevel());
        SetRandomImage();
        gameObject.SetActive(false);
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
        if (loadingComplete && Input.GetKeyDown(KeyCode.E))
        {
            gameObject.SetActive(false);
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
        pressToPlayText.gameObject.SetActive(true);
        pressToPlayText.text = "Press E to play";
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