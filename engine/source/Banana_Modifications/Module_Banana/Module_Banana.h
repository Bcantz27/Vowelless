struct Struct_Module_Banana
{

int Int_Counter;

};

struct Struct_Module_Banana* Machine_Banana_Create();

void Machine_Banana_Destroy(struct Struct_Module_Banana*);

void Machine_Banana_Process(struct Struct_Module_Banana*);

extern struct Struct_Module_Banana *Struct_Module_Banana_This;
