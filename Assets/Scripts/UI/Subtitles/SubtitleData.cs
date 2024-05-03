using UnityEngine;

[CreateAssetMenu(fileName = "SubtitleData", menuName = "Subtitle Data", order = 1)]
public class SubtitleData : ScriptableObject
{
    public AudioClip audioClip;
    [TextArea(3, 200)]
    public string subtitleText;
}
