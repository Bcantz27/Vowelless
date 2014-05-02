function Game::displayBattleGame()
{
	MainScene.clear();
	Game.displayScore();
	Game.displayNewWord();
	Game.displayHealth();
	Game.displayTime();
	Canvas.pushDialog(GameGui);
}

function Game::startNewRound()
{
	if(stricmp(Game.Mode,"Battle") == 0)
	{
		if(Player.Health <= 0)
		{
			Canvas.popDialog(GameGui);
			Game.displayEndScreen();
			return;
		}
		else if(AI.Health <= 0)
		{
			Canvas.popDialog(GameGui);
			Game.displayEndScreen();
			return;
		}
	
		if(Game.Round == 3)
		{
			//Determine Winner
			Player.Damage = 0;
			AI.Damage = 0;
		}
		else
		{
			Game.Round++;
			Game.Time = 30;
			Player.Damage = 0;
			AI.Damage = 0;
			Game.displayBattleGame();
			Game.schedule(2000,"incrementTime");
		}
	}
}

function Game::displayRound()
{

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
		Text = Player.Health;
	};  
		
	MainScene.add(%health);
	
	%Enemyhealth = new ImageFont()  
	{   
		Image = "GameAssets:font";
		Position = "30 10";
		FontSize = "4 4";
		Layer = 2;
		TextAlignment = "Center";
		Text = AI.Health;
	};  
		
	MainScene.add(%Enemyhealth);
	
	%playerDamage = new ImageFont()  
	{   
		Image = "GameAssets:font";
		Position = "-30 0";
		FontSize = "2 2";
		Layer = 2;
		TextAlignment = "Center";
		Text = Player.Damage;
	};  
		
	MainScene.add(%playerDamage);
}

function Game::endBattleGame(%winner)
{

}

function startBattle()
{
	Canvas.popDialog(GameGui);
	Game.displayBattleStats();
	Player.schedule(1000,"changeHealth",-AI.Damage/3);
	Game.schedule(1000,"displayBattleStats");
	Player.schedule(2000,"Player.changeHealth",-AI.Damage/3);
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