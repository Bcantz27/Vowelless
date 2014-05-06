function Game::displayPracticeGame()
{
	MainScene.clear();
	Game.displayScore();
	Game.displayNewWord();
	Game.displayBackPanel("GameAssets:panelbeige");
	Game.displayVowelButtons();
}
