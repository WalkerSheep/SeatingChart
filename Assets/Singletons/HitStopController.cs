using System.Collections;
using UnityEngine;

public class HitStopController : MonoBehaviour
{
    public static HitStopController instance;

    public bool hitStopping;
    public bool fading;

    void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void HitStop(float intensity, float length, bool fade)
    {
        StopAllCoroutines();
        StartCoroutine(DoHitStop(intensity, length, fade));
    }

    private IEnumerator DoHitStop(float intensity, float length, bool fade)
    {
        hitStopping = true;
        fading = fade;

        Time.timeScale = intensity;

        if (!fade)
        {
            yield return new WaitForSecondsRealtime(length);

            hitStopping = false;
            fading = false;

            RestoreTimeScale();

            yield break;
        }

        float timer = 0f;

        while (timer < length)
        {
            float progress = timer / length;

            Time.timeScale = Mathf.Lerp(intensity, 1f, progress);

            timer += Time.unscaledDeltaTime;

            yield return null;
        }

        hitStopping = false;
        fading = false;

        RestoreTimeScale();
    }

    private void RestoreTimeScale()
    {

        Time.timeScale = 1f;
    }

    public void Cancel()
    {
        StopAllCoroutines();

        hitStopping = false;
        fading = false;

        Time.timeScale = 0f;
    }
}

[System.Serializable]
public class HitStopData
{
    public float intensity;
    public float length;
    public bool fade;
}