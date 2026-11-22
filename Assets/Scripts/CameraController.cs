
using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    [Header("References")]
    [SerializeField] private Camera cam;

    private Vector3 camStartLocalPos;
    private float currentIntensity;
    private Coroutine shakeRoutine;

    void Awake()
    {
        instance = this;

        if (cam == null)
        {
            cam = GetComponentInChildren<Camera>();
        }

        camStartLocalPos = cam.transform.localPosition;
    }

    public void ShakeCamera(float intensity, float length)
    {
        if (intensity <= currentIntensity || length <= 0f)
            return;

        if (shakeRoutine != null)
        {
            StopCoroutine(shakeRoutine);
        }

        cam.transform.localPosition = camStartLocalPos;
        currentIntensity = intensity;

        shakeRoutine = StartCoroutine(DoShake(intensity, length));
    }

    private IEnumerator DoShake(float intensity, float length)
    {
        float timer = 0f;

        while (timer < length)
        {
            float progress = timer / length;

            currentIntensity = Mathf.Lerp(intensity, 0f, progress);

            Vector3 shakeOffset = new Vector3(
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f),
                0f
            ) * currentIntensity;

            cam.transform.localPosition = camStartLocalPos + shakeOffset;

            timer += Time.unscaledDeltaTime;

            yield return null;
        }

        currentIntensity = 0f;
        cam.transform.localPosition = camStartLocalPos;
        shakeRoutine = null;
    }

    void OnDisable()
    {
        if (cam != null)
        {
            cam.transform.localPosition = camStartLocalPos;
        }

        currentIntensity = 0f;
        shakeRoutine = null;
    }
}

[System.Serializable]
public class CameraShakeData
{
    public float intensity;
    public float length;
}
