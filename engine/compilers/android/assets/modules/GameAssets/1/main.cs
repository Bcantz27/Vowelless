function GameAssets::create( %this )
{
	exec("./assets/gui/guiProfiles.cs");
	
	createWindow();
	createScene();

	GameAssets.add(TamlRead("./assets/gui/GameGui.gui.taml"));
    GameAssets.add(TamlRead("./assets/gui/MenuDialog.gui.taml"));
	GameAssets.add(TamlRead("./assets/gui/OptionsDialog.gui.taml"));
	echo("Gui's Loaded");
}

//-----------------------------------------------------------------------------

function GameAssets::destroy( %this )
{

}

