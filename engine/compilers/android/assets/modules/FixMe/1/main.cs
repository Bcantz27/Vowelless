function FixMe::create( %this )
{
    // Load system scripts
    exec("./scripts/constants.cs");
    exec("./scripts/defaultPreferences.cs");
    exec("./scripts/canvas.cs");
    exec("./scripts/openal.cs");
	exec("./scripts/scene.cs");
	exec("./assets/gui/guiProfiles.cs");
    
    // Initialize the canvas
    initializeCanvas("Voweless");
    
    // Set the canvas color
    Canvas.BackgroundColor = "CornflowerBlue";
    Canvas.UseBackgroundColor = true;

    // Initialize audio
    initializeOpenAL();

	AppCore.add(TamlRead("./assets/gui/GameGui.gui.taml"));
	AppCore.add(TamlRead("./assets/gui/MenuDialog.gui.taml"));
	AppCore.add(TamlRead("./assets/gui/OptionsDialog.gui.taml"));

	createWindow();
	createScene();
	
	hideSplashScreen();
	
	Canvas.pushDialog(MenuDialog);
}

//-----------------------------------------------------------------------------

function FixMe::destroy( %this )
{

}

