void Function_Struct_Module_Main_Link(struct Struct_Module *Pointer_Struct_Module_Parent,
struct Struct_Module *Pointer_Struct_Module_Child)
{

/*

Linked list format:

{First_Node, Last_Node}

Node format:

{Node_Previous, Node_Next, Data}

*/

//New linked list.
if (*(Pointer_Struct_Module_Parent->Pointer_Struct_Module_Linked_List_Children)==NULL)
{

unsigned int *Pointer_Struct_Module_Linked_List_Node=
(unsigned int*)malloc((sizeof(unsigned int)*2)+sizeof(struct Struct_Module *));

*(Pointer_Struct_Module_Linked_List_Node)=(unsigned int)Pointer_Struct_Module_Linked_List_Node;

*(Pointer_Struct_Module_Linked_List_Node+1)=(unsigned int)Pointer_Struct_Module_Linked_List_Node;

*(Pointer_Struct_Module_Linked_List_Node+2)=(unsigned int)Pointer_Struct_Module_Child;

*(Pointer_Struct_Module_Parent->Pointer_Struct_Module_Linked_List_Children)=
(unsigned int)Pointer_Struct_Module_Linked_List_Node;

*(Pointer_Struct_Module_Parent->Pointer_Struct_Module_Linked_List_Children+1)=
(unsigned int)Pointer_Struct_Module_Linked_List_Node;

}
else
{

unsigned int *Pointer_Struct_Module_Linked_List_Node_Penultimate=
(unsigned int*)(*(Pointer_Struct_Module_Parent->Pointer_Struct_Module_Linked_List_Children+1));

unsigned int *Pointer_Struct_Module_Linked_List_Node=
(unsigned int*)malloc((sizeof(unsigned int)*2)+sizeof(struct Struct_Module *));

*(Pointer_Struct_Module_Linked_List_Node_Penultimate+1)=(unsigned int)Pointer_Struct_Module_Linked_List_Node;

*(Pointer_Struct_Module_Linked_List_Node)=(unsigned int)Pointer_Struct_Module_Linked_List_Node_Penultimate;

*(Pointer_Struct_Module_Linked_List_Node+1)=(unsigned int)Pointer_Struct_Module_Linked_List_Node;

*(Pointer_Struct_Module_Linked_List_Node+2)=(unsigned int)Pointer_Struct_Module_Child;

*(Pointer_Struct_Module_Parent->Pointer_Struct_Module_Linked_List_Children+1)=
(unsigned int)Pointer_Struct_Module_Linked_List_Node;

}

/****************************************************************************************/

if (*(Pointer_Struct_Module_Child->Pointer_Struct_Module_Linked_List_Parents)==NULL)
{

unsigned int *Pointer_Struct_Module_Linked_List_Node=
(unsigned int*)malloc((sizeof(unsigned int)*2)+sizeof(struct Struct_Module *));

*(Pointer_Struct_Module_Linked_List_Node)=(unsigned int)Pointer_Struct_Module_Linked_List_Node;

*(Pointer_Struct_Module_Linked_List_Node+1)=(unsigned int)Pointer_Struct_Module_Linked_List_Node;

*(Pointer_Struct_Module_Linked_List_Node+2)=(unsigned int)Pointer_Struct_Module_Parent;

*(Pointer_Struct_Module_Child->Pointer_Struct_Module_Linked_List_Parents)=
(unsigned int)Pointer_Struct_Module_Linked_List_Node;

*(Pointer_Struct_Module_Child->Pointer_Struct_Module_Linked_List_Parents+1)=
(unsigned int)Pointer_Struct_Module_Linked_List_Node;

}
else
{

unsigned int *Pointer_Struct_Module_Linked_List_Node_Penultimate=
(unsigned int*)(*(Pointer_Struct_Module_Child->Pointer_Struct_Module_Linked_List_Parents+1));

unsigned int *Pointer_Struct_Module_Linked_List_Node=
(unsigned int*)malloc((sizeof(unsigned int)*2)+sizeof(struct Struct_Module *));

*(Pointer_Struct_Module_Linked_List_Node_Penultimate+1)=(unsigned int)Pointer_Struct_Module_Linked_List_Node;

*(Pointer_Struct_Module_Linked_List_Node)=(unsigned int)Pointer_Struct_Module_Linked_List_Node_Penultimate;

*(Pointer_Struct_Module_Linked_List_Node+1)=(unsigned int)Pointer_Struct_Module_Linked_List_Node;

*(Pointer_Struct_Module_Linked_List_Node+2)=(unsigned int)Pointer_Struct_Module_Parent;

*(Pointer_Struct_Module_Child->Pointer_Struct_Module_Linked_List_Parents+1)=
(unsigned int)Pointer_Struct_Module_Linked_List_Node;

}

}
