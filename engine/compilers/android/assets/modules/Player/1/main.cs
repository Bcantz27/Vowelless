function Player::create( %this )
{	
	//exec("./scripts/Controls.cs");
	
    Player.Name = "None";
    Player.Lives = 5;
	Player.Score = 0;
	Player.Alive = true;
	Player.CurrentWord = 0;
	Player.Difficulty = 1;
}

function Player::destroy( %this )
{

}

function Player::reset(%this)
{
	
	%this.spawnPlayer();
}

//-----------------------------------------------------------------------------

