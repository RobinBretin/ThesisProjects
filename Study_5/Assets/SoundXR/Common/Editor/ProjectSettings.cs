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
/// @file ProjectSettings.cs
/// @brief Unity Project Settings
///

using System.Linq;
using UnityEngine;
using UnityEditor;

namespace Soundxr {

[InitializeOnLoad]
public static class ProjectSettings {
    /// System Sample Rate.
    /// Unity's default sample rate for iOS/Android is 24kHz.
    /// we strongly recommend setting to 48kHz.
    public static int AudioSampleRate { get; } = 48000; // [Hz]

    static ProjectSettings() {
        //--- AudioManager
        string assetPath = "ProjectSettings/AudioManager.asset";
        Object manager = AssetDatabase.LoadAllAssetsAtPath(assetPath).FirstOrDefault();
        SerializedObject obj = new SerializedObject(manager);
        // Project Settings > Audio > System Sample Rate
        obj.FindProperty("m_SampleRate").intValue = AudioSampleRate;
        obj.ApplyModifiedProperties();
    }
}

} // namespace Soundxr
