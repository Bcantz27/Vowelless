// ------------------------- GENERAL --------------------------- //

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
	Game.displayScore("0 10");
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

function Game::displayCategory(%this, %position)
{
	%obj = new ImageFont()  
	{   
		Image = "GameAssets:font";
		Position = %position;
		FontSize = "2 2";
		SceneLayer = 3;
		TextAlignment = "Center";
		Text = %this.Category;
	};  
	
	MainScene.add(%obj);
}

function Game::displayScore(%this, %position)
{
	if(isObject(Score))
		Score.delete();

	%obj = new ImageFont(Score)  
	{   
		Image = "GameAssets:ActionComic";
		Position = %position;
		FontSize = "5 5";
		SceneLayer = 3;
		TextAlignment = "Center";
		Text = Player.score;
	}; 
	
	MainScene.add(%obj);
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

function Game::displayVowelButtons(%this, %position)
{
	%obj = new Sprite()
	{
		class = ClickVowel;
	};
	%obj.Size = "8 8";
	%obj.Vowel  = "a";
	%obj.Position = VectorAdd(%position,"-30 0");
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
	%obj.Position = VectorAdd(%position,"-15 0");
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
	%obj.Position = %position;
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
	%obj.Position = VectorAdd(%position,"15 0");
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
	%obj.Position = VectorAdd(%position,"30 0");
	%obj.setSceneLayer(3);
	%obj.setBodyType("static");
	%obj.Image = "GameAssets:Woodhouse";
	%obj.Frame = getASCIIValue("u");
	
	MainScene.add(%obj);
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

// ------------------------- END GENERAL --------------------------- //
// ------------------------- BATTLE MODE --------------------------- //
function Game::displayBattleGame()
{
	MainScene.clear();
	Game.displayScore("40 33");
	Game.displayWord(true);
	Player.displayDefenseBar(Player.Defense,"-10 30");
	Player.displayHealthBar(Player.Health,"-10 28");
	Player.displayPowerUps();
	Game.displayTime();
	Game.displayCategory("0 25");
	Game.displayRound("-38 33");
	Game.displayVowelButtons("0 -20");
	Game.displayBackPanel("GameAssets:panelbeige");
}

function Game::displayRound(%this,%position)
{
	%obj = new ImageFont()  
	{   
		Image = "GameAssets:Woodhouse";
		Position = %position;
		FontSize = "2 2";
		SceneLayer = 2;
		TextAlignment = "Center";
		Text = "Round";
	};  
		
	MainScene.add(%obj);
	
	%obj = new ImageFont()  
	{   
		Image = "GameAssets:Woodhouse";
		Position = VectorAdd(%position, "0 -2");
		FontSize = "2 2";
		SceneLayer = 2;
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
		SceneLayer = 2;
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
		SceneLayer = 2;
		TextAlignment = "Center";
		Text = "You obtained" SPC %name;
	};  
	
	%obj.setBodyType("dynamic");
	%obj.setLinearVelocityY(4);
	%obj.schedule(2000,"safeDelete");
	
	MainScene.add(%obj);
}

function Game::displayBattleStats(%this)
{
	if(isObject(Score))
		Score.delete();

	if(isObject(YouIcon))
		YouIcon.delete();
		
	if(isObject(SelectedVowel))
		SelectedVowel.delete();

	Game.displayBackPanel("GameAssets:ToonBackground_light");	
	
	%obj = new ImageFont(YouIcon)  
	{   
		Image = "GameAssets:font";
		Position = "-30 30";
		FontSize = "4 4";
		SceneLayer = 2;
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
		SceneLayer = 2;
		TextAlignment = "Center";
		Text = AI.Name;
	};  
		
	MainScene.add(%obj);
	
	Player.displayDefenseBar(Player.Defense,"-40 22");
	Player.displayHealthBar(Player.Health,"-40 20");
	AI.displayDefenseBar(AI.Defense,"20 22");
	AI.displayHealthBar(AI.Health,"20 20");
	if(Game.Multiplayer)
	{
		AI.displayElo("30 18");
		Player.displayElo("-30 18");
	}
}

// ------------------------- END BATTLE MODE --------------------------- //