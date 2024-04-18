using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro; 
using UnityEngine;

public class CaseFile : MonoBehaviour
{
    [Header ("CaseFile")]
    public GameObject caseFile;

    [Header ("Subtitles Interact")] 
    public TMPro.TextMeshPro Subtitles;
    public string[] SubtitlesPlacement;
    public int placement;

    bool player_detection = false;

    public void Start()
    {
        caseFile.SetActive(false);
    }

    private void Update()
    {
        if (player_detection == true && Input.GetKeyDown(KeyCode.E))
        {
            placement = 1; 
            caseFile.SetActive(true);
            Subtitles.text = SubtitlesPlacement[placement];
        }
    }
}
