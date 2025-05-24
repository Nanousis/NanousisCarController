using TMPro;
using UnityEngine;


[System.Serializable]
public struct CarObject
{
    public string name;
    public GameObject carPrefab;
    public bool Player;
    public int checkpointPosition;
}
public class LappingSystem : MonoBehaviour
{
    public CarObject[] cars;
    public Transform[] checkpoints;
    public float checkpointRange = 25f;
    public int totalLaps=2;

    public TextMeshProUGUI positionTxt;
    public TextMeshProUGUI lapTxt;
    public int[] orderList;
    private int playerIndex;
    public GameObject finishedPanel;
    public TextMeshProUGUI finishedPosTxt;
    private bool finished = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        finishedPanel.SetActive(false);

        for (int i = 0; i < cars.Length; i++)
        {
            if (cars[i].Player)
            {
                playerIndex = i;
                break;
            }
        }
        orderList = new int[cars.Length];
        for (int i = 0; i < cars.Length; i++)
        {
            orderList[i] = i;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!finished)
        {
            HandleCheckpoints();
            RenderLapText();
            OrderCars();
            RenderPositionText();
        }
    }

    void RenderPositionText()
    {
        string tmpString = "";
        for (int i = 0; i < orderList.Length; i++)
        {
            CarObject car = cars[orderList[i]];

            tmpString += (i + 1) + ": " + car.name;
            if (i > 0)
            {
                float distance = Vector3.Distance(cars[orderList[0]].carPrefab.transform.position, cars[orderList[i]].carPrefab.transform.position);
                tmpString += " (" + distance.ToString("F1") + "yds)";
            }
            tmpString += "\n";
        }
        positionTxt.text = tmpString;
    }
    void RenderLapText()
    {
        lapTxt.text = "Lap: " + (cars[playerIndex].checkpointPosition / (checkpoints.Length + 1)+1) + "/" + totalLaps;
    }
    void HandleCheckpoints()
    {
        for (int i = 0; i < cars.Length; i++)
        {
            int currentCheckpoint = cars[i].checkpointPosition % (checkpoints.Length + 1);
            if (currentCheckpoint < checkpoints.Length)
            {
                float distance = Vector3.Distance(cars[i].carPrefab.transform.position, checkpoints[currentCheckpoint].position);
                if (distance < checkpointRange)
                {
                    cars[i].checkpointPosition += 1;
                }
            }
        }
    }

    // // An optimized version of Bubble Sort 
    // void bubbleSort(vector<int>& arr) {
    //     int n = arr.size();
    //     bool swapped;

    //     for (int i = 0; i < n - 1; i++) {
    //         swapped = false;
    //         for (int j = 0; j < n - i - 1; j++) {
    //             if (arr[j] > arr[j + 1]) {
    //                 swap(arr[j], arr[j + 1]);
    //                 swapped = true;
    //             }
    //         }

    //         // If no two elements were swapped, then break
    //         if (!swapped)
    //             break;
    //     }
    // }
    void OrderCars()
    {
        int n = cars.Length;
        for (int i = 0; i < n - 1; i++)
        {
            bool swapped = false;
            for (int j = 0; j < n - i - 1; j++)
            {
                if (cars[orderList[j]].checkpointPosition < cars[orderList[j + 1]].checkpointPosition)
                {
                    int temp = orderList[j];
                    orderList[j] = orderList[j + 1];
                    orderList[j + 1] = temp;
                    swapped = true;
                }
                else if (cars[orderList[j]].checkpointPosition == cars[orderList[j + 1]].checkpointPosition)
                {
                    int currentCheckpoint = cars[j].checkpointPosition % (checkpoints.Length + 1);
                    Vector3 checkPosition;
                    if (currentCheckpoint < checkpoints.Length)
                    {
                        checkPosition = checkpoints[currentCheckpoint].position;
                    }
                    else
                    {
                        checkPosition = transform.position;
                    }
                    float distance1 = Vector3.Distance(cars[orderList[j]].carPrefab.transform.position, checkPosition);
                    float distance2 = Vector3.Distance(cars[orderList[j + 1]].carPrefab.transform.position, checkPosition);

                    if (distance1 > distance2)
                    {
                        int temp = orderList[j];
                        orderList[j] = orderList[j + 1];
                        orderList[j + 1] = temp;
                        swapped = true;
                    }
                }
                
            }
            // If no two elements were swapped, then break
            if (!swapped)
                break;
        }
    }

    
    void OnTriggerEnter(Collider other)
    {
        GameObject car = other.transform.root.gameObject;
        for (int i = 0; i < cars.Length; i++)
        {
            if (car == cars[i].carPrefab)
            {
                int currentCheckpoint = cars[i].checkpointPosition % (checkpoints.Length + 1);
                if (currentCheckpoint == checkpoints.Length)
                {
                    cars[i].checkpointPosition++;
                }
                if ((cars[playerIndex].checkpointPosition / (checkpoints.Length + 1)) >= totalLaps)
                {
                    finished = true;
                    FinishedPanel();
                }
            }
        }
    }
    void FinishedPanel()
    {
        finishedPanel.SetActive(true);
        int index = 0;
        for (int i = 0; i < orderList.Length; i++)
        {
            if (cars[orderList[i]].Player)
            {
                index = i;
                break;
            }
        }
        finishedPosTxt.text = "YOU FINISHED!\nYOUR POSITION: " + (index+1);
    }
}
