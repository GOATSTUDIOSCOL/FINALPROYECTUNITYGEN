using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering.PostProcessing;
using Unity.Netcode;

public class PostProcessingDamage : NetworkBehaviour
{
    public float intensity = 0;
    public GameObject deadTimeline;
    public GameObject playerCamera;

    PostProcessVolume _volume;
    Vignette _vignette;
    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            enabled = false; 
            return;
        }
    }
    void Start()
    {
        if(IsOwner)
        {
            enabled = false; // mientras se arreglan los bugs
            //deadTimeline = GameObject.FindGameObjectWithTag("Timeline");
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
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_vignette != null)
            _vignette.intensity.Override(intensity);
    }

    public void FirstDamageState()
    {
        if(IsOwner)
        {
            _vignette.enabled.Override(true);
            intensity = 0.3f;
        }
        
    }

    public void UltimateDamageState()
    {
        if(IsOwner)
        {
            intensity = 0.6f;
        }
        
    }

    // public void DeadCameraState()
    // {
    //     if(IsOwner)
    //     {
    //         intensity = 0.6f;
    //         playerCamera.SetActive(false);
    //         deadTimeline.GetComponent<PlayableDirector>().enabled = true;
    //         deadTimeline.GetComponentInChildren<Canvas>().enabled = true;
    //     }
        
    // }
}
