function Game::create( %this )
{
	exec("./scripts/ClickVowel.cs");	
	exec("./scripts/PracticeMode.cs");
	exec("./scripts/RaceMode.cs");
	exec("./scripts/TimeMode.cs");
	exec("./scripts/BattleMode.cs");
	exec("./scripts/Utils.cs");
	
	Game.FullWordListSize = 0;
	Game.GameWordListSize = 0;
	Game.Mode = "";
	Game.ComboDelay = 5000;
	Game.Time = -1;
	Game.Round = -1;
	Game.RaceTo = 3000;
	Game.VowelSel = "";
	Game.MissingVowels = 0;
	Game.CorrectVowels = 0;

	setupVowels();
	readWordsFile();

	hideSplashScreen();
	
	$menuMusic = alxPlay("GameAssets:Menuloop");
}

function Game::setupGame(%this)
{
	alxStopAll();
	if(stricmp(Game.Mode,"Battle") == 0)
	{
		Game.Round = 1;
		Game.Time = 30;
		%this.displayBattleGame();
		%this.startTimeGame();
	}
	else if(stricmp(Game.Mode,"Race") == 0)
	{
		Game.Time = 3;
		%this.startTimeGame();
		%this.displayRaceGame();
	}
	else if(stricmp(Game.Mode,"Time") == 0)
	{
		Game.Time = 60;
		%this.displayTimeGame();
		%this.startTimeGame();
	}
	else if(stricmp(Game.Mode,"Practice") == 0)
	{
		SkipButton.Visible = 1;
		%this.displayPracticeGame();
	}
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
	Game.VowelSel = "";
	Game.MissingVowels = 0;
	Game.CorrectVowels = 0;
	Player.reset();
	AI.reset();
	$menuMusic = alxPlay("GameAssets:Menuloop");
}

function Game::checkAnswer(%this)
{
	%answer = Answer.getText();
	
	echo("Guess:" SPC %answer SPC "Answer:" SPC $GameWordList[Player.CurrentWord]);
	if(stricmp(%answer,$GameWordList[Player.CurrentWord]) == 0)
	{
		Player.incrementScore(getWordValue(%answer));
		Player.CurrentWord++;
		Player.Streak++;
		alxPlay("GameAssets:correctSound");
		
		if(stricmp(Game.Mode,"Battle") == 0)
		{
			Player.Damage = Player.Damage + getWordDamage($GameWordList[Player.CurrentWord]);
		}
		else if(stricmp(Game.Mode,"Race") == 0)
		{
			if(Player.Score >= Game.RaceTo)
			{
				Game.endRace();
				return;
			}
		}
		
		if(Player.Combo == 0 || ((getRealTime() - Player.LastCorrectTime) <= Game.ComboDelay))
		{
			Player.Combo++;
			Player.schedule(Game.ComboDelay,"checkCombo");
			Player.endCombo = false;
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
		
		alxPlay("GameAssets:Wronganswer");
		
		if(stricmp(Game.Mode,"Battle") == 0)
		{
			Player.incrementScore(-5);
		}
		else if(stricmp(Game.Mode,"Time") == 0)
		{
			Player.incrementScore(-5);
		}
		else if(stricmp(Game.Mode,"Practice") == 0)
		{
			Player.incrementScore(-5);
		}
	}
}

function Game::displayVowelButtons(%this)
{
	%obj = new Sprite()
	{
		class = ClickVowel;
	};
	%obj.Size = "10 10";
	%obj.Vowel  = "a";
	%obj.Position = "-30 -20";
	%obj.SceneLayer = 3;
	%obj.setBodyType("static");
	%obj.Image = "GameAssets:Woodhouse";
	%obj.Frame = getASCIIValue("a");
	
	MainScene.add(%obj);
	
	%obj = new Sprite()
	{
		class = ClickVowel;
	};
	%obj.Size = "10 10";
	%obj.Vowel  = "e";
	%obj.Position = "-15 -20";
	%obj.SceneLayer = 3;
	%obj.setBodyType("static");
	%obj.Image = "GameAssets:Woodhouse";
	%obj.Frame = getASCIIValue("e");
	
	MainScene.add(%obj);
	
	%obj = new Sprite()
	{
		class = ClickVowel;
	};
	%obj.Size = "10 10";
	%obj.Vowel  = "i";
	%obj.Position = "0 -20";
	%obj.SceneLayer = 3;
	%obj.setBodyType("static");
	%obj.Image = "GameAssets:Woodhouse";
	%obj.Frame = getASCIIValue("i");
	
	MainScene.add(%obj);
	
	%obj = new Sprite()
	{
		class = ClickVowel;
	};
	%obj.Size = "10 10";
	%obj.Vowel  = "o";
	%obj.Position = "15 -20";
	%obj.SceneLayer = 3;
	%obj.setBodyType("static");
	%obj.Image = "GameAssets:Woodhouse";
	%obj.Frame = getASCIIValue("o");
	
	MainScene.add(%obj);
	
	%obj = new Sprite()
	{
		class = ClickVowel;
	};
	%obj.Size = "10 10";
	%obj.Vowel  = "u";
	%obj.Position = "30 -20";
	%obj.SceneLayer = 3;
	%obj.setBodyType("static");
	%obj.Image = "GameAssets:Woodhouse";
	%obj.Frame = getASCIIValue("u");
	
	MainScene.add(%obj);
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
		
		if(stricmp(Game.Mode,"Battle") == 0)
		{
			AI.Attacking = false;
			%this.startBattle();
		}
		else if(stricmp(Game.Mode,"Race") == 0)
		{
			%this.startRace();
			if(isObject(Time))
				Time.delete();
		}
		else if(stricmp(Game.Mode,"Time") == 0)
		{
			Canvas.popDialog(GameGui);
			%this.displayWinScreen();
		}
	}
}

function Game::startTimeGame(%this)
{
	%this.schedule(3000,"incrementTime");
	
	if(stricmp(Game.Mode,"Battle") == 0)
	{
		AI.schedule(3000,"readWord");
	}
}

function Game::updateComboAndStreak(%this)
{
	%this.displayCombo();
	%this.displayStreak();
}

function Game::displayBackPanel(%this, %image)
{
	%backPanel = new Sprite();
	%backPanel.Size = "104 77";
	%backPanel.Position = "0 0";
	%backPanel.SceneLayer = 31;
	%backPanel.setBodyType("static");
	%backPanel.Image = %image;
	MainScene.add(%backPanel);
}

function Game::displayNewWord(%this)
{
	if(isObject(Word))
		Word.delete();
		
	%word = $VowellessList[Player.CurrentWord];
	
	%this.MissingVowels = getNumberOfVowels($GameWordList[Player.CurrentWord]);
	
	%this.Word = new CompositeSprite(Word)  ;
	%this.Word.SetBatchLayout("off");
	%this.Word.Layer = 2;
	%startPoint = -(strlen(%word)*10/2);
	
	// Add some sprites.
	for( %n = 0; %n < strlen(%word); %n++ )
	{
		%letter = getsubstr(%word, %n, 1);
		
        // Add a sprite with no logical position.
        %this.Word.addSprite();
        
        // Set the sprites location position to a random location.
        %this.Word.setSpriteLocalPosition( %n*10 + %startPoint, 0 );
                
        // Set size.
        %this.Word.setSpriteSize( 12 );

        // Set the sprite image with a random frame.
        // We could also use an animation here. 
        %this.Word.setSpriteImage( "GameAssets:Woodhouse", getASCIIValue(%letter) );                       
	}  
	
	MainScene.add(%this.Word);
	
	echo("New Word Displayed:" SPC Player.CurrentWord SPC $VowellessList[Player.CurrentWord]);
}

function Game::displayCorrectWord(%this)
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
		Text = $GameWordList[Player.CurrentWord];
	};  
		
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

function Game::displayScore(%this)
{
	if(isObject(Score))
		Score.delete();

	%obj = new ImageFont(Score)  
	{   
		Image = "GameAssets:ActionComic";
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
		Image = "GameAssets:ActionComic";
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
		Image = "GameAssets:ActionComic";
		Position = "40 25";
		FontSize = "2 2";
		Layer = 2;
		TextAlignment = "Center";
		Text = (Player.Streak SPC "STREAK");
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
		Image = "GameAssets:ActionComic";
		Position = "40 20";
		FontSize = "2 2";
		Layer = 2;
		TextAlignment = "Center";
		Text = (Player.Combo SPC "COMBO");
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
		Position = "0 35";
		FontSize = "2 2";
		Layer = 2;
		TextAlignment = "Center";
		Text = ("Time:" SPC Game.Time);
	};  
	
	MainScene.add(%obj);
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

function Game::displayWinScreen()
{
	MainScene.clear();
	Game.displayFinalScore();
	Player.reset();
	AI.reset();
	Canvas.pushDialog(LoseDialog);
}

function Game::displayLoseScreen()
{
	MainScene.clear();
	Player.reset();
	AI.reset();
	Canvas.pushDialog(LoseDialog);
}

function Game::skipWord()
{
	echo("Skipping word");
	Player.Streak = 0;
	Player.Combo = 0;
	Game.updateComboAndStreak();
	Game.displayCorrectWord();
	Player.CurrentWord++;
	Game.schedule(1000,"displayNewWord");
}
