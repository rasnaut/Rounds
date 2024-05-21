using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSpawner : MonoBehaviour
{
  [SerializeField] private Player     _playerPrefab; // Префаб игрока
                   private PhotonView _photonView;   // Переменная для работы с сетевым представлением объекта
                   private Location   _location;     // Игровая локация

  private void Awake() // Выполняется первым в MonoBehaviour
  {
    _photonView = GetComponent<PhotonView>();   // Получаем доступ к сетевому представлению объекта
    _location   = FindObjectOfType<Location>(); // Находим в сцене объект типа Location
  }
  private void Start()
  {
    // Если это наш объект То есть он принадлежит локальному игроку
    if (_photonView.IsMine)
    {
      _location.Init(); // Инициализируем локацию
      SpawnPlayer();    // Вызываем метод SpawnPlayer()
    }
  }
  private void SpawnPlayer() // Запускаем игрока на сцену
  {
    // Выбираем точку для появления игрока
    // На основе его уникального номера в игре
    PlayerSpawnPoint spawnPoint = _location.SpawnPoints[PhotonNetwork.LocalPlayer.ActorNumber - 1];

    // Создаём сетевой объект игрока в заданной точке
    PhotonNetwork.Instantiate(_playerPrefab.name, spawnPoint.transform.position, Quaternion.identity);
  }
}