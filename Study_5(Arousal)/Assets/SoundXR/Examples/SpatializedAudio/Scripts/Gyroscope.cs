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

namespace Soundxr.Effect.Spatializer.Examples
{
    /// <summary>
    /// Rotate objects with gyroscope
    /// </summary>
    public class Gyroscope : MonoBehaviour
    {
        /// <summary>
        /// target Transform
        /// </summary>
        public Transform target;

        Quaternion basisAttitude;

        Quaternion Attitude => GyroToUnity(Input.gyro.attitude);

        float timeReset; // reset time
        Quaternion resetAttitude; // attitude at reset
        Quaternion firstAttitude = Quaternion.identity;

        public bool Enable
        {
            get { return Input.gyro.enabled; }
            set {
                if (target && value && Enable != value)
				{
                    timeReset = Time.time;
                    resetAttitude = target.rotation;
                }
                Input.gyro.enabled = value;
            }
        }

        void Awake()
        {
            Enable = false;
            timeReset = 0.0f;
            firstAttitude = target.rotation;
        }

        void Update()
        {
            if (!Enable || !target)
            {
                return;
            }
            if (timeReset > 0.0f)
            {
                // Wait for a while because there is a time lag between turning on the gyro and getting the value
                if (Time.time < timeReset + 0.5f)
                    return;
                basisAttitude = resetAttitude * Quaternion.Inverse(Attitude);
                timeReset = 0.0f;
            }
            target.rotation = basisAttitude * Attitude; 
        }

        public void Reset()
		{
            if (Enable)
			{
                timeReset = Time.time;
                resetAttitude = firstAttitude;
            }
        }

        static Quaternion GyroToUnity(Quaternion q)
        {
            // Convert left-handed (device) to right-handed (Unity)
            return new Quaternion(q.x, q.y, -q.z, -q.w);
        }
    }

}
