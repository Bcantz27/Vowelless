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
		Position = "-15 10";
		FontSize = "3 3";
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
		Position = "15 10";
		FontSize = "3 3";
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
		Position = "19 46";
		FontSize = "2 2";
		SceneLayer = 3;
		TextAlignment = "Center";
		Text = ("Time:" @ Game.Time);
	};  
	
	MainScene.add(%obj);
}

function Game::displayWinScreen()
{
	MainScene.clear();
	Player.reset();
	AI.reset();
	
	Game.displayBackPanel("GameAssets:ToonBackground_light");
	
	%obj = new ImageFont(YouIcon)  
	{   
		Image = "GameAssets:font";
		Position = "-15 45";
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
		Position = "15 45";
		FontSize = "4 4";
		SceneLayer = 2;
		TextAlignment = "Center";
		Text = AI.Name;
	};  
		
	MainScene.add(%obj);
	
	%playerrating = new Sprite();
	%playerrating.Size = "25 10";
	%playerrating.Position = "-15 20";
	%playerrating.setSceneLayer(6);
	%playerrating.setBodyType("static");
	%playerrating.Image = "GameAssets:PlayerRating";
	MainScene.add(%playerrating);
	
	%playerhighscore = new Sprite();
	%playerhighscore.Size = "25 10";
	%playerhighscore.Position = "-15 10";
	%playerhighscore.setSceneLayer(6);
	%playerhighscore.setBodyType("static");
	%playerhighscore.Image = "GameAssets:PlayerHighscore";
	MainScene.add(%playerhighscore);
	
	%enemyrating = new Sprite();
	%enemyrating.Size = "25 10";
	%enemyrating.Position = "15 20";
	%enemyrating.setSceneLayer(6);
	%enemyrating.setBodyType("static");
	%enemyrating.Image = "GameAssets:EnemyRating";
	MainScene.add(%enemyrating);
	
	%enemyhighscore = new Sprite();
	%enemyhighscore.Size = "25 10";
	%enemyhighscore.Position = "15 10";
	%enemyhighscore.setSceneLayer(6);
	%enemyhighscore.setBodyType("static");
	%enemyhighscore.Image = "GameAssets:EnemyHighscore";
	MainScene.add(%enemyhighscore);
	
	%winner = new Sprite();
	%winner.Size = "40 15";
	%winner.Position = "0 -20";
	%winner.setSceneLayer(6);
	%winner.setBodyType("static");
	%winner.Image = "GameAssets:Winner";
	MainScene.add(%winner);
	
	Canvas.pushDialog(EndGameDialog);
}

function Game::displayLoseScreen()
{
	MainScene.clear();
	Player.reset();
	AI.reset();
	
	Game.displayBackPanel("GameAssets:ToonBackground_light");
	
	%obj = new ImageFont(YouIcon)  
	{   
		Image = "GameAssets:font";
		Position = "-15 45";
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
		Position = "15 45";
		FontSize = "4 4";
		SceneLayer = 2;
		TextAlignment = "Center";
		Text = AI.Name;
	};  
		
	MainScene.add(%obj);
	
	%playerrating = new Sprite();
	%playerrating.Size = "25 10";
	%playerrating.Position = "-15 20";
	%playerrating.setSceneLayer(6);
	%playerrating.setBodyType("static");
	%playerrating.Image = "GameAssets:PlayerRating";
	MainScene.add(%playerrating);
	
	Player.displayElo("20 20");
	
	%playerhighscore = new Sprite();
	%playerhighscore.Size = "25 10";
	%playerhighscore.Position = "-15 10";
	%playerhighscore.setSceneLayer(6);
	%playerhighscore.setBodyType("static");
	%playerhighscore.Image = "GameAssets:PlayerHighscore";
	MainScene.add(%playerhighscore);
	
	%enemyrating = new Sprite();
	%enemyrating.Size = "25 10";
	%enemyrating.Position = "15 20";
	%enemyrating.setSceneLayer(6);
	%enemyrating.setBodyType("static");
	%enemyrating.Image = "GameAssets:EnemyRating";
	MainScene.add(%enemyrating);
	
	AI.displayElo("20 20");
	
	%enemyhighscore = new Sprite();
	%enemyhighscore.Size = "25 10";
	%enemyhighscore.Position = "15 10";
	%enemyhighscore.setSceneLayer(6);
	%enemyhighscore.setBodyType("static");
	%enemyhighscore.Image = "GameAssets:EnemyHighscore";
	MainScene.add(%enemyhighscore);
	
	%loser = new Sprite();
	%loser.Size = "40 15";
	%loser.Position = "0 -20";
	%loser.setSceneLayer(6);
	%loser.setBodyType("static");
	%loser.Image = "GameAssets:Loser";
	MainScene.add(%loser);
	
	Canvas.pushDialog(EndGameDialog);
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
		Text = "Category:" SPC %this.Category;
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
	$letterOffset = 8;
	%startPoint = -((strlen(%word)-1)*$letterOffset/2);
	
	while(%startPoint < -25)
	{
		$letterOffset--;
		%startPoint = -((strlen(%word)-1)*$letterOffset/2);
	}
	
	// Add some sprites.
	for( %n = 0; %n < strlen(%word); %n++ )
	{
		%letter = getsubstr(%word, %n, 1);
		
        // Add a sprite with no logical position.
        %this.Word.addSprite();
        
        // Set the sprites location position to a random location.
        %this.Word.setSpriteLocalPosition( %n*$letterOffset + %startPoint, 0 );
                
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
	%letterback = new Sprite();
	%letterback.Size = "10 10";
	%letterback.Position = VectorAdd(%position,"-20 0");
	%letterback.setSceneLayer(6);
	%letterback.setBodyType("static");
	%letterback.Image = "GameAssets:Background-03-Square";
	MainScene.add(%letterback);

	%obj = new Sprite()
	{
		class = ClickVowel;
	};
	%obj.Size = "8 8";
	%obj.Vowel  = "a";
	%obj.Position = VectorAdd(%position,"-20 0");
	%obj.setSceneLayer(3);
	%obj.setBodyType("static");
	%obj.Image = "GameAssets:Woodhouse";
	%obj.Frame = getASCIIValue("a");
	
	MainScene.add(%obj);
	
	%letterback = new Sprite();
	%letterback.Size = "10 10";
	%letterback.Position = VectorAdd(%position,"-10 0");
	%letterback.setSceneLayer(6);
	%letterback.setBodyType("static");
	%letterback.Image = "GameAssets:Background-03-Square";
	MainScene.add(%letterback);
	
	%obj = new Sprite()
	{
		class = ClickVowel;
	};
	%obj.Size = "8 8";
	%obj.Vowel  = "e";
	%obj.Position = VectorAdd(%position,"-10 0");
	%obj.setSceneLayer(3);
	%obj.setBodyType("static");
	%obj.Image = "GameAssets:Woodhouse";
	%obj.Frame = getASCIIValue("e");
	
	MainScene.add(%obj);
	
	%letterback = new Sprite();
	%letterback.Size ="10 10";
	%letterback.Position = VectorAdd(%position,"0 0");
	%letterback.setSceneLayer(6);
	%letterback.setBodyType("static");
	%letterback.Image = "GameAssets:Background-03-Square";
	MainScene.add(%letterback);
	
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
	
	%letterback = new Sprite();
	%letterback.Size = "10 10";
	%letterback.Position = VectorAdd(%position,"10 0");
	%letterback.setSceneLayer(6);
	%letterback.setBodyType("static");
	%letterback.Image = "GameAssets:Background-03-Square";
	MainScene.add(%letterback);
	
	%obj = new Sprite()
	{
		class = ClickVowel;
	};
	%obj.Size = "8 8";
	%obj.Vowel  = "o";
	%obj.Position = VectorAdd(%position,"10 0");
	%obj.setSceneLayer(3);
	%obj.setBodyType("static");
	%obj.Image = "GameAssets:Woodhouse";
	%obj.Frame = getASCIIValue("o");
	
	MainScene.add(%obj);
	
	%letterback = new Sprite();
	%letterback.Size = "10 10";
	%letterback.Position = VectorAdd(%position,"20 0");
	%letterback.setSceneLayer(6);
	%letterback.setBodyType("static");
	%letterback.Image = "GameAssets:Background-03-Square";
	MainScene.add(%letterback);
	
	%obj = new Sprite()
	{
		class = ClickVowel;
	};
	%obj.Size = "8 8";
	%obj.Vowel  = "u";
	%obj.Position = VectorAdd(%position,"20 0");
	%obj.setSceneLayer(3);
	%obj.setBodyType("static");
	%obj.Image = "GameAssets:Woodhouse";
	%obj.Frame = getASCIIValue("u");
	
	MainScene.add(%obj);
}

function Game::displayBackPanel(%this, %image)
{
	%backPanel = new Sprite();
	%backPanel.Size = "64 104";
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
	
	%wordback = new Sprite();
	%wordback.Size = "65 15";
	%wordback.Position = "0 0";
	%wordback.setSceneLayer(28);
	%wordback.setBodyType("static");
	%wordback.Image = "GameAssets:Background-03-Rectangle";
	MainScene.add(%wordback);
	
	//Game.displayScore("40 33");
	Game.displayWord(true);
	Player.displayDefenseBar(Player.Defense,"-10 28");
	Player.displayHealthBar(Player.Health,"-10 22");
	Player.displayPowerUps();
	Game.displayTime();
	Game.displayPlayerDamage("0 34");
	Game.displayCategory("0 40");
	Game.displayRound("-14 46");
	Game.displayVowelButtons("0 -20");
	Game.displayVitals("-15 28");
	Game.displayBackPanel("GameAssets:panelbeige");
}

function Game::displayPlayerDamage(%this,%position)
{
	if(isObject(PlayerDamage))
		PlayerDamage.delete();

	Player.Damage = mFloatLength(Player.Damage, 0);
		
	%obj = new ImageFont(PlayerDamage)  
	{   
		Image = "GameAssets:ActionComic";
		Position = %position;
		FontSize = "2 2";
		SceneLayer = 2;
		TextAlignment = "Center";
		Text = Player.Damage;
	};  
		
	MainScene.add(%obj);
}

function Game::displayAIDamage(%this,%position)
{
	if(isObject(AIDamage))
		AIDamage.delete();

	AI.Damage = mFloatLength(AI.Damage, 0);
		
	%obj = new ImageFont(AIDamage)  
	{   
		Image = "GameAssets:ActionComic";
		Position = %position;
		FontSize = "2 2";
		SceneLayer = 2;
		TextAlignment = "Center";
		Text = AI.Damage;
	};  
		
	MainScene.add(%obj);
}

function Game::displayVitals(%this,%position)
{
	%vitals = new Sprite();
	%vitals.Size = "6 18";
	%vitals.Position = %position;
	%vitals.setSceneLayer(3);
	%vitals.setBodyType("static");
	%vitals.Image = "GameAssets:Vitals";
	MainScene.add(%vitals);
}

function Game::displayRound(%this,%position)
{
	
	%backbox = new Sprite();
	%backbox.Size = "63 7";
	%backbox.Position = "0 46";
	%backbox.setSceneLayer(5);
	%backbox.setBodyType("static");
	%backbox.Image = "GameAssets:Background-11-Rectangle";
	MainScene.add(%backbox);
	
	%obj = new ImageFont()  
	{   
		Image = "GameAssets:Woodhouse";
		Position = %position;
		FontSize = "2 2";
		SceneLayer = 2;
		TextAlignment = "Center";
		Text = "Round" SPC Game.Round @ " of 3";
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
		Position = "-15 45";
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
		Position = "15 45";
		FontSize = "4 4";
		SceneLayer = 2;
		TextAlignment = "Center";
		Text = AI.Name;
	};  
		
	MainScene.add(%obj);
	
	Game.displayVitals("-27 20");
	Game.displayVitals("4 20");
	
	Game.displayPlayerDamage("-12 26");
	Game.displayAIDamage("18 26");
	
	Player.displayDefenseBar(Player.Defense,"-22 20");
	Player.displayHealthBar(Player.Health,"-22 14");
	AI.displayDefenseBar(AI.Defense,"8 20");
	AI.displayHealthBar(AI.Health,"8 14");
}

// ------------------------- END BATTLE MODE --------------------------- //