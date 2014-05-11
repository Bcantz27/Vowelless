function Game::displayBattleGame()
{
	MainScene.clear();
	Game.displayScore();
	Game.displayWord(true);
	Player.displayDefenseBar(Player.Defense,"-10 30");
	Player.displayHealthBar(Player.Health,"-10 28");
	Player.displayPowerUps();
	Game.displayTime();
	Game.displayCategory();
	Game.displayRound();
	Game.displayVowelButtons();
	Game.displayBackPanel("GameAssets:panelbeige");

	//Canvas.pushDialog(GameGui);
}

function Game::startNewRound()
{
	if(stricmp(Game.Mode,"Battle") == 0)
	{
		if(Player.Health <= 0)
		{
			Game.displayLoseScreen();
			return;
		}
		else if(AI.Health <= 0)
		{
			if(Game.Multiplayer)
			{
				MSClient.setWinner(Player.GameID, Player.Name);
			}
			else
			{
				Game.displayWinScreen();
			}
			return;
		}
	
		if(Game.Round == 3)	//Determine Winner
		{
			if(Player.Health > AI.Health)
			{
				if(Game.Multiplayer)
				{
					MSClient.setWinner(Player.GameID, Player.Name);
				}
				else
				{
					Game.displayWinScreen();
				}
			}
			else if(Player.Health < AI.Health)
			{
				Game.displayLoseScreen();
			}
			else
			{
				Game.displayLoseScreen();
			}
		}
		else
		{
			Game.Round++;
			Game.Time = 30;
			Player.Damage = 0;
			AI.Damage = 0;
			
			Player.CurrentWord = 0;
			Game.setupWordList();
			AI.CurrentWord++;
			Game.displayBattleGame();
			Player.Battling = false;
			Game.schedule(2000,"incrementTime");
			AI.schedule(2000,"readWord");
		}
	}
}

function Game::displayRound()
{
	%obj = new ImageFont()  
	{   
		Image = "GameAssets:Woodhouse";
		Position = "-38 33";
		FontSize = "2 2";
		Layer = 2;
		TextAlignment = "Center";
		Text = "Round";
	};  
		
	MainScene.add(%obj);
	
	%obj = new ImageFont()  
	{   
		Image = "GameAssets:Woodhouse";
		Position = "-38 31";
		FontSize = "2 2";
		Layer = 2;
		TextAlignment = "Center";
		Text = Game.Round @ " of 3";
	};  
		
	MainScene.add(%obj);
}

function Game::displayHitDamage(%this,%damage,%position)
{
	%damage = mFloatLength(%damage, 0);
	%obj = new ImageFont()  
	{   
		Image = "GameAssets:Woodhouse";
		Position = %position;
		FontSize = "2 2";
		Layer = 1;
		TextAlignment = "Center";
		Text = -%damage;
	};  
	
	%obj.setBodyType("dynamic");
	%obj.setLinearVelocityY(4);
	%obj.schedule(1000,"safeDelete");
	
	MainScene.add(%obj);
}

function Game::displayPowerUpPickup(%this, %id, %position)
{
	%name = Powerup.getPowerUpName(%id);
	%obj = new ImageFont()  
	{   
		Image = "GameAssets:font";
		Position = %position;
		FontSize = "2 2";
		Layer = 1;
		TextAlignment = "Center";
		Text = "You obtained" SPC %name;
	};  
	
	%obj.setBodyType("dynamic");
	%obj.setLinearVelocityY(4);
	%obj.schedule(2000,"safeDelete");
	
	MainScene.add(%obj);
}

function Game::displayBattleStats()
{
	if(isObject(Score))
		Score.delete();

	if(isObject(YouIcon))
		YouIcon.delete();
		
	if(isObject(SelectedVowel))
		SelectedVowel.delete();

	%obj = new ImageFont(YouIcon)  
	{   
		Image = "GameAssets:font";
		Position = "-30 30";
		FontSize = "4 4";
		Layer = 2;
		TextAlignment = "Center";
		Text = Player.Name;
	};
	
	MainScene.add(%obj);
	
	if(isObject(AIIcon))
		AIIcon.delete();
	
	%obj = new ImageFont(AIIcon)  
	{   
		Image = "GameAssets:font";
		Position = "30 30";
		FontSize = "4 4";
		Layer = 2;
		TextAlignment = "Center";
		Text = AI.Name;
	};  
		
	MainScene.add(%obj);
	
	Player.displayDefenseBar(Player.Defense,"-40 2");
	Player.displayHealthBar(Player.Health,"-40 0");
	AI.displayDefenseBar(AI.Defense,"20 2");
	AI.displayHealthBar(AI.Health,"20 0");
}

function Game::playImpactSound(%this)
{
	setRandomSeed(getRealTime());
	
	%roll = getRandom(0,2);
	
	if(%roll == 0)
	{
		alxPlay("GameAssets:impact1");
	}
	else if(%roll == 1)
	{
		alxPlay("GameAssets:impact2");
	}
	else if(%roll == 2)
	{
		alxPlay("GameAssets:impact3");
	}
}

function Game::playHitSound(%this, %damage)
{
	if(%damage == 0)
	{
		echo("Not playing sound because there is no damage." SPC %damage);
		return;
	}

	if(mAbs(%damage) <= 5)
	{
		alxPlay("GameAssets:Weakhit");
	}
	else if(mAbs(%damage) <= 10)
	{
		alxPlay("GameAssets:Mediumhit");
	}
	else
	{
		alxPlay("GameAssets:Stronghit");
	}
}

function Game::startBattle(%this, %damageToPlayer)
{
	MainScene.clear();
	Game.displayBattleStats();
	Player.schedule(1000,"attackAI",-Player.Damage/3);
	Player.schedule(2000,"attackAI",-Player.Damage/3);
	Player.schedule(3000,"attackAI",-Player.Damage/3);
	AI.schedule(4000,"attackPlayer",-%damageToPlayer/3);
	AI.schedule(5000,"attackPlayer",-%damageToPlayer/3);
	AI.schedule(6000,"attackPlayer",-%damageToPlayer/3);
	Game.schedule(7000,"startNewRound");
}