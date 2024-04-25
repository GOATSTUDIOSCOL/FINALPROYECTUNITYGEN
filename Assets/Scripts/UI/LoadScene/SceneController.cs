using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public string sceneLoadName;
    public TextMeshProUGUI textProgress;
    public GameObject play;
    public Slider sliderProgress;
    public float currentPercent;
    private AsyncOperation loadAsync;

    public void Start()
    {
        StartCoroutine(LoadScene(sceneLoadName));
    }

    public IEnumerator LoadScene(string nameToLoad)
    {
        textProgress.text = "Loading.. 00%";
        loadAsync = SceneManager.LoadSceneAsync(nameToLoad);
        loadAsync.allowSceneActivation = false;
        while (!loadAsync.isDone)
        {
            // 0.9 -> 100
            // 0.9?   0.9 *100 /0.9
            currentPercent = loadAsync.progress * 100 / 0.9f;
            textProgress.text = "Loading.. " + currentPercent.ToString("00")+"%";
            yield return null;
        }
    }
    private void Update()
    {
        sliderProgress.value = Mathf.MoveTowards(sliderProgress.value, currentPercent, 10 * Time.deltaTime);

        if (loadAsync != null && currentPercent >= 100)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                loadAsync.allowSceneActivation = true;
                play.SetActive(true);
            }
        }
      
    }
}
