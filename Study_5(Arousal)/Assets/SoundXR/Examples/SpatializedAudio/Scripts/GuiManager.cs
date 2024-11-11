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

using UnityEngine;
using UnityEngine.UI;

namespace Soundxr.Effect.Spatializer.Examples
{
    /// <summary>
    /// Manage screens according to status and GUI operations
    /// </summary>
    public class GuiManager : MonoBehaviour
    {
        /// <summary>
        /// gyroscope button
        /// </summary>
        public Toggle gyroButton = null;

        /// <summary>
        // SwipeController for rotation
        /// </summary>
        public SwipeController rotationPanel = null;

        /// <summary>
        // SwipeController for moving
        /// </summary>
        public SwipeController movingPanel = null;

        /// <summary>
        /// Gyroscope component
        /// </summary>
        public Gyroscope gyroscope = null;

        private Color selectedColor = (Color)new Color32(0x4B, 0x1E, 0x78, 0xFF);

        void Awake()
        {
            Init();
        }

        public void Init()
        {
            if (Application.platform != RuntimePlatform.Android && Application.platform != RuntimePlatform.IPhonePlayer)
            {
                gyroButton.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// processing when operating the Gyroscope toggle button
        /// </summary>
        public void ToggleGyroscope()
        {
            var on = gyroButton.isOn;
            gyroButton.GetComponent<Image>().color = on ? selectedColor : Color.white;
            gyroButton.GetComponentInChildren<Text>().color = on ? Color.white : Color.black;
            rotationPanel.gameObject.SetActive(!on);
        }

        public void ResetAttitude()
        {
            gyroscope?.Reset();
            rotationPanel?.Reset();
            movingPanel?.Reset();
        }
    }
}
