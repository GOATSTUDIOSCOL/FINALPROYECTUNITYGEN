using UnityEngine;

public class SubtitleManager : MonoBehaviour
{
    public static SubtitleManager instance;
    public AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }



    public void PlaySubtitle(SubtitleData subtitleData)
    {
        if (subtitleData.audioClip != null)
        {
            audioSource.clip = subtitleData.audioClip;
            audioSource.Play();
        }
        DialogManager.instance.HandleShowDialog(subtitleData.subtitleText);

    }
}
