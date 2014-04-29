void Function_Struct_Module_Main_Initialize(struct Struct_Module *Pointer_Struct_Module)
{

Pointer_Struct_Module->Int_Counter=0;

Pointer_Struct_Module->Bool_Loop=1;

char *Pointer_Char_Array_Identifier=(char *)malloc(sizeof(char)*512);

memset(Pointer_Char_Array_Identifier,NULL,512);

char Char_Array_Identifier[]="Module_Main";

memcpy(Pointer_Char_Array_Identifier,Char_Array_Identifier,sizeof(Char_Array_Identifier));

Pointer_Struct_Module->Pointer_Char_Array_Identifier=Pointer_Char_Array_Identifier;

Pointer_Struct_Module->Int_Size_Identifier=sizeof(Char_Array_Identifier);

Pointer_Struct_Module->Pointer_Function_Create=Function_Struct_Module_Main_Create;

Pointer_Struct_Module->Pointer_Function_Initialize=Function_Struct_Module_Main_Initialize;

Pointer_Struct_Module->Pointer_Function_Destroy=Function_Struct_Module_Main_Destroy;

Pointer_Struct_Module->Pointer_Function_Link=Function_Struct_Module_Main_Link;

Pointer_Struct_Module->Pointer_Function_Node_Data_Add=Function_Struct_Module_Main_Node_Data_Add;

Pointer_Struct_Module->Pointer_Function_Loop=Function_Struct_Module_Main_Loop;

/******************************************************************************/

Pointer_Struct_Module->Pointer_Struct_Module_Linked_List_Parents=
(unsigned int*)malloc((sizeof(unsigned int)*2));

*(Pointer_Struct_Module->Pointer_Struct_Module_Linked_List_Parents)=NULL;

*(Pointer_Struct_Module->Pointer_Struct_Module_Linked_List_Parents+1)=NULL;

/******************************************************************************/

Pointer_Struct_Module->Pointer_Struct_Module_Linked_List_Children=
(unsigned int*)malloc((sizeof(unsigned int*)*2));

*(Pointer_Struct_Module->Pointer_Struct_Module_Linked_List_Children)=NULL;

*(Pointer_Struct_Module->Pointer_Struct_Module_Linked_List_Children+1)=NULL;

/******************************************************************************/

Pointer_Struct_Module->Pointer_Linked_List_Struct_Data=
(unsigned int*)malloc((sizeof(unsigned int)*2));

*(Pointer_Struct_Module->Pointer_Linked_List_Struct_Data)=NULL;

*(Pointer_Struct_Module->Pointer_Linked_List_Struct_Data+1)=NULL;

Pointer_Struct_Module->Pointer_Function_Node_Data_Add(Pointer_Struct_Module);

/******************************************************************************/

}
