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
/// @file AmbisonicsApi.cs
/// @brief Wrapper class for Ambisonics Native Plugin
///

using System.Runtime.InteropServices;
using UnityEngine;

namespace Soundxr.Effect.Spatializer {

    /// Wrapper class for Ambisonics Native Plugin
    internal static class AmbisonicsApi {

        public enum ResultCode
        {
            Success = 0,
            InvalidId,
            Failed,
            ErrorWaveFile, // open file error
            NotWaveFormat, // file format : WAVE
            InvalidWaveFormat, // wave format type : PCM
            InvalidWaveChannelNum, // channel num : 4, 9, 16, 25, 36 ch
            InvalidWaveSamplingRate, // sampling rate : 48000 
            InvalidWaveBitsPerSample, // bits per sample : 16, 24 bps
        };

        static public ResultCode Attach(int id)
        {
            return Binding.SoundxRAmbisonicsAttach(id);
        }

        static public ResultCode Detach(int id)
        {
            return Binding.SoundxRAmbisonicsDetach(id);
        }

        static public ResultCode SetWaveFile(int id, string path)
        {
            if (path == null) path =  "";
            return Binding.SoundxRAmbisonicsSetWaveFile(id, path);
        }

        static public ResultCode GetOrder(int id, out int order)
        {
            return Binding.SoundxRAmbisonicsGetOrder(id, out order);
        }

        static public ResultCode GetLength(int id, out uint length)
        {
            return Binding.SoundxRAmbisonicsGetLength(id, out length);
        }

        static public ResultCode SetListenerMatrix(int id, Matrix4x4 matrix)
        {
            float[] position = new float[3];
            float[] rotation = new float[9];
            for (var i = 0; i < 3; i++) {
                position[i] = matrix[3 * 4 + i];
                for (var j = 0; j < 3; j++) {
                    rotation[i * 3 + j] = matrix[j * 4 + i];
                }
            }
            return Binding.SoundxRAmbisonicsSetListenerInfo(id, position, rotation);
        }

        static public ResultCode SetSourceMatrix(int id, Matrix4x4 matrix)
        {
            float[] position = new float[3];
            float[] rotation = new float[9];
            for (var i = 0; i < 3; i++) {
                position[i] = matrix[3 * 4 + i];
                for (var j = 0; j < 3; j++) {
                    rotation[i * 3 + j] = matrix[j * 4 + i];
                }
            }
            return Binding.SoundxRAmbisonicsSetSourceInfo(id, position, rotation);
        }

        static public ResultCode SetDecay(int id, bool on, int curve)
        {
            return Binding.SoundxRAmbisonicsSetDecay(id, on, curve);
        }

        static public ResultCode SetHRTFType(int id, int type)
        {
            return Binding.SoundxRAmbisonicsSetHRTFType(id, type);
        }

        static public ResultCode Process(int id, float[] buffer, ref int length, uint position, int order, int speakerSet)
        {
            return Binding.SoundxRAmbisonicsProcess(id, buffer, ref length, position, order, speakerSet);
        }

        /// DllImport each method of Ambisonics Native Plugin (internal)
        static class Binding
        {
#if UNITY_IOS && !UNITY_EDITOR
        private const string LIBNAME = "__Internal"; // iOS requires static link library
#else
            private const string LIBNAME = "AudioPluginSoundxR";
#endif

#if (UNITY_STANDALONE_WIN && !UNITY_EDITOR) || UNITY_EDITOR_WIN
            private const CharSet charSet = CharSet.Unicode;
#else
            private const CharSet charSet = CharSet.Ansi;
#endif

            [DllImport(LIBNAME)] internal static extern ResultCode SoundxRAmbisonicsAttach(int id);
            [DllImport(LIBNAME)] internal static extern ResultCode SoundxRAmbisonicsDetach(int id);
            [DllImport(LIBNAME, CharSet = charSet)] internal static extern ResultCode SoundxRAmbisonicsSetWaveFile(int id, string waveFilePath);
            [DllImport(LIBNAME)] internal static extern ResultCode SoundxRAmbisonicsGetOrder(int id, out int order);
            [DllImport(LIBNAME)] internal static extern ResultCode SoundxRAmbisonicsGetLength(int id, out uint length);
            [DllImport(LIBNAME)] internal static extern ResultCode SoundxRAmbisonicsSetListenerInfo(int id, float[] position, float[] rotation);
            [DllImport(LIBNAME)] internal static extern ResultCode SoundxRAmbisonicsSetSourceInfo(int id, float[] position, float[] rotation);
            [DllImport(LIBNAME)] internal static extern ResultCode SoundxRAmbisonicsSetDecay(int id, bool on, int curve);
            [DllImport(LIBNAME)] internal static extern ResultCode SoundxRAmbisonicsSetHRTFType(int id, int type);
            [DllImport(LIBNAME)] internal static extern ResultCode SoundxRAmbisonicsProcess(int id, [In, Out] float[] buffer, ref int length, uint position, int order, int speakerSet);
        }
    }

} // namespace Soundxr.Effect.Spatializer
