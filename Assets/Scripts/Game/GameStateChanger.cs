using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
public class GameStateChanger : BaseStateChanger
{
  [SerializeField] private PlayerSpawner _playerSpawnerPrefab; // Префаб объекта появления игроков
  [SerializeField] private Location      _location           ; // Игровая локация
  [SerializeField] private int BonusForChooseCount = 3;    // Константа, задающая количество бонусов для выбора

  public static int ReadyCount;    // Количество игроков, готовых к игре

  private PhotonView    _photonView        ;    // Переменная для работы с сетевым представлением объекта
  private PlayerSpawner _localPlayerSpawner;    // Локальный объект появления игроков
  private int           _locationSpawnCount;    // Количество инициализированных локаций
  //private const int BonusForChooseCount = 3;    // Константа, задающая количество бонусов для выбора
  private BonusDataAccesser _bonusDataAccesser; // Объект типа BonusDataAccesser

  // Объект типа PlayerBonusesAccesser
  private PlayerBonusesAccesser _playerBonusAccesser;
  public void AfterLocationSpawn()
  {
    // Отправляем RPC-сообщения всем игрокам
    // Чтобы обработать завершение создания локации
    _photonView.RPC(nameof(RPCAfterLocationSpawn), RpcTarget.All);
  }
  protected override void OnInit()
  {
    RefreshWaitScreen();
    _photonView = GetComponent<PhotonView>();

    // НОВОЕ: Находим компонент BonusDataAccesser
    _bonusDataAccesser = FindObjectOfType<BonusDataAccesser>();

    // НОВОЕ: Создаём объект PlayerBonusesAccesser
    _playerBonusAccesser = new PlayerBonusesAccesser();

    // НОВОЕ: Инициализируем его
    _playerBonusAccesser.Init();

    // НОВОЕ: Показываем экран выбора бонусов
    BonusChooseScreen bonusChooseScreen = ScreensController.ShowScreen<BonusChooseScreen>();

    // НОВОЕ: Обрабатываем событие OnOkButtonClick
    // Вызываем метод BonusSelected()
    bonusChooseScreen.OnOkButtonClick += BonusSelected;

    // НОВОЕ: Получаем случайные бонусы
    List<BonusData> randomBonuses = _bonusDataAccesser.GetRandomBonuses(_playerBonusAccesser.ExistingBonusTypes, BonusForChooseCount);

    // НОВОЕ: Отображаем случайные бонусы
    bonusChooseScreen.ShowBonuses(randomBonuses);

    // Получаем доступ к экрану обратного отсчёта
    CountDownScreen countDownScreen = ScreensController.GetScreen<CountDownScreen>();

    // Обрабатываем событие OnCountDownEnd
    countDownScreen.OnCountDownEnd += StartGame; // Вызываем метод StartGame()

    // Получаем экран EndRoundScreen
    EndRoundScreen endRoundScreen = ScreensController.GetScreen<EndRoundScreen>();

    // Обрабатываем событие OnGiveUpButtonClick
    // Вызываем метод LeaveGame()
    endRoundScreen.OnGiveUpButtonClick += LeaveGame;

    // Обрабатываем событие OnPlayButtonClick
    // Вызываем метод NextRound()
    endRoundScreen.OnPlayButtonClick += NextRound;

    // Получаем экран EndGameScreen
    EndGameScreen endGameScreen = ScreensController.GetScreen<EndGameScreen>();

    // Обрабатываем событие OnLeaveButtonClick
    // Вызываем метод LeaveGame()
    endGameScreen.OnLeaveButtonClick += LeaveGame;

    // Обрабатываем событие OnDieWithObject
    // Вызываем метод SetLoser()
    CharacterHealth.OnDieWithObject += SetLoser;
  }
  private void BonusSelected(BonusType? selectedType) // Вызывается при выборе бонуса
  { 
    // Если бонус выбран
    if (selectedType.HasValue)
    {
      // Добавляем выбранный бонус
      _playerBonusAccesser.AddBonus(selectedType.Value);
    }
    // Показываем экран ожидания
    ScreensController.ShowScreen<WaitScreen>();

    if(!_photonView)
    {
      Debug.Log("_photonView null!");
    }
    // Вызываем сетевую функцию готовности
    _photonView.RPC(nameof(RPCSendReady), RpcTarget.All);
  }
  [PunRPC] // Специальный атрибут Для синхронизации действий игроков
  private void RPCSendReady()
  {
    // НОВОЕ: Увеличиваем счётчик готовых игроков на 1
    ReadyCount++;

    // НОВОЕ: Вызываем метод RefreshWaitScreen()
    RefreshWaitScreen();

    // НОВОЕ: Заменили приватную переменную на публичную
    if (ReadyCount >= PhotonNetwork.CurrentRoom.MaxPlayers)
    {
      // НОВОЕ: Обнуляем счётчик готовых игроков
      ReadyCount = 0;

      PrepareGame();
    }
  }
  private void RefreshWaitScreen()
  {
    // Вызываем перегруженный одноимённый метод
    // Передаём в него текущее число готовых игроков
    // И максимальное число игроков в текущей комнате
    RefreshWaitScreen(ReadyCount, PhotonNetwork.CurrentRoom.MaxPlayers);
  }
  private void RefreshWaitScreen(int current, int max)
  {
    // Получаем экран ожидания
    WaitScreen waitScreen = ScreensController.GetScreen<WaitScreen>();
    waitScreen.SetCountText(current, max); // Устанавливаем там текст С текущим и максимальным числом игроков
  }
  private void PrepareGame()
  {
    if (PhotonNetwork.IsMasterClient) { // Если текущий клиент — мастер-клиент
      SpawnLocation();                  // Вызываем метод SpawnLocation()
    }
  }
  private void SpawnLocation()
  {
    if (PhotonNetwork.IsMasterClient) { // Если текущий клиент — мастер-клиент
      // Мастер-клиент создаёт объект в игре
      // Который виден всем игрокам в комнате
      PhotonNetwork.InstantiateRoomObject(_location.name, Vector3.zero, Quaternion.identity, 0, 
        new object[] { _photonView.ViewID });
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
    if (!ScreensController) { // Если нет объекта контроллера экранов
      return; // Выходим из метода
    }
    // НОВОЕ: Получаем экран выбора бонусов
    BonusChooseScreen bonusChooseScreen = ScreensController.GetScreen<BonusChooseScreen>();

    // НОВОЕ: Если он существует
    if (bonusChooseScreen)
    {
      // НОВОЕ: Отписываемся от события OnOkButtonClick
      bonusChooseScreen.OnOkButtonClick -= BonusSelected;
    }
    // Получаем экран обратного отсчёта
    CountDownScreen countDownScreen = ScreensController.GetScreen<CountDownScreen>();

    // Если есть экран обратного отсчёта
    if (countDownScreen)
    {
      // Отписываемся от события OnCountDownEnd
      countDownScreen.OnCountDownEnd -= StartGame;
    }
    // Получаем экран EndGameScreen
    EndGameScreen endGameScreen = ScreensController.GetScreen<EndGameScreen>();

    // Если он существует
    if (endGameScreen)
    {
      // Отписываемся от события OnLeaveButtonClick
      endGameScreen.OnLeaveButtonClick -= LeaveGame;
    }
    // Отписываемся от события OnDieWithObject
    CharacterHealth.OnDieWithObject -= SetLoser;
  }
  public override void OnLeftRoom() // Вызывается при выходе из комнаты
  {
    // Удаляем бонусы
    _playerBonusAccesser.ClearBonuses();

    // Загружаем главное меню
    ScenesLoader.LoadMainMenu();
  }
  
  // Вызывается при выходе указанного игрока из комнаты
  public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
  {
    // Если вышедший игрок — это не локальный игрок
    if (otherPlayer != PhotonNetwork.LocalPlayer)
    {
      // Показываем экран EndGameScreen
      ScreensController.ShowScreen<EndGameScreen>();
    }
  }
  private void LeaveGame() // Завершаем игру
  {
    // Выходим из комнаты Photon
    PhotonNetwork.LeaveRoom();

    // Показываем экран LoadingScreen
    ScreensController.ShowScreen<LoadingScreen>(false);
  }
  // Переходим на следующий раунд
  private void NextRound()
  {
    // Загружаем сцену этого раунда
    ScenesLoader.LoadGame();
  }
  // Устанавливаем проигравшего
  private void SetLoser(CharacterHealth health)
  {
    // Отписываемся от события OnDieWithObject
    CharacterHealth.OnDieWithObject -= SetLoser;

    // Отправляем эти данные с помощью RPC
    _photonView.RPC(nameof(RPCSetLoser), RpcTarget.All, health.PhotonView.Controller.ActorNumber);
  }
  // Специальный атрибут
  // Для синхронизации действий игроков
  [PunRPC]

  // Устанавливаем проигравшего по сети
  private void RPCSetLoser(int loseActorNumber)
  {
    // Останавливаем действия локального игрока
    _localPlayerSpawner.StopPlayer();

    // Проверяем, равен ли номер локального игрока
    // Номеру, который получили в методе
    bool lose = PhotonNetwork.LocalPlayer.ActorNumber == loseActorNumber;

    // Показываем экран EndRoundScreen
    EndRoundScreen endRoundScreen = ScreensController.ShowScreen<EndRoundScreen>();

    // Устанавливаем результат игры
    // (Проиграл ли локальный игрок или нет)
    // На экране окончания раунда
    endRoundScreen.SetResult(lose);
  }
}
