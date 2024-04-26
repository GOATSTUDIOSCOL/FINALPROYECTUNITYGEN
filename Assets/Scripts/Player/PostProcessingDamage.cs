using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingDamage : MonoBehaviour
{
    public float intensity = 0;
    public GameObject deadTimeline;
    public GameObject playerCamera;

    PostProcessVolume _volume;
    Vignette _vignette;
    void Start()
    {
        deadTimeline = GameObject.FindGameObjectWithTag("Timeline");
        deadTimeline.SetActive(false);
        deadTimeline.GetComponent<PlayableDirector>().enabled = true;
        deadTimeline.GetComponentInChildren<Canvas>().enabled = true;
        _volume = GetComponent<PostProcessVolume>();
        _volume.profile.TryGetSettings<Vignette>(out _vignette);

        if (!_vignette)
        {
            print("vignette empty!!!!");
        }
        else
        {
            _vignette.enabled.Override(false);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_vignette != null)
            _vignette.intensity.Override(intensity);
    }

    public void FirstDamageState()
    {
        _vignette.enabled.Override(true);
        intensity = 0.3f;
    }

    public void UltimateDamageState()
    {
        intensity = 0.42f;
    }

    public void DeadCameraState()
    {
        intensity = 0.42f;
        playerCamera.SetActive(false);
        deadTimeline.SetActive(true);
    }
}
