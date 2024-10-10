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

 package com.yamaha.soundxr;

import android.content.Context;
import android.database.Cursor;
import android.net.Uri;
import android.os.Environment;
import android.provider.DocumentsContract;
import android.provider.MediaStore;
import android.util.Log;

public class PathUtils {

    static public String getPath(Uri uri, Context context) {
        if (DocumentsContract.isDocumentUri(context, uri)) {
            if ("com.android.externalstorage.documents".equals(uri.getAuthority())) {
                final String docId = DocumentsContract.getDocumentId(uri);
                final String[] split = docId.split(":");
                return Environment.getExternalStorageDirectory() + "/" + split[1];
            }
        }
        
        final String scheme = uri.getScheme();
        if ("content".equalsIgnoreCase(scheme)) {
            final String columnName = MediaStore.Audio.Media.DATA;
            final String[] projection = { columnName };
            try {
                Cursor cursor = context.getContentResolver().query(uri, projection, null, null, null);
                int index = cursor.getColumnIndexOrThrow(columnName);
                if (cursor.moveToFirst()) {
                    return cursor.getString(index);
                }
            } catch (Exception e) {
                Log.d("soundxr", e.toString());
            }
        }
        if ("file".equalsIgnoreCase(scheme)) {
            return uri.getPath();
        }
        return null;
    }
}
