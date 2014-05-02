function AI::create( %this )
{	
    AI.Health = 100;
	AI.Score = 0;
	AI.Damage = 0;
	AI.Alive = true;
	AI.CurrentWord = 0;
	AI.Streak = 0;
	AI.WrongStreak = 0;
	AI.Combo = 0;
	AI.LastCorrectTime = 0;
}

function AI::destroy( %this )
{

}

function AI::reset(%this)
{
	%this.Health = 100;
	%this.Score = 0;
	%this.Alive = true;
	%this.CurrentWord = 0;
	%this.Difficulty = 1;
	%this.Streak = 0;
	%this.Combo = 0;
	%this.LastCorrectTime = 0;
	%this.Damage = 0;
}

function AI::changeHealth(%this,%amount)
{
	if((%this.Health + %amount) > 0)
	{
		%this.Health = %this.Health + %amount;
	}
	else
	{
		%this.Health = 0;
		%this.onDeath();
	}
}

function AI::onDeath(%this)
{
	Game.cancel();
	%this.cancel();
	%this.Alive = false;
	Game.endGame();
}

//-----------------------------------------------------------------------------

