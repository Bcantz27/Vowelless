function getASCIIValue(%letter)
{
	%testStr = "0123456789:;<=>?@ABCDEFGHIJKLMNOPQRSTUVWXYZ[\]^_'abcdefghijklmnopqrstuvwxyz";
	if(strpos(%testStr, %letter) != -1)
	{
		%index = strpos(%testStr, %letter);
		%val = %index + 17;
	}
	
	echo(%letter SPC %val);
	
	return %val; 
}

function removeVowels(%word)
{
	%word = stripChars(%word,"A");
	%word = stripChars(%word,"E");
	%word = stripChars(%word,"I");
	%word = stripChars(%word,"O");
	%word = stripChars(%word,"U");
	//%word = stripChars(%word,"Y");
	%word = stripChars(%word,"a");
	%word = stripChars(%word,"e");
	%word = stripChars(%word,"i");
	%word = stripChars(%word,"o");
	%word = stripChars(%word,"u");
	//%word = stripChars(%word,"y");
	//%word = stripTrailingSpaces(%word);
	
	//echo("Word:" SPC %word);
	
	return %word;
}

function removeVowelsAndPutUnderScore(%word)
{
	//echo("Removing Vowels from:" SPC %word);
	for(%i = 0; %i < 10; %i++)
	{
		%charIndex = strpos(%word,$Vowels[%i]);
		if(%charIndex != -1)
		{
			%firstHalf = getSubStr(%word, 0 , %charIndex);
			if(%charIndex != (strlen(%word)-1))
			{
				%secondHalf = getSubStr(%word, %charIndex + 1, (strlen(%word)-(%charIndex+1)));
				%word = %firstHalf @ "_" @  %secondHalf;
			}
			else
			{
				%word = %firstHalf @ "_";
			}
			
			//echo("First:" SPC %firstHalf SPC "Second:" SPC %secondHalf);
			%i--;
		}
	}

	//%word = stripTrailingSpaces(%word);
	
	//echo("Finished Word:" SPC %word);
	
	return %word;
}

function getNumberOfVowels(%word)
{
	%vowelCount = 0;

	for(%i = 0; %i < 10; %i++)
	{
		%charIndex = strpos(%word,$Vowels[%i]);
		if(%charIndex != -1)
		{
			%firstHalf = getSubStr(%word, 0 , %charIndex);
			%secondHalf = getSubStr(%word, %charIndex + 1, (strlen(%word)-(%charIndex+1)));
			%word = %firstHalf @ "_" @  %secondHalf;
			%vowelCount++;
			%i--;
		}
	}
	
	//echo("Word:" SPC %word SPC "Vowel Count:" SPC %vowelCount);
	
	return %vowelCount;
}

function getWordDifficulty(%word)
{
	%vowelCount = getNumberOfVowels(%word);
	%wordDiff = 1;
	
	if(%vowelCount <= 2)
	{
		%wordDiff = 1;
	}
	else if(%vowelCount <= 4)
	{
		%wordDiff = 2;
	}
	else if(%vowelCount <= 6)
	{
		%wordDiff = 3;
	}
	else
	{
		%wordDiff = 4;
	}
	
	//echo("Word:" SPC %word SPC "Word Difficulty:" SPC %wordDiff);
	
	return %wordDiff;
}

function Game::setCategory(%this,%catnumber)
{
	if(%catnumber == 0)
	{
		%this.Category = "Sports";
	}
	else if(%catnumber == 1)
	{
		%this.Category = "Food";
	}
	else if(%catnumber == 2)
	{
		%this.Category = "Art";
	}
	else if(%catnumber == 3)
	{
		%this.Category = "Bathroom";
	}
	else if(%catnumber == 4)
	{
		%this.Category = "Clothing";
	}
	else if(%catnumber == 5)
	{
		%this.Category = "Computer";
	}
	else if(%catnumber == 6)
	{
		%this.Category = "Farm";
	}
	else if(%catnumber == 7)
	{
		%this.Category = "Flowers";
	}
	else if(%catnumber == 8)
	{
		%this.Category = "Public Buildings";
	}
	else if(%catnumber == 9)
	{
		%this.Category = "Human Body";
	}
	else if(%catnumber == 10)
	{
		%this.Category = "Jobs";
	}
	else if(%catnumber == 11)
	{
		%this.Category = "Money";
	}
	else if(%catnumber == 12)
	{
		%this.Category = "Office";
	}
	else if(%catnumber == 13)
	{
		%this.Category = "School";
	}
	else if(%catnumber == 14)
	{
		%this.Category = "Weather";
	}
}

function shuffleGameWordList()
{
	if(!Game.Multiplayer)
	{
		setRandomSeed(getRealTime());
	}
	else
	{
		setRandomSeed(Game.Seed);
	}
	%size = Game.GameWordListSize;

	if(%size > 1)
	{
		for(%i = 0; %i < (%size - 1); %i++)
		{
			%j = %i + getRandom(0,((%size-1)-%i));
			%temp = $GameWordList[%j];
			$GameWordList[%j] = $GameWordList[%i];
			//echo("Swapping" SPC $GameWordList[%i] SPC "with" SPC %temp);
			$GameWordList[%i] = %temp;
		}
	}
	echo("Shuffled Array");
}

function Game::setupWordList(%this)
{
	if(!Game.Multiplayer)
	{
		echo(getRealTime());
		setRandomSeed(getRealTime());
	}
	else
	{
		echo(Game.Seed);
		setRandomSeed(Game.Seed);
		echo(getRandomSeed());
	}
	
	%catnumber = getRandom(0,(Game.NumberOfCategories-1));
	
	Game.setCategory(%catnumber);
	
	echo("Cat#:" SPC %catnumber SPC "Cat:" SPC Game.Category);
	
	readWordsFile(%catnumber);
}

function readWordsFile(%cata)
{
	%file = new FileObject();
	
	Game.FullWordListSize = 0;
	Game.GameWordListSize = 0;
	
	if(%cata $= "Sports" || %cata == 0)
	{
		%path = "./data/Wordlists/Sports.txt";
	}
	else if(%cata $= "Food" || %cata == 1)
	{
		%path = "./data/Wordlists/Food.txt";
	}
	else if(%cata $= "Art" || %cata == 2)
	{
		%path = "./data/Wordlists/Art.txt";
	}
	else if(%cata $= "Bathroom" || %cata == 3)
	{
		%path = "./data/Wordlists/Bathroom.txt";
	}
	else if(%cata $= "Clothing" || %cata == 4)
	{
		%path = "./data/Wordlists/Clothing.txt";
	}
	else if(%cata $= "Computer" || %cata == 5)
	{
		%path = "./data/Wordlists/Computer.txt";
	}
	else if(%cata $= "Farm" || %cata == 6)
	{
		%path = "./data/Wordlists/Farm.txt";
	}
	else if(%cata $= "Flowers" || %cata == 7)
	{
		%path = "./data/Wordlists/Flowers.txt";
	}
	else if(%cata $= "Public Buildings" || %cata == 8)
	{
		%path = "./data/Wordlists/Public Buildings.txt";
	}
	else if(%cata $= "Human Body" || %cata == 9)
	{
		%path = "./data/Wordlists/Human Body.txt";
	}
	else if(%cata $= "Jobs" || %cata == 10)
	{
		%path = "./data/Wordlists/Jobs.txt";
	}
	else if(%cata $= "Money" || %cata == 11)
	{
		%path = "./data/Wordlists/Money.txt";
	}
	else if(%cata $= "Office" || %cata == 12)
	{
		%path = "./data/Wordlists/Office.txt";
	}
	else if(%cata $= "School" || %cata == 13)
	{
		%path = "./data/Wordlists/School.txt";
	}
	else if(%cata $= "Weather" || %cata == 14)
	{
		%path = "./data/Wordlists/Weather.txt";
	}
	
	if(%file.openForRead(%path))
	{
		%x=1;
		while(!%file.isEof())
		{
			%line = %file.readLine();
			//echo("line" @ %x @ " = " @ %line);
			$FullWordList[%x-1] = %line;
			Game.FullWordListSize++;
			%x++;
		}
	}
	else
	{
		error("CANNOT OPEN FOR READ");
	}
	
	%file.close();
	%file.delete();
	
	echo("Done Reading File. Words Found:" SPC Game.FullWordListSize);
	
	setupGameWordList();
}

function setupGameWordList()
{
	for(%i = 0; %i < Game.FullWordListSize; %i++)
	{
		$GameWordList[Game.GameWordListSize] = $FullWordList[%i];
		Game.GameWordListSize++;
	}
	
	shuffleGameWordList();
	
	echo("Done Game List. Words:" SPC Game.GameWordListSize);
	
	setupVowellessWordList();
}

function setupVowellessWordList()
{
	for(%i = 0; %i < Game.GameWordListSize; %i++)
	{
		if(Player.Difficulty == 4)
		{
			$VowellessList[%i] = removeVowels($GameWordList[%i]);
		}
		else
		{
			$VowellessList[%i] = removeVowelsAndPutUnderScore($GameWordList[%i]);
		}
	}
}

function isLetterAVowel(%letter)
{
	%flag = false;
	
	for(%i = 0; %i < 10; %i++)
	{
		if(stricmp(%letter,$Vowels[%i]) == 0)
		{
			%flag = true;
		}
	}
	
	return %flag;
}

function setupVowels()
{
	$Vowels[0] = "A";
	$Vowels[1] = "a";
	$Vowels[2] = "E";
	$Vowels[3] = "e";
	$Vowels[4] = "I";
	$Vowels[5] = "i";
	$Vowels[6] = "O";
	$Vowels[7] = "o";
	$Vowels[8] = "U";
	$Vowels[9] = "u";
}

function getNumberOfVisibleLetters(%word)
{
	return (strlen(%word) - getNumberOfVowels(%word));
}

function getWordValue(%word)
{
	%baseScore = 100;
	%baseComboScore = 100;
	%baseStreakScore = 25;
	%score = (3/getNumberOfVisibleLetters(%word))*%baseScore + Player.Combo*%baseComboScore + Player.Steak*%baseStreakScore;
	
	return %score;
}

function getWordDamage(%word)
{
	%baseDamage = 8;
	%damage = (%baseDamage/getNumberOfVisibleLetters(%word))*getNumberOfVowels(%word);
	%damage = mFloatLength(%damage, 2);
	
	echo(%word SPC %damage);
	
	return %damage;
}