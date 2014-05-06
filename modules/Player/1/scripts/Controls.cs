Player.InputController = new ScriptObject()
{
    class = InputHandler;
};

//-----------------------------------------------------------------------------

function InputHandler::initialize( %this )
{
	echo("Input Handler initialized");
    // Add touch gester as an input listener.
    MainWindow.addInputListener( %this );
}

//-----------------------------------------------------------------------------

function InputHandler::onTouchDown(%this, %touchID, %worldPosition)
{    
	echo("Touch Down");
}

function InputHandler::onTouchUp(%this, %touchID, %worldPosition)
{    
	echo("Touch Up");
    // Fetch the composite sprite.
    %compositeSprite = Game.Word;
    
    // Pick sprites.
    %sprites = %compositeSprite.pickPoint( %worldPosition );    

    // Fetch sprite count.    
    %spriteCount = %sprites.count;
	
	%word = $VowellessList[Player.CurrentWord];
	%correctWord = $GameWordList[Player.CurrentWord];
	%startPoint = -(strlen(%word)*10/2);
    
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
				%compositeSprite.setSpriteLocalPosition( (%spriteId - 1)*10 + %startPoint, 0 );
						
				// Set size.
				%compositeSprite.setSpriteSize( 12 );

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
						
						if((Player.Defense + Player.Combo) < Player.MaxDefense)
							Player.Defense = Player.Defense + (Player.Combo/2);
						else
							Player.Defense = Player.MaxDefense;
							
						Player.displayDefenseBar(Player.Defense,"-10 30");
							
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
					Game.CorrectVowels = 0;
				}
				else
				{
					echo("CorrectVowels" SPC Game.CorrectVowels SPC "MissingVowels" SPC Game.MissingVowels);
				}
			}
			else
			{
				echo("Guess" SPC Game.VowelSel SPC "Answer" SPC getSubStr(%correctWord,%spriteId - 1, 1));
				SelectedVowel.safeDelete();
				Game.VowelSel = "";
				Player.Streak = 0;
				Player.Combo = 0;
				Game.updateComboAndStreak();
				
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

function InputHandler::onTouchDragged(%this, %touchID, %worldPosition)
{    
	if(isLetterAVowel(Game.VowelSel))
	{
		//echo("Selected Vowel:" SPC Game.VowelSel);
		if(!isObject(SelectedVowel))
		{
			%obj = new Sprite(SelectedVowel);
			%obj.Size = "10 10";
			%obj.Position = %worldPosition;
			%obj.SceneLayer = 1;
			%obj.setBodyType("dynamic");
			%obj.Image = "GameAssets:Woodhouse";
			%obj.Frame = getASCIIValue(Game.VowelSel);
			
			MainScene.add(%obj);
		}
		else
		{
			SelectedVowel.MoveTo(%worldPosition, 600, true, true);
		}
	}
}