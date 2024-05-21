using Photon.Pun;                  // Используем инструменты Photon.Pun
using UnityEngine.SceneManagement; // Работаем со сценами Unity

public class ScenesLoader 
{
  public const string MainMenuSceneName = "MainMenuScene"; // Константа с названием сцены главного меню
  public const string GameSceneName     = "GameScene";     // Константа с названием сцены игры

  public static void LoadMainMenu() // Загружаем сцену главного меню
  {
    SceneManager.LoadScene(MainMenuSceneName); // Используем SceneManager для загрузки
  }
  public static void LoadGame() // Загружаем сцену игры
  {
    PhotonNetwork.LoadLevel(GameSceneName); // Используем PhotonNetwork для загрузки
  }
}
