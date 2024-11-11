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
/// @file PingPong.cs
/// @brief PingPong class
///

using UnityEngine;

namespace Soundxr.Effect.Spatializer {
namespace Examples {

/// Ping-pong move attached object
[AddComponentMenu("Sound xR/Effect/Spatializer/Examples/PingPong")]
public class PingPong : MonoBehaviour {

    public bool move_x = true; ///< move on the x-axis
    public bool move_y = false; ///< move on the y-axis
    public bool move_z = false; ///< move on the z-axis

    /// ping-pong movement amount
    [Range(0.0f, 20.0f)] public float move_range = 10.0f;

    /// center coordinates for ping-pong movement
    private Vector3 offset;

    // Use this for initialization
    void Start () {
        offset = transform.position;
#if UNITY_EDITOR
        VSAttributionWrapper.SendAttributionEvent("SpatializerSample");
#endif
    }

    // Update is called once per frame
    void Update () {
        float delta = Mathf.PingPong(Time.time, move_range) - move_range / 2.0f;
        float dx = move_x ? delta : 0.0f;
        float dy = move_y ? delta : 0.0f;
        float dz = move_z ? delta : 0.0f;
        transform.position = offset + new Vector3(dx, dy, dz);
    }
}

} // namespace Examples
} // namespace Soundxr.Effect.Spatializer