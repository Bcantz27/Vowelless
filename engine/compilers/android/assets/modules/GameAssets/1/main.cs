function GameAssets::create( %this )
{
	exec("./assets/gui/guiProfiles.cs");
	
	createWindow();
	createScene();

	GameAssets.add(TamlRead("./assets/gui/QueueDialog.gui.taml"));
	GameAssets.add(TamlRead("./assets/gui/MultiplayerModeDialog.gui.taml"));
    GameAssets.add(TamlRead("./assets/gui/MenuDialog.gui.taml"));
	GameAssets.add(TamlRead("./assets/gui/OptionsDialog.gui.taml"));
	GameAssets.add(TamlRead("./assets/gui/GameModeDialog.gui.taml"));
	GameAssets.add(TamlRead("./assets/gui/LoseDialog.gui.taml"));
	echo("Gui's Loaded");
}

//-----------------------------------------------------------------------------

function GameAssets::destroy( %this )
{

}

