function Powerup::create( %this )
{	
	Powerup.TotalPowerUps = 7;
	Powerup.TriggerThreshold = 3; //Number of missing vowels to trigger a powerup
	
	echo("Powerups Loaded");
}

function Powerup::destroy( %this )
{

}

function Powerup::getPowerUpName(%this,%id)
{
	if(%id == 0)
	{
		return "Skip Word";
	}
	else if(%id == 1)
	{
		return "Fill Defense";
	}
	else if(%id == 2)
	{
		return "Reset";
	}
	else if(%id == 3)
	{
		return "Blind";
	}
	else if(%id == 4)
	{
		return "Heal";
	}
	else if(%id == 5)
	{
		return "Flip";
	}
	else if(%id == 6)
	{
		return "Freeze";
	}
	else
	{
		return "INVALID POWER UP ID";
	}
}

function Powerup::getPowerUpIcon(%this,%id)
{
	if(%id == 0)
	{
		return "GameAssets:SkipButton";
	}
	else if(%id == 1)
	{
		return "GameAssets:FillDefenseButton";
	}
	else if(%id == 2)
	{
		return "GameAssets:ResetButton";
	}
	else if(%id == 3)
	{
		return "GameAssets:BlindButton";
	}
	else if(%id == 4)
	{
		return "GameAssets:HealButton";
	}
	else if(%id == 5)
	{
		return "GameAssets:FlipButton";
	}	
	else if(%id == 6)
	{
		return "GameAssets:FreezeButton";
	}
	else
	{
		return "INVALID POWER UP ID";
	}
}

