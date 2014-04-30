function Player::create( %this )
{	
    Player.Health = 100;
	Player.Score = 0;
	Player.Alive = true;
	Player.CurrentWord = 0;
	Player.Difficulty = 1;
	Player.Streak = 0;
	Player.WrongStreak = 0;
	Player.Combo = 0;
	Player.LastCorrectTime = 0;
}

function Player::destroy( %this )
{

}

function Player::reset(%this)
{
	Player.Health = 100;
	Player.Score = 0;
	Player.Alive = true;
	Player.CurrentWord = 0;
	Player.Difficulty = 1;
	Player.Streak = 0;
	Player.Combo = 0;
	Player.LastCorrectTime = 0;
}

function Player::changeHealth(%this,%amount)
{
	if(%amount < 0)
	{
		MainWindow.startCameraShake((10*%amount), 2);
	}

	if((Player.Health + %amount) > 0)
	{
		Player.Health = Player.Health + %amount;
		Game.displayHealth();
	}
	else
	{
		Player.Health = 0;
		%this.onPlayerDeath();
	}
}

function Player::onPlayerDeath(%this)
{
	Game.endGame();
}

//-----------------------------------------------------------------------------

