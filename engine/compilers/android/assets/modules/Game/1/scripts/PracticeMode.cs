function Game::displayPracticeGame()
{
	MainScene.clear();
	Game.displayScore();
	Game.displayNewWord();
	Game.displayCategory();
	Game.displayBackPanel("GameAssets:panelbeige");
	Game.displayVowelButtons();
}
