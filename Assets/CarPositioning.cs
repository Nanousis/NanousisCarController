using TMPro;
using Unity.VisualScripting;
using UnityEngine;


[System.Serializable]
public struct CarObject1
{
    public string name;
    public GameObject carPrefab;
    public bool Player;
    public int waypointPosition;
}



public class CarPositioning : MonoBehaviour
{
    public CarObject1[] CarObject1s;

    public float waypointRange=25f;
    public int LapCount = 3;
    public Transform[] waypoints;
    public TextMeshProUGUI positioningText;
    public TextMeshProUGUI lapText;
    private int playerIndex = 0;

    private int[] positioningIDs;

    private bool finished = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < CarObject1s.Length; i++)
        {
            if (CarObject1s[i].Player)
            {
                playerIndex = i;
                break;
            }
        }
        positioningIDs = new int[CarObject1s.Length];
        for (int i = 0; i < positioningIDs.Length; i++)
        {
            positioningIDs[i] = i;
        }
        RenderLapText();
        RenderPosText();
    }
    // Update is called once per frame
    void Update()
    {
        if (!finished)
        {
            OrderCars();
            HandleWaypoints();
            RenderLapText();
            RenderPosText();
        }
    }
    void RenderPosText()
    {
        string tempPosTxt = "";
        for (int i = 0; i < positioningIDs.Length; i++)
        {
            tempPosTxt += (i + 1) + ": " + CarObject1s[positioningIDs[i]].name;
            if (i > 0)
            {
                float distance = Vector3.Distance(
                    CarObject1s[positioningIDs[i]].carPrefab.transform.position,
                    CarObject1s[positioningIDs[0]].carPrefab.transform.position
                );
                tempPosTxt += " (" + distance.ToString("F1") + "yds)";
            }
            tempPosTxt += "\n";
        }
        positioningText.text = tempPosTxt;
    }
    void RenderLapText()
    {
        lapText.text = "Lap " + (CarObject1s[playerIndex].waypointPosition / (waypoints.Length + 1)+1) + "/" + LapCount;
    }
    void HandleWaypoints()
    {
        for (int i = 0; i < CarObject1s.Length; i++)
        {
            if (CarObject1s[i].carPrefab != null)
            {
                int currentWaypoint = CarObject1s[i].waypointPosition % (waypoints.Length + 1);
                if (currentWaypoint < waypoints.Length)
                {
                    float distance = Vector3.Distance(
                        CarObject1s[i].carPrefab.transform.position,
                        
                        waypoints[currentWaypoint].position
                    );
                    if (distance < waypointRange)
                    {
                        CarObject1s[i].waypointPosition++;
                    }
                }
            }
        }
    }
    void OrderCars()
    {
        for (int i = 0; i < CarObject1s.Length; i++)
        {
            bool swapped = false;
            for (int j = 0; j < CarObject1s.Length - 1; j++)
            {
                int carOneWaypoint = CarObject1s[positioningIDs[j]].waypointPosition;
                int carTwoWaypoint = CarObject1s[positioningIDs[j + 1]].waypointPosition;

                if (carOneWaypoint < carTwoWaypoint)
                {
                    int temp = positioningIDs[j];
                    positioningIDs[j] = positioningIDs[j + 1];
                    positioningIDs[j + 1] = temp;
                    swapped = true;
                }
                if (carOneWaypoint == carTwoWaypoint)
                {
                    Vector3 waypointPos;
                    if (carOneWaypoint % (waypoints.Length + 1) == waypoints.Length)
                    {
                        waypointPos = transform.position;
                    }
                    else
                    {
                      waypointPos = waypoints[carOneWaypoint % (waypoints.Length + 1)].position;  
                    } 
                    float carOneDistance = Vector3.Distance(
                        CarObject1s[positioningIDs[j]].carPrefab.transform.position,
                        waypointPos
                    );
                    float carTwoDistance = Vector3.Distance(
                        CarObject1s[positioningIDs[j + 1]].carPrefab.transform.position,
                        waypointPos
                    );
                    if (carOneDistance > carTwoDistance)
                    {
                        int temp = positioningIDs[j];
                        positioningIDs[j] = positioningIDs[j + 1];
                        positioningIDs[j + 1] = temp;
                        swapped = true;
                    }
                }
            }
            if (!swapped)
                break;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        GameObject carEntered = other.transform.root.gameObject;
        for(int i = 0; i < CarObject1s.Length; i++)
        {
            if (carEntered==CarObject1s[i].carPrefab)
            {
                if (CarObject1s[i].waypointPosition % (waypoints.Length + 1) == waypoints.Length)
                {
                    CarObject1s[i].waypointPosition++;
                    if (CarObject1s[i].Player && CarObject1s[i].waypointPosition / (waypoints.Length + 1) >= LapCount)
                    {
                        finished = true;
                        lapText.text = "Finished!";
                    }
                }
            }

        }
    }
}
