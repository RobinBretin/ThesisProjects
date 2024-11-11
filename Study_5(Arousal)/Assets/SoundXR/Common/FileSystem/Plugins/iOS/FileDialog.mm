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

//
//  FileDialog.mm
//

#import <Foundation/Foundation.h>
#import <UIkit/UIKit.h>
#import <UniformTypeIdentifiers/UniformTypeIdentifiers.h>

extern "C" {
	typedef void (*FileDialogCallback)(const char* path, char* bookmark, int bookmarkSize);
}

@interface Picker : UIDocumentPickerViewController<UIDocumentPickerDelegate>
@property FileDialogCallback callback;
@end
@implementation Picker
- (void)documentPicker:(UIDocumentPickerViewController*)controller didPickDocumentsAtURLs:(NSArray<NSURL*>*)urls
{
	NSURL* url = [urls objectAtIndex:0];
	if (url == nullptr)
		return;

	[url startAccessingSecurityScopedResource];

	NSError* error = nil;
	NSData* bookmark = [url bookmarkDataWithOptions:NSURLBookmarkCreationMinimalBookmark
					 includingResourceValuesForKeys:nil
									  relativeToURL:nil
											  error:&error];
	if (bookmark == nil || error != nil)
	{
		NSLog(@"Cannot create bookmark. [%@]", error);
		[url stopAccessingSecurityScopedResource];
		return;
	}

	const char *path = [[url absoluteString] cStringUsingEncoding:NSUTF8StringEncoding];
	_callback(path, (char*)bookmark.bytes, (int)bookmark.length);
}
@end

extern "C" {

NSURL* ToURL(const char* bookmark, int size)
{
	NSData* bookmarkData = [[NSData alloc] initWithBytes:bookmark length:size];
	BOOL isStale = true;
	NSURL* url = [[NSURL alloc] initByResolvingBookmarkData:bookmarkData
													options:NSURLBookmarkCreationMinimalBookmark
											  relativeToURL:nil
										bookmarkDataIsStale:&isStale
													  error:nil];
	return url;
}

void ReleaseBookmark(const char* bookmark, int size)
{
	NSURL* url = ToURL(bookmark, size);
	[url stopAccessingSecurityScopedResource];
}

void OpenFileDialog(const char* title,
					const char* directory,
					const char* extension,
					FileDialogCallback callback)
{
	// NSString* strTitle = [NSString stringWithCString:title ?: "" encoding:NSUTF8StringEncoding];
	// NSString* strDir = [NSString stringWithCString:directory ?: "" encoding:NSUTF8StringEncoding];
	NSString* strExt = [NSString stringWithCString:extension ?: "" encoding:NSUTF8StringEncoding];

	Picker* picker = nil;
	if (@available(iOS 14.0, *)) // iOS14 or later
	{
		NSArray<UTType*>* types = @[UTTypeContent];
		if ([strExt length] != 0)
		{
			if ([strExt isEqual:@"wav"]) types = @[UTTypeWAV];
			else types = @[[UTType typeWithFilenameExtension:strExt]];
		}
		picker = [[Picker alloc] initForOpeningContentTypes:types];
	}
	else // Less than iOS14
	{
		NSArray<NSString*>* types = @[@"public.content"];
		if ([strExt length] != 0)
		{
			if ([strExt isEqual:@"wav"]) types = @[@"com.microsoft.waveform-audio"];
		}
		picker = [[Picker alloc] initWithDocumentTypes:types inMode:UIDocumentPickerModeOpen];
	}
	if (picker == nil) {
		return;
	}

	picker.delegate = picker;
	picker.callback = callback;

	UIViewController* unity = UnityGetGLViewController();
	[unity presentViewController:picker
						animated:true
					  completion:nil];
}

}
