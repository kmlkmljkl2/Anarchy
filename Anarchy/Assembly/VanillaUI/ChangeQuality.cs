﻿using Optimization.Caching;
using UnityEngine;

public class ChangeQuality : MonoBehaviour
{
    private bool init;
    public static bool isTiltShiftOn;

    private static void setQuality(float val)
    {
        if (val < 0.167f)
        {
            QualitySettings.SetQualityLevel(0, true);
        }
        else if (val < 0.33f)
        {
            QualitySettings.SetQualityLevel(1, true);
        }
        else if (val < 0.5f)
        {
            QualitySettings.SetQualityLevel(2, true);
        }
        else if (val < 0.67f)
        {
            QualitySettings.SetQualityLevel(3, true);
        }
        else if (val < 0.83f)
        {
            QualitySettings.SetQualityLevel(4, true);
        }
        else if (val <= 1f)
        {
            QualitySettings.SetQualityLevel(5, true);
        }
    }

    private void OnSliderChange()
    {
        if (!this.init)
        {
            this.init = true;
            if (PlayerPrefs.HasKey("GameQuality"))
            {
                base.gameObject.GetComponent<UISlider>().sliderValue = PlayerPrefs.GetFloat("GameQuality");
            }
            else
            {
                PlayerPrefs.SetFloat("GameQuality", base.gameObject.GetComponent<UISlider>().sliderValue);
            }
        }
        else
        {
            PlayerPrefs.SetFloat("GameQuality", base.gameObject.GetComponent<UISlider>().sliderValue);
        }
        ChangeQuality.setQuality(base.gameObject.GetComponent<UISlider>().sliderValue);
    }

    public static void setCurrentQuality()
    {
        if(Anarchy.Configuration.VideoSettings.Quality != null)
        {
            setQuality(Anarchy.Configuration.VideoSettings.Quality.Value);
        }
    }

    public static void turnOffTiltShift()
    {
        ChangeQuality.isTiltShiftOn = false;
        if (IN_GAME_MAIN_CAMERA.BaseCamera)
        {
            IN_GAME_MAIN_CAMERA.BaseCamera.GetComponent<TiltShift>().enabled = false;
        }
    }

    public static void turnOnTiltShift()
    {
        ChangeQuality.isTiltShiftOn = true;
        if (IN_GAME_MAIN_CAMERA.BaseCamera)
        {
            IN_GAME_MAIN_CAMERA.BaseCamera.GetComponent<TiltShift>().enabled = true;
        }
    }
}