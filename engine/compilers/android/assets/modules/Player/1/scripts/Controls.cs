Player.InputController = new ScriptObject()
{
    class = InputHandler;
};

//-----------------------------------------------------------------------------

function InputHandler::initialize( %this )
{
	echo("Input Handler initialized");
    // Add touch gester as an input listener.
    MainWindow.addInputListener( %this );
}

//-----------------------------------------------------------------------------

function InputHandler::onTouchDown(%this, %touchID, %worldPosition)
{
	
}

function InputHandler::onTouchUp(%this, %touchID, %worldPosition)
{
    Game.checkAnswer(%worldPosition);
	Game.checkPowerUp(%worldPosition);
}

function InputHandler::onTouchDragged(%this, %touchID, %worldPosition)
{    
	if(isLetterAVowel(Game.VowelSel))
	{
		//echo("Selected Vowel:" SPC Game.VowelSel);
		if(!isObject(SelectedVowel))
		{
			if(Player.Battling)
				return;
				
			%obj = new Sprite(SelectedVowel);
			%obj.Size = "8 8";
			%obj.Position = %worldPosition;
			%obj.SceneLayer = 1;
			%obj.setBodyType("dynamic");
			%obj.Image = "GameAssets:Cloudy";
			%obj.Frame = getASCIIValue(Game.VowelSel);
			
			MainScene.add(%obj);
		}
		else
		{
			SelectedVowel.MoveTo(%worldPosition, 1000, true, true);
		}
	}
}