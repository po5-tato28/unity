using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using Firebase.Storage;
using UnityEngine;
using UnityEngine.Events;

public class DisplayScreenshot : MonoBehaviour
{
    const string PATH = "gs://ougi-fb.appspot.com/";
    public ScreenshotEvent OnScreenshotDownloaded = new ScreenshotEvent();

    public static Task downloadTask;
    // Start is called before the first frame update

    private void Start()
    {
        StartCoroutine(DisplayScreenshotCoroutine());
    }

    public void StartDisplayScreenshot()
    {
        StartCoroutine(DisplayScreenshotCoroutine());
    }

    private IEnumerator DisplayScreenshotCoroutine()
    {
        // 다운할 파일의 경로지정
        var storage = FirebaseStorage.DefaultInstance;
        // 파일이름 임의로 지정
        StorageReference screenshotReference = storage.GetReferenceFromUrl(PATH).Child($"/screenshots/dog-2785077_640.jpg"); 

        #region Print-On-Texture2D
        // Storage에서 이미지 다운받아 Texture에 출력하기

        //GetBytesAsync(long.MaxValue); => 최대 사이즈 지정해준 것
        var downloadTask = screenshotReference.GetBytesAsync(long.MaxValue);
        yield return new WaitUntil(() => downloadTask.IsCompleted);
        if (downloadTask.Exception != null)
        {
            Debug.LogError($"이미지 가져오기에 실패했습니다. {downloadTask.Exception}");
            yield break;
        }
        else
        {
            // 다운 받은 이미지를 texture에 출력해준다!
            var texture = new Texture2D(2, 2);
            texture.LoadImage(downloadTask.Result);
            OnScreenshotDownloaded.Invoke(texture);
        }

        #endregion
    }

    [System.Serializable]
    public class ScreenshotEvent : UnityEvent<Texture2D>
    {
    }
}
