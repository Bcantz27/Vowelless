function Game::create( %this )
{
	exec("./scripts/Utils.cs");
	
	Game.FullWordListSize = 0;
	Game.GameWordListSize = 0;

	setupVowels();
	readWordsFile();

	hideSplashScreen();
	
	%this.displayNewWord();
	%this.displayScore();
	%this.displayDifficulty();
}

function Game::destroy( %this )
{
	
}

function Game::reset(%this)
{

}

function Game::checkAnswer(%this)
{

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
	Word.destroy();
	
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

function Game::displayScore(%this)
{
	Score.destroy();

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
