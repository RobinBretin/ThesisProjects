/*!
 * Copyright 2022 Yamaha Corp. All Rights Reserved.
 * 
 * The content of this file includes portions of the Yamaha Sound xR
 * released in source code form as part of the SDK package.
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
/// @file FileSystemBuildProcessor.cs
/// @brief pre/post build process (related to filesystems)
///

using UnityEditor;
using UnityEditor.Callbacks;
#if UNITY_IOS
using System.IO;
using UnityEditor.iOS.Xcode;
#endif

namespace Soundxr.Common.FileSystem {

    public static class FileSystemBuildProcessor
    {
        [PostProcessBuild]
        public static void OnPostProcessBuild(BuildTarget buildTarget, string buildPath)
        {
#if UNITY_IOS
            if (buildTarget == BuildTarget.iOS)
            {
                ModifyXcodePlist(buildPath);
            }
#endif
        }


#if UNITY_IOS
        // Change file sharing settings in plist
        private static void ModifyXcodePlist(string buildPath)
        {
            string filePath = Path.Combine(buildPath, "Info.plist");
            if (!File.Exists(filePath))
                return;

            PlistDocument plist = new PlistDocument();
            
            plist.ReadFromString(File.ReadAllText(filePath));

            PlistElementDict rootDict = plist.root;
            // "Application supports iTunes file sharing" = YES
            rootDict.SetBoolean("UIFileSharingEnabled", true);
            // "Supports opening documents in place" = YES
            rootDict.SetBoolean("LSSupportsOpeningDocumentsInPlace", true);

            File.WriteAllText(filePath, plist.WriteToString());
        }
#endif
    }

} // namespace Soundxr.Common.FileSystem
