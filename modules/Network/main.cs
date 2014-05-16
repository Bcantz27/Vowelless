function Network::create( %this )
{
	exec("./scripts/MasterServerClient.cs");

}

function Network::playerDisconnect(%this)
{
	MSClient.playerDisconnect(Player.Name);
	MSClient.disconnect();
}

function Network::startMultiplayer(%this)
{
	initializeMasterServerClient("25.13.242.180",9100);
	MSClient.registerUser(Player.Name);
}

function Network::searchForGame(%this,%mode,%name)
{
	if(isObject(MSClient))
	{
		MSClient.searchForGame(%mode,%name);
	}
	else
	{
		echo("NO CLIENT");
		initializeMasterServerClient("25.13.242.180",9100);
		MSClient.searchForGame(%mode,%name);
	}
}

function Network::destroy(%this)
{
	if(Game.Multiplayer)
		%this.playerDisconnect();
}