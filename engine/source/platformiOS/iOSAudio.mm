//-----------------------------------------------------------------------------
// Copyright (c) 2013 GarageGames, LLC
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.
//-----------------------------------------------------------------------------

#include "platformiOS/platformiOS.h"
#include "platformiOS/iOSUtil.h"
#include "platform/platformAL.h"
#import <AudioToolbox/AudioToolbox.h>  

ConsoleFunction(doDeviceVibrate, void, 1, 1, "Makes the device do a quick vibration. Only works on the iPhone line of devices - the iPod Touch line does not have vibration functionality.")  
{  
	if(IsDeviceiPhone())  
	{
		AudioServicesPlaySystemSound (kSystemSoundID_Vibrate);  
	}
}  

namespace Audio
{
	
	/*!  The MacOS X build links against the OpenAL framework.
     It can be built to use either an internal framework, or the system framework.
     Since OpenAL is weak-linked in at compile time, we don't need to init anything.
     Stub it out...
	 */
	bool OpenALDLLInit() {  return true; }
	
	/*!   Stubbed out, see the note on OpenALDLLInit().  
	 */
	void OpenALDLLShutdown() { }   
	
	
} // namespace Audio



// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
typedef ALvoid	AL_APIENTRY	(*alcMacOSXMixerOutputRateProcPtr) (const ALdouble value);
ALvoid  alcMacOSXMixerOutputRateProc(const ALdouble value)
{
	static	alcMacOSXMixerOutputRateProcPtr	proc = NULL;
    
    if (proc == NULL) {
        proc = (alcMacOSXMixerOutputRateProcPtr) alcGetProcAddress(NULL, (const ALCchar*) "alcMacOSXMixerOutputRate");
    }
    
    if (proc)
        proc(value);
	
    return;
}