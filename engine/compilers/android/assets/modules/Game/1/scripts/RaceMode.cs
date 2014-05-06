function Game::displayRaceGame()
{
	MainScene.clear();
	Game.displayScore();
	Game.displayNewWord();
	Game.displayBackPanel("GameAssets:panelbeige");
	Game.displayVowelButtons();
	Answer.Visible = 0;
}

function Game::startRace()
{
	Answer.Visible = 1;
	AI.readWord();
}
function Game::endRace()
{
	if(Player.Score >= Game.RaceTo)
	{
		Canvas.popDialog(GameGui);
		Game.displayWinScreen();
	}
	else if(AI.Score >= Game.RaceTo)
	{
		Canvas.popDialog(GameGui);
		Game.displayLoseScreen();
	}
}
