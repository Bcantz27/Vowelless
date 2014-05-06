function ClickVowel::onAdd(%this, %scenegraph)  
{  
	%this.Vowel = "a";
	
	MainWindow.setUseObjectInputEvents(true);  
	
	// now we can set this object to receive mouse events  
   %this.setUseInputEvents(true); 
}
 
function ClickVowel::OnTouchDown(%this, %touchID, %worldPosition)  
{  
	echo("Clicked Vowel");
	Game.VowelSel = %this.Vowel;
}