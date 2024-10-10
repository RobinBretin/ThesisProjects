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
/// @file SpatializedBuildProcessor.cs
/// @brief pre/post build process
///

using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Soundxr.Effect.Spatializer
{

    public class SpatializedBuildProcessor
        : IActiveBuildTargetChanged
        , IPreprocessBuildWithReport
        , IPostprocessBuildWithReport
    {
        public int callbackOrder => 0;

        private readonly string messageARM64 = $"Sound xR: Support ARM64 only.";

        public void OnActiveBuildTargetChanged(BuildTarget previousTarget, BuildTarget newTarget)
        {
            if (newTarget == BuildTarget.Android)
            {
                Debug.LogWarning(messageARM64);
            }
        }

        public void OnPreprocessBuild(BuildReport report)
        {
            if (report.summary.platform == BuildTarget.Android)
            {
                Debug.LogWarning(messageARM64);
            }
        }

        public void OnPostprocessBuild(BuildReport report)
        {
            if (report.summary.platform == BuildTarget.iOS) 
            {
                ModifyUnityAppController(report.summary.outputPath);
            }
        }

        // the native audio plugin registration has to be added manually to the generated XCode project.
        private const string txtAudioPluginInterface = "#import \"AudioPluginInterface.h\"";
        private const string txtPreStartUnity_before = "- (void)preStartUnity";
        private const string txtPreStartUnity_after = "- (void)preStartUnity               { UnityRegisterAudioPlugin(&UnityGetAudioEffectDefinitions); }";
        private static void ModifyUnityAppController(string buildPath)
        {
            string filePath = Path.Combine(buildPath, "Classes/UnityAppController.mm");
            if (!File.Exists(filePath))
                return;

            // store the original file
            string filePathOrg = filePath + ".org";
            if (File.Exists(filePathOrg))
                File.Delete(filePathOrg);
            File.Move(filePath, filePathOrg);

            // insert and replace texts
            using (StreamReader sr = new StreamReader(filePathOrg))
            {
                using (StreamWriter sw = File.CreateText(filePath))
                {
                    sw.WriteLine(txtAudioPluginInterface);
                    while (sr.Peek() >= 0)
                    {
                        string line = sr.ReadLine();
                        line = line.Contains(txtPreStartUnity_before) ? txtPreStartUnity_after : line;
                        sw.WriteLine(line);
                    }
                }
            }
        }
    }

} // namespace Soundxr.Effect.Spatializer