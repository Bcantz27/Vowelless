function AppCore::create( %this )
{
    // Load system scripts
    exec("./scripts/constants.cs");
    exec("./scripts/defaultPreferences.cs");
    exec("./scripts/canvas.cs");
    exec("./scripts/openal.cs");
	exec("./scripts/scene.cs");
    
    // Initialize the canvas
    initializeCanvas("Voweless");
    
    // Set the canvas color
    Canvas.BackgroundColor = "CornflowerBlue";
    Canvas.UseBackgroundColor = true;

    // Initialize audio
    initializeOpenAL();
    
    ModuleDatabase.loadGroup("gameBase");
}

//-----------------------------------------------------------------------------

function AppCore::destroy( %this )
{

}

