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
			Game.FreezeTime = false;
			Game.FlipWords = false;
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