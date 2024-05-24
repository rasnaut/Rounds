using UnityEngine;
using Photon.Pun;

public class GameStateChanger : BaseStateChanger
{
  // Префаб объекта появления игроков
  [SerializeField] private PlayerSpawner _playerSpawnerPrefab;

  // Игровая локация
  [SerializeField] private Location _location;

  // Переменная для работы с сетевым представлением объекта
  private PhotonView _photonView;

  // Локальный объект появления игроков
  private PlayerSpawner _localPlayerSpawner;

  // Количество игроков, готовых к игре
  private int _readyCount;

  // Количество инициализированных локаций
  private int _locationSpawnCount;
  public void AfterLocationSpawn()
  {
    // Отправляем RPC-сообщения всем игрокам
    // Чтобы обработать завершение создания локации
    _photonView.RPC(nameof(RPCAfterLocationSpawn), RpcTarget.All);
  }
  protected override void OnInit()
  {
    // Обнуляем начальное количество готовых игроков
    SetReadyCount(0);

    // Инициализируем счётчик появлений локаций значением 0
    _locationSpawnCount = 0;

    // Показываем экран ожидания игроков
    ScreensController.ShowScreen<WaitScreen>();

    // Получаем компонент PhotonView
    _photonView = GetComponent<PhotonView>();

    // Вызываем удалённый метод готовности RPCSendReady()
    // На всех подключенных устройствах
    _photonView.RPC(nameof(RPCSendReady), RpcTarget.All);

    // Получаем доступ к экрану обратного отсчёта
    CountDownScreen countDownScreen = ScreensController.GetScreen<CountDownScreen>();

    // Обрабатываем событие OnCountDownEnd
    // Вызываем метод StartGame()
    countDownScreen.OnCountDownEnd += StartGame;
  }
  // Специальный атрибут
  // Для синхронизации действий игроков
  [PunRPC]
  private void RPCSendReady()
  {
    // Увеличиваем число готовых игроков на одного
    SetReadyCount(_readyCount + 1);

    // Если это число >= максимальному числу игроков в комнате
    if (_readyCount >= PhotonNetwork.CurrentRoom.MaxPlayers)
    {
      // Вызываем метод PrepareGame()
      PrepareGame();
    }
  }
  private void SetReadyCount(int value)
  {
    // Устанавливаем текущее число готовых игроков
    _readyCount = value;

    // Вызываем метод RefreshWaitScreen()
    // Передаём в него текущее и максимальное число игроков
    RefreshWaitScreen(_readyCount, PhotonNetwork.CurrentRoom.MaxPlayers);
  }
  private void RefreshWaitScreen(int current, int max)
  {
    // Получаем экран ожидания
    WaitScreen waitScreen = ScreensController.GetScreen<WaitScreen>();

    // Устанавливаем там текст
    // С текущим и максимальным числом игроков
    waitScreen.SetCountText(current, max);
  }
  private void PrepareGame()
  {
    // Если текущий клиент — мастер-клиент
    if (PhotonNetwork.IsMasterClient)
    {
      // Вызываем метод SpawnLocation()
      SpawnLocation();
    }
  }
  private void SpawnLocation()
  {
    // Если текущий клиент — мастер-клиент
    if (PhotonNetwork.IsMasterClient)
    {
      // Мастер-клиент создаёт объект в игре
      // Который виден всем игрокам в комнате
      PhotonNetwork.InstantiateRoomObject(_location.name, Vector3.zero, Quaternion.identity, 0, new object[] { _photonView.ViewID });
    }
  }
  // Специальный атрибут
  // Для синхронизации действий игроков
  [PunRPC]
  private void RPCAfterLocationSpawn()
  {
    // Увеличиваем счётчик созданных локаций
    _locationSpawnCount++;

    // Если количество созданных локаций
    // >= максимальному числу всех игроков в комнате
    if (_locationSpawnCount >= PhotonNetwork.CurrentRoom.MaxPlayers)
    {
      // Вызываем метод SpawnPlayerSpawner()
      SpawnPlayerSpawner();

      // Показываем экран с обратным отсчётом
      CountDownScreen countDownScreen = ScreensController.ShowScreen<CountDownScreen>();

      // Начинаем обратный отсчёт на экране
      countDownScreen.StartCountDown();
    }
  }
  private void SpawnPlayerSpawner()
  {
    // Создаём объект появления игроков
    GameObject playerSpawnerGO = PhotonNetwork.Instantiate(_playerSpawnerPrefab.name, Vector3.zero, Quaternion.identity);

    // Получаем у него компонент PhotonView
    PhotonView spawnerPhotonView = playerSpawnerGO.GetComponent<PhotonView>();

    // Инициализируем объект появления игроков
    // Для всех клиентов через сеть
    _photonView.RPC(nameof(RPCInitPlayerSpawner), RpcTarget.All, spawnerPhotonView.ViewID);
  }
  // Специальный атрибут
  // Для синхронизации действий игроков
  [PunRPC]
  private void RPCInitPlayerSpawner(int spawnerPhotonViewId)
  {
    // Получаем объект PhotonView с указанным ID
    PhotonView spawnerPhotonView = PhotonNetwork.GetPhotonView(spawnerPhotonViewId);

    // Получаем компонент PlayerSpawner
    PlayerSpawner playerSpawner = spawnerPhotonView.GetComponent<PlayerSpawner>();

    // Инициализируем его
    playerSpawner.Init();

    // Если этот PhotonView принадлежит текущему игроку
    if (spawnerPhotonView.IsMine)
    {
      // Сохраняем ссылку на его PlayerSpawner
      _localPlayerSpawner = playerSpawner;
    }
  }
  private void StartGame()
  {
    // Отображаем экран игры
    ScreensController.ShowScreen<GameScreen>();

    // Активируем игрока — делаем его управляемым
    _localPlayerSpawner.ActivatePlayer();
  }
  private void OnDestroy()
  {
    // Если нет объекта контроллера экранов
    if (!ScreensController)
    {
      // Выходим из метода
      return;
    }
    // Получаем экран обратного отсчёта
    CountDownScreen countDownScreen = ScreensController.GetScreen<CountDownScreen>();

    // Если есть экран обратного отсчёта
    if (countDownScreen)
    {
      // Отписываемся от события OnCountDownEnd
      countDownScreen.OnCountDownEnd -= StartGame;
    }
  }
}
