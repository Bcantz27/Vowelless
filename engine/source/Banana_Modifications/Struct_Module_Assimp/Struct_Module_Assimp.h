#include "Banana_Modifications/Struct_Module/Struct_Module.h"

//All custom data for this module will be contained in a linked list of structures.
struct Struct_Module_Assimp_Data_Banana
{

int Banana;
/*
const struct aiScene* scene;

GLuint scene_list;

aiVector3D scene_min, scene_max, scene_center;

static float angle;
*/
};

struct Struct_Module_Assimp_Data_Nanana
{

int Nanana;

};

extern struct Struct_Module *Pointer_Struct_Module_Assimp;

struct Struct_Module* Function_Struct_Module_Assimp_Create();

void Function_Struct_Module_Assimp_Destroy(struct Struct_Module *);

void Function_Struct_Module_Assimp_Link(struct Struct_Module *,struct Struct_Module *);

void Function_Struct_Module_Assimp_Loop(struct Struct_Module *);

void Function_Struct_Module_Assimp_Initialize(struct Struct_Module *);
