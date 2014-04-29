#ifndef _ASSET_MANAGER_H_
#include "assets/assetManager.h"
#endif

#include "platform/platform.h"
#ifndef _PLATFORMAUDIO_H_
#include "platform/platformAudio.h"
#endif

#ifndef _SIMBASE_H_
#include "sim/simBase.h"
#endif

#ifndef _AUDIODATABLOCK_H_
#include "audio/audioDataBlock.h"
#endif

#ifndef _AUDIO_ASSET_H_
#include "audio/audioAsset.h"
#endif

/*#include "audio\audiere\include\audiere.h"
audiere::AudioDevicePtr audiere_device;

class audiere_soundhandle:public SimObject
{
private:

typedef SimObject Parent;

public:

audiere::OutputStreamPtr soundhandle;

audiere_soundhandle();
virtual ~audiere_soundhandle();

virtual bool onAdd();
virtual void onRemove();

static void initPersistFields();

DECLARE_CONOBJECT(audiere_soundhandle);
};

audiere_soundhandle::audiere_soundhandle()
{
soundhandle=NULL;
}

audiere_soundhandle::~audiere_soundhandle()
{
if (soundhandle!=NULL)
{
soundhandle->stop();
return;
}
}

void audiere_soundhandle::initPersistFields()
{
// Call parent.
Parent::initPersistFields();
// Add my fields here.
}

bool audiere_soundhandle::onAdd()
{
// Fail if the parent fails.
if (!Parent::onAdd()){Con::printf("parent failure!");return false;}

// Do some work here.
return true;
}
      
void audiere_soundhandle::onRemove()
{
// Do some work here.
Parent::onRemove(); 
}

IMPLEMENT_CONOBJECT(audiere_soundhandle);  

ConsoleFunctionGroupBegin(Audiere, "Functions dealing with the OpenAL audio layer.\n\n"
                          "@see www.OpenAL.org for what these functions do. Variances from posted"
                          "     behaviour is described below.");


ConsoleFunction(InitAudiere, bool, 1, 1, "() Use the OpenALInitDriver function to initialize the OpenAL driver.\n"
                                                                "This must be done before all other OpenAL operations.\n"
                                                                "@return Returns true on successful initialization, false otherwise.\n"
                                                                "@sa OpenALShutdownDriver")
{
	audiere_device=audiere::OpenDevice();

   return true;
}
//-----------------------------------------------

ConsoleFunction(Audiere_OpenSound, void , 3, 3, "(audio-filepath) - Load the audio file.\n"
                                    "@param audio-filepath The asset file path to play.\n"
                                    "@return 1 on success or 0 on error." )
{

//get the simobject who wants to play a sound
audiere_soundhandle *soundobject=dynamic_cast<audiere_soundhandle *>(Sim::findObject(argv[1]));

//get filename
const char* filename=argv[2];

soundobject->soundhandle=audiere::OpenSound(audiere_device,filename, false);

}

//-----------------------------------------------
//simobject,,bool loop,float volume
ConsoleFunction(Audiere_Play, void , 4, 4, "(audio-filepath) - Play the audio file.\n"
                                    "@param audio-filepath The asset file path to play.\n"
                                    "@return 1 on success or 0 on error." )
{

//get the simobject who wants to play a sound
audiere_soundhandle *soundobject=dynamic_cast<audiere_soundhandle *>(Sim::findObject(argv[1]));

//stop previous sound
if (soundobject->soundhandle!=NULL)
{
soundobject->soundhandle->stop();
}

soundobject->soundhandle->setRepeat(dAtob(argv[2]));
soundobject->soundhandle->setVolume(dAtof(argv[3]));
soundobject->soundhandle->play();

}

ConsoleFunction(Audiere_Stop, void, 2, 2, "( handle ) Use the alxStop function to stop a currently playing sound as specified by handle.\n"
                                                                "@param handle The ID (a non-negative integer) corresponding to a previously set up sound source.\n"
                                                                "@return No return value.\n"
                                                                "@sa alxIsPlaying, alxPlay, alxStopAll")
{

//get the simobject who wants to play a sound
audiere_soundhandle *soundobject=dynamic_cast<audiere_soundhandle *>(Sim::findObject(argv[1]));
soundobject->soundhandle->stop();

}

ConsoleFunction(Audiere_Reset, void, 2, 2, "( handle ) Use the alxStop function to stop a currently playing sound as specified by handle.\n"
                                                                "@param handle The ID (a non-negative integer) corresponding to a previously set up sound source.\n"
                                                                "@return No return value.\n"
                                                                "@sa alxIsPlaying, alxPlay, alxStopAll")
{

//get the simobject who wants to play a sound
audiere_soundhandle *soundobject=dynamic_cast<audiere_soundhandle *>(Sim::findObject(argv[1]));
soundobject->soundhandle->reset();

}

//-----------------------------------------------
ConsoleFunctionGroupEnd(Audiere);
*/