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

using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace Soundxr
{
internal class AboutWindow : EditorWindow
{
    [MenuItem("Sound xR/About", false, 1001)]
    static void ShowAboutWindow()
    {
        GetWindowWithRect<AboutWindow>(new Rect(100, 100, 592, 276), true, "About Sound xR");
    }

    readonly string uri = "https://cloud-solutions.yamaha.com/soundxr/";
    readonly string version = "Version 1.4.0";
    readonly string copyright = "© Yamaha Corporation. All Rights Reserved.";

    readonly string logoPng = "Assets/SoundXR/Common/Editor/Images/Sound_xR_G3_white_horizontal_logo.png";
    readonly float logoScale = 0.72f;

    VisualElement linkElement;

    public void OnEnable()
    {
    	var root = rootVisualElement;
    	
        var image = AssetDatabase.LoadAssetAtPath<Texture2D>(logoPng);
        var logo = new Image();
        logo.image = image;
        logo.scaleMode = ScaleMode.StretchToFill;
        logo.style.alignSelf = Align.Center;
        logo.style.width = image.width * logoScale;
        logo.style.height = image.height * logoScale;
        logo.style.marginTop = 15;
        root.Add(logo);

        linkElement = new MyLinkButton(uri);
        root.Add(linkElement);

        root.Add(new MyLabel(version));

        root.Add(new MyLabel(copyright));
#if UNITY_EDITOR
        VSAttributionWrapper.SendAttributionEvent("About");
#endif
    }

    public void OnGUI()
    {
        EditorGUIUtility.AddCursorRect(linkElement.worldBound, MouseCursor.Link);
    }

    private class MyLinkButton : Button
    {
        public MyLinkButton(String uri) : base()
        {
            text = uri;
            clicked += () => Application.OpenURL(uri);

            style.fontSize = 18f;
            style.unityFontStyleAndWeight = FontStyle.Bold;
            style.alignSelf = Align.Center;
            style.color = new Color(.26f, .51f, .75f);
            style.backgroundColor = new Color(0, 0, 0, 0);

            // no border
            style.borderLeftWidth = 0f;
            style.borderTopWidth = 0f;
            style.borderRightWidth = 0f;

            // underline
            style.borderBottomWidth = 1f;
            style.borderBottomColor = style.color;
            style.borderBottomLeftRadius = 0;
            style.borderBottomRightRadius = 0;

            style.marginTop = 30f;
            style.marginBottom = 25f;
            style.paddingBottom = 0f;
        }
    }

    private class MyLabel : Label
    {
        public MyLabel(String label) : base(label)
        {
            style.fontSize = 17f;
            style.unityFontStyleAndWeight = FontStyle.Bold;
            style.alignSelf = Align.Center;
        }
    }

}

} // namespace Soundxr
