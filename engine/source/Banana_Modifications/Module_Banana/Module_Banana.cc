#ifndef _CONSOLE_H_
#include "console/console.h"
#endif

#include "Module_Banana.h"

struct Struct_Module_Banana *Struct_Module_Banana_This=NULL;

struct Struct_Module_Banana* Machine_Banana_Create()
{
	struct Struct_Module_Banana *Struct_Module_Banana=(struct Struct_Module_Banana*)malloc(sizeof(struct Struct_Module_Banana));

	Struct_Module_Banana->Int_Counter=0;

	return Struct_Module_Banana;

}

void Machine_Banana_Destroy(struct Struct_Module_Banana *Struct_Module_Banana)
{

	free(Struct_Module_Banana);

}

void Machine_Banana_Process(struct Struct_Module_Banana *Struct_Module_Banana)
{

	Con::printf("Banana %d Int_Counter=%d\n",Struct_Module_Banana,Struct_Module_Banana->Int_Counter);

	Struct_Module_Banana->Int_Counter++;

}
