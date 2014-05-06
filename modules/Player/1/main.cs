function Player::create( %this )
{	
	exec("./scripts/Controls.cs");

    Player.Health = 100;
	Player.Score = 0;
	Player.Damage = 0;
	Player.Alive = true;
	Player.CurrentWord = 0;
	Player.Difficulty = 1;
	Player.Streak = 0;
	Player.WrongStreak = 0;
	Player.Combo = 0;
	Player.endCombo = true;
	Player.LastCorrectTime = 0;
	
	Player.InputController.initialize();
}

function Player::destroy( %this )
{

}

function Player::incrementScore(%this,%amount)
{
	%this.score = %this.score + %amount;
	Game.displayScore();
}

function Player::checkCombo(%this)
{
	if(%this.endCombo)
	{
		Player.Combo = 0;
		Game.displayCombo();
	}
	else
	{
		%this.endCombo = true;
	}
}

function Player::reset(%this)
{
	Player.Health = 100;
	Player.Score = 0;
	Player.Alive = true;
	Player.CurrentWord = 0;
	Player.Difficulty = 1;
	Player.Streak = 0;
	Player.Combo = 0;
	Player.LastCorrectTime = 0;
	Player.Damage = 0;
}

function Player::changeHealth(%this,%amount)
{
	if(%amount < 0)
	{
		MainWindow.startCameraShake(%amount, 1);
	}

	if((Player.Health + %amount) > 0)
	{
		Player.Health = Player.Health + %amount;
		Player.Health = mFloatLength(Player.Health, 0);
		Game.displayHitDamage(%amount,"-30 10");
	}
	else
	{
		Player.Health = 0;
		%this.onDeath();
	}
}

function Player::displayHealthBar(%this,%health,%position)
{
	if(isObject(%this.LeftBack))
		%this.LeftBack.delete();

	if(isObject(%this.LeftRed))
		%this.LeftRed.delete();
	
	if(isObject(%this.MidBack))
		%this.MidBack.delete();
	
	if(isObject(%this.MidRed))
		%this.MidRed.delete();
	
	if(isObject(%this.RightBack))
		%this.RightBack.delete();
		
	if(isObject(%this.RightRed))
		%this.RightRed.delete();
	
	%this.leftBack = new Sprite();
	%this.leftBack.Size = "1 2";
	%this.leftBack.Position = %position;
	%this.leftBack.SceneLayer = 30;
	%this.leftBack.setBodyType("static");
	%this.leftBack.Image = "GameAssets:barBackLeft";
	
	if(%health > 0)
	{
		%this.leftRed = new Sprite();
		%this.leftRed.Size = "1 2";
		%this.leftRed.Position = %position;
		%this.leftRed.SceneLayer = 29;
		%this.leftRed.setBodyType("static");
		%this.leftRed.Image = "GameAssets:barRedLeft";
		MainScene.add(%this.leftRed);
	}
	
	%this.midBack = new Sprite();
	%this.midBack.Size = "20 2";
	%this.midBack.Position = VectorAdd(%position,"10.5 0");
	%this.midBack.SceneLayer = 30;
	%this.midBack.setBodyType("static");
	%this.midBack.Image = "GameAssets:barBackMid";
	
	%this.midRed = new Sprite();
	%this.midRed.Size = 20*(%health/100) SPC "2";
	%this.midRed.Position = VectorAdd(%position, (((20*(%health/100))/2)+0.5) SPC "0");
	%this.midRed.SceneLayer = 29;
	%this.midRed.setBodyType("static");
	%this.midRed.Image = "GameAssets:barRedMid";
	
	%this.rightBack = new Sprite();
	%this.rightBack.Size = "1 2";
	%this.rightBack.Position = VectorAdd(%position,"21 0");
	%this.rightBack.SceneLayer = 30;
	%this.rightBack.setBodyType("static");
	%this.rightBack.Image = "GameAssets:barBackRight";
	
	if(%health == 100)
	{
		%this.rightRed = new Sprite();
		%this.rightRed.Size = "1 2";
		%this.rightRed.Position = VectorAdd(%position,"21 0");
		%this.rightRed.SceneLayer = 29;
		%this.rightRed.setBodyType("static");
		%this.rightRed.Image = "GameAssets:barRedRight";
		MainScene.add(%this.rightRed);
	}
	
	MainScene.add(%this.midRed);
	MainScene.add(%this.leftBack);
	MainScene.add(%this.midBack);
	MainScene.add(%this.rightBack);
}

function Player::attackAI(%this, %damage)
{
	AI.changeHealth(%damage);
	Game.playHitSound(%damage);
	Game.displayBattleStats();
}

function Player::onDeath(%this)
{
	%this.Alive = false;
	//Game.endGame();
}

//-----------------------------------------------------------------------------

