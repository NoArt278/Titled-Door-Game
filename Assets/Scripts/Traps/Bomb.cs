using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] GameManager gm;
    [SerializeField] TMP_Text timerText;
    [SerializeField] AudioSource timerSound, explosionSound;
    private float countdownTime = 180f;
    private float currCountdown;
    private bool wasStarted = false;
    private Coroutine countdownRoutine;

    private void OnEnable()
    {
        wasStarted = false;
    }

    public void StartCountdown()
    {
        if (!wasStarted)
        {
            wasStarted = true;
            countdownRoutine = StartCoroutine(Countdown());
        }
    }

    public void StopCountdown()
    {
        if (countdownRoutine != null)
        {
            StopCoroutine(countdownRoutine);
        }
    }

    public void SetTimer(int time)
    {
        countdownTime = time;
    }

    IEnumerator Countdown()
    {
        currCountdown = countdownTime;
        int minute = Mathf.FloorToInt(currCountdown / 60);
        float seconds = currCountdown % 60;
        timerText.text = string.Concat("0", minute.ToString(), ":", (seconds < 10) ? "0" : "", seconds.ToString());
        while (currCountdown > 0)
        {
            currCountdown--;
            minute = Mathf.FloorToInt(currCountdown / 60);
            seconds = currCountdown % 60;
            timerText.text = string.Concat("0", minute.ToString(), ":", (seconds < 10) ? "0" : "", seconds.ToString());
            timerSound.Play();
            yield return new WaitForSeconds(1);
        }
        explosionSound.Play();
        gm.RestartGame();
    }
}
