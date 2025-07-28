using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static Vector2 movementInput;
    public static Vector2 cameraInput;
    public static Dictionary<string, ButtonPress> buttonMap;

    void Start()
    {
        movementInput = new Vector2();
        cameraInput = new Vector2();
        buttonMap = new Dictionary<string, ButtonPress>
        {
            {"Jump", new ButtonPress("Jump")},
            {"Roll", new ButtonPress("Roll")}
        };
    }

    void Update()
    {
        movementInput.x = Input.GetAxis("Horizontal");
        movementInput.y = Input.GetAxis("Vertical");

        cameraInput.x = Input.GetAxis("HorizontalCam");
        cameraInput.y = Input.GetAxis("VerticalCam");

        foreach (string key in buttonMap.Keys)
        {
            buttonMap[key].Update();
        }
    }
}

/** Object representing a button press */
public class ButtonPress
{
    /** Name of the button according to the Input Manager */
    private string buttonName;
    /** Whether the button is currently being pressed */
    private bool buttonActive;
    /** Whether the button was pressed this frame */
    private bool buttonPressedThisFrame;
    /** Whether the button was released this frame */
    private bool buttonReleasedThisFrame;
    /** The length that the button was held, in seconds */
    private float lengthOfPress;
    /** The time since the button was last released, in seconds */
    private float timeSinceLastPress;

    /**
    Creates a button press object
    @param buttonName   The name of the button this object represents
    */
    public ButtonPress(string buttonName)
    {
        this.buttonName = buttonName;
    }

    /** Updates the buttons attributes. Make sure to call each frame. */

    public void Update()
    {
        buttonActive = Input.GetButton(buttonName);

        if (buttonActive)
        {
            lengthOfPress += Time.deltaTime;
            timeSinceLastPress = 0;
        }
        else
        {
            timeSinceLastPress += Time.deltaTime;
        }

        buttonPressedThisFrame = Input.GetButtonDown(buttonName);

        if (buttonPressedThisFrame)
        {
            lengthOfPress = 0f;
        }

        buttonReleasedThisFrame = Input.GetButtonUp(buttonName);
    }

    /** Returns whether the button is currently being pressed */
    public bool Active()
    {
        return buttonActive;
    }

    /** Returns whether the button was pressed this frame */
    public bool PressedThisFrame()
    {
        return buttonPressedThisFrame;
    }

    /** Returns whether the button was released this frame */
    public bool ReleasedThisFrame()
    {
        return buttonReleasedThisFrame;
    }

    /** Returns how long the button was last pressed. Resets every new button press */
    public float GetLengthOfPress()
    {
        return lengthOfPress;
    }

    /** Returns how long its been since the button was last released. Resets every new button press */
    public float GetTimeSinceLastPress()
    {
        return timeSinceLastPress;
    }
}