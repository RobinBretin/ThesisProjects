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
    /// Rotate/Move objects with keyboard
    /// </summary>
    public class KeyController : MonoBehaviour
    {
        /// <summary>
        /// target Transform
        /// </summary>
        public Transform target;

        /// <summary>
        /// rotational speed [degree/sec]
        /// </summary>
        public float rotateSpeed = 50.0f;

        /// <summary>
        /// moving speed [m/sec]
        /// </summary>
        public float moveSpeed = 3.0f;

        void Update()
        {
            Rotate();
            Move();
        }

        /// <summary>
        /// change angle by keyboard operation
        /// </summary>
        private void Rotate()
        {
            if (!target)
                return;
            if (rotateSpeed == 0.0f)
                return;

            var value = rotateSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.D))
            {
                target.Rotate(new Vector3(0, value, 0), Space.World);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                target.Rotate(new Vector3(0, -value, 0), Space.World);
            }
            if (Input.GetKey(KeyCode.W))
            {
                target.Rotate(new Vector3(-value, 0, 0), Space.Self);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                target.Rotate(new Vector3(value, 0, 0), Space.Self);
            }
        }

        /// <summary>
        /// change position by keyboard operation
        /// </summary>
        private void Move()
        {
            if (!target)
                return;
            if (moveSpeed == 0.0f)
                return;

            var value = moveSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.UpArrow))
            {
                target.Translate(0, 0, value);
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                target.Translate(0, 0, -value);
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                target.Translate(value, 0, 0);
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                target.Translate(-value, 0, 0);
            }
        }

    }
}
