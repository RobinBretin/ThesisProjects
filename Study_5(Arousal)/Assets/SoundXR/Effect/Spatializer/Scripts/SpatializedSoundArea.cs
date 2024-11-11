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
/// @file SpatializedSoundArea.cs
/// @brief setting the spatialized sound area
///

using UnityEngine;

namespace Soundxr.Effect.Spatializer
{
    [AddComponentMenu("Sound xR/Effect/Spatializer/SpatializedSoundArea")]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(AudioSource))]
    public class SpatializedSoundArea : MonoBehaviour
    {

        public bool area = true;

        public GameObject playbackArea;

        [Range(0.0f, 50.0f)]
        public float decayArea = 2.0f;

        private AudioSource _audioSource;

        private AudioListener _audioListener;

        void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioListener = GameObject.FindObjectOfType<AudioListener>();
            if (_audioListener == null)
                Debug.LogWarning("There are no audio listeners in the scene.");
        }

        void Update()
        {
            if (!area || playbackArea == null || _audioListener == null)
                return;

            var areaTransform = playbackArea.transform;
            var bounds1 = new Bounds(Vector3.zero, areaTransform.lossyScale);
            var bounds2 = bounds1;
            bounds2.Expand(decayArea * 2);

            var ptSource = areaTransform.worldToLocalMatrix.MultiplyPoint3x4(transform.position);
            var ptListener = areaTransform.worldToLocalMatrix.MultiplyPoint3x4(_audioListener.transform.position);
            ptSource.Scale(areaTransform.lossyScale);
            ptListener.Scale(areaTransform.lossyScale);

            Place typeSource = CheckPlace(bounds1, bounds2, ptSource);
            Place typeListener = CheckPlace(bounds1, bounds2, ptListener);

            float volume = 1.0f; // 100%

            switch(typeListener)
            {
                case Place.Inside:
                    switch (typeSource)
                    {
                        case Place.Inside:
                            volume = 1.0f;
                            break;
                        case Place.Middle:
                            // Decay according to AudioSource position.
                            volume = CalcDecay(bounds1, decayArea, ptSource);
                            break;
                        case Place.Outside:
                            volume = 0.0f;
                            break;
                    }
                    break;

                case Place.Middle:
                    switch (typeSource)
                    {
                        case Place.Inside:
                            // Decay according to AudioListener position.
                            volume = CalcDecay(bounds1, decayArea, ptListener);
                            break;
                        case Place.Middle:
                        case Place.Outside:
                            volume = 1.0f;
                            break;
                    }
                    break;

                case Place.Outside:
                    switch (typeSource)
                    {
                        case Place.Inside:
                            volume = 0.0f;
                            break;
                        case Place.Middle:
                        case Place.Outside:
                            volume = 1.0f;
                            break;
                    }
                    break;
            }

            _audioSource.volume = volume;
        }

        enum Place { Inside, Middle, Outside }

        static Place CheckPlace(Bounds inner, Bounds outer, Vector3 pt)
        {
            if (inner.Contains(pt))
                return Place.Inside;
            if (outer.Contains(pt))
                return Place.Middle;
            return Place.Outside;
        }

        static float CalcDecay(Bounds bounds, float distanceMax, Vector3 pt)
        {
            var closest = bounds.ClosestPoint(pt);
            var distance = Vector3.Distance(closest, pt);
            return Mathf.Lerp(1.0f, 0.0f, distance / distanceMax);
        }

        //void OnDrawGizmos()
        void OnDrawGizmosSelected()
        {
            if (!enabled || !area)
                return;
            if (playbackArea == null)
                return;
            var areaTransform = playbackArea.transform;

            var cache = Gizmos.matrix;
            Gizmos.matrix = Matrix4x4.TRS(areaTransform.position, areaTransform.rotation, Vector3.one);

            Gizmos.color = colorInner;
            Gizmos.DrawWireCube(Vector3.zero, areaTransform.lossyScale);

            float offset = decayArea * 2;
            Gizmos.color = colorOuter;
            Gizmos.DrawWireCube(Vector3.zero, areaTransform.lossyScale + new Vector3(offset, offset, offset));

            Gizmos.matrix = cache;
        }

        static readonly Color colorInner = (Color)new Color32(0x4B, 0x1E, 0x78, 0xFF);
        static readonly Color colorOuter = (Color)new Color32(0x7D, 0x60, 0x99, 0xFF); 

    }
}
