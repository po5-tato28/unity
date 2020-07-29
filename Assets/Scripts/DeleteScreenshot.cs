using System;
using System.Collections;
using System.Collections.Generic;
using Firebase.Storage;
using UnityEngine;
using UnityEngine.Events;

public class DeleteScreenshot : MonoBehaviour
{
    public void Trigger(string path)
    {
        StartCoroutine(DeleteScreenshotCoroutine(path));
    }

    private IEnumerator DeleteScreenshotCoroutine(string path)
    {
        var storage = FirebaseStorage.DefaultInstance;
        // 테스트할 때마다 경로 바꿔서 테스트 가능
        var screenshotReference = storage.GetReference(path).Child($"/screenshots/44799135-88f8-4350-b473-0757ea3a97e8.png");
        
        var deleteTask = screenshotReference.DeleteAsync();
        yield return new WaitUntil(() => deleteTask.IsCompleted);
        if (deleteTask.Exception != null)
        {
            Debug.LogError($"삭제에 실패했습니다. {deleteTask.Exception}");
            yield break;
        }
        else
        {
            Debug.Log("삭제 완료" + screenshotReference);
        }

        yield return null;
    }
}
