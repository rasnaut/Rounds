using Photon.Pun;
using UnityEngine;

public class Location : MonoBehaviour
{
  private PlayerSpawnPoint[] _spawnPoints;  // Массив точек появления игроков
  private PhotonView         _photonView = null;   // НОВОЕ: Переменная для работы с сетевым представлением объекта
  public  PlayerSpawnPoint[] SpawnPoints => _spawnPoints; // Свойство для доступа к массиву точек
                                                          // НОВОЕ: Добавили метод Awake()
  private void Awake()
  {
    // Получаем компонент PhotonView
    _photonView = GetComponent<PhotonView>();
    if (!_photonView)
      Debug.LogWarning("PhotonView NOT SET!!!");
    else
      Debug.Log("PhotonView SET Succsess");

    // Получаем компоненты точек появления игроков
    // Из дочерних объектов локации
    _spawnPoints = GetComponentsInChildren<PlayerSpawnPoint>();
    //Debug.Log("InstantiationData length: " + _photonView.InstantiationData.Length.ToString());
    // Извлекаем Id игрока
    // Который изменил состояние игры
    int gameStateChangeId = (int)_photonView.InstantiationData[0];

    // С помощью PhotonView ищем по Id объект
    PhotonView gameStateChangerPhotonView = PhotonNetwork.GetPhotonView(gameStateChangeId);

    // Получаем компонент GameStateChanger
    GameStateChanger gameStateChanger = gameStateChangerPhotonView.GetComponent<GameStateChanger>();

    gameStateChanger.AfterLocationSpawn(); // Вызываем из него метод AfterLocationSpawn()
  }
}