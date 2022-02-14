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
    [SerializeField] private weaponsManager playerWM;
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
            framesPerSecondText.enabled = true;
            if (Time.realtimeSinceStartup > mFpsNextPeriod)
            {
                mCurrentFps = (int)(mFpsAccumulator / FPSMeasurePeriod);
                mFpsAccumulator = 0;
                mFpsNextPeriod += FPSMeasurePeriod;
                framesPerSecondText.text = string.Format(display, mCurrentFps);
            }
        }
        else
        {
            framesPerSecondText.enabled = false;
        }

        if (showHealthPoints)
        {
            // display health on screen
            healthPointsText.enabled = true;
            healthPointsText.text = playerHealth.GetPlayerHealth().ToString();
        }
        else
        {
            healthPointsText.enabled = false;
        }

        if (showMovementDetails)
        {
            // show movement speed
            movementDetailsText.enabled = true;
            movementDetailsText.text = "movspeed: " + playerMovement.GetMovementSpeedAsUnits().ToString();
        }
        else
        {
            movementDetailsText.enabled = false;
        }
    }
}
