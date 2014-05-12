function MainMenu::create( %this )
{
	Canvas.pushDialog(MenuDialog);
}

//-----------------------------------------------------------------------------

function MainMenu::destroy( %this )
{

}

// Adding command for SinglePlayerButton.
function SinglePlayerButton::onClick(%this)
{
    Canvas.popDialog(MenuDialog);
	Canvas.pushDialog(GameModeDialog);
}

// Adding command for MutliPlayerButton.
function MutliPlayerButton::onClick(%this)
{
	Canvas.popDialog(MenuDialog);
	Canvas.pushDialog(MultiplayerModeDialog);
	Network.startMultiplayer();
}

// Adding command for OptionsButton.
function OptionsButton::onClick(%this)
{
    Canvas.popDialog(MenuDialog);
	Canvas.pushDialog(OptionsDialog);
}

// Adding command for QuitButton.
function QuitButton::onClick(%this)
{
    quit();
}

// Adding command for BackButton.
function BackButton::onClick(%this)
{
    Canvas.popDialog(OptionsDialog);
	Canvas.pushDialog(MenuDialog);
}

// Adding command for GameModeBackButton.
function GameModeBackButton::onClick(%this)
{
	Canvas.popDialog(GameModeDialog);
	Canvas.popDialog(MultiplayerModeDialog);
	Canvas.pushDialog(MenuDialog);
}

// Adding command for SoundButton.
function SoundButton::onClick(%this)
{

}

// Adding command for BattleButton.
function BattleButton::onClick(%this)
{
	Game.Mode = "Battle";
	Canvas.popDialog(GameModeDialog);
	Game.setupGame(false);
}

// Adding command for RaceButton.
function RaceButton::onClick(%this)
{
	Game.Mode = "Race";
	Canvas.popDialog(GameModeDialog);
	Game.setupGame(false);
}

// Adding command for TimeButton.
function TimeButton::onClick(%this)
{
	Game.Mode = "Time";
	Canvas.popDialog(GameModeDialog);
	Game.setupGame(false);
}

// Adding command for PracticeButton.
function PracticeButton::onClick(%this)
{
	Game.Mode = "Practice";
	Canvas.popDialog(GameModeDialog);
	Game.setupGame(false);
}

// Adding command for PlayAgainButton.
function PlayAgainButton::onClick(%this)
{
	if(Game.Multiplayer)
	{
		MSClient.playerWantsRematch(Player.GameID, Player.Name);
	}
	else
	{
		MainScene.clear();
		Game.setupGame(false);
		Canvas.popDialog(LoseDialog);
	}
}

// Adding command for SkipButton.
function SkipButton::onClick(%this)
{
	if(stricmp(Game.Mode,"Practice") == 0)
	{
		Game.skipWord();
	}
}

// Adding command for BackToMenuButton.
function BackToMenuButton::onClick(%this)
{
	if(Game.Multiplayer)
	{
		Network.playerDisconnect();
		Game.Multiplayer = false;
	}
	
	MainScene.clear();
	Canvas.popDialog(LoseDialog);
	Canvas.pushDialog(MenuDialog);
	Game.reset();
}

function MultiplayerBattleButton::onClick(%this)
{
	Canvas.popDialog(MultiplayerModeDialog);
	Canvas.pushDialog(QueueDialog);
	Network.searchForGame("Battle",Player.Name);
}

function MultiplayerRaceButton::onClick(%this)
{

}

function MultiplayerGameModeBackButton::onClick(%this)
{
	Canvas.popDialog(MultiplayerModeDialog);
	Canvas.pushDialog(MenuDialog);
}

function LeaveQueueButton::onClick(%this)
{
	Canvas.popDialog(QueueDialog);
	Network.playerDisconnect();
	Canvas.pushDialog(MenuDialog);
}

