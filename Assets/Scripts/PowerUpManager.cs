using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public float extraDamage, scoreBonus;

    public bool DamageBoostActive = false, ScoreBoostActive = false;

    private Coroutine damageBoostTimer;
    private Coroutine scoreBoostTimer;

   private const float powerUpDuration = 5f;

    public void ActivatePowerUp(GameObject powerUp)
    {
        if (powerUp.CompareTag("DamageBoost"))
        {
            ActivateDamageBoost();
        }
        else if (powerUp.CompareTag("ScoreBoost"))
        {
            ActivateScoreBoost();
        }
    }

    public void ActivateDamageBoost()
    {
        if (!ScoreBoostActive)
        {
            DamageBoostActive = true;
            extraDamage = 100f;
            Debug.Log("DAMAGE UP");

            StartCoroutine(TimerCoroutine(powerUpDuration));
        }
    }

    public void ActivateScoreBoost()
    {
        if (!DamageBoostActive)
        {
            ScoreBoostActive = true;
            scoreBonus = 2f;

            Debug.Log("SCORE UP");

            StartCoroutine(TimerCoroutine(powerUpDuration));
        }
    }

    private IEnumerator TimerCoroutine(float timer)
    {
        
        while (timer > 0)
        {
            yield return new WaitForSeconds(1f);
            timer -= 1f;
            Debug.Log("Timer: " + timer);
        }

        if (DamageBoostActive)
        {
            DeactivateDamageBoost();
        }
        else if (ScoreBoostActive)
        {
            DeactivateScoreBoost();
        }
    }
    public void DeactivateDamageBoost()
    {
        DamageBoostActive = false;
        Debug.Log("DAMAGE DOWN");
    }

    public void DeactivateScoreBoost()
    {
        ScoreBoostActive = false;
        Debug.Log("SCORE DOWN");
    }
}
