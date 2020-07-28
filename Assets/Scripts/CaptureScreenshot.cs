using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CaptureScreenshot : MonoBehaviour, IPointerClickHandler
{
    public ScreenshotEvent OnScreenCaptured = new ScreenshotEvent();

    public void OnPointerClick(PointerEventData eventData)
    {
        StartCoroutine(CaptureScreenshotCoroutine());
    }

    private IEnumerator CaptureScreenshotCoroutine()
    {
        yield return new WaitForEndOfFrame();
        var screenshot = ScreenCapture.CaptureScreenshotAsTexture();
        OnScreenCaptured.Invoke(screenshot);
    }

    [System.Serializable]
    public class ScreenshotEvent : UnityEvent<Texture2D>
    {
    }
}
