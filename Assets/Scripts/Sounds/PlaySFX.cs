using UnityEngine;

public class PlaySFX : MonoBehaviour
{
    [SerializeField] private AudioClip[] sounds;

    public void Play(int i)
    {
        GetComponent<AudioSource>().PlayOneShot(sounds[i]);
    }
}
