function MainMenu::create( %this )
{
	//Canvas.pushDialog(MenuDialog);
}

//-----------------------------------------------------------------------------

function MainMenu::destroy( %this )
{

}

// Adding command for PlayButton.
function PlayButton::onClick(%this)
{
    Canvas.popDialog(MenuDialog);
	Canvas.pushDialog(GameGui);
	//Game.displayNewWord();
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

// Adding command for SoundButton.
function SoundButton::onClick(%this)
{

}

// Adding command for ControlsButton.
function ControlsButton::onClick(%this)
{

}
