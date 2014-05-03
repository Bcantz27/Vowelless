function Game::displayBattleGame()
{
	MainScene.clear();
	Game.displayScore();
	Game.displayNewWord();
	Game.displayHealth();
	Game.displayTime();
	Game.displayRound();
	Canvas.pushDialog(GameGui);
}

function Game::startNewRound()
{
	if(stricmp(Game.Mode,"Battle") == 0)
	{
		if(Player.Health <= 0)
		{
			Canvas.popDialog(GameGui);
			Game.displayLoseScreen();
			return;
		}
		else if(AI.Health <= 0)
		{
			Canvas.popDialog(GameGui);
			Game.displayWinScreen();
			return;
		}
	
		if(Game.Round == 3)	//Determine Winner
		{
			Canvas.popDialog(GameGui);
			
			if(Player.Health > AI.Health)
			{
				Game.displayWinScreen();
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
			Player.CurrentWord++;
			AI.CurrentWord++;
			Answer.setText("");
			Game.displayBattleGame();
			Game.schedule(2000,"incrementTime");
			AI.schedule(2000,"attackPlayer");
		}
	}
}

function Game::displayRound()
{
	%obj = new ImageFont()  
	{   
		Image = "GameAssets:font";
		Position = "-38 33";
		FontSize = "2 2";
		Layer = 2;
		TextAlignment = "Center";
		Text = "Round:" SPC Game.Round @ "/3";
	};  
		
	MainScene.add(%obj);
}

function Game::displayBattleStats()
{
	MainScene.clear();

	%obj = new ImageFont()  
	{   
		Image = "GameAssets:font";
		Position = "-30 30";
		FontSize = "8 8";
		Layer = 2;
		TextAlignment = "Center";
		Text = "You";
	};
	
	MainScene.add(%obj);
	
	%obj = new ImageFont()  
	{   
		Image = "GameAssets:font";
		Position = "30 30";
		FontSize = "8 8";
		Layer = 2;
		TextAlignment = "Center";
		Text = "AI";
	};  
		
	MainScene.add(%obj);
	
	%health = new ImageFont()  
	{   
		Image = "GameAssets:font";
		Position = "-30 10";
		FontSize = "4 4";
		Layer = 2;
		TextAlignment = "Center";
		Text = "HP:" SPC Player.Health;
	};  
		
	MainScene.add(%health);
	
	%Enemyhealth = new ImageFont()  
	{   
		Image = "GameAssets:font";
		Position = "30 10";
		FontSize = "4 4";
		Layer = 2;
		TextAlignment = "Center";
		Text = "HP:" SPC AI.Health;
	};  
		
	MainScene.add(%Enemyhealth);
	
	%playerDamage = new ImageFont()  
	{   
		Image = "GameAssets:font";
		Position = "-30 0";
		FontSize = "2 2";
		Layer = 2;
		TextAlignment = "Center";
		Text = "Damage:" SPC Player.Damage;
	};  
		
	MainScene.add(%playerDamage);
}

function Game::endBattleGame(%winner)
{

}

function Game::startBattle()
{
	Canvas.popDialog(GameGui);
	Game.displayBattleStats();
	Player.schedule(1000,"changeHealth",-AI.Damage/3);
	Game.schedule(1000,"displayBattleStats");
	Player.schedule(2000,"changeHealth",-AI.Damage/3);
	Game.schedule(2000,"displayBattleStats");
	Player.schedule(3000,"changeHealth",-AI.Damage/3);
	Game.schedule(3000,"displayBattleStats");
	AI.schedule(4000,"changeHealth",-Player.Damage/3);
	Game.schedule(4000,"displayBattleStats");
	AI.schedule(5000,"changeHealth",-Player.Damage/3);
	Game.schedule(5000,"displayBattleStats");
	AI.schedule(6000,"changeHealth",-Player.Damage/3);
	Game.schedule(6000,"displayBattleStats");
	Game.schedule(7000,"startNewRound");
}