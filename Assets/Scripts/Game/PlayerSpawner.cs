using UnityEngine;
using Photon.Pun;

public class PlayerSpawner : MonoBehaviour
{
  [SerializeField] private Player          _playerPrefab;
  [SerializeField] private PlayerGamePanel _playerGamePanelPrefab; // НОВОЕ: Префаб шкалы здоровья игрока

  private PhotonView _photonView;
  private Location   _location;
  private Character  _localPlayer;
  private Transform  _playerGamePanelsParent; // НОВОЕ: Трансформа родительского объекта шкалы
  public void Init()
  {
    _photonView             = GetComponent<PhotonView>();
    _location               = FindObjectOfType<Location>();
    _playerGamePanelsParent = ScreensController.Current.GetScreen<GameScreen>().transform; // Получаем трансформу родительского объекта

    if (_photonView.IsMine) {
      SpawnPlayer();
    }
  }
  public void ActivatePlayer() { _localPlayer.Activate(); }
  private void SpawnPlayer()
  {
    PlayerSpawnPoint spawnPoint = _location.SpawnPoints[PhotonNetwork.LocalPlayer.ActorNumber - 1];

    // НОВОЕ: Создаём игрока в заданной точке
    GameObject playerGO = PhotonNetwork.Instantiate(_playerPrefab.name, spawnPoint.transform.position, Quaternion.identity);

    PhotonView playerPhotonView = playerGO.GetComponent<PhotonView>(); // НОВОЕ: Получаем PhotonView этого игрока

    // НОВОЕ: Вызываем сетевую функцию
    _photonView.RPC(nameof(RPCInitPlayer), RpcTarget.All, playerPhotonView.ViewID); // Для инициализации всех копий игрока в сети
  }
  
  // Специальный атрибут
  // Для синхронизации действий игроков
  [PunRPC]
  private void RPCInitPlayer(int playerPhotonViewId) // Инициализируем игрока по сети
  {
    // Получаем PhotonView экземпляра игрока по его id
    PhotonView playerPhotonView = PhotonNetwork.GetPhotonView(playerPhotonViewId);
    Character  player           = playerPhotonView.GetComponent<Player>(); // Получаем компонент Character

    player.Init();                // Инициализируем игрока
    SpawnPlayerGamePanel(player); // Создаём для него шкалу здоровья
    player.Stop();                // Останавливаем игрока, Например, если он должен ждать старта игры

    if (playerPhotonView.IsMine) { // Если PhotonView принадлежит текущему игроку
      _localPlayer = player;       // Сохраняем ссылку на этого игрока
    }
  }
  private void SpawnPlayerGamePanel(Character player) // Создаём шкалу здоровья игрока
  {
    CharacterHealth health = player.GetComponent<CharacterHealth>(); // Получаем компонент CharacterHealth

    // Создаём шкалу здоровья
    PlayerGamePanel gamePanel = Instantiate(_playerGamePanelPrefab, Vector3.zero, Quaternion.identity);
    gamePanel.transform.SetParent(_playerGamePanelsParent); // Устанавливаем её в родительский объект
    gamePanel.SetTarget(health);                            // Задаём цель
  }
}