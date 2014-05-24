function Game::displayPracticeGame()
{
	MainScene.clear();
	
	%wordback = new Sprite();
	%wordback.Size = "65 15";
	%wordback.Position = "0 0";
	%wordback.setSceneLayer(28);
	%wordback.setBodyType("static");
	%wordback.Image = "GameAssets:Background-03-Rectangle";
	MainScene.add(%wordback);
	
	Game.displayScore("-22 46");
	Game.displayWord(true);
	Game.displayCategory("0 40");
	Game.displayVowelButtons("0 -20");
	Game.displayBackPanel("GameAssets:panelbeige");
}
