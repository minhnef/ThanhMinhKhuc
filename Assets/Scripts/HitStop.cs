using System.Collections;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    public static HitStop Instance;
    private float hitStopDuration = 0.1f;
    private bool isHitStopping = false;
    void Awake()
    {
        Instance = this;
    }

    public void TriggerHitStop(float duration)
    {
        if (!isHitStopping)
        {
            hitStopDuration = duration;
            StartCoroutine(HitStopCoroutine());
        }
    }
    IEnumerator HitStopCoroutine()
    {
        isHitStopping = true;
        float originalTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(hitStopDuration);
        Time.timeScale = originalTimeScale;
        isHitStopping = false;
    }
}
