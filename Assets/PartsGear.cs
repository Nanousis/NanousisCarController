using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartsGear : MonoBehaviour
{
    public string partName;
    private GarageCamera garageCamera;
    // Start is called before the first frame update
    void Start()
    {
        garageCamera = FindObjectOfType<GarageCamera>();
    }

    // Update is called once per frame

    public void SelectItem()
    {
        garageCamera.LookAt(transform);
        FindObjectOfType<PartsManagerUI>().GoToSelectMenu(partName);
    }

}
