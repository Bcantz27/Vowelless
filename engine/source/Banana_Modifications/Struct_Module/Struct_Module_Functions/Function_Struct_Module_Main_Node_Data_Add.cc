void Function_Struct_Module_Main_Node_Data_Add(struct Struct_Module *Pointer_Struct_Module)
{

struct Struct_Module* Pointer_Struct_Module_Data=Function_Struct_Module_Main_Create();

Pointer_Struct_Module_Data->Int_Counter=1234;

//New linked list.
if (*(Pointer_Struct_Module->Pointer_Linked_List_Struct_Data)==NULL)
{

unsigned int *Pointer_Struct_Module_Linked_List_Node=
(unsigned int*)malloc((sizeof(unsigned int)*2)+sizeof(struct Struct_Module *));

*(Pointer_Struct_Module_Linked_List_Node)=(unsigned int)Pointer_Struct_Module_Linked_List_Node;

*(Pointer_Struct_Module_Linked_List_Node+1)=(unsigned int)Pointer_Struct_Module_Linked_List_Node;

*(Pointer_Struct_Module_Linked_List_Node+2)=(unsigned int)Pointer_Struct_Module_Data;

*(Pointer_Struct_Module->Pointer_Linked_List_Struct_Data)=
(unsigned int)Pointer_Struct_Module_Linked_List_Node;

*(Pointer_Struct_Module->Pointer_Linked_List_Struct_Data+1)=
(unsigned int)Pointer_Struct_Module_Linked_List_Node;

}
else
{

unsigned int *Pointer_Struct_Module_Linked_List_Node_Penultimate=
(unsigned int*)(*(Pointer_Struct_Module->Pointer_Linked_List_Struct_Data+1));

unsigned int *Pointer_Struct_Module_Linked_List_Node=
(unsigned int*)malloc((sizeof(unsigned int)*2)+sizeof(struct Struct_Module *));

*(Pointer_Struct_Module_Linked_List_Node_Penultimate+1)=(unsigned int)Pointer_Struct_Module_Linked_List_Node;

*(Pointer_Struct_Module_Linked_List_Node)=(unsigned int)Pointer_Struct_Module_Linked_List_Node_Penultimate;

*(Pointer_Struct_Module_Linked_List_Node+1)=(unsigned int)Pointer_Struct_Module_Linked_List_Node;

*(Pointer_Struct_Module_Linked_List_Node+2)=(unsigned int)Pointer_Struct_Module_Data;

*(Pointer_Struct_Module->Pointer_Linked_List_Struct_Data+1)=
(unsigned int)Pointer_Struct_Module_Linked_List_Node;

}

}
