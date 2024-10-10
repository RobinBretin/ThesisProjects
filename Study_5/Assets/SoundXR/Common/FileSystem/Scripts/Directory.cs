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
/// @file Directory.cs
/// @brief Directory class
///

using System;
using System.Text.RegularExpressions;
#if UNITY_ANDROID && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

namespace Soundxr.Common.FileSystem {

    public static class Directory {

#if UNITY_ANDROID && !UNITY_EDITOR

        private delegate void GetFilesCallback(string filename);

        [AOT.MonoPInvokeCallback(typeof(GetFilesCallback))]
        private static void OnGetFilesCallback(string filename)
        {
            Array.Resize(ref files, files.Length + 1);
            files[files.Length - 1] = filename;
        }

        static string[] files = null;

#endif

        static public string[] GetFiles(string dir, string extension, bool recursive)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            const string KEY__JAR_SCHEME = "jar:file://";
            const string SEPARATOR_APK = "!/";
            if (dir.StartsWith(KEY__JAR_SCHEME)) // Inside Unity's Assets/StreamingAsset/ directory
            {
                var p1 = dir.Substring(KEY__JAR_SCHEME.Length);
                int pos = p1.IndexOf(SEPARATOR_APK); // Separate jar(=apk,zip) file and contents
                var apkPath = p1.Substring(0, pos); // extension apk, zip format
                var assetPath = p1.Substring(pos + SEPARATOR_APK.Length); // reference file path in the apk

                files = new string[0];
                Binding.SoundxrFileSystem_GetFilenamesInZip(apkPath, assetPath, extension, recursive, OnGetFilesCallback);
                return files;
            }
#endif

            if (!System.IO.Directory.Exists(dir))
                return new string[0];

            var rx = new Regex($"\\.({extension})$", RegexOptions.IgnoreCase);
            var option = recursive ? System.IO.SearchOption.AllDirectories : System.IO.SearchOption.TopDirectoryOnly;
            var paths = System.IO.Directory.GetFiles(dir, "*", option);
            var results = new string[0];
            for (int i = 0; i < paths.Length; i++)
            {
                if (rx.IsMatch(paths[i])) {
                    var path = paths[i].Substring(dir.Length);
                    path = path.Replace("\\", "/");
                    if (path.StartsWith("/"))
                        path = path.Substring(1);
                    Array.Resize(ref results, results.Length + 1);
                    results[results.Length - 1] = path;
                }
            }
            return results;
        }

#if UNITY_ANDROID && !UNITY_EDITOR

        static class Binding {
            private const string LIBNAME = "SoundxrFileSystem";
            [DllImport(LIBNAME)] internal static extern int SoundxrFileSystem_GetFilenamesInZip(string zip, string dir, string filters, bool recursive, GetFilesCallback callback);
        }

#endif
    }

} // namespace Soundxr.Common.FileSystem
