using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] PlayerCam playerCam;

    public void SetMusicVol(float vol)
    {
        audioMixer.SetFloat("MusicVol", Mathf.Log10(vol) * 20);
    }

    public void SetSFXVol(float vol)
    {
        audioMixer.SetFloat("SFXVol", Mathf.Log10(vol) * 20);
    }

    public void SetSensitivity(float sense)
    {
        playerCam.lookSpeed = sense;
    }
}
