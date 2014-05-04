function Game::displayTimeGame()
{
	MainScene.clear();
	Game.displayScore();
	Game.displayNewWord();
	Game.displayTime();
	Game.displayBackPanel("GameAssets:panelbeige");
	Canvas.pushDialog(GameGui);
}
