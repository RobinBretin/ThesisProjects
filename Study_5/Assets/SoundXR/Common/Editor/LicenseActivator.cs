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
/// @file LicenseActivator.cs
/// @brief License Activator class
///

using System.Runtime.InteropServices; // DllImport
using UnityEngine; // AudioSettings
using UnityEditor;

namespace Soundxr {

[InitializeOnLoad]
public class LicenseActivator {

    private static string SpatializerPluginName { get; } = "Sound xR Core";

    /// @brief check the license key when Unity loads or this script is recompiled.
    static LicenseActivator() {
        bool activated = Binding.SoundxRActivateLicense(null, 0);

        if (!activated) {
#if UNITY_EDITOR
            VSAttributionWrapper.SendAttributionEvent("ActivationCheckError");
#endif
            Debug.LogError("Sound xR: License is expired.");
        }

        if (activated) {
            CheckSpatializer();
        }

    }

    /// @brief Check the project's spatializer plugin.
    private static void CheckSpatializer() {
        string currentPluginName = AudioSettings.GetSpatializerPluginName();
        if (currentPluginName.Equals(SpatializerPluginName))
            return;
        else 
            Debug.LogWarning("Sound xR: Current spatializer plugin is not " + SpatializerPluginName + ".");
    }

    /// @brief Binding of Native Audio Plugin's methods
    internal static class Binding {
#if UNITY_IOS && !UNITY_EDITOR
        private const string LIBNAME = "__Internal"; // iOS requires static link library
#else
        private const string LIBNAME = "AudioPluginSoundxR";
#endif
        [DllImport(LIBNAME)] internal static extern bool SoundxRActivateLicense(byte[] key, int size);
        [DllImport(LIBNAME)] internal static extern bool SoundxRReturnLicense();
    }
}

} // namespace Soundxr