function AI::create( %this )
{
	AI.Name = "AI";
	AI.MaxHealth = 100;
    AI.Health = AI.MaxHealth;
	AI.MaxDefense = 50;
	AI.Defense = AI.MaxDefense;
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
	AI.NumberOfPowerUps = 0;
	AI.Elo = 0;
}

function AI::destroy( %this )
{

}

function AI::readWord(%this)
{
	if(Game.Multiplayer)
		return;

	echo("Starting Attack");
	setRandomSeed(getRealTime());
	%this.Attacking = true;
	%this.tryToAnswer();
}

function AI::displayElo(%this, %position)
{
	if(isObject(%this.EloDisplay))
		%this.EloDisplay.delete();
	
	%this.EloDisplay = new ImageFont()  
	{   
		Image = "GameAssets:Woodhouse";
		Position = %position;
		FontSize = "3 3";
		SceneLayer = 3;
		TextAlignment = "Center";
		Text = %this.Elo;
	};  
		
	MainScene.add(%this.EloDisplay);
}

function AI::setDefense(%this,%amount)
{
	if(!Player.Battling)
	{
		AI.Defense = %amount;
	}
	else
	{
		AI.schedule(500,"setDefense",%amount);
	}
}

function AI::setHealth(%this,%amount)
{
	if(!Player.Battling)
	{
		AI.Health = %amount;
	}
	else
	{
		AI.schedule(500,"setHealth",%amount);
	}
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
	%this.Health = %this.MaxHealth;
	%this.Defense = %this.MaxDefense;
	%this.Score = 0;
	%this.Alive = true;
	%this.CurrentWord = 0;
	%this.Difficulty = 1;
	%this.Streak = 0;
	%this.Combo = 0;
	%this.LastCorrectTime = 0;
	%this.Damage = 0;
	%this.NumberOfPowerUps = 0;
}

function AI::addPowerUp(%this, %id)
{
	%this.PowerUps[%this.NumberOfPowerUps] = %id;
	%this.NumberOfPowerUps++;
}

function AI::clearPowerUps(%this)
{
	for(%i = 0; %i < %this.NumberOfPowerUps; %i++)
	{
		%this.PowerUps[%i] = -1;
	}
	
	%this.NumberOfPowerUps = 0;
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
		
	if(isObject(%this.HealthText))
		%this.HealthText.delete();
	
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
	%this.midRed.Size = 20*(%health/%this.MaxHealth) SPC "2";
	%this.midRed.Position = VectorAdd(%position, (((20*(%health/%this.MaxHealth))/2)+0.5) SPC "0");
	%this.midRed.SceneLayer = 29;
	%this.midRed.setBodyType("static");
	%this.midRed.Image = "GameAssets:barRedMid";
	
	%this.rightBack = new Sprite();
	%this.rightBack.Size = "1 2";
	%this.rightBack.Position = VectorAdd(%position,"21 0");
	%this.rightBack.SceneLayer = 30;
	%this.rightBack.setBodyType("static");
	%this.rightBack.Image = "GameAssets:barBackRight";
	
	if(%health == %this.MaxHealth)
	{
		%this.rightRed = new Sprite();
		%this.rightRed.Size = "1 2";
		%this.rightRed.Position = VectorAdd(%position,"21 0");
		%this.rightRed.SceneLayer = 29;
		%this.rightRed.setBodyType("static");
		%this.rightRed.Image = "GameAssets:barRedRight";
		MainScene.add(%this.rightRed);
	}
	
	%this.HealthText = new ImageFont()  
	{   
		Image = "GameAssets:font";
		Position = VectorAdd(%position,"10.5 0");
		FontSize = "1.5 1.5";
		Layer = 3;
		TextAlignment = "Center";
		Text = %this.Health @ "/" @ %this.MaxHealth;
	};  
	
	MainScene.add(%this.HealthText);
	
	MainScene.add(%this.midRed);
	MainScene.add(%this.leftBack);
	MainScene.add(%this.midBack);
	MainScene.add(%this.rightBack);
}

function AI::displayDefenseBar(%this,%health,%position)
{
	if(isObject(%this.LeftBackDefense))
		%this.LeftBackDefense.delete();

	if(isObject(%this.LeftRedDefense))
		%this.LeftRedDefense.delete();
	
	if(isObject(%this.MidBackDefense))
		%this.MidBackDefense.delete();
	
	if(isObject(%this.MidRedDefense))
		%this.MidRedDefense.delete();
	
	if(isObject(%this.RightBackDefense))
		%this.RightBackDefense.delete();
		
	if(isObject(%this.RightRedDefense))
		%this.RightRedDefense.delete();
		
	if(isObject(%this.DefenseText))
		%this.DefenseText.delete();
	
	%this.leftBackDefense = new Sprite();
	%this.leftBackDefense.Size = "1 2";
	%this.leftBackDefense.Position = %position;
	%this.leftBackDefense.SceneLayer = 30;
	%this.leftBackDefense.setBodyType("static");
	%this.leftBackDefense.Image = "GameAssets:barBackLeft";
	
	if(%health > 0)
	{
		%this.leftRedDefense = new Sprite();
		%this.leftRedDefense.Size = "1 2";
		%this.leftRedDefense.Position = %position;
		%this.leftRedDefense.SceneLayer = 29;
		%this.leftRedDefense.setBodyType("static");
		%this.leftRedDefense.Image = "GameAssets:barBlueLeft";
		MainScene.add(%this.leftRedDefense);
	}
	
	%this.midBackDefense = new Sprite();
	%this.midBackDefense.Size = "20 2";
	%this.midBackDefense.Position = VectorAdd(%position,"10.5 0");
	%this.midBackDefense.SceneLayer = 30;
	%this.midBackDefense.setBodyType("static");
	%this.midBackDefense.Image = "GameAssets:barBackMid";
	
	%this.midRedDefense = new Sprite();
	%this.midRedDefense.Size = 20*(%health/%this.MaxDefense) SPC "2";
	%this.midRedDefense.Position = VectorAdd(%position, (((20*(%health/%this.MaxDefense))/2)+0.5) SPC "0");
	%this.midRedDefense.SceneLayer = 29;
	%this.midRedDefense.setBodyType("static");
	%this.midRedDefense.Image = "GameAssets:barBlueMid";
	
	%this.rightBackDefense = new Sprite();
	%this.rightBackDefense.Size = "1 2";
	%this.rightBackDefense.Position = VectorAdd(%position,"21 0");
	%this.rightBackDefense.SceneLayer = 30;
	%this.rightBackDefense.setBodyType("static");
	%this.rightBackDefense.Image = "GameAssets:barBackRight";
	
	if(%health == %this.MaxDefense)
	{
		%this.rightRedDefense = new Sprite();
		%this.rightRedDefense.Size = "1 2";
		%this.rightRedDefense.Position = VectorAdd(%position,"21 0");
		%this.rightRedDefense.SceneLayer = 29;
		%this.rightRedDefense.setBodyType("static");
		%this.rightRedDefense.Image = "GameAssets:barBlueRight";
		MainScene.add(%this.rightRedDefense);
	}
	
	%this.DefenseText = new ImageFont()  
	{   
		Image = "GameAssets:font";
		Position = VectorAdd(%position,"10.5 0");
		FontSize = "1.5 1.5";
		Layer = 3;
		TextAlignment = "Center";
		Text = %this.Defense @ "/" @ %this.MaxDefense;
	};  
	
	MainScene.add(%this.DefenseText);
	
	MainScene.add(%this.midRedDefense);
	MainScene.add(%this.leftBackDefense);
	MainScene.add(%this.midBackDefense);
	MainScene.add(%this.rightBackDefense);
}


function AI::changeHealth(%this,%amount)
{
	if((%this.Health + %amount) > 0)
	{
		if(AI.Defense > 0)
		{
			AI.Defense = AI.Defense + %amount;
			AI.Defense = mFloatLength(AI.Defense, 0);
			
			if(AI.Defense < 0)
			{
				AI.Health = AI.Health + AI.Defense;
				AI.Health = mFloatLength(AI.Health, 0);
				AI.Defense = 0;
			}
			
			Game.displayHitDamage(%amount,"14 20");
		}
		else
		{
			AI.Health = AI.Health + %amount;
			AI.Health = mFloatLength(AI.Health, 0);
			Game.displayHitDamage(%amount,"14 14");
		}
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

