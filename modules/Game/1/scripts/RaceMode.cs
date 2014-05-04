function Game::displayRaceGame()
{
	MainScene.clear();
	Game.displayScore();
	Game.displayNewWord();
	Game.displayTime();
	Game.displayBackPanel("GameAssets:panelbeige");
	Canvas.pushDialog(GameGui);
}
