
//The buttler is expected to be waiting on port 9100 always
function initializeMasterServerClient(%ip, %port) {
   new TCPObject(MSClient);
   MSClient.connect(%ip @ ":" @ %port); //The public ip here
}

function getFirstWord(%word)
{
	%space = strpos(%word," ");
	if(%space == -1)
		%word = getSubStr(%word, 0, strlen(%word));
	else
		%word = getSubStr(%word, 0, %space);
	return %word;
}

//Create a command and send the data
function MSClient::sendCommand(%this, %command) {
   %command = trim(%command);
   %this.send(%command @ "\n");
}

function MSClient::onLine(%this, %line) {
   echo(%line);
   if(%line $= "YESMASTER?") {
      echo("Connection established with MasterServer");
      //Now you can start sending commands
	  
      return;
   }

   %cmd = getFirstWord (%line);
   %line = restWords(%line);
   %result = getFirstWord (%line);
   %params = restWords(%line);
   
   if(%cmd $= "SUCCESS") {
      switch$(%result) {
         case "UNREGISTER":
            echo(%line);   //your game with the id was unregistered
         case "REGISTER":
            echo(%line);   //your game with the id was registered
      }
   }
   else if(%cmd $= "ERROR") {
      switch$(%result) {
         case "DUPLICATE":
            echo(%line);   //you should call some other function to change the id and register again
         case "NOTFOUND":
            echo(%line);   //you should call another function to verify if this game id is the peoper one
		 case "NOGAMEFOUND":
			echo(%line);
      }
   }
   else if(%cmd $= "GAMELIST") {
      //Prepare to receive a bunch of calls for the game list
      echo(%line);   //you should call another function to setup some data structure to receive the data      
   }
   else if(%cmd $= "GAMEINFO") {
      //You receive many of this calls when you request the list
      echo(%line);   //Call a function to put your data into a list or refresh your interface
      
	}
	else if(%cmd $= "ENDGAMELIST") {
		//Game listing ends
		echo(%line);   //The list is finished, go on with your business
	}
	else if(%cmd $= "STARTGAME") {
		Canvas.popDialog(QueueDialog);
		Game.Mode = %result;
		Player.GameID = %params;
		Game.setupGame(true);
	}
	else if(%cmd $= "REMATCH") {
		MainScene.clear();
		Game.setupGame(true);
		Canvas.popDialog(LoseDialog);
	}
	else if(%cmd $= "DAMAGEPLAYER") {
		Game.startBattle(%result);
	}
	else if(%cmd $= "SETDEF") {
		Player.setDefense(%result);
		AI.setDefense(%params);
	}
	else if(%cmd $= "SETHEALTH") {
		Player.setHealth(%result);
		AI.setHealth(%params);
	}
	else if(%cmd $= "SEED") {
		Game.Seed = %result;
	}
	else if(%cmd $= "OPPNAME") {
		AI.Name = %result;
	}
	else if(%cmd $= "FLIP") {
		Game.FlipWords();
	}
	else if(%cmd $= "RESETCOMBO") {
		Player.resetCombo();
	}
	else if(%cmd $= "BLIND") {
		Game.displayBlindPanel("GameAssets:panelbeige");
	}
	else if(%cmd $= "STATS") {
		Player.Elo = %result;
	}
	else if(%cmd $= "OPPELO") {
		AI.Elo = %result;
	}
	else if(%cmd $= "ENDGAME") {
		if(%result $= "WIN")
		{
			Game.displayWinScreen();
		}
		else if(%result $= "LOSE")
		{
			Game.displayLoseScreen();
		}
	}
	else
	{
		echo("CMD: " @ %cmd @ " LINE: " @ %line @ " RESULT: " @ %result @ " PARAMS: " @ %params);
	}
}

function MSClient::registerUser(%this, %name) {
   %this.send("registerUser" SPC %name @ "\n");
}

function MSClient::blindOpp(%this, %uid, %name) {
   %this.send("blindOpp" SPC %uid SPC %name @ "\n");
}

function MSClient::resetOpp(%this, %uid, %name) {
   %this.send("resetOpp" SPC %uid SPC %name @ "\n");
}

function MSClient::flipOppWord(%this, %uid, %name) {
   %this.send("flipOppWord" SPC %uid SPC %name @ "\n");
}

function MSClient::playerWantsRematch(%this, %uid, %name) {
   %this.send("playerWantsRematch" SPC %uid SPC %name @ "\n");
}

function MSClient::playerDisconnect(%this, %name) {
   %this.send("playerDisconnect" SPC %name @ "\n");
}

function MSClient::setWinner(%this, %uid, %name) {
   %this.send("setWinner" SPC %uid SPC %name @ "\n");
}

function MSClient::setPlayerHealth(%this, %uid, %name,%health) {
   %this.send("setPlayerHealth" SPC %uid SPC %name SPC %health @ "\n");
}

function MSClient::setPlayerDefense(%this, %uid, %name,%defense) {
   %this.send("setPlayerDefense" SPC %uid SPC %name SPC %defense @ "\n");
}

function MSClient::setPlayerDamage(%this, %uid, %name, %damage) {
   %this.send("setPlayerDamage" SPC %uid SPC %name SPC %damage @ "\n");
}

function MSClient::searchForGame(%this, %mode, %name) {
	echo("Search for game");
   %this.send("searchForGame" SPC %mode SPC %name @ "\n");
}

function MSClient::registerGame(%this, %uid, %ip, %port, %name) {
   %this.send("register" SPC %uid SPC %ip SPC %port SPC %name @ "\n");
}

function MSClient::unregisterGame(%this, %uid) {
   %this.send("unregister" SPC %uid @ "\n");
}

function MSClient::listGames(%this) {
   %this.send("listgames" SPC %uid @ "\n");
}

