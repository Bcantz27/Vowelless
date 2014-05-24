function Game::create( %this )
{
	exec("./scripts/ClickVowel.cs");	
	exec("./scripts/PracticeMode.cs");
	exec("./scripts/RaceMode.cs");
	exec("./scripts/TimeMode.cs");
	exec("./scripts/BattleMode.cs");
	exec("./scripts/Utils.cs");
	exec("./scripts/GuiSetup.cs");
	
	Game.FullWordListSize = 0;
	Game.GameWordListSize = 0;
	Game.Mode = "";
	Game.ComboDelay = 5000;
	Game.Time = -1;
	Game.Round = 0;
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
	
	//populateFontCacheRange("Showcard Gothic", 14, 0, 65535);
	//writeFontCache ();
	dumpFontCacheStatus();
	
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
		Game.displayPreGame();
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
		MSClient.updateWPR(Player.Name, Player.WordsPerRound);
		MSClient.updateMostDamage(Player.Name, Player.Damage);
		
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
	if(Player.Battling || !isObject(Game.Word) || !isObject(SelectedVowel))
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
	%startPoint = -((strlen(%word)-1)*$letterOffset/2);
	
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
				%compositeSprite.setSpriteLocalPosition( (%spriteId - 1)*$letterOffset + %startPoint, 0 );
						
				// Set size.
				%compositeSprite.setSpriteSize( $wordFontSize );
				
				if(Game.FlipWords)
					%this.Word.setSpriteAngle( 180 );

				// Set the sprite image with a random frame.
				// We could also use an animation here. 
				%compositeSprite.setSpriteImage( "GameAssets:Cloudy", getASCIIValue(Game.VowelSel) );   
				
				Game.VowelSel = "";
				Game.CorrectVowels++;
				
				
				if(Game.CorrectVowels == Game.MissingVowels)
				{
					Player.CurrentWord++;
					Player.Streak++;
					Player.WordsPerRound++;
					alxPlay("GameAssets:correctSound");
					%this.FlipWords = false;
					
					if(stricmp(Game.Mode,"Battle") == 0)
					{
						Player.Damage = Player.Damage + getWordDamage($GameWordList[Player.CurrentWord]);
						Game.displayPlayerDamage("0 34");
					}
					else if(stricmp(Game.Mode,"Race") == 0)
					{
						Player.incrementScore(getWordValue($GameWordList[Player.CurrentWord]));
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
								
							Player.displayDefenseBar(Player.Defense,"-10 28");
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
				
				Player.displayDefenseBar(Player.Defense,"-10 28");
				
				alxPlay("GameAssets:Wronganswer");
				
				if(stricmp(Game.Mode,"Battle") == 0)
				{
					
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

function Game::removeBlindPanel(%this)
{
	if(isObject(BlindPanel))
		BlindPanel.delete();
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
