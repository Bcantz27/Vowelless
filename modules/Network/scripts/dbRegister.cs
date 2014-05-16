function sendMessage(%message) {      
	%server = "localhost:80";  
	%path = "/torque/";  
	%script = "login.php";  
	%query = "message="@%message;  
	%hto = new HTTPObject(DataBase);  
	%hto.get(%server,%path @ %script,%query);  
}  

function DataBase::onLine(%this, %line) {  
	//echo(%line);
	if(%line $= "STATS")
	{
		echo(%line);
	}
}  
  
function DataBase::onDisconnect(%this) { 
	echo("On Disconnect."); 
   %this.delete();
}  