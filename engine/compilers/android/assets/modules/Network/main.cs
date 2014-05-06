function Network::create( %this )
{
	
	exec("./scripts/MasterServerClient.cs");

	// Load the Network Gui's
	TamlRead("./gui/NetworkMenu.gui.taml");
	TamlRead("./gui/startServer.gui.taml");
	TamlRead("./gui/joinServer.gui.taml");
	TamlRead("./gui/chatGui.gui.taml");
	TamlRead("./gui/waitingForServer.gui.taml");
	TamlRead("./gui/messageBoxOk.gui.taml");
}

function Network::startMultiplayer(%this)
{
	initializeMasterServerClient("127.0.0.1",9100);
}