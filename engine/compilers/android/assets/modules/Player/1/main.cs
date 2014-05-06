function Player::create( %this )
{	
    Player.Health = 100;
	Player.Score = 0;
	Player.Damage = 0;
	Player.Alive = true;
	Player.CurrentWord = 0;
	Player.Difficulty = 1;
	Player.Streak = 0;
	Player.WrongStreak = 0;
	Player.Combo = 0;
	Player.endCombo = true;
	Player.LastCorrectTime = 0;
}

function Player::destroy( %this )
{

}

function Player::incrementScore(%this,%amount)
{
	%this.score = %this.score + %amount;
	Game.displayScore();
}

function Player::checkCombo(%this)
{
	if(%this.endCombo)
	{
		Player.Combo = 0;
		Game.displayCombo();
	}
	else
	{
		%this.endCombo = true;
	}
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
	Player.Damage = 0;
}

function Player::changeHealth(%this,%amount)
{
	if(%amount < 0)
	{
		MainWindow.startCameraShake(%amount, 1);
	}

	if((Player.Health + %amount) > 0)
	{
		Player.Health = Player.Health + %amount;
		Player.Health = mFloatLength(Player.Health, 0);
	}
	else
	{
		Player.Health = 0;
		%this.onDeath();
	}
}

function Player::attackAI(%this, %damage)
{
	AI.changeHealth(%damage);
	Game.playHitSound(%damage);
	Game.displayBattleStats();
}

function Player::onDeath(%this)
{
	%this.Alive = false;
	//Game.endGame();
}

//-----------------------------------------------------------------------------

