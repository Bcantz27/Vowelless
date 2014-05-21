//-----------------------------------------------------------------------------
// Copyright (c) 2013 GarageGames, LLC
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.
//-----------------------------------------------------------------------------

function createWindow()
{
    if ( !isObject(MainWindow) )
    {
        // Create the scene window.
        new SceneWindow(MainWindow);

        // Set profile.        
        MainWindow.Profile = GuiDefaultProfile;
        
        // Push the window.
        Canvas.setContent( MainWindow );                     
    }

    // Set camera to a canonical state.
    %allBits = "0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24 25 26 27 28 29 30 31";
    MainWindow.stopCameraMove();
    MainWindow.dismount();
    MainWindow.setViewLimitOff();
    MainWindow.setRenderGroups( %allBits );
    MainWindow.setRenderLayers( %allBits );
    MainWindow.setObjectInputEventGroupFilter( %allBits );
    MainWindow.setObjectInputEventLayerFilter( %allBits );
    MainWindow.setLockMouse( true );
    MainWindow.setCameraPosition( 0, 0 );
    MainWindow.setCameraSize( 100, 75 );
    MainWindow.setCameraZoom( 1 );
    MainWindow.setCameraAngle( 0 );
	
	echo("Created Window.");
}

//-----------------------------------------------------------------------------

function destroyWindow()
{
    // Finish if no window available.
    if ( !isObject(MainWindow) )
        return;
    
    // Delete the window.
    MainWindow.delete();
}

//-----------------------------------------------------------------------------

function createScene()
{
    // Destroy the scene if it already exists.
    if ( isObject(MainScene) )
        destroyScene();
    
    // Create the scene.
    new Scene(MainScene);
            
    // Sanity!
    if ( !isObject(MainWindow) )
    {
        error( "Sandbox: Created scene but no window available." );
        return;
    }
	
	//MainScene.setDebugOn("collision", "position", "aabb");
        
    // Set window to scene.
    setSceneToWindow();    
	echo("Created Scene.");
}

//-----------------------------------------------------------------------------

function destroyScene()
{
    // Finish if no scene available.
    if ( !isObject(MainScene) )
        return;

    // Delete the scene.
    MainScene.delete();
}

//-----------------------------------------------------------------------------

function setSceneToWindow()
{
    // Sanity!
    if ( !isObject(MainScene) )
    {
        error( "Cannot set Scene to Window as the Scene is invalid." );
        return;
    }
    
     // Set scene to window.
    MainWindow.setScene( MainScene );

    // Set camera to a canonical state.
    %allBits = "0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19 20 21 22 23 24 25 26 27 28 29 30 31";
    MainWindow.stopCameraMove();
    MainWindow.dismount();
    MainWindow.setViewLimitOff();
    MainWindow.setRenderGroups( %allBits );
    MainWindow.setRenderLayers( %allBits );
    MainWindow.setObjectInputEventGroupFilter( %allBits );
    MainWindow.setObjectInputEventLayerFilter( %allBits );
    MainWindow.setLockMouse( true );
    MainWindow.setCameraPosition( 0, 0 );
    MainWindow.setCameraSize( 100, 75 );
    MainWindow.setCameraZoom( 1 );
    MainWindow.setCameraAngle( 0 );
}

//-----------------------------------------------------------------------------

function setCustomScene( %scene )
{
    // Sanity!
    if ( !isObject(%scene) )
    {
        error( "Cannot set Sandbox to use an invalid Scene." );
        return;
    }
   
    // Destroy the existing scene.  
    destroyScene();

    // The Sandbox needs the scene to be named this.
    %scene.setName( "MainScene" );    
    
    // Set the scene to the window.
    setSceneToWindow();
}
