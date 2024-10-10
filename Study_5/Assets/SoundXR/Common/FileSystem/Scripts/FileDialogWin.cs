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
/// @file FileDialogWin.cs
/// @brief FileDialog class for Windows
///

#if (UNITY_STANDALONE_WIN && !UNITY_EDITOR) || UNITY_EDITOR_WIN

using System;
using System.Runtime.InteropServices;

namespace Soundxr.Common.FileSystem {

    internal static class FileDialogWin
    {
        public static void OpenFileDialog(string title, string directory, string extension, Action<string> cb)
        {
            OpenFileName ofn = new OpenFileName();
            ofn.structSize = Marshal.SizeOf(ofn);
            ofn.dlgOwner = GetActiveWindow();
            ofn.filter = $"*.{extension}\0*.{extension}\0All Files(*.*)\0*.*\0\0";
            ofn.file = new string (new char [260]);
            ofn.maxFile = ofn.file.Length;
            ofn.initialDir = directory;
            ofn.title = title;

            ofn.flags = 0;
            ofn.flags |= 0x00080000; //OFN_EXPLORER
            ofn.flags |= 0x00001000; //OFN_FILEMUSTEXIST
            ofn.flags |= 0x00000800; //OFN_PATHMUSTEXIST
            ofn.flags |= 0x00000008; //OFN_NOCHANGEDIR (Required)

            if (GetOpenFileName(ofn))
            {
                cb(ofn.file);
            }
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetActiveWindow();

        [DllImport("Comdlg32.dll", SetLastError=true, ThrowOnUnmappableChar=true, CharSet=CharSet.Auto)]
        private static extern bool GetOpenFileName([In, Out] OpenFileName ofn);

        [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto)]
        private class OpenFileName
        {
            public int structSize = 0;
            public IntPtr dlgOwner = IntPtr.Zero;
            public IntPtr instance = IntPtr.Zero;
            public String filter = null;
            public String customFilter = null;
            public int maxCustFilter = 0;
            public int filterIndex = 0;
            public String file = null;
            public int maxFile = 0;
            public String fileTitle = null;
            public int maxFileTitle = 0;
            public String initialDir = null;
            public String title = null;
            public int flags = 0;
            public short fileOffset = 0;
            public short fileExtension = 0;
            public String defExt = null;
            public IntPtr custData = IntPtr.Zero;
            public IntPtr hook = IntPtr.Zero;
            public String templateName = null;
            public IntPtr reservedPtr = IntPtr.Zero;
            public int reservedInt = 0;
            public int flagsEx = 0;
        }

    }

} // namespace Soundxr.Common.FileSystem

#endif
