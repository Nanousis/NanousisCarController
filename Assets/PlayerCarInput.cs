using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(CarController))]
public class PlayerCarInput : MonoBehaviour
{
    private CarInput input;
    private float throttleInput;
    private float throttleDamp;
    private float steeringInput;
    private float steeringDamp;
    private float clutchInput;
    private float clutchDamp;
    private float handBrakeInput;
    private CarController carController;
    public float dampenSpeed=1;
    public Mybutton gasPedal;
    public Mybutton brakePedal;
    public Mybutton leftButton;
    public Mybutton rightButton;
    public AnimationCurve steeringCurve;

    // Start is called before the first frame update
    void Awake()
    {
        input = new CarInput();
        carController = GetComponent<CarController>();
    }
    private void OnEnable()
    {
        input.Enable();
        input.Car.Throttle.performed += ApplyThrottle;
        input.Car.Throttle.canceled += ReleaseThrottle;
        input.Car.Steering.performed += ApplySteering;
        input.Car.Steering.canceled += ReleaseSteering;
        input.Car.Clutch.performed += ApplyClutch;
        input.Car.Clutch.canceled += ReleaseClutch;
        input.Car.Handbrake.performed += ApplyHandbrake;
        input.Car.Handbrake.canceled += ReleaseHandbrake;
    }
    private void HandleUI()
    {
        if (gasPedal!=null&& gasPedal.isPressed)
        {
            throttleInput += gasPedal.dampenPress;
        }
        if (brakePedal != null&&brakePedal.isPressed)
        {
            throttleInput -= brakePedal.dampenPress;
        }
        if (rightButton != null && rightButton.isPressed)
        {
            steeringInput += rightButton.dampenPress;
        }
        if (leftButton != null && leftButton.isPressed)
        {
            steeringInput -= leftButton.dampenPress;
        }
    }
    private void Update()
    {
        HandleUI();
        throttleDamp = DampenedInput(throttleInput, throttleDamp);
        steeringDamp = DampenedInput(steeringInput, steeringDamp);
        clutchDamp = DampenedInput(clutchInput, clutchDamp);
        carController.SetInput(throttleDamp, steeringDamp * steeringCurve.Evaluate(carController.speed), clutchDamp, handBrakeInput);

    }

    private void OnDisable()
    {
        input.Disable();

    }

    private float DampenedInput(float input, float output)
    {
        return Mathf.Lerp(output, input, Time.deltaTime * dampenSpeed);
    }

    private void ApplyThrottle(InputAction.CallbackContext value)
    {
        throttleInput = value.ReadValue<float>();
    }
    private void ReleaseThrottle(InputAction.CallbackContext value)
    {
        throttleInput = 0;
    }
    private void ApplySteering(InputAction.CallbackContext value)
    {
        steeringInput = value.ReadValue<float>();
    }
    private void ReleaseSteering(InputAction.CallbackContext value)
    {
        steeringInput = 0;
    }

    private void ApplyClutch(InputAction.CallbackContext value)
    {
        clutchInput = value.ReadValue<float>();
    }
    private void ReleaseClutch(InputAction.CallbackContext value)
    {
        clutchInput = 0;
    }
    private void ApplyHandbrake(InputAction.CallbackContext value)
    {
        handBrakeInput = value.ReadValue<float>();
    }
    private void ReleaseHandbrake(InputAction.CallbackContext value)
    {
        handBrakeInput = 0;
    }
}
