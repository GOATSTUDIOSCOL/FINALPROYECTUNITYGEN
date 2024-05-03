using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SubtitleLogic : MonoBehaviour
{
    public TextMeshProUGUI subtitleText;
    private Queue<string> subtitlesQueue = new Queue<string>(); 
    private bool displayingSubtitle = false; 
    private float displayDuration = 2.7f; 
    private Coroutine displayCoroutine; 

    // Method to add a long subtitle to the queue
    public void AddLongSubtitle(string longSubtitle)
    {
        string[] parts = SplitLongSubtitle(longSubtitle);
        foreach (string part in parts)
        {
            subtitlesQueue.Enqueue(part);
        }

        if (!displayingSubtitle && subtitlesQueue.Count > 0)
        {
            StartDisplayCoroutine();
        }
    }

    // Method to split a long subtitle into smaller parts
    private string[] SplitLongSubtitle(string longSubtitle)
    {
        // Split the long subtitle into parts based on a character limit, ensuring words are not cut in half
        int maxCharactersPerPart = 50;
        List<string> parts = new List<string>();
        int startIndex = 0;
        while (startIndex < longSubtitle.Length)
        {
            int endIndex = Mathf.Min(startIndex + maxCharactersPerPart, longSubtitle.Length);
            if (endIndex < longSubtitle.Length && !char.IsWhiteSpace(longSubtitle[endIndex]))
            {
                // Move endIndex backwards until a whitespace is found
                while (endIndex > startIndex && !char.IsWhiteSpace(longSubtitle[endIndex]))
                {
                    endIndex--;
                }
            }
            parts.Add(longSubtitle.Substring(startIndex, endIndex - startIndex).Trim());
            startIndex = endIndex + 1; // Move startIndex to the next word
        }
        return parts.ToArray();
    }


    // Coroutine to display subtitles sequentially
    private IEnumerator DisplaySubtitleCoroutine()
    {
        displayingSubtitle = true;
        while (subtitlesQueue.Count > 0)
        {
            DisplaySubtitle(subtitlesQueue.Dequeue());
            yield return new WaitForSeconds(displayDuration);
        }
        displayingSubtitle = false;
        ClearSubtitle();
    }

    // Method to start displaying subtitles coroutine
    private void StartDisplayCoroutine()
    {
        displayCoroutine = StartCoroutine(DisplaySubtitleCoroutine());
    }

    // Method to stop displaying subtitles
    public void StopDisplayingSubtitles()
    {
        if (displayCoroutine != null)
        {
            StopCoroutine(displayCoroutine);
            displayingSubtitle = false;
        }
    }

    // Method to display a subtitle
    private void DisplaySubtitle(string subtitle)
    {
        subtitleText.text = subtitle;
    }

    // Method to clear the subtitle display
    private void ClearSubtitle()
    {
        subtitleText.text = "";
    }
}
