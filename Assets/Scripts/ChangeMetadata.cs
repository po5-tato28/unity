using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Storage;

public class ChangeMetadata : MonoBehaviour
{
    const string PATH = "gs://ougi-fb.appspot.com/";

    // Start is called before the first frame update
    void Start()
    {
        var storage = FirebaseStorage.DefaultInstance;
        // 테스트할 때마다 경로 바꿔서 테스트 가능
        var screenshotReference = storage.GetReferenceFromUrl(PATH).Child($"/screenshots/city-4298285_640.jpg");

        #region Set-Metadata
        var metadataChange = new MetadataChange()
        {
            CustomMetadata = new Dictionary<string, string>()
            {
                //{ "Position", Camera.main.transform.position.ToString()},
                { "Location", "카페"}
            }
        };
        #endregion

        screenshotReference.UpdateMetadataAsync(metadataChange).ContinueWith(task => {
            if (!task.IsFaulted && !task.IsCanceled)
            {
                // access the updated meta data
                Firebase.Storage.StorageMetadata meta = task.Result;
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
