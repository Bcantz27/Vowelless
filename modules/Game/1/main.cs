function Game::create( %this )
{
	Game.setupWords();

	hideSplashScreen();
}

function Game::destroy( %this )
{

}

function Game::checkAnswer(%this)
{

}

function Game::setupWords(%this)
{
	$FullWords[0] = "Cat";
	$VowellessWords[0] = Game.removeVowels($FullWords[0]);
	$FullWords[1] = "Monitor";
	$VowellessWords[1] = Game.removeVowels($FullWords[1]);
	$FullWords[2] = "Speaker";
	$VowellessWords[2] = Game.removeVowels($FullWords[2]);
}

function Game::displayNewWord(%this)
{
	Word.destroy();
	
	%obj = new ImageFont(Word)  
	{   
		Image = "GameAssets:font";
		Position = "0 0";
		FontSize = "12 10";
		Layer = 2;
		TextAlignment = "Center";
		Text = $VowellessWords[Player.CurrentWord];
	};  
	
	echo("New Word Displayed:" SPC Player.CurrentWord SPC $VowellessWords[Player.CurrentWord]);
	
	MainScene.add(%obj);
}

function Game::reset(%this)
{

}

function Game::removeVowels(%this, %word)
{
	%word = stripChars(%word,"A");
	%word = stripChars(%word,"E");
	%word = stripChars(%word,"I");
	%word = stripChars(%word,"O");
	%word = stripChars(%word,"U");
	%word = stripChars(%word,"Y");
	%word = stripChars(%word,"a");
	%word = stripChars(%word,"e");
	%word = stripChars(%word,"i");
	%word = stripChars(%word,"o");
	%word = stripChars(%word,"u");
	%word = stripChars(%word,"y");
	%word = stripTrailingSpaces(%word);
	
	//echo("Word:" SPC %word);
	
	return %word;
}

