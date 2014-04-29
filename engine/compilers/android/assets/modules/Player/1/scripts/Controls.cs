Player.InputController = new ScriptObject()
{
    class = InputHandler;
};

//-----------------------------------------------------------------------------

function InputHandler::initialize( %this )
{
    // Add touch gester as an input listener.
    MainWindow.addInputListener( %this );
}

//-----------------------------------------------------------------------------

function InputHandler::onTouchDown(%this, %touchID, %worldPosition)
{    
	//echo("Down!");
    if(!Player.launched &&  %worldPosition = PlayerRocket.getPosition())
	{
		echo("Rocket Launched!");
		Player.launched = true;
		LevelFloor.destroy();
		LevelBackground.ScrollY = Player.speed;
	}
}

function InputHandler::onTouchDragged(%this, %touchID, %worldPosition)
{    
	//echo("Drag!");
	if(Player.launched)
	{
		%adjustedSpeed = Player.speed / Player.maxSpeed;

		%relativePosition = VectorSub(%worldPosition, PlayerRocket.getPosition());

		%scaledVelocity = VectorScale(%relativePosition, %adjustedSpeed);

		PlayerRocket.setLinearVelocity( getWord(%scaledVelocity, 0), getWord(%scaledVelocity, 1) );
	}
}