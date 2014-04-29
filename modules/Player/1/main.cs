function Player::create( %this )
{	
	//exec("./scripts/Controls.cs");
	
    Player.Name = 5;
    Player.Lives = 3;
	Player.Score = 0;
	Player.Alive = true;
	Player.CurrentWord = 0;
}

function Player::destroy( %this )
{

}

function Player::reset(%this)
{
	Player.launched = false;
	
	%this.spawnPlayer();
}

//-----------------------------------------------------------------------------

