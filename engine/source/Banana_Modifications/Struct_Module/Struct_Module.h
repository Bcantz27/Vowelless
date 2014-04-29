//Modular Structure by practicing01

struct Struct_Module
{

int Int_Counter;

//If true, go through this module's loop.
char Bool_Loop;

//Identifier this module sets.
char *Pointer_Char_Array_Identifier;

//Size of the identifier array.
int Int_Size_Identifier;

//Linked list of modules that will process this module.
unsigned int *Pointer_Struct_Module_Linked_List_Parents;

//Linked list of modules that this module will process.
unsigned int *Pointer_Struct_Module_Linked_List_Children;

//Linked list of this modules custom structures.
unsigned int *Pointer_Linked_List_Struct_Data;

//Create() dynamically allocates a Struct_Model linked list node.
struct Struct_Module *(*Pointer_Function_Create)();

//Initialize() initializes this module's stuffs.
void (*Pointer_Function_Initialize)(struct Struct_Module *Pointer_Struct_Module);

//Destroy() destroys this module's stuffs. A parent will call its children's Destroy().
void (*Pointer_Function_Destroy)(struct Struct_Module *Pointer_Struct_Module);

//Link() adds the modules to each others' module linked lists.
void (*Pointer_Function_Link)
(struct Struct_Module *Pointer_Struct_Module_Parent,
struct Struct_Module *Pointer_Struct_Module_Child);

//Node_Data_Add() adds an unsigned int pointer, to the data list.
void (*Pointer_Function_Node_Data_Add)(struct Struct_Module *Pointer_Struct_Module);

//Loop() processes this module's stuffs.
void (*Pointer_Function_Loop)(struct Struct_Module *Pointer_Struct_Module);

};

//The main module.
extern struct Struct_Module *Pointer_Struct_Module_Main;

struct Struct_Module* Function_Struct_Module_Main_Create();

void Function_Struct_Module_Main_Destroy(struct Struct_Module *);

void Function_Struct_Module_Main_Link(struct Struct_Module *,struct Struct_Module *);

void Function_Struct_Module_Main_Loop(struct Struct_Module *);

void Function_Struct_Module_Main_Node_Data_Add(struct Struct_Module *);

void Function_Struct_Module_Main_Initialize(struct Struct_Module *);
