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
/// @file SpatializedAudioSource.cs
/// @brief edit the parameters of the spatializer
///

using UnityEngine;

namespace Soundxr.Effect.Spatializer {

    /// For editing the Spatializer parameters of an AudioSource. Add for each AudioSource object.
    [AddComponentMenu("Sound xR/Effect/Spatializer/SpatializedAudioSource")]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(AudioSource))]
    public class SpatializedAudioSource : MonoBehaviour {

        /// distance decay curve type
        public enum DecayCurve {
            slow = 0, ///< slow decay
            normal, ///< normal decay
            fast ///< fast decay
        }

        /// HRTF preset
        public enum Preset {
            AVG80,
            TC4,
        }

        // default parameters
        private const float volumeDefault = 0.0f;
        private const bool distanceDecayDefault = false;
        private const DecayCurve decayCurveDefault = DecayCurve.normal;
        private const Preset hrtfTypeDefault = Preset.AVG80;

        // encapsulated parameters
        [Tooltip("set output volume [dB]"), SerializeField, Range(-96.0f, 20.0f)]
        private float _volume = volumeDefault;
        [Tooltip("enable distance decay"), SerializeField]
        private bool _distanceDecay = distanceDecayDefault;
        [Tooltip("set distance decay curve"), SerializeField]
        private DecayCurve _decayCurve = decayCurveDefault;
        [Tooltip("set HRTF type"), SerializeField]
        private Preset _HRTFType = hrtfTypeDefault;

        private AudioSource _audioSource = null;
        private AudioSource audioSource {
            get {
                if (_audioSource == null) {
                    _audioSource = GetComponent<AudioSource>();
                    if (_audioSource == null) {
                        Debug.LogWarning(gameObject.name + ": Not found AudioSource.");
                    }
                }
                return _audioSource;
            }
        }

        private bool _dirty = true;

#region accessible parameters
        /// output volume [dB]
        public float volume {
            get { return _volume; }
            set {
                _dirty |= !SetNativeParameterVolume(value);
                _volume = value;
            }
        }
        /// distance decay On/Off
        public bool distanceDecay {
            get { return _distanceDecay; }
            set {
                _dirty |= !SetNativeParameterDistanceDecay(value);
                _distanceDecay = value;
            }
        }
        /// distance decay rate ( when DistanceDecay is enabled )
        public DecayCurve decayCurve {
            get { return _decayCurve; }
            set {
                _dirty |= !SetNativeParameterDecayCurve(value);
                _decayCurve = value;
            }
        }
        /// HTRF type
        public Preset HRTFType {
            get { return _HRTFType; }
            set {
                _dirty |= !SetNativeParameterPreset(value);
                _HRTFType = value;
            }
        }
#endregion

#region Unity Event
        private void Awake() {
            Soundxr.Common.SoundxrInitalizer.Init();
        }

        private void OnEnable() {
            _dirty = true;
#if UNITY_EDITOR
            VSAttributionWrapper.SendAttributionEvent("SpatializedAudioSource");
#endif
        }

        private void Update() {
            if (audioSource) {
                if (_dirty && audioSource.enabled && audioSource.spatialize) {
                    SetParameter();
                }
                else if (!_dirty && (!audioSource.enabled || !audioSource.spatialize)) {
                    _dirty = true;
                }
            }
        }

        /// this function is called when the script is loaded or a value is changed in the Inspector. (Editor Only)
        private void OnValidate() {
            _dirty = true;
        }
#endregion

        private void SetParameter() {
            _dirty = false;

            volume = _volume;
            distanceDecay = _distanceDecay;
            decayCurve = _decayCurve;
            HRTFType = _HRTFType;
        }

#region Native Audio Plugin's Interfaces
        // parameter index of the spatializer.
        private enum NativePluginParameterIndex {
            volume = 0,
            distanceDecay,
            decayCurve,
            hrtfType,
        }

        private bool SetNativeParameter(NativePluginParameterIndex index, float value) {
            if (!enabled || !audioSource || !audioSource.enabled || !audioSource.spatialize) {
                return false;
            }
            return audioSource.SetSpatializerFloat((int)index, value);
        }

        /// @brief set the spatializer's volume
        /// @param[in] value output volume [dB]
        /// @return true:succeded, false:failed
        private bool SetNativeParameterVolume(float value) {
            return SetNativeParameter(NativePluginParameterIndex.volume, value);
        }

        /// @brief enable or disable the spatializer's distance decay
        /// @param[in] value true:enable, false:disable
        /// @return succeded:true, failed:false
        private bool SetNativeParameterDistanceDecay(bool value) {
            return SetNativeParameter(NativePluginParameterIndex.distanceDecay, value ? 1.0f : 0.0f);
        }

        /// @brief set the distance decay curve
        /// @param[in] value slow/normal/falst
        /// @return succeded:true, failed:false
        private bool SetNativeParameterDecayCurve(DecayCurve value) {
            return SetNativeParameter(NativePluginParameterIndex.decayCurve, (float)value);
        }

        /// @brief set HRTF preset
        /// @param[in] value AVG80/TC4
        /// @return succeded:true, failed:false
        private bool SetNativeParameterPreset(Preset value) {
            return SetNativeParameter(NativePluginParameterIndex.hrtfType, (float)value);
        }
#endregion
    }
} // namespace Soundxr.Effect.Spatializer
