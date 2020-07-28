using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Storage;
using UnityEngine;
using UnityEngine.Events;

public class DownloadScreenshot : MonoBehaviour
{
    public ScreenshotEvent OnScreenshotDownloaded = new ScreenshotEvent();

    public void Trigger(string path)
    {
        StartCoroutine(DownloadScreenshotCoroutine(path));
    }

    private IEnumerator DownloadScreenshotCoroutine(string path)
    {
        var storage = FirebaseStorage.DefaultInstance;
        var screenshotReference = storage.GetReference(path);

        var downloadTask = screenshotReference.GetBytesAsync(long.MaxValue);
        yield return new WaitUntil(() => downloadTask.IsCompleted);

        var texture = new Texture2D(2, 2);
        texture.LoadImage(downloadTask.Result);
        OnScreenshotDownloaded.Invoke(texture);
    }

    [System.Serializable]
    public class ScreenshotEvent:UnityEvent<Texture2D>
    {
    }
}
