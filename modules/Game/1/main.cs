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
	Game.Multiplayer = false;
	Game.Category = "";
	Game.NumberOfCategories = 15;
	Game.Seed = 0;
	Game.WordPowerUp = -1;
	Game.FreezeTime = false;
	Game.FlipWords = false;

	setupVowels();

	hideSplashScreen();
	
	//$menuMusic = alxPlay("GameAssets:Menuloop");
}

function Game::flipWords(%this)
{
	%this.FlipWords = true;
	%this.displayWord(false);
}

function Game::freezeTime(%this)
{
	%this.FreezeTime = true;
	%this.schedule(5000,"unfreezeTime");
}

function Game::unfreezeTime(%this)
{
	%this.FreezeTime = false;
}

function Game::setupGame(%this, %multiplayer)
{
	Game.Multiplayer = %multiplayer;
	
	alxStopAll();
	
	Game.setupWordList();
	
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
	Game.Multiplayer = false;
	Game.WordPowerUp = -1;
	Player.reset();
	AI.reset();
	$menuMusic = alxPlay("GameAssets:Menuloop");
}

function Game::displayVowelButtons(%this)
{
	%obj = new Sprite()
	{
		class = ClickVowel;
	};
	%obj.Size = "8 8";
	%obj.Vowel  = "a";
	%obj.Position = "-30 -20";
	%obj.setSceneLayer(3);
	%obj.setBodyType("static");
	%obj.Image = "GameAssets:Woodhouse";
	%obj.Frame = getASCIIValue("a");
	
	MainScene.add(%obj);
	
	%obj = new Sprite()
	{
		class = ClickVowel;
	};
	%obj.Size = "8 8";
	%obj.Vowel  = "e";
	%obj.Position = "-15 -20";
	%obj.setSceneLayer(3);
	%obj.setBodyType("static");
	%obj.Image = "GameAssets:Woodhouse";
	%obj.Frame = getASCIIValue("e");
	
	MainScene.add(%obj);
	
	%obj = new Sprite()
	{
		class = ClickVowel;
	};
	%obj.Size = "8 8";
	%obj.Vowel  = "i";
	%obj.Position = "0 -20";
	%obj.setSceneLayer(3);
	%obj.setBodyType("static");
	%obj.Image = "GameAssets:Woodhouse";
	%obj.Frame = getASCIIValue("i");
	
	MainScene.add(%obj);
	
	%obj = new Sprite()
	{
		class = ClickVowel;
	};
	%obj.Size = "8 8";
	%obj.Vowel  = "o";
	%obj.Position = "15 -20";
	%obj.setSceneLayer(3);
	%obj.setBodyType("static");
	%obj.Image = "GameAssets:Woodhouse";
	%obj.Frame = getASCIIValue("o");
	
	MainScene.add(%obj);
	
	%obj = new Sprite()
	{
		class = ClickVowel;
	};
	%obj.Size = "8 8";
	%obj.Vowel  = "u";
	%obj.Position = "30 -20";
	%obj.setSceneLayer(3);
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
		if(!%this.FreezeTime)
		{
			Game.Time--;
			%this.displayTime();
		}
		%this.schedule(1000,"incrementTime");
	}
	else
	{
		Game.Time = 0;
		%this.displayTime();
		
		if(stricmp(Game.Mode,"Battle") == 0)
		{
			Player.Battling = true;
			if(Game.Multiplayer)
			{
				Player.Defense = mFloatLength(Player.Defense, 0);
				Player.Damage = mFloatLength(Player.Damage, 0);
				MSClient.setPlayerHealth(Player.GameID, Player.Name, Player.Health);
				MSClient.setPlayerDefense(Player.GameID, Player.Name, Player.Defense);
				MSClient.setPlayerDamage(Player.GameID, Player.Name, Player.Damage);
				//TODO Dont allow player to continue answering.
			}
			else
			{
				AI.Attacking = false;
				%this.startBattle(AI.Damage);
			}
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

function Game::checkPowerUp(%this, %worldPosition)
{
	if(Player.Battling || !isObject(PowerupIcons))
	{
		return;
	}
	
	%compositeSprite = PowerupIcons;
	%compositeSprite.setSceneLayer(3);
    
    // Pick sprites.
    %sprites = %compositeSprite.pickPoint( %worldPosition );    

    // Fetch sprite count.    
    %spriteCount = %sprites.count;
	
    // Finish if no sprites picked.
    if ( %spriteCount == 0 )
	{
		return; 
	}
        
    // Iterate sprites.
    for( %i = 0; %i < %spriteCount; %i++ )
    {
        // Fetch sprite Id.
        %spriteId = getWord( %sprites, %i );
		%powerupId = Player.PowerUps[%spriteId-1];
		echo("Sprite ID:" SPC (%spriteId-1) SPC "PowerUp ID:" SPC %powerupId);
		
		%compositeSprite.selectSpriteId( %spriteId );
		%compositeSprite.removeSprite();
		Player.usePowerUp(%powerupId);
    }
}

function Game::checkAnswer(%this, %worldPosition)
{
	if(Player.Battling || !isObject(Game.Word))
	{
		return;
	}

	// Fetch the composite sprite.
    %compositeSprite = Game.Word;
	%compositeSprite.setSceneLayer(3);
    
    // Pick sprites.
    %sprites = %compositeSprite.pickPoint( %worldPosition );    

    // Fetch sprite count.    
    %spriteCount = %sprites.count;
	
	%word = $VowellessList[Player.CurrentWord];
	%correctWord = $GameWordList[Player.CurrentWord];
	%startPoint = -((strlen(%word)-1)*8/2);
    
    // Finish if no sprites picked.
    if ( %spriteCount == 0 )
	{
		echo("Removing Vowel");
		SelectedVowel.safeDelete();
		Game.VowelSel = "";
		return; 
	}
        
    // Iterate sprites.
    for( %i = 0; %i < %spriteCount; %i++ )
    {
        // Fetch sprite Id.
        %spriteId = getWord( %sprites, %i );
		
		//echo("Sprite ID:" SPC %spriteID SPC "Char:" SPC getSubStr($VowellessList[Player.CurrentWord],(%spriteId - 1), 1));
        
		if(strcmp(getSubStr(%word,%spriteId - 1, 1),"_") == 0)
		{
			if(strcmp(getSubStr(%correctWord,%spriteId - 1, 1),Game.VowelSel) == 0)
			{
				SelectedVowel.safeDelete();
				%compositeSprite.selectSpriteId( %spriteId );
				%compositeSprite.removeSprite();
				
				// Add a sprite with no logical position.
				%compositeSprite.addSprite();
				
				// Set the sprites location position to a random location.
				%compositeSprite.setSpriteLocalPosition( (%spriteId - 1)*8 + %startPoint, 0 );
						
				// Set size.
				%compositeSprite.setSpriteSize( 8 );
				
				if(Game.FlipWords)
					%this.Word.setSpriteAngle( 180 );

				// Set the sprite image with a random frame.
				// We could also use an animation here. 
				%compositeSprite.setSpriteImage( "GameAssets:Woodhouse", getASCIIValue(Game.VowelSel) );   
				
				Game.VowelSel = "";
				Game.CorrectVowels++;
				
				
				if(Game.CorrectVowels == Game.MissingVowels)
				{
					Player.incrementScore(getWordValue($GameWordList[Player.CurrentWord]));
					Player.CurrentWord++;
					Player.Streak++;
					alxPlay("GameAssets:correctSound");
					%this.FlipWords = false;
					
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
						
						if(Game.Mode $= "Battle")
						{
							if((Player.Defense + Player.Combo) < Player.MaxDefense)
								Player.Defense = Player.Defense + Player.Combo;
							else
								Player.Defense = Player.MaxDefense;
								
							Player.displayDefenseBar(Player.Defense,"-10 30");
						}
						
						Player.schedule(Game.ComboDelay,"checkCombo");
						Player.endCombo = false;
					}
					else
					{
						Player.Combo = 0;
					}
					
					if(Game.WordPowerUp >= 0)
					{
						Player.addPowerup(Game.WordPowerUp);
						Game.WordPowerUp = -1;
					}
					
					Player.LastCorrectTime = getRealTime();
					Game.updateComboAndStreak();
					Game.DisplayWord(true);
					Game.CorrectVowels = 0;
				}
				else
				{
					//echo("CorrectVowels" SPC Game.CorrectVowels SPC "MissingVowels" SPC Game.MissingVowels);
				}
			}
			else
			{
				echo("Guess" SPC Game.VowelSel SPC "Answer" SPC getSubStr(%correctWord,%spriteId - 1, 1));
				SelectedVowel.safeDelete();
				Game.VowelSel = "";
				Player.resetCombo();
				Game.WordPowerUp = -1;
				
				Player.Defense = Player.Defense - 2;
				
				if(Player.Defense < 0)
					Player.Defense = 0;
				
				Player.displayDefenseBar(Player.Defense,"-10 30");
				
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
		else
		{
			SelectedVowel.safeDelete();
			Game.VowelSel = "";
			return;
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
	%backPanel.setSceneLayer(31);
	%backPanel.setBodyType("static");
	%backPanel.Image = %image;
	MainScene.add(%backPanel);
}

function Game::displayBlindPanel(%this, %image)
{
	%backPanel = new Sprite(BlindPanel);
	%backPanel.Size = "104 77";
	%backPanel.Position = "0 0";
	%backPanel.setSceneLayer(1);
	%backPanel.setBodyType("static");
	%backPanel.Image = %image;
	MainScene.add(%backPanel);
	
	Game.schedule(5000,"removeBlindPanel");
}

function Game::removeBlindPanel(%this)
{
	if(isObject(BlindPanel))
		BlindPanel.delete();
}

function Game::displayWord(%this, %newWord)
{
	if(isObject(Word))
		Word.delete();

	%word = $VowellessList[Player.CurrentWord];
		
	if(%newWord)
	{
		%this.CorrectVowels = 0;
		%this.WordPowerUp = -1;
		%this.MissingVowels = getNumberOfVowels($GameWordList[Player.CurrentWord]);
		
		if(%this.MissingVowels >= Powerup.TriggerThreshold)
		{
			%this.WordPowerUp = getRandom(0,Powerup.TotalPowerUps-1);
			echo("Adding word power up" SPC Game.WordPowerUp);
		}
		else if(%this.MissingVowels == Powerup.TriggerThreshold-1)
		{
			%random = getRandom(1,2);
			if(%random == 1)
			{
				%this.WordPowerUp = getRandom(0,Powerup.TotalPowerUps-1);
				echo("Adding word power up" SPC Game.WordPowerUp);
			}
		}
		
	}
	
	%this.Word = new CompositeSprite(Word)  ;
	%this.Word.SetBatchLayout("off");
	%this.Word.setSceneLayer(3);
	%startPoint = -((strlen(%word)-1)*8/2);
	
	// Add some sprites.
	for( %n = 0; %n < strlen(%word); %n++ )
	{
		%letter = getsubstr(%word, %n, 1);
		
        // Add a sprite with no logical position.
        %this.Word.addSprite();
        
        // Set the sprites location position to a random location.
        %this.Word.setSpriteLocalPosition( %n*8 + %startPoint, 0 );
                
        // Set size.
        %this.Word.setSpriteSize( 8 );
		
		if(Game.FlipWords)
			%this.Word.setSpriteAngle( 180 );

        // Set the sprite image with a random frame.
        // We could also use an animation here. 
        %this.Word.setSpriteImage( "GameAssets:Woodhouse", getASCIIValue(%letter) );                       
	}  
	
	MainScene.add(%this.Word);
	
	//echo("New Word Displayed:" SPC Player.CurrentWord SPC $VowellessList[Player.CurrentWord]);
}

function Game::displayCorrectWord(%this)
{
	if(isObject(Word))
		Word.delete();
	
	%obj = new ImageFont(Word)  
	{   
		Image = "GameAssets:Woodhouse";
		Position = "0 0";
		FontSize = "8 8";
		SceneLayer = 3;
		TextAlignment = "Center";
		Text = $GameWordList[Player.CurrentWord];
	};  
		
	MainScene.add(%obj);
}

function Game::displayCategory(%this)
{
	%obj = new ImageFont()  
	{   
		Image = "GameAssets:font";
		Position = "0 25";
		FontSize = "2 2";
		SceneLayer = 3;
		TextAlignment = "Center";
		Text = %this.Category;
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
		SceneLayer = 3;
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
		SceneLayer = 3;
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
		SceneLayer = 3;
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
		SceneLayer = 3;
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
		SceneLayer = 3;
		TextAlignment = "Center";
		Text = ("Time:" SPC Game.Time);
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
	Game.schedule(1000,"displayWord", true);
}
