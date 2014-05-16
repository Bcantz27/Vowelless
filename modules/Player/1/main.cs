function Player::create( %this )
{	
	exec("./scripts/Controls.cs");

	setRandomSeed(getRealTime());
	Player.Name = "Steve" @ getRandom(0,100);
	Player.MaxHealth = 100;
    Player.Health = Player.MaxHealth;
	Player.MaxDefense = 50;
	Player.Defense = Player.MaxDefense;
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
	Player.GameID = "";
	Player.NumberOfPowerUps = 0;
	Player.Battling = false;
	Player.Elo = 0;
	
	Player.InputController.initialize();
}

function Player::destroy( %this )
{

}

function Player::resetCombo(%this)
{
	Player.Streak = 0;
	Player.Combo = 0;
	Game.updateComboAndStreak();
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

function Player::displayElo(%this)
{
	if(isObject(PlayerElo))
		PlayerElo.delete();
	
	%obj = new ImageFont(PlayerElo)  
	{   
		Image = "GameAssets:Woodhouse";
		Position = "0 15";
		FontSize = "2 2";
		SceneLayer = 3;
		TextAlignment = "Center";
		Text = Player.Elo;
	};  
		
	MainScene.add(%obj);
}

function Player::setHealth(%this,%amount)
{
	if(!Player.Battling)
	{
		Player.Health = %amount;
	}
	else
	{
		Player.schedule(500,"setHealth",%amount);
	}
}

function Player::setDefense(%this,%amount)
{
	if(!Player.Battling)
	{
		Player.Defense = %amount;
	}
	else
	{
		Player.schedule(500,"setDefense",%amount);
	}
}

function Player::displayPowerUps(%this)
{
	if(isObject(PowerupIcons))
		PowerupIcons.delete();

	%pwrups = new CompositeSprite(PowerupIcons)  ;
	%pwrups.SetBatchLayout("off");
	%pwrups.SceneLayer = 2;
	%startPoint = -(%this.NumberOfPowerUps*5/2);
	
	// Add some sprites.
	for( %n = 0; %n < %this.NumberOfPowerUps; %n++ )
	{
        // Add a sprite with no logical position.
        %pwrups.addSprite();
        
        // Set the sprites location position to a random location.
        %pwrups.setSpriteLocalPosition( %n*5 + %startPoint, -30 );
                
        // Set size.
        %pwrups.setSpriteSize( 4 );

        %pwrups.setSpriteImage( Powerup.getPowerUpIcon(%this.PowerUps[%n]), 0 );                       
	}  
	
	MainScene.add(PowerupIcons);
}

function Player::usePowerUp(%this, %id)
{
	echo("Use Powerup" SPC PowerUp.getPowerUpName(%id));
	
	if(%id == 0)	//Skip Word
	{
		Game.skipWord();
	}
	else if(%id == 1)	//Fill Defense
	{
		Player.Defense = Player.MaxDefense;
		Player.displayDefenseBar(Player.Defense,"-10 30");
	}
	else if(%id == 2)	//Reset
	{
		if(Game.Multiplayer)
		{
			MSClient.resetOpp(Player.GameID, Player.Name);
		}
	}
	else if(%id == 3)	//Blind
	{
		if(Game.Multiplayer)
		{
			MSClient.blindOpp(Player.GameID, Player.Name);
		}
	}
	else if(%id == 4)	//Heal
	{
		Player.changeHealth(25);
	}
	else if(%id == 5)	//Flip
	{
		if(Game.Multiplayer)
		{
			MSClient.flipOppWord(Player.GameID, Player.Name);
		}
	}
	else if(%id == 6)	//Freeze
	{
		Game.freezeTime();
	}
	else
	{
		return "INVALID POWER UP ID";
	}
	
	%this.removePowerUp(%id);
}

function Player::removePowerUp(%this, %id)
{
	%powerUpIndex = -1;
	
	for(%i = 0; %i < %this.NumberOfPowerUps; %i++)
	{
		if(%this.PowerUps[%i] == %id)
		{
			%powerUpIndex = %i;
			break;
		}	
	}
	
	if(%powerUpIndex >= 0)
	{
		for(%i = %powerUpIndex; %i < %this.NumberOfPowerUps; %i++)
		{
			if(%i < %this.NumberOfPowerUps)
			{
				%this.PowerUps[%i] = %this.PowerUps[%i + 1];
			}
		}
		
		%this.NumberOfPowerUps--;
	}
	
	%this.displayPowerUps();
}

function Player::addPowerUp(%this, %id)
{
	echo("Adding Power Up to player" SPC PowerUp.getPowerUpName(%id));
	Game.displayPowerUpPickup(%id,"0 0");
	%this.PowerUps[%this.NumberOfPowerUps] = %id;
	%this.NumberOfPowerUps++;
	
	%this.displayPowerUps();
}

function Player::clearPowerUps(%this)
{
	for(%i = 0; %i < %this.NumberOfPowerUps; %i++)
	{
		%this.PowerUps[%i] = -1;
	}
	
	%this.NumberOfPowerUps = 0;
	
	%this.displayPowerUps();
}


function Player::reset(%this)
{
	Player.Health = Player.MaxHealth;
	Player.Defense = Player.MaxDefense;
	Player.Score = 0;
	Player.Alive = true;
	Player.CurrentWord = 0;
	Player.Difficulty = 1;
	Player.Streak = 0;
	Player.Combo = 0;
	Player.LastCorrectTime = 0;
	Player.Damage = 0;
	Player.NumberOfPowerUps = 0;
	Player.Battling = false;
}

function Player::changeHealth(%this,%amount)
{
	echo("Changing Health" SPC %amount);
	if(%amount < 0)
	{
		MainWindow.startCameraShake(%amount, 1);
		Game.displayHitDamage(%amount,"-30 10");
	}

	if((Player.Health + %amount) > 0)
	{
		if(Player.Defense > 0 && %amount < 0)
		{
			Player.Defense = Player.Defense + %amount;
			Player.Defense = mFloatLength(Player.Defense, 0);
			
			if(Player.Defense < 0)
			{
				Player.Health = Player.Health + Player.Defense;
				Player.Health = mFloatLength(Player.Health, 0);
				Player.Defense = 0;
			}
		}
		else
		{
			Player.Health = Player.Health + %amount;
			Player.Health = mFloatLength(Player.Health, 0);
			
			if(Player.Health > Player.MaxHealth)
			{
				Player.Health = Player.MaxHealth;
			}
		}
	}
	else
	{
		Player.Health = 0;
		%this.onDeath();
	}
	Player.displayHealthBar(Player.Health,"-10 28");
}

function Player::displayHealthBar(%this,%health,%position)
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
		SceneLayer = 3;
		TextAlignment = "Center";
		Text = %this.Health @ "/" @ %this.MaxHealth;
	};  
	
	MainScene.add(%this.HealthText);
	MainScene.add(%this.midRed);
	MainScene.add(%this.leftBack);
	MainScene.add(%this.midBack);
	MainScene.add(%this.rightBack);
}

function Player::displayDefenseBar(%this,%health,%position)
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
		SceneLayer = 3;
		TextAlignment = "Center";
		Text = %this.Defense @ "/" @ %this.MaxDefense;
	};  
	
	MainScene.add(%this.DefenseText);
	
	MainScene.add(%this.midRedDefense);
	MainScene.add(%this.leftBackDefense);
	MainScene.add(%this.midBackDefense);
	MainScene.add(%this.rightBackDefense);
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

