using UnityEngine;
using Unity.Netcode;

public class PostProcessingDamage : NetworkBehaviour
{
    public static PostProcessingDamage instance { get; private set; }
    public GameObject damage1, damage2;

    private void Awake() {
        if (instance != null)
            Destroy(this);
        else
            instance = this;
    }

    private void Start() {
        damage1.SetActive(false);
        damage2.SetActive(false);
    }
    // public void FirstDamageState()
    // {
    //     damage1.SetActive(true);
    // }

    // public void UltimateDamageState()
    // {
    //     damage2.SetActive(true);
    // }

}