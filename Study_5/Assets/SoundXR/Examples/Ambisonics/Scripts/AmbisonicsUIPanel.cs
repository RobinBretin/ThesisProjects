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
/// @file AmbisonicsUIPanel.cs
/// @brief Sample AmbisonicsAudioFilter
///

using System;
using UnityEngine;

namespace Soundxr.Effect.Spatializer {
namespace Examples {

[AddComponentMenu("Sound xR/Effect/Spatializer/Examples/AmbisonicsUIPanel")]
public class AmbisonicsUIPanel : MonoBehaviour
{
    public AmbisonicsAudioFilter ambisonics;
    public AudioListener listener;

    Rect windowRect;
    bool visibleSelectWindow = false;
    Vector2 scrollPosition = Vector2.zero;
    string[] audioItems;
    Vector3 eulerAngles;

    const String EXTENSION = "wav";

    void Start()
    {
        windowRect = CalcSelectWindowRect();

        audioItems = Soundxr.Common.FileSystem.Directory.GetFiles(Application.streamingAssetsPath, EXTENSION, true);

        PullEulerAngles();
#if UNITY_EDITOR
        VSAttributionWrapper.SendAttributionEvent("AmbisonicsSample");
#endif
    }

    void OnValidate()
    {
        PullEulerAngles();
    }

    void OnDisable()
    {
#if UNITY_IOS && !UNITY_EDITOR
        bookmark = null;
#endif
    }

    void OnGUI()
    {
        using (new GUILayout.AreaScope(new Rect(10, 10, Screen.width - 20, Screen.height - 20)))
        {
            DrawTransform("<Listener>", listener ? listener.transform : null);
            GUILayout.FlexibleSpace();

            using (new GUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();
                DrawAmbisonicSetting();
            }
            DrawAudioInfoGUI();

            if (visibleSelectWindow)
                windowRect = GUILayout.Window(0, windowRect, DoAudioSelectWindow, "Select HOA", StyleConstant.windwow);
        }
    }

    void DrawAmbisonicSetting()
    {
        var valid = ambisonics != null;
        GUI.enabled = valid;
        using (new GUILayout.VerticalScope("box"))
        {
            GUILayout.Label("<Ambisonics Audio Filter>");
            var labelWidth = 100;
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label("HOA File : ", GUILayout.Width(labelWidth));
                var audioName = valid ? ambisonics.audioFile : "";
                if (audioName != null && audioName.Length > 0) {
                    var names = audioName.Split('/');
                    audioName = names[names.Length - 1];
                }
                GUILayout.Label($"{audioName}", GUILayout.MinWidth(120));
                if (GUILayout.Button("...", GUILayout.Width(25)))
                {
                    Soundxr.Common.FileSystem.FileDialog.OpenFileDialog("Select HOA File", null, EXTENSION, (Soundxr.Common.FileSystem.FileDialog.FileInfo info) => {
#if UNITY_IOS && !UNITY_EDITOR
                        bookmark = info.bookmark;
#endif
                        var uri = new Uri(info.path);
                        if (uri.IsFile)
                            ambisonics.audioFile = uri.LocalPath;
#if UNITY_IOS && !UNITY_EDITOR
                        else
                            bookmark = null;
#endif
                    });
                }
                if (GUILayout.Button("...", GUILayout.Width(25)))
                {
                    visibleSelectWindow = true;
                }
            }
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label("Order : ", GUILayout.Width(labelWidth));

                const int ORDER_MAX = 5;
                var audioOrder = 0;
                if (valid) {
                    audioOrder = ambisonics.audioOrder > 0 ? ambisonics.audioOrder : ORDER_MAX;
                    var items = new string[audioOrder];
                    for (int i = 0; i < items.Length; i++)
                        items[i] = $"{i + 1}";
                    var order = (valid ? ambisonics.order : 0) - 1;
                    int selected = GUILayout.SelectionGrid(order, items, items.Length);
                    if (valid && order != selected)
                        ambisonics.order = selected + 1;
                }
                GUI.enabled = false;
                for (int i = audioOrder; i < ORDER_MAX; i++)
                    GUILayout.Button($"{i + 1}");
            }

            GUI.enabled = valid;
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label("Speaker Set : ", GUILayout.Width(labelWidth));
                var items = new string[] { "12", "20", "32", "42" };
                var value = valid ? (int)ambisonics.speakerSet : -1;
                int newValue = GUILayout.SelectionGrid(value, items, 4);
                if (valid && value != newValue)
                    ambisonics.speakerSet = (AmbisonicsAudioFilter.SpeakerSet)newValue;
            }
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label("Distance Decay : ", GUILayout.Width(labelWidth));
                var value = valid ? ambisonics.distanceDecay : false;
                var newValue = GUILayout.Toggle(value, "");
                if (valid && value != newValue)
                    ambisonics.distanceDecay = newValue;
            }
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label("Decay Curve : ", GUILayout.Width(labelWidth));
                var items = new string[] { "Slow", "Normal", "Fast" };
                var value = valid ? (int)ambisonics.decayCurve : -1;
                int newValue = GUILayout.SelectionGrid(value, items, 3);
                if (valid && value != newValue)
                    ambisonics.decayCurve = (AmbisonicsAudioFilter.DecayCurve)newValue;
            }
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label("HRTF Type : ", GUILayout.Width(labelWidth));
                var items = new string[] { "AVG80", "TC4" };
                var value = valid ? (int)ambisonics.HRTFType : -1;
                int newValue = GUILayout.SelectionGrid(value, items, 2);
                if (valid && value != newValue)
                    ambisonics.HRTFType = (AmbisonicsAudioFilter.Preset)newValue;
            }
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label("Loop : ", GUILayout.Width(labelWidth));
                var value = valid ? ambisonics.loop : false;
                var newValue = GUILayout.Toggle(value, "");
                if (valid && value != newValue)
                    ambisonics.loop = newValue;
            }
        }
    }

    void DrawAudioInfoGUI()
    {
        bool valid = ambisonics && ambisonics.validAudio;
        GUI.enabled = valid;
        using (new GUILayout.VerticalScope("box"))
        {
            var total = 0.0f;
            var nowTime = 0.0f;
            if (valid)
            {
                int audioOrder = ambisonics.audioOrder;
                var samples = ambisonics.audioSamples;
                total = ambisonics.audioLength;
                GUILayout.Label($"Order={ambisonics.audioOrder},  Length={samples}[samples]", StyleConstant.label);
                nowTime = ambisonics.time;
            }

            using (new GUILayout.HorizontalScope())
            {
                if (valid)
                    GUILayout.Label(SecondToString(nowTime), GUILayout.ExpandWidth(false));
                using (new GUILayout.VerticalScope())
                {
                    GUILayout.Space(9);
                    var newTime = GUILayout.HorizontalSlider(nowTime, 0, total);
                    if (valid && nowTime != newTime)
                        ambisonics.time = newTime;
                }
                if (valid)
                    GUILayout.Label(SecondToString(total), GUILayout.ExpandWidth(false));
            }
            DrawPlayerGUI();
        }
    }

    void DrawPlayerGUI()
    {
        using (new GUILayout.HorizontalScope())
        {
            GUILayout.FlexibleSpace();
            bool valid = ambisonics && ambisonics.validAudio;
            GUI.enabled = valid && !ambisonics.Playing;
            if (GUILayout.Button("Play", GUILayout.MinWidth(60)))
            {
                ambisonics.Play();
            }
            GUI.enabled = valid && (ambisonics.Playing || ambisonics.Paused);
            if (GUILayout.Button("Stop", GUILayout.MinWidth(60)))
            {
                ambisonics.Stop();
            }
            GUI.enabled = valid && !ambisonics.Paused;
            if (GUILayout.Button("Pause", GUILayout.MinWidth(60)))
            {
                ambisonics.Pause();
            }
            GUI.enabled = valid && ambisonics.Paused;
            if (GUILayout.Button("UnPause", GUILayout.MinWidth(60)))
            {
                ambisonics.UnPause();
            }
            GUILayout.FlexibleSpace();
        }
    }

    void DrawTransform(string label, Transform t)
    {
        GUI.enabled = t != null;
        using (new GUILayout.VerticalScope("box"))
        {
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label($"{label}");
                if (GUILayout.Button("Reset", GUILayout.ExpandWidth(false)))
                {
                    eulerAngles = Vector3.zero;
                    t.eulerAngles = eulerAngles;
                }
            }
            DrawTransformEulerAngle(ref eulerAngles, t, 0);
            DrawTransformEulerAngle(ref eulerAngles, t, 1);
            DrawTransformEulerAngle(ref eulerAngles, t, 2);
        }
    }

    void DrawTransformEulerAngle(ref Vector3 angles, Transform t, int index)
    {
        using (new GUILayout.HorizontalScope())
        {
            GUI.enabled = t != null;
            var axis = index == 0 ? "X" : index == 1 ? "Y" : "Z";
            GUILayout.Label($"Rotation {axis}", GUILayout.Width(65));
            var value = angles[index];
            using (new GUILayout.VerticalScope())
            {
                GUILayout.Space(9);
                value = GUILayout.HorizontalSlider(value, -180.0f, 180.0f);
                if (t != null && value != angles[index])
                {
                    angles[index] = value;
                    t.eulerAngles = angles;
                }
            }
            GUILayout.Label($"{value:f1}", StyleConstant.label, GUILayout.Width(36));
        }
    }

    void PullEulerAngles()
    {
        if (listener == null)
        {
            eulerAngles = Vector3.zero;
        }
        else
        {
            eulerAngles = listener.transform.eulerAngles;
            Func<float, float> normalizeAngle = (a) => a > 180.0f ? (a - 360.0f) : (a <= -180.0f ? (a + 360.0f) : a);
            for (int i = 0; i < 3; i++)
                eulerAngles[i] = normalizeAngle(eulerAngles[i]);
        }
    }

    static class StyleConstant
    {
        public static GUIStyle windwow;
        public static GUIStyle box;
        public static GUIStyle label;
        public static GUIStyle list;

        static StyleConstant()
        {
            var darkColor = new Color(0.3f, 0.3f, 0.3f, 1.0f);
            var nonTransparentTexture = new Texture2D(1, 1);
            nonTransparentTexture.SetPixel(0, 0, darkColor);
            nonTransparentTexture.Apply();

            windwow = new GUIStyle("window");
            windwow.normal.background = nonTransparentTexture;

            box = new GUIStyle("box");
            box.normal.background = nonTransparentTexture;

            label = new GUIStyle("label");
            label.alignment = TextAnchor.MiddleRight;

            list = new GUIStyle("button");
            list.alignment = TextAnchor.MiddleLeft;
        }
    }

    string SecondToString(float sec)
    {
        int _sec1 = (int)sec;
        int _sec2 = _sec1 % 60;
        int _min1 = _sec1 / 60;
        int _min2 = _min1 % 60;
        int _msec = (int)Math.Round((sec - _sec1) * 1000);
        int _hour = _min1 / 60;
        if (_hour > 0)
            return $"{_hour}:{_min2:d2}:{_sec2:d2}.{_msec:d3}";
        return $"{_min2:d2}:{_sec2:d2}.{_msec:d3}";
    }

    Rect CalcSelectWindowRect()
    {
        const int MARGIN = 30;
        const int WIDTH = 300;
        int width = WIDTH;
        if (Screen.width < WIDTH + MARGIN * 2)
            width = Screen.width - MARGIN * 2;

        int left = (Screen.width - width) / 2;
        return new Rect(Screen.width - width - MARGIN, MARGIN, width, Screen.height - MARGIN * 2);
    }

    void DoAudioSelectWindow(int windowID)
    {
        using (new GUILayout.VerticalScope(StyleConstant.box))
        {
            using (var scrollViewScope = new GUILayout.ScrollViewScope(scrollPosition))
            {
                scrollPosition = scrollViewScope.scrollPosition;

                if (GUILayout.Button($"None", StyleConstant.list))
                {
                    ambisonics.audioFile = null;
                    visibleSelectWindow = false;
#if UNITY_IOS && !UNITY_EDITOR
                    bookmark = null;
#endif
                }
                foreach (var item in audioItems)
                {
                    if (GUILayout.Button($"{item}", StyleConstant.list))
                    {
                        ambisonics.audioFile = item;
                        visibleSelectWindow = false;
#if UNITY_IOS && !UNITY_EDITOR
                        bookmark = null;
#endif
                    }
                }
            }

            if (GUILayout.Button("Cancel"))
            {
                visibleSelectWindow = false;
            }
        }
        GUI.DragWindow();
    }

#if UNITY_IOS && !UNITY_EDITOR

    byte[] _bookmark = null;

    byte[] bookmark
    {
        get {
            return _bookmark;
        }
        set {
            if (_bookmark != null)
                Soundxr.Common.FileSystem.FileDialog.ReleaseBookmark(_bookmark);
            _bookmark = value;
        }
    }

#endif

}

} // namespace Examples
} // namespace Soundxr.Effect.Spatializer
