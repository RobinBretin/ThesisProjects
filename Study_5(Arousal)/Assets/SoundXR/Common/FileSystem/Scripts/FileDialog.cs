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
/// @file FileDialog.cs
/// @brief FileDialog class
///

using System;

namespace Soundxr.Common.FileSystem {

    public static class FileDialog
    {
        public class FileInfo
        {
            public string path;
#if UNITY_IOS && !UNITY_EDITOR
            public byte[] bookmark;
#endif

#if UNITY_IOS && !UNITY_EDITOR
            public FileInfo(string path, byte[] bookmark)
#else
            public FileInfo(string path)
#endif
            {
                this.path = path;
#if UNITY_IOS && !UNITY_EDITOR
                this.bookmark = bookmark;
#endif
            }
        }

        public static void OpenFileDialog(string title, string directory, string extension, Action<FileInfo> cb)
        {
#if (UNITY_STANDALONE_OSX && !UNITY_EDITOR) || UNITY_EDITOR_OSX
            FileDialogMac.OpenFileDialog(title, directory, extension, (string path) => { cb(new FileInfo(path)); });
#elif (UNITY_STANDALONE_WIN && !UNITY_EDITOR) || UNITY_EDITOR_WIN
            FileDialogWin.OpenFileDialog(title, directory, extension, (string path) => { cb(new FileInfo(path)); });
#elif UNITY_ANDROID && !UNITY_EDITOR
            FileDialogAndroid.OpenFileDialog(title, directory, extension, (string path) => { cb(new FileInfo(path)); });
#elif UNITY_IOS && !UNITY_EDITOR
            FileDialogIOS.OpenFileDialog(title, directory, extension, (string path, byte[] bookmark) => { cb(new FileInfo(path, bookmark)); });
#endif
        }

#if UNITY_IOS && !UNITY_EDITOR
        public static void ReleaseBookmark(byte[] bookmark)
        {
            FileDialogIOS.ReleaseBookmark(bookmark);
        }
#endif

    }

} // namespace Soundxr.Common.FileSystem
