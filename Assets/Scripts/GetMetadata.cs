using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Firebase.Storage;
using UnityEngine;
using UnityEngine.Events;


public class GetMetadata : MonoBehaviour
{
    public void GetMetadataTask(string path)
    {
        // Create reference to the file whose metadata we want to retrieve
        var storage = FirebaseStorage.DefaultInstance;
        // 파일이름 임의로 지정
        var screenshotReference = storage.GetReference(path).Child($"/screenshots/2020-07-29+22-38-14.png");

        screenshotReference.GetMetadataAsync().ContinueWith((Task<StorageMetadata> task) =>
        {
            if(task.Exception != null)
            {
                Debug.LogError("메타데이터 수집에 실패했습니다." + task.Exception);
            }
            else
            {
                StorageMetadata meta = task.Result;
                Debug.Log("메타데이터 수집에 성공했습니다." + meta.Path);
            }
        });
       
    }
}
