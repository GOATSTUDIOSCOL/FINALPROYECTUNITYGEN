using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.VisualScripting.Icons;
using static UnityEngine.Rendering.PostProcessing.SubpixelMorphologicalAntialiasing;
using UnityEngine.Rendering;
using WebSocketSharp;

public class LanguageSetter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playGame;
    [SerializeField] private TextMeshProUGUI options;
    [SerializeField] private TextMeshProUGUI exit;
    [SerializeField] private TextMeshProUGUI optionsTitle;
    [SerializeField] private TextMeshProUGUI language;
    [SerializeField] private TextMeshProUGUI resolution;
    [SerializeField] private TextMeshProUGUI screenMode;
    [SerializeField] private TextMeshProUGUI graphics;
    [SerializeField] private TextMeshProUGUI volume;
    [SerializeField] private TextMeshProUGUI back;
    [SerializeField] private TextMeshProUGUI next;
    [SerializeField] private TextMeshProUGUI login;
    [SerializeField] private TextMeshProUGUI nickname;
    [SerializeField] private TextMeshProUGUI lobbyList;
    [SerializeField] private TextMeshProUGUI updateList;
    [SerializeField] private TextMeshProUGUI lobbyName;
    [SerializeField] private TextMeshProUGUI createLobby;
    [SerializeField] private TextMeshProUGUI playGameCharacter;


    public TMP_FontAsset japanFont;
    public TMP_FontAsset normalFont;

    public static LanguageSetter instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        SetLanguage();
        if (PlayerPrefs.GetString("Language", "en") == "jp")
        {
            ChangeFont(japanFont);
        }
    }

    public void SetLanguage()
    {
        playGame.text = LanguageManager.instance.GetText("PLAY");
        options.text = LanguageManager.instance.GetText("OPTIONS");
        exit.text = LanguageManager.instance.GetText("EXIT");
        optionsTitle.text = LanguageManager.instance.GetText("OPTIONS");
        language.text = LanguageManager.instance.GetText("LANGUAGE");
        resolution.text = LanguageManager.instance.GetText("RESOLUTION");
        screenMode.text = LanguageManager.instance.GetText("SCREENMODE");
        graphics.text = LanguageManager.instance.GetText("QUALITY");
        volume.text = LanguageManager.instance.GetText("VOLUME");
        back.text = LanguageManager.instance.GetText("BACK");
        next.text = LanguageManager.instance.GetText("CONTINUE");
        login.text = LanguageManager.instance.GetText("LOGIN");
        nickname.text = LanguageManager.instance.GetText("NICKNAME");
        lobbyList.text = LanguageManager.instance.GetText("SERVER");
        updateList.text = LanguageManager.instance.GetText("UPDATE");
        lobbyName.text = LanguageManager.instance.GetText("LOBBYNAME");
        createLobby.text = LanguageManager.instance.GetText("CREATELOBBY");
        //playGameCharacter.text = LanguageManager.instance.GetText("PLAY");
    }

    public void ChangeFont(TMP_FontAsset newFont)
    {
        playGame.font = newFont;
        options.font = newFont;
        exit.font = newFont;
        optionsTitle.font = newFont;
        language.font = newFont;
        resolution.font = newFont;
        screenMode.font = newFont;
        graphics.font = newFont;
        volume.font = newFont;
        back.font = newFont;
        next.font = newFont;
        login.font = newFont;
        nickname.font = newFont;
        lobbyList.font = newFont;
        updateList.font = newFont;
        lobbyName.font = newFont;
        createLobby.font = newFont;
    }
}
