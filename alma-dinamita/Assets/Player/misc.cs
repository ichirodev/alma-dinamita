using System;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Serialization;

public class misc : MonoBehaviour
{
    private const float FPSMeasurePeriod = 1.2f;
    private int mFpsAccumulator = 0;
    private float mFpsNextPeriod = 0;
    private int mCurrentFps;
    const string display = "{0} FPS";
    // Required data to display
    [SerializeField] private health playerHealth;
    [SerializeField] private movement playerMovement;
    // Control what GUI should be displayed
    public bool showFPSCounter = true;
    public bool showHealthPoints = true;
    public bool showMovementDetails = true;
    // Graphical user interface texts
    [FormerlySerializedAs("framesPerSecondUI")] [SerializeField] Text framesPerSecondText;
    [SerializeField] Text healthPointsText;
    [SerializeField] Text movementDetailsText;
    
    private void Start()
    {
        mFpsNextPeriod = Time.realtimeSinceStartup + FPSMeasurePeriod;
    }

    private void Update()
    {
        UpdateGUIText();
    }
    private void UpdateGUIText()
    {
        if (showFPSCounter)
        {
            // measure average frames per second
            mFpsAccumulator++;
            if (Time.realtimeSinceStartup > mFpsNextPeriod)
            {
                mCurrentFps = (int)(mFpsAccumulator / FPSMeasurePeriod);
                mFpsAccumulator = 0;
                mFpsNextPeriod += FPSMeasurePeriod;
                framesPerSecondText.text = string.Format(display, mCurrentFps);
            }
        }

        if (showHealthPoints)
        {
            // display health on screen
            healthPointsText.text = playerHealth.GetPlayerHealth().ToString();
        }

        if (showMovementDetails)
        {
            // show movement speed
            movementDetailsText.text = "Movement speed: " + playerMovement.GetMovementSpeedAsUnits().ToString();
        }
    }
}
