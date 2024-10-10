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
using UnityEngine.EventSystems;

namespace Soundxr.Effect.Spatializer.Examples
{
    /// <summary>
    /// Rotate objects on swipe
    /// </summary>
    public class SwipeController
        : MonoBehaviour
        , IBeginDragHandler
        , IDragHandler
        , IEndDragHandler
    {
        /// <summary>
        /// target Transform
        /// </summary>
        public Transform target;

        public enum GestureType { Translate, Rotate }

        public GestureType type = GestureType.Rotate;

        public float coef = 1.0f;

        bool dragging = false;

        Vector2 touchPosition;

        Vector3 delta;

        const float moveThreshold = 64f;

        Vector3 resetPosition = Vector3.zero;
        Quaternion resetRotation = Quaternion.identity;

        void Awake()
		{
            if (target == null)
                return;

            resetPosition = target.position;
            resetRotation = target.rotation;
        }
        void Update()
        {
            if (type != GestureType.Translate)
                return;
            if (!dragging || target == null)
                return;

            // Continue to translate in the dragged direction
            target.Translate(delta * Time.deltaTime);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            delta = Vector3.zero;
            touchPosition = eventData.position;
            dragging = true;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            dragging = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (target == null)
                return;

            switch (type)
            {
                case GestureType.Translate:
                    {
                        Vector2 diff = eventData.position - touchPosition;
                        if (Vector2.SqrMagnitude(diff) < moveThreshold * moveThreshold)
                        {
                            delta = Vector2.zero;
                        }
                        else
						{
                            var value = diff.normalized * coef;
                            delta = new Vector3(value.x, 0.0f, value.y);
                        }
                        break;
                    }
                case GestureType.Rotate:
                    {
                        delta = Vector2.zero;

                        target.Rotate(0.0f, eventData.delta.x * coef, 0.0f, Space.World);
                        target.Rotate(-eventData.delta.y * coef, 0.0f, 0.0f, Space.Self);
                        break;
                    }
            }

        }

		public void Reset()
		{
            if (target == null)
                return;

            switch (type)
            {
                case GestureType.Translate:
                    target.position = resetPosition;
                    break;

                case GestureType.Rotate:
                    target.rotation = resetRotation;
                    break;
            }
        }
    }
}
