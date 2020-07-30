using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Storage;
using UnityEngine;
using UnityEngine.Events;

public class DownloadScreenshot : MonoBehaviour
{
    public ScreenshotEvent OnScreenshotDownloaded = new ScreenshotEvent();

    // path = "gs://ougi-fb.appspot.com/screenshots"
    // inspecter 창에서 입력해주면 됨
   
    public void StartDownloadScreenshot(string path)
    {
        StartCoroutine(DownloadScreenshotCoroutine(path));
    }

    private IEnumerator DownloadScreenshotCoroutine(string path)
    {
        // 다운할 파일의 경로지정
        var storage = FirebaseStorage.DefaultInstance;
        // 파일이름 임의로 지정
        var screenshotReference = storage.GetReference(path).Child($"/screenshots/2020-07-29+22-38-14.png");

        // Textrue 2D => DisplayScreenshot 으로 갔습니다.

        #region Save-At-Local-File
        // Storage에서 이미지 다운 받아 로컬 파일에 저장하기

        // 로컬 파일 경로 지정 (저장할 곳)
        string local_url = Application.dataPath + "/StreamingAssets/2020-07-29+22-38-14.png";

        var imgDownloadTask = screenshotReference.GetFileAsync(local_url);
        yield return new WaitUntil(() => imgDownloadTask.IsCompleted);
        if (imgDownloadTask.Exception != null)
        {
            Debug.LogError(imgDownloadTask.Exception);
        }
        else
        { 
            Debug.Log("이미지 저장 완료" + local_url);
        }
        #endregion
    }

    [System.Serializable]
    public class ScreenshotEvent:UnityEvent<Texture2D>
    {
    }
}
