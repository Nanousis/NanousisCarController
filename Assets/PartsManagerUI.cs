using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PartsManagerUI : MonoBehaviour
{

    public CanvasGroup GearsCanvas;
    public CanvasGroup SelectItemCanvas;
    private PartsManager partsManager;
    public TMP_Text selectText;
    private int currentSelected = 0;
    private int tempSelected = 0;
    public GameObject nextButton;
    public GameObject previousButton;
    private PartType selectPart;


    // Start is called before the first frame update
    void Start()
    {
        partsManager = FindObjectOfType<PartsManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToSelectMenu(string partName)
    {
        StartCoroutine(FadeBetween(SelectItemCanvas, GearsCanvas));
        selectPart = partsManager.GetPartType(partName);
        currentSelected = selectPart.selected;
        tempSelected = selectPart.selected;
        previousButton.gameObject.SetActive(tempSelected > 0);
        nextButton.SetActive(tempSelected < selectPart.parts.Length - 1);
        selectText.text = "Selected";

    }

    public void ChangePart(int move)
    {
        if(tempSelected+move>=0&& tempSelected + move < selectPart.parts.Length)
        {
            tempSelected += move;
            partsManager.SetPartFromId(selectPart, tempSelected);
            previousButton.gameObject.SetActive(tempSelected > 0);
            nextButton.SetActive(tempSelected < selectPart.parts.Length - 1);
            if (currentSelected == tempSelected)
            {
                selectText.text = "Selected";
            }
            else
            {
                selectText.text = "Select";
            }
        }
    }

    public void SelectPart()
    {
        currentSelected = tempSelected;
        selectText.text = "Selected";
    }


    IEnumerator FadeBetween(CanvasGroup enable,CanvasGroup disable)
    {
        enable.gameObject.SetActive(true);
        enable.alpha = 0f;
        disable.alpha = 1f;
        while(enable.alpha<1 && disable.alpha > 0)
        {
            enable.alpha += 0.01f;
            disable.alpha -= 0.01f;
        yield return new WaitForEndOfFrame();
        }
    }
    public void GoBack()
    {

        partsManager.SetPartFromId(selectPart, currentSelected);
        StartCoroutine(FadeBetween( GearsCanvas, SelectItemCanvas));
        FindObjectOfType<GarageCamera>().state = CameraState.Orbiting;


    }
}
