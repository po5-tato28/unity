using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Storage;

public class UploadScreenshot : MonoBehaviour
{
    public void StartUpload(Texture2D screenshot)
    {
        StartCoroutine(UploadCoroutine(screenshot));
    }

    private IEnumerator UploadCoroutine(Texture2D screenshot)
    {
        FirebaseStorage storage = FirebaseStorage.DefaultInstance;
        StorageReference screenshotReference = storage.GetReferenceFromUrl("gs://ougi-fb.appspot.com").Child($"/screenshots/{Guid.NewGuid()}.png");

        var metadataChange = new MetadataChange()
        {
            ContentEncoding = "image/png",
            /*
            CustomMetadata = new Dictionary<string, string>()
            {
                { "Position", Camera.main.transform.position.ToString()},
                { "Rotation", Camera.main.transform.position.ToString()}
            }
            */
        };

        var bytes = screenshot.EncodeToPNG();
        var uploadTask = screenshotReference.PutBytesAsync(bytes, metadataChange);
        yield return new WaitUntil(() => uploadTask.IsCompleted);

        if(uploadTask.Exception != null)
        {
            Debug.LogError($"업로드에 실패했습니다. {uploadTask.Exception}");
            yield break;
        }

        var getUrlTask = screenshotReference.GetDownloadUrlAsync();
        yield return new WaitUntil(() => getUrlTask.IsCompleted);

        if(getUrlTask.Exception != null)
        {
            Debug.LogError($"다운로드 실패 {getUrlTask.Exception}");
            yield break;
        }

        Debug.Log($"{getUrlTask.Result}에서 받아옴");
    }
}
