using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    public static DialogManager instance;
    Dictionary<string, string> dialogsMap = new Dictionary<string, string>();
    public GameObject dialogPanelUi;
    [SerializeField]
    private TextMeshProUGUI dialogText;
    public bool dialogState = false;
    public List<SubtitleData> subtitleDataList = new List<SubtitleData>();
    private int currentIndex = 0;
    private SubtitleLogic subtitleLogic;

    private void Awake() {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        subtitleLogic = GetComponent<SubtitleLogic>();
        subtitleLogic.subtitleText = dialogText;
        SetDialog();
    }
    public void SetDialog()
    {
        if (currentIndex < subtitleDataList.Count)
        {
            Debug.Log("sub: " + currentIndex);
            SubtitleManager.instance.PlaySubtitle(subtitleDataList[currentIndex]);
            currentIndex++;
        }
        else
        {
            Debug.LogWarning("No hay más datos de subtítulos para reproducir.");
        }
    }

    public void HandleShowDialog(string dialog) {
        toggleDialogState(true);
        subtitleLogic.AddLongSubtitle(dialog);
    }


    public void HandleHideDialog() {
        toggleDialogState(false);
    }

    private void toggleDialogState(bool newState) {

        dialogState = newState;
        dialogPanelUi.SetActive(dialogState);
    }

}
