using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private new Camera camera = null;

    private CameraConfig cameraConfig;

    private Coroutine currentCameraShakeRoutine = null;
    private Vector3 originalCameraPosition;

    public void Init()
    {
        cameraConfig = Root.ConfigManager.CameraConfig;

        originalCameraPosition = camera.transform.position;
    }
    
    public Vector2 WorldToScreenPoint(Vector3 position)
    {
        return camera.WorldToScreenPoint(position);
    }

    private void StartCameraShake(float duration)
    {
        if (currentCameraShakeRoutine != null)
        {
            StopCoroutine(currentCameraShakeRoutine);
            camera.transform.position = originalCameraPosition;
        }

        currentCameraShakeRoutine = StartCoroutine(ShakeCameraRoutine(duration));
    }

    private IEnumerator ShakeCameraRoutine(float duration)
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            float percentComplete = elapsedTime / duration;
            float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);

            // map value to [-1, 1]
            float x = Random.value * 2.0f - 1.0f;
            float y = Random.value * 2.0f - 1.0f;
            x *= cameraConfig.shakeGeneralMagnitude * damper;
            y *= cameraConfig.shakeGeneralMagnitude * damper;

            camera.transform.position = originalCameraPosition + new Vector3(x, y);

            yield return null;
        }

        camera.transform.position = originalCameraPosition;
    }
}
