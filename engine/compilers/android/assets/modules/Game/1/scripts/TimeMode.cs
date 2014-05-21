function Game::displayTimeGame()
{
	MainScene.clear();
	Game.displayScore();
	Game.displayNewWord();
	Game.displayTime();
	Game.displayBackPanel("GameAssets:panelbeige");
	Game.displayVowelButtons();
	Game.displayCategory();
}
