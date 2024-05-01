using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    private Dictionary<string, string> textDictionary;
    public static LanguageManager instance;

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

        textDictionary = new Dictionary<string, string>();

        // Load the CSV file
        TextAsset csvFile = Resources.Load<TextAsset>("Texts");

        // Parse the CSV file
        StringReader reader = new StringReader(csvFile.text);
        string[] header = reader.ReadLine().Split(',');
        while (reader.Peek() != -1)
        {
            string[] line = reader.ReadLine().Split(',');
            if (line.Length >= header.Length)
            {
                string key = line[0];
                for (int i = 1; i < header.Length; i++)
                {
                    string language = header[i];
                    string value = line[i];
                    string dictionaryKey = $"{key}_{language}";
                    textDictionary[dictionaryKey] = value;
                }
            }
        }
    }
    public void SetLanguage(string language)
    {
        PlayerPrefs.SetString("Language", language);
    }

    public string GetText(string key)
    {
        string language = PlayerPrefs.GetString("Language", "en"); // Get the current language from PlayerPrefs or use "en" if none is set
        string dictionaryKey = $"{key}_{language}";
        if (textDictionary.ContainsKey(dictionaryKey))
        {
            return textDictionary[dictionaryKey];
        }
        else
        {
            Debug.LogWarning($"Missing text for key '{key}' in language '{language}'");
            return key;
        }
    }
}