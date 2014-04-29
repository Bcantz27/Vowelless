void Function_Struct_Module_Main_Loop(struct Struct_Module *Pointer_Struct_Module)
{

//Con::printf("Loop Struct_Module %d Int_Counter=%d\n",Pointer_Struct_Module,
//Pointer_Struct_Module->Int_Counter);

Pointer_Struct_Module->Int_Counter++;

if (*(Pointer_Struct_Module->Pointer_Linked_List_Struct_Data)!=NULL)
{

unsigned int *Pointer_Struct_Module_Linked_List_Node=
(unsigned int*)(*(Pointer_Struct_Module->Pointer_Linked_List_Struct_Data));

struct Struct_Module *Pointer_Struct_Module_Cast_Linked_List_Node=
(struct Struct_Module *)(*(Pointer_Struct_Module_Linked_List_Node+2));

do
{

//Con::printf("Loop Struct_Module Data %d Int_Counter=%d\n",Pointer_Struct_Module_Cast_Linked_List_Node,
//Pointer_Struct_Module_Cast_Linked_List_Node->Int_Counter);

//Pointer_Struct_Module_Cast_Linked_List_Node->Int_Counter++;

Pointer_Struct_Module_Linked_List_Node=(unsigned int*)(*(Pointer_Struct_Module_Linked_List_Node+1));

}
while(Pointer_Struct_Module_Linked_List_Node!=(unsigned int*)(*(Pointer_Struct_Module_Linked_List_Node+1)));


}

/****************************************************************************************/

if (*(Pointer_Struct_Module->Pointer_Struct_Module_Linked_List_Children)==NULL)
{

return;

}

unsigned int *Pointer_Struct_Module_Linked_List_Node=
(unsigned int*)(*(Pointer_Struct_Module->Pointer_Struct_Module_Linked_List_Children));

struct Struct_Module *Pointer_Struct_Module_Cast_Linked_List_Node=
(struct Struct_Module *)(*(Pointer_Struct_Module_Linked_List_Node+2));

do
{

if (Pointer_Struct_Module_Cast_Linked_List_Node->Bool_Loop==1)
{

Pointer_Struct_Module_Cast_Linked_List_Node->Pointer_Function_Loop(Pointer_Struct_Module_Cast_Linked_List_Node);

}

Pointer_Struct_Module_Linked_List_Node=(unsigned int*)(*(Pointer_Struct_Module_Linked_List_Node+1));

}
while(Pointer_Struct_Module_Linked_List_Node!=(unsigned int*)(*(Pointer_Struct_Module_Linked_List_Node+1)));

}
