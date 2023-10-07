using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CameraState
{
    Orbiting,
    LookingAt
};

public class GarageCamera : MonoBehaviour
{
    public Transform target; // The object you want to rotate around
    public float rotationSpeed = 5.0f;

    private Vector3 lastMousePosition;
    private Transform tempCameraPosition;
    public float damping = 5;

    public CameraState state = CameraState.Orbiting;
    private Transform targeObject;
    public float lookingAtDistance = 1f;

    void Start()
    {
        tempCameraPosition = new GameObject().transform;
        tempCameraPosition.position = transform.position;
        tempCameraPosition.rotation = transform.rotation;
        if (target == null)
        {
            Debug.LogError("Target object is not assigned!");
            enabled = false;
        }
    }

    void LateUpdate()
    {
        switch(state)
        {
            case CameraState.Orbiting:
                // Check for mouse button down to start rotation
                if (Input.GetMouseButtonDown(0))
                {
                    lastMousePosition = Input.mousePosition;
                }

                // Check for mouse button release to stop rotation
                if (Input.GetMouseButtonUp(0))
                {
                    lastMousePosition = Vector3.zero;
                }

                // Rotate the camera if the mouse button is held down
                if (Input.GetMouseButton(0))
                {
                    Vector3 deltaMouse = lastMousePosition - Input.mousePosition;
                    float rotationX = deltaMouse.y * rotationSpeed * Time.deltaTime;
                    float rotationY = -deltaMouse.x * rotationSpeed * Time.deltaTime;



                    // Rotate the camera around the target object
                    tempCameraPosition.RotateAround(target.position, Vector3.up, rotationY);
                    //tempCameraPosition.RotateAround(target.position, target.right, rotationX);
                    lastMousePosition = Input.mousePosition;
                }

                transform.rotation = Quaternion.Slerp(transform.rotation, tempCameraPosition.rotation, Time.deltaTime * damping);

                transform.position = Vector3.Lerp(transform.position, tempCameraPosition.position, Time.deltaTime * damping);
                //transform.LookAt(target);
                break;
            case CameraState.LookingAt:
                transform.position = Vector3.Lerp(transform.position, targeObject.position - targeObject.forward * lookingAtDistance, Time.deltaTime * damping * 0.3f);
                var targetRotation = Quaternion.LookRotation(targeObject.transform.position - transform.position);

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, damping * Time.deltaTime * 0.6f);
                if (Input.GetKey(KeyCode.Escape))
                {
                    state = CameraState.Orbiting;
                    FindObjectOfType<PartsManagerUI>().GoBack();
                }
                break;


        }
        
    }


    public void LookAt(Transform lookAt) {

        targeObject = lookAt;
        state = CameraState.LookingAt;

    }

    

}
