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
/// @file AmbisonicsAudioFilter.cs
/// @brief AudioFilter component that plays HOA files
///

using System;
using System.IO;
using UnityEngine;

namespace Soundxr.Effect.Spatializer {

    /// AudioFilter component that plays HOA files
    [AddComponentMenu("Sound xR/Effect/Spatializer/AmbisonicsAudioFilter")]
    [RequireComponent(typeof(AudioSource))]
    public class AmbisonicsAudioFilter : MonoBehaviour
    {

#region public enum

        /// virtual speaker type
        public enum SpeakerSet {
            TypeA_12ch,
            TypeA_20ch,
            TypeA_32ch,
            TypeA_42ch
        }

        /// distance decay curve type
        public enum DecayCurve {
            slow, ///< slow decay
            normal, ///< normal decay
            fast ///< fast decay
        }

        /// HRTF preset
        public enum Preset {
            AVG80,
            TC4,
        }

#endregion

#region default parameters

        private const string audioFileDefault = null;
        private const int orderDefault = 1;
        private const SpeakerSet speakerSetDefault = SpeakerSet.TypeA_12ch;
        private const bool distanceDecayDefault = true;
        private const DecayCurve decayCurveDefault = DecayCurve.normal;
        private const Preset hrtfTypeDefault = Preset.AVG80;
        private const bool playOnAwakeDefault = false;

#endregion

#region serialization parameters

        [SerializeField]
        private string _audioFile = audioFileDefault;
        [SerializeField, Range(1, 5), Tooltip("Order")]
        private int _order = orderDefault;
        [SerializeField]
        private SpeakerSet _speakerSet = speakerSetDefault;
        [Tooltip("enable distance decay"), SerializeField]
        private bool _distanceDecay = distanceDecayDefault;
        [Tooltip("set distance decay curve"), SerializeField]
        private DecayCurve _decayCurve = decayCurveDefault;
        [Tooltip("set HRTF type"), SerializeField]
        private Preset _HRTFType = hrtfTypeDefault;
        [SerializeField, Tooltip("Play the sound when the scene loads.")]
        private bool _playOnAwake = playOnAwakeDefault;

#endregion

#region Related components

        private AmbisonicsAudioListener _audioListener = null;
        private AmbisonicsAudioListener audioListener
        {
            get {
                if (_audioListener == null)
                {
                    _audioListener = FindObjectOfType<AmbisonicsAudioListener>();
                    if (_audioListener == null)
                    {
                        Debug.LogWarning("Not found AmbisonicsAudioListener. \nPlease ensure there is always one \"Ambisonics Audio Listener\" with \"Audio Listener\" in the scene.");
                    }
                }
                return _audioListener;
            }
        }

        private AudioSource _audioSource = null;
        private AudioSource audioSource
        {
            get {
                if (_audioSource == null)
                {
                    _audioSource = GetComponent<AudioSource>();
                    if (_audioSource == null)
                    {
                        Debug.LogWarning("Not found AudioSource.");
                    }
                }
                return _audioSource;
            }
        }

#endregion

#region private variable, etc...

        private int _instanceID; ///< instance id (set if already attached to NativePlugin)
        private bool _playing = false; ///< playing flag
        private bool _paused = false; ///< paused flag
        private uint _position = 0; ///< play position [Sample]
        private string _audioFileN; ///< HOA file path already set in NativePlugin
        private int _orderOfFileN; ///< number of orders in HOA file set in NativePlugin
        private uint _lengthOfFileN; ///< number of samples in HOA file set in NativePlugin
        private bool _distanceDecayN; ///< _distanceDecay value already set in NativePlugin
        private int _decayCurveN;  ///< _decayCurve value already set in NativePlugin
        private int _HRTFTypeN; ///< _HRTFType value already set in NativePlugin
        private bool _loop = false; ///< copy of audioSource.loop (for reference outside the main thread)

        private const float SAMPLERATE = 48000.0f;
        private bool Running => _instanceID != 0; ///< ambisonics running (attached to NativePlugin and ready to operate)

        bool _hasChanged_AudioListenerTransform = false;

#endregion

#region property

        public bool Playing => _playing && !_paused; ///< true if playing
        public bool Paused => _paused; ///< true if paused
        public bool validAudio => !String.IsNullOrEmpty(_audioFileN) && _orderOfFileN > 0; ///< true if the HOA file setting is valid
        public int audioOrder => _orderOfFileN; ///< number of orders in configured HOA file
        public float audioLength => _lengthOfFileN / SAMPLERATE; ///< configured HOA file length [seconds]
        public uint audioSamples => _lengthOfFileN; ///< configured HOA file length [sample]

        /// HOA file path
        public string audioFile
        {
            get { return _audioFile; }
            set {
                _playing = false;
                _position = 0;
                _audioFile = (value == null) ? null : value.Replace("\\", "/");
                SetNativeParameterAudioClip();
            }
        }

        /// HOA order
        public int order
        {
            get { return _order; }
            set { _order = value; }
        }

        /// virtual speaker type
        public SpeakerSet speakerSet
        {
            get { return _speakerSet; }
            set { _speakerSet = value; }
        }

        /// distance decay On/Off
        public bool distanceDecay {
            get { return _distanceDecay; }
            set {
                _distanceDecay = value;
                SetNativeParameterDecay();
            }
        }

        /// distance decay rate ( when DistanceDecay is enabled )
        public DecayCurve decayCurve {
            get { return _decayCurve; }
            set {
                _decayCurve = value;
                SetNativeParameterDecay();
            }
        }

        /// HTRF type
        public Preset HRTFType {
            get { return _HRTFType; }
            set {
                _HRTFType = value;
                SetNativeParameterHRTFType();
            }
        }

        /// automatically start playing audio at Start()
        public bool playOnAwake
        {
            get { return _playOnAwake; }
            set { _playOnAwake = value; }
        }

        /// loop playback
        public bool loop
        {
            get { return audioSource.loop; }
            set { audioSource.loop = value; }
        }

        /// play position [seconds]
        public float time
        {
            get { return _position / SAMPLERATE; }
            set {
                value = Math.Max(0.0f, value);
                uint samples = (uint)(value * SAMPLERATE);
                _position = Math.Min(samples, _lengthOfFileN);
            }
        }

#endregion

#region Player

        /// start playing
        public void Play()
        {
            if (!validAudio)
            {
                return;
            }
            if (!audioSource.isPlaying)
            {
                Debug.LogWarning($"The AudioSource state is incorrect for AmbisonicsAudioFilter behavior.\n" +
                                 $"Must be playing. Please do Play() or set \"Play On Awake\" to On.");
            }

            _playing = true;
            _paused = false;
            if (_position >= _lengthOfFileN) {
                _position = 0;
            }
        }

        /// stop playing
        public void Stop()
        {
            _playing = false;
        }

        /// pause playing
        public void Pause()
        {
            _paused = true;
        }

        /// unpause playing
        public void UnPause()
        {
            _paused = false;
        }

#endregion

#region Unity Event

        void OnEnable()
        {
            lock (this) {
                _instanceID = GetInstanceID();
                var rc = AmbisonicsApi.Attach(_instanceID);
                if (rc != AmbisonicsApi.ResultCode.Success)
                    Debug.LogError($"Ambisonics Attach() rc = {rc}");
                DirtyNativeFlag();
                SetParameters();

                if (audioListener != null)
                    audioListener.AddAudioFilter(this);
            }
#if UNITY_EDITOR
            VSAttributionWrapper.SendAttributionEvent("AmbisonicsAudioFilter");
#endif
        }

        void OnDisable()
        {
            lock (this) {
                if (_audioListener != null)
                    _audioListener.RemoveAudioFilter(this);

                var rc = AmbisonicsApi.Detach(_instanceID);
                if (rc != AmbisonicsApi.ResultCode.Success)
                    Debug.LogError($"Ambisonics Detach() rc = {rc}");
                _instanceID = 0;
            }
        }

        void Start()
        {
            if (!audioSource.isPlaying && audioSource.playOnAwake) 
            {
                audioSource.Play();
            }
            if (!audioSource.isPlaying)
            {
                Debug.LogWarning($"The AudioSource state is incorrect for AmbisonicsAudioFilter behavior.\n" +
                                 $"Must be playing. Please do Play() or set \"Play On Awake\" to On.");
            }
            SetAudioSourceInfo(true);
            SetAudioListenrInfo(true);

            if (_playOnAwake)
            {
                Play();
            }
        }

        void Update()
        {
            SetParameters();
            SetAudioSourceInfo();
            SetAudioListenrInfo();
            _loop = audioSource.loop;
        }

        void OnValidate()
        {
            SetParameters();
        }

        void OnAudioFilterRead(float[] data, int channels)
        {
            const int CH = 2;
            if (channels != CH)
            {
                Debug.LogWarning($"AmbisonicsAudioFilter: {channels} ch is not supported.");
                return;
            }

            lock (this) {
                if (!Running || !Playing || String.IsNullOrEmpty(_audioFileN))
                {
                    return;
                }

                int needLength = (int)(data.Length / CH);
                int length = needLength;
                var rc = AmbisonicsApi.Process(_instanceID, data, ref length, _position, _order, (int)_speakerSet);
                if (rc != AmbisonicsApi.ResultCode.Success) {
                    Debug.LogError($"Ambisonics Process() rc = {rc}");
                    return;
                }
                _position += (uint)length;

                if (needLength > length) {
                    if (_loop) {
                        int remain = needLength - length;
                        float[] temp = new float[remain * CH];
                        rc = AmbisonicsApi.Process(_instanceID, temp, ref remain, 0, _order, (int)_speakerSet);
                        if (rc != AmbisonicsApi.ResultCode.Success) {
                            Debug.LogError($"Ambisonics Process() rc = {rc}");
                            return;
                        }

                        Array.Copy(temp, 0, data, length * CH, remain * CH);

                        _position = (uint)remain;
                    }
                    else
                    {
                        Stop();
                    }
                }
            }
        }

#endregion

#region AmbisonicsAudioListener Event

        /// transform change notification from AmbisonicsAudioListener
        public void OnChangedAudioListenerTransform()
        {
            _hasChanged_AudioListenerTransform = true;
            SetAudioListenrInfo();
        }

#endregion

#region private methods

        private void SetParameters()
        {
            SetNativeParameterAudioClip();
            SetNativeParameterDecay();
            SetNativeParameterHRTFType();
        }

        private void SetAudioSourceInfo(bool force = false)
        {
            if (!transform.hasChanged && !force)
                return;
            lock (this) {
                if (!Running)
                    return;
                var rc = AmbisonicsApi.SetSourceMatrix(_instanceID, transform.localToWorldMatrix);
                if (rc != AmbisonicsApi.ResultCode.Success)
                    Debug.LogError($"Ambisonics SetSourceMatrix() rc = {rc}");
                else
                    transform.hasChanged = false;
            }
        }

        private void SetAudioListenrInfo(bool force = false)
        {
            if (!_hasChanged_AudioListenerTransform && !force)
                return;
            lock (this) {
                if (!Running)
                    return;
                if (audioListener == null)
                    return;
                var rc = AmbisonicsApi.SetListenerMatrix(_instanceID, audioListener.transform.localToWorldMatrix);
                if (rc != AmbisonicsApi.ResultCode.Success)
                    Debug.LogError($"Ambisonics SetListenerMatrix() rc = {rc}");
                else
                    _hasChanged_AudioListenerTransform = false;
            }
        }

        private void DirtyNativeFlag()
        {
            _audioFileN = null;
            _distanceDecayN = true;
            _decayCurveN = -1;
            _HRTFTypeN = -1;
        }

        private bool SetNativeParameterAudioClip()
        {
            if (_audioFileN == _audioFile)
                return true;

            lock (this) {
                if (!Running)
                    return false;

                _playing = false;
                _position = 0;

                _audioFileN = _audioFile;
                _orderOfFileN = 0;
                _lengthOfFileN = 0;
                _position = 0;

                var path = _audioFileN;
                if (!String.IsNullOrEmpty(path) && !Path.IsPathRooted(path))
                {
                    path = Application.streamingAssetsPath + "/" + path;
                }
                var rc = AmbisonicsApi.SetWaveFile(_instanceID, path);
                if (rc != AmbisonicsApi.ResultCode.Success)
                {
                    Debug.LogError($"Ambisonics SetWaveFile() rc = {rc}");
                }
                else
                {
                    AmbisonicsApi.GetOrder(_instanceID, out _orderOfFileN);
                    AmbisonicsApi.GetLength(_instanceID, out _lengthOfFileN);
                }
            }
            return true;
        }

        private bool SetNativeParameterDecay()
        {
            if (_distanceDecayN == _distanceDecay && _decayCurveN == (int)_decayCurve)
                return true;

            lock (this) {
                if (!Running)
                    return false;

                _distanceDecayN = _distanceDecay;
                _decayCurveN = (int)_decayCurve;
                var rc = AmbisonicsApi.SetDecay(_instanceID, _distanceDecayN, _decayCurveN);
                if (rc != AmbisonicsApi.ResultCode.Success)
                    Debug.LogError($"Ambisonics SetDecay() rc = {rc}");
            }
            return true;
        }

        private bool SetNativeParameterHRTFType()
        {
            if (_HRTFTypeN == (int)_HRTFType)
                return true;

            lock (this) {
                if (!Running)
                    return false;

                _HRTFTypeN = (int)_HRTFType;
                var rc = AmbisonicsApi.SetHRTFType(_instanceID, _HRTFTypeN);
                if (rc != AmbisonicsApi.ResultCode.Success)
                    Debug.LogError($"Ambisonics SetHRTFType() rc = {rc}");
            }
            return true;
        }

#endregion

    }

} // namespace Soundxr.Effect.Spatializer
