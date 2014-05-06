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
	AI.Attacking = true;
	AI.HitChance = 6;
}

function AI::destroy( %this )
{

}

function AI::readWord(%this)
{
	echo("Starting Attack");
	setRandomSeed(getRealTime());
	%this.Attacking = true;
	%this.tryToAnswer();
}

function AI::incrementScore(%this,%amount)
{
	%this.score = %this.score + %amount;
}

function AI::tryToAnswer(%this)
{
	%roll = getRandom(1,%this.HitChance + getWordDifficulty($GameWordList[AI.CurrentWord]));
	//echo("AI: Trying to attack" SPC %roll);
	if(%roll == 1)
	{
		//echo("AI: Attack" SPC getWordDamage($GameWordList[AI.CurrentWord]));
		AI.incrementScore(getWordValue($GameWordList[AI.CurrentWord]));

		if(stricmp(Game.Mode,"Battle") == 0)
		{
			%this.Damage = %this.Damage + getWordDamage($GameWordList[AI.CurrentWord]);
		}
		else if(stricmp(Game.Mode,"Race") == 0)
		{
			if(AI.Score >= Game.RaceTo)
			{
				Game.endRace();
				return;
			}
		}
		
		AI.CurrentWord++;
	}

	if(%this.Attacking)
		%this.schedule(500,"tryToAnswer");
}

function AI::attackPlayer(%this, %damage)
{
	Player.changeHealth(%damage);
	Game.playHitSound(%damage);
	Game.displayBattleStats();
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

function AI::displayHealthBar(%this,%health,%position)
{
	if(isObject(%this.LeftBack))
		%this.LeftBack.delete();

	if(isObject(%this.LeftRed))
		%this.LeftRed.delete();
	
	if(isObject(%this.MidBack))
		%this.MidBack.delete();
	
	if(isObject(%this.MidRed))
		%this.MidRed.delete();
	
	if(isObject(%this.RightBack))
		%this.RightBack.delete();
		
	if(isObject(%this.RightRed))
		%this.RightRed.delete();
	
	%this.leftBack = new Sprite();
	%this.leftBack.Size = "1 2";
	%this.leftBack.Position = %position;
	%this.leftBack.SceneLayer = 30;
	%this.leftBack.setBodyType("static");
	%this.leftBack.Image = "GameAssets:barBackLeft";
	
	if(%health > 0)
	{
		%this.leftRed = new Sprite();
		%this.leftRed.Size = "1 2";
		%this.leftRed.Position = %position;
		%this.leftRed.SceneLayer = 29;
		%this.leftRed.setBodyType("static");
		%this.leftRed.Image = "GameAssets:barRedLeft";
		MainScene.add(%this.leftRed);
	}
	
	%this.midBack = new Sprite();
	%this.midBack.Size = "20 2";
	%this.midBack.Position = VectorAdd(%position,"10.5 0");
	%this.midBack.SceneLayer = 30;
	%this.midBack.setBodyType("static");
	%this.midBack.Image = "GameAssets:barBackMid";
	
	%this.midRed = new Sprite();
	%this.midRed.Size = 20*(%health/100) SPC "2";
	%this.midRed.Position = VectorAdd(%position, (((20*(%health/100))/2)+0.5) SPC "0");
	%this.midRed.SceneLayer = 29;
	%this.midRed.setBodyType("static");
	%this.midRed.Image = "GameAssets:barRedMid";
	
	%this.rightBack = new Sprite();
	%this.rightBack.Size = "1 2";
	%this.rightBack.Position = VectorAdd(%position,"21 0");
	%this.rightBack.SceneLayer = 30;
	%this.rightBack.setBodyType("static");
	%this.rightBack.Image = "GameAssets:barBackRight";
	
	if(%health == 100)
	{
		%this.rightRed = new Sprite();
		%this.rightRed.Size = "1 2";
		%this.rightRed.Position = VectorAdd(%position,"21 0");
		%this.rightRed.SceneLayer = 29;
		%this.rightRed.setBodyType("static");
		%this.rightRed.Image = "GameAssets:barRedRight";
		MainScene.add(%this.rightRed);
	}
	
	MainScene.add(%this.midRed);
	MainScene.add(%this.leftBack);
	MainScene.add(%this.midBack);
	MainScene.add(%this.rightBack);
}

function AI::changeHealth(%this,%amount)
{
	if((%this.Health + %amount) > 0)
	{
		%this.Health = %this.Health + %amount;
		AI.Health = mFloatLength(AI.Health, 0);
		Game.displayHitDamage(%amount,"30 10");
	}
	else
	{
		%this.Health = 0;
		%this.onDeath();
	}
}

function AI::onDeath(%this)
{
	%this.Alive = false;
	//Game.endGame();
}

//-----------------------------------------------------------------------------

