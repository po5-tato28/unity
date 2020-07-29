using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Storage;

public class UploadScreenshot : MonoBehaviour
{
    const string PATH = "gs://ougi-fb.appspot.com";

    public void StartUpload(Texture2D screenshot)
    {
        StartCoroutine(UploadCoroutine(screenshot));
    }

    private IEnumerator UploadCoroutine(Texture2D screenshot)
    {
        #region DateTime-Set
        DateTime dateTime = DateTime.Now;
        // 시-분-초
        string time = dateTime.Hour.ToString() + "-" + dateTime.Minute.ToString() + "-" + dateTime.Second.ToString();
        #endregion

        string TOTAL_DATE = dateTime.ToShortDateString() + "+" + time;

        FirebaseStorage storage = FirebaseStorage.DefaultInstance;
        StorageReference screenshotReference = storage.GetReferenceFromUrl(PATH).Child($"/screenshots/{TOTAL_DATE}.png");

        #region Set-Metadata
        var metadataChange = new MetadataChange()
        {
            ContentEncoding = "image/png",
            /* 여기는 메타데이터 설정
            CustomMetadata = new Dictionary<string, string>()
            {
                { "Position", Camera.main.transform.position.ToString()},
                { "Rotation", Camera.main.transform.position.ToString()}
            }
            */
        };
        #endregion

        var bytes = screenshot.EncodeToPNG();
        var uploadTask = screenshotReference.PutBytesAsync(bytes, metadataChange);
        yield return new WaitUntil(() => uploadTask.IsCompleted);

        if(uploadTask.Exception != null)
        {
            Debug.LogError($"업로드에 실패했습니다. {uploadTask.Exception}");
            yield break;
        }
    }
}
