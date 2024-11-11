/*!
 * Copyright 2022 Yamaha Corp. All Rights Reserved.
 * 
 * The content of this file includes portions of the Yamaha Sound xR
 * released in source code form as part of the plugin package.
 * 
 * Commercial License Usage
 * 
 * Licensees holding valid commercial licenses to the Yamaha Sound xR
 * may use this file in accordance with the end user license agreement
 * provided with the software or, alternatively, in accordance with the
 * terms contained in a written agreement between you and Yamaha Corp.
 * 
 * Apache License Usage
 * 
 * Alternatively, this file may be used under the Apache License, Version 2.0 (the "Apache License");
 * you may not use this file except in compliance with the Apache License.
 * You may obtain a copy of the Apache License at 
 * http://www.apache.org/licenses/LICENSE-2.0.
 * 
 * Unless required by applicable law or agreed to in writing, software distributed
 * under the Apache License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES
 * OR CONDITIONS OF ANY KIND, either express or implied. See the Apache License for
 * the specific language governing permissions and limitations under the License.
 */

///
/// @file FileDialogAndroid.cs
/// @brief FileDialog class for Android
///

#if UNITY_ANDROID && !UNITY_EDITOR

using System;
using UnityEngine;
using UnityEngine.Android;

namespace Soundxr.Common.FileSystem {

    internal static class FileDialogAndroid
    {
        private static Action<string> _openFileCallback;

        public static void OpenFileDialog(string title, string directory, string extension, Action<string> cb)
        {
            if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead))
            {
                Debug.LogWarning("No Permission.ExternalStorageRead.");
                return;
            }

            // title unused

            var type = "*/*";
            // if (extension == "wav") type = "audio/wav";
            // do not use extension

            _openFileCallback = cb;

            using (var unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (var activity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity"))
            using (var intent = new AndroidJavaObject("android.content.Intent"))
            {
                intent.Call<AndroidJavaObject>("setAction", "android.intent.action.OPEN_DOCUMENT");
                intent.Call<AndroidJavaObject>("setType", type);
                intent.Call<AndroidJavaObject>("addCategory", "android.intent.category.OPENABLE");
                activity.Call("setListener", new ActivityListener());
                activity.Call("startActivityForResult", intent, REQUEST_CODE);
            }
        }

        private const int REQUEST_CODE = 101;

        public class ActivityListener : AndroidJavaProxy
        {
            public ActivityListener() : base("com.yamaha.soundxr.ExActivityListener") {}

            public void onActivityResult(int requestCode, int resultCode, AndroidJavaObject data)
            {
                if (requestCode != REQUEST_CODE)
                    return;
                if (resultCode != -1) // -1 : Activity.RESULT_OK
                    return;

                using (var uri = data.Call<AndroidJavaObject>("getData"))
                {
                    var path = getPath(uri);
                    if (path != null)
                        _openFileCallback(path);
                }
            }

            private String getPath(AndroidJavaObject uri)
            {
                using (var pathUtils = new AndroidJavaClass("com.yamaha.soundxr.PathUtils"))
                using (var unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                using (var activity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity"))
                using (var context = activity.Call<AndroidJavaObject>("getApplicationContext"))
                {
                    return pathUtils.CallStatic<string>("getPath", uri, context);
                }
            }
        }
    }

} // namespace Soundxr.Common.FileSystem

#endif // UNITY_ANDROID
