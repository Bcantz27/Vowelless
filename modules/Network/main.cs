function Network::create( %this )
{
	
	exec("./scripts/MasterServerClient.cs");
	
}

function Network::startMultiplayer(%this)
{
	initializeMasterServerClient("25.13.242.180",9100);
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
		searchForGame(%mode,%name);
	}
}