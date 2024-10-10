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
/// @file AmbisonicsAudioFilterEditor.cs
/// @brief Editor class of AmbisonicsAudioFilter
///

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Soundxr.Effect.Spatializer {

    [CustomEditor(typeof(AmbisonicsAudioFilter))]
    [CanEditMultipleObjects]
    public class AmbisonicsAudioFilterEditor : Editor
    {
        SerializedProperty audioFileProp;
        SerializedProperty orderProp;
        SerializedProperty speakerSetProp;
        SerializedProperty distanceDecayProp;
        SerializedProperty decayCurveProp;
        SerializedProperty hrtfTypeProp;
        SerializedProperty playOnAwakeProp;

        static readonly string order1 = "1st order";
        static readonly string order2 = "2nd order";
        static readonly string order3 = "3rd order";
        static readonly string order4 = "4th order";
        static readonly string order5 = "5th order";
        static readonly string[][] orderItemsX = {
            new string[] { order1 },
            new string[] { order1, order2 },
            new string[] { order1, order2, order3 },
            new string[] { order1, order2, order3, order4 },
            new string[] { order1, order2, order3, order4, order5 }
        };
        static readonly string[] speakerSetItems = { "12, TypeA", "20, TypeA", "32, TypeA", "42, TypeA"};
        static GUIContent labelAudioFile = new GUIContent("HOA File", "HOA Wave in \"Assets/StreamingAsset/\"folder");
        static GUIContent labelOrder = new GUIContent("Order", "Ambisonics order to use");
        static GUIContent labelSpeakerSet = new GUIContent("Speaker Set", "Virtuality Speaker-Set");

        void OnEnable()
        {
            audioFileProp = serializedObject.FindProperty("_audioFile");
            orderProp = serializedObject.FindProperty("_order");
            speakerSetProp = serializedObject.FindProperty("_speakerSet");
            distanceDecayProp = serializedObject.FindProperty("_distanceDecay");
            decayCurveProp = serializedObject.FindProperty("_decayCurve");
            hrtfTypeProp = serializedObject.FindProperty("_HRTFType");
            playOnAwakeProp = serializedObject.FindProperty("_playOnAwake");
        }

        void OnDisable()
        {
            audioFileProp = null;
            orderProp = null;
            speakerSetProp = null;
            distanceDecayProp = null;
            decayCurveProp = null;
            hrtfTypeProp = null;
            playOnAwakeProp = null;
        }

        public override void OnInspectorGUI()
        {
            // order is less than or equal to HOA file order
            int audioOrder = 5;
            foreach (var data in targets.Cast<AmbisonicsAudioFilter>())
            {
                if (data.audioOrder > 0)
                    audioOrder = Math.Min(audioOrder, data.audioOrder);
            }
            var orderItems = orderItemsX[audioOrder - 1];

            var root = Application.streamingAssetsPath + "/";
            var paths1 = GetFiles(root)
                .Select(value => value.Replace("\\", "/"))
                .ToArray();
            var paths2 = new string[paths1.Length + 1];
            paths2[0] = "";
            Array.Copy(paths1, 0, paths2, 1, paths1.Length);

            int sel = (audioFileProp.hasMultipleDifferentValues) ? -1 : Array.IndexOf(paths2, audioFileProp.stringValue);
            paths2[0] = "None";

            var audioItems = paths2.Select(value => new GUIContent(value, value)).ToArray();

            serializedObject.Update();

            using (var horizontal = new EditorGUILayout.HorizontalScope())
            using (new EditorGUI.PropertyScope(horizontal.rect, labelAudioFile, audioFileProp))
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                var selected = EditorGUILayout.Popup(labelAudioFile, sel, audioItems);
                if (check.changed)
                {
                    audioFileProp.stringValue = selected == 0 ? null : audioItems[selected].text;
                }
            }

            using (var horizontal = new EditorGUILayout.HorizontalScope())
            using (new EditorGUI.PropertyScope(horizontal.rect, labelOrder, orderProp))
            {
                PopupOrder(orderProp, labelOrder, orderItems);
            }

            using (var horizontal = new EditorGUILayout.HorizontalScope())
            using (new EditorGUI.PropertyScope(horizontal.rect, labelSpeakerSet, speakerSetProp))
            {
                PopupEnum(speakerSetProp, labelSpeakerSet, speakerSetItems);
            }

            EditorGUILayout.PropertyField(distanceDecayProp);
            EditorGUILayout.PropertyField(decayCurveProp);
            EditorGUILayout.PropertyField(hrtfTypeProp);
            EditorGUILayout.PropertyField(playOnAwakeProp);

            serializedObject.ApplyModifiedProperties();
        }

        static bool PopupEnum(SerializedProperty property, GUIContent label, string[] items)
        {
            Debug.Assert(property != null);
            int sel = property.hasMultipleDifferentValues ? -1 : property.enumValueIndex;
            bool changed = Popup(label, items, ref sel);
            if (changed)
            {
                property.enumValueIndex = sel;
            }
            return changed;
        }

        static bool PopupOrder(SerializedProperty property, GUIContent label, string[] items)
        {
            Debug.Assert(property != null);
            int sel = property.hasMultipleDifferentValues ? -1 : (property.intValue - 1);
            bool changed = Popup(label, items, ref sel);
            if (changed)
            {
                property.intValue = sel + 1;
            }
            return changed;
        }

        static bool Popup(GUIContent label, string[] items, ref int sel)
        {
            if (sel >= items.Count())
            {
                sel = -1;
            }
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                var selected = EditorGUILayout.Popup(label, sel, items);
                if (check.changed)
                {
                    sel = selected;
                }
                return check.changed;
            }
        }

        static IEnumerable<string> GetFiles(string basePath)
        {
            var files = Soundxr.Common.FileSystem.Directory.GetFiles(basePath, "wav", true);
            foreach (var file in files)
                yield return file;
        }

    }
}
