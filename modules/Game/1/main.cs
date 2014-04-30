function Game::create( %this )
{
	exec("./scripts/Utils.cs");
	
	Game.FullWordListSize = 0;
	Game.GameWordListSize = 0;
	Game.Mode = "";
	Game.ComboDelay = 3000;
	Game.Time = -1;

	setupVowels();
	readWordsFile();

	hideSplashScreen();
}

function Game::setupGame(%this)
{
	if(stricmp(Game.Mode,"Battle") == 0)
	{
		%this.displayHealth();
	}
	else if(stricmp(Game.Mode,"Time") == 0)
	{
		Game.Time = 60;
		%this.displayTime();
		%this.startTimeGame();
	}
	else if(stricmp(Game.Mode,"Practice") == 0)
	{
	
	}
	
	%this.displayScore();
	%this.displayDifficulty();
	%this.displayNewWord();
}

function Game::destroy( %this )
{
	
}

function Game::shuffleWordList()
{
	setupGameWordList();
}

function Game::reset(%this)
{
	Game.FullWordListSize = 0;
	Game.GameWordListSize = 0;
	Game.Mode = "";
	Game.ComboDelay = 3000;
	Game.Time = -1;
	Player.reset();
}
function Game::checkAnswer(%this)
{
	%answer = Answer.getText();
	
	echo("Guess:" SPC %answer SPC "Answer:" SPC $GameWordList[Player.CurrentWord]);
	if(stricmp(%answer,$GameWordList[Player.CurrentWord]) == 0)
	{
		%this.incrementScore(getWordValue(%answer));
		Player.CurrentWord++;
		Player.Streak++;
		
		if(Player.Combo == 0 || ((getRealTime() - Player.LastCorrectTime) <= Game.ComboDelay))
		{
			Player.Combo++;
		}
		else
		{
			Player.Combo = 0;
		}
		
		Player.LastCorrectTime = getRealTime();
		Game.updateComboAndStreak();
		Game.DisplayNewWord();
		Answer.setText("");
	}
	else
	{
		Player.Streak = 0;
		Player.Combo = 0;
		Game.updateComboAndStreak();
		
		if(stricmp(Game.Mode,"Battle") == 0)
		{
			%this.incrementScore(-5);
			Player.changeHealth(getWordDamage($GameWordList[Player.CurrentWord]));
		}
		else if(stricmp(Game.Mode,"Time") == 0)
		{
			%this.incrementScore(-5);
		}
		else if(stricmp(Game.Mode,"Practice") == 0)
		{
			%this.incrementScore(-5);
		}
	}
}

function Game::getDifficulty(%this)
{
	if(Player.difficulty == 1)
	{
		return "Easy";
	}
	else if(Player.difficulty == 2)
	{
		return "Medium";
	}
	else if(Player.difficulty == 3)
	{
		return "Hard";
	}
	else if(Player.difficulty == 4)
	{
		return "Insane";
	}
}

function Game::displayNewWord(%this)
{
	if(isObject(Word))
		Word.delete();
	
	%obj = new ImageFont(Word)  
	{   
		Image = "GameAssets:font";
		Position = "0 0";
		FontSize = "12 12";
		Layer = 2;
		TextAlignment = "Center";
		Text = $VowellessList[Player.CurrentWord];
	};  
		
	echo("New Word Displayed:" SPC Player.CurrentWord SPC $VowellessList[Player.CurrentWord]);
	
	MainScene.add(%obj);
}

function Game::displayDifficulty(%this)
{
	%obj = new ImageFont()  
	{   
		Image = "GameAssets:font";
		Position = "40 30";
		FontSize = "2 2";
		Layer = 2;
		TextAlignment = "Center";
		Text = %this.getDifficulty();
	};  
	
	MainScene.add(%obj);
}

function Game::incrementTime(%this)
{
	if(Game.Time > 0)
	{
		Game.Time--;
		%this.displayTime();
		%this.schedule(1000,"incrementTime");
	}
	else
	{
		Game.Time = 0;
		%this.displayTime();
		%this.endGame();
	}
}

function Game::startTimeGame(%this)
{
	%this.schedule(3000,"incrementTime");
}

function Game::incrementScore(%this,%amount)
{
	Player.score = Player.score + %amount;
	%this.displayScore();
}

function Game::updateComboAndStreak(%this)
{
	%this.displayCombo();
	%this.displayStreak();
}

function Game::displayScore(%this)
{
	if(isObject(Score))
		Score.delete();

	%obj = new ImageFont(Score)  
	{   
		Image = "GameAssets:font";
		Position = "40 33";
		FontSize = "5 5";
		Layer = 2;
		TextAlignment = "Center";
		Text = Player.score;
	}; 
	
	MainScene.add(%obj);
}

function Game::displayFinalScore(%this)
{
	if(isObject(Score))
		Score.delete();

	if(isObject(FinalScore))
		FinalScore.delete();

	%obj = new ImageFont(FinalScore)  
	{   
		Image = "GameAssets:font";
		Position = "0 10";
		FontSize = "7 7";
		Layer = 2;
		TextAlignment = "Center";
		Text = Player.score;
	}; 
	
	MainScene.add(%obj);
}

function Game::displayStreak(%this)
{
	if(isObject(Streak))
		Streak.delete();
	
	if(Player.Streak == 0)
		return;
	
	%obj = new ImageFont(Streak)  
	{   
		Image = "GameAssets:font";
		Position = "40 25";
		FontSize = "2 2";
		Layer = 2;
		TextAlignment = "Center";
		Text = ("Steak:" SPC Player.Streak);
	};  
	
	MainScene.add(%obj);
}

function Game::displayCombo(%this)
{
	if(isObject(Combo))
		Combo.delete();
	
	if(Player.Combo == 0)
		return;
	
	%obj = new ImageFont(Combo)  
	{   
		Image = "GameAssets:font";
		Position = "40 20";
		FontSize = "2 2";
		Layer = 2;
		TextAlignment = "Center";
		Text = ("Combo:" SPC Player.Combo);
	};  
	
	MainScene.add(%obj);
}

function Game::displayTime(%this)
{
	if(isObject(Time))
		Time.delete();

	%obj = new ImageFont(Time)  
	{   
		Image = "GameAssets:font";
		Position = "0 33";
		FontSize = "2 2";
		Layer = 2;
		TextAlignment = "Center";
		Text = ("Time:" SPC Game.Time);
	};  
	
	MainScene.add(%obj);
}

function Game::endGame(%this)
{
	MainScene.clear();
	Game.displayFinalScore();
	Player.reset();
	Canvas.popDialog(GameGui);
	Canvas.pushDialog(LoseDialog);
}

function Game::displayHealth(%this)
{
	if(isObject(Health))
		Health.delete();

	%obj = new ImageFont(Health)  
	{   
		Image = "GameAssets:font";
		Position = "0 33";
		FontSize = "2 2";
		Layer = 2;
		TextAlignment = "Center";
		Text = ("Health:" SPC Player.Health);
	};  
	
	MainScene.add(%obj);
}
