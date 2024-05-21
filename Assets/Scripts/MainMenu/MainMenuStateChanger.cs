using UnityEngine;                // Используем команды Unity
using Photon.Pun;                 // Работаем с инструментами Photon.Pun
using Photon.Realtime;            // Используем подключения к серверам Photon
using System.Collections.Generic; // Работаем с коллекциями

public class MainMenuStateChanger : MonoBehaviourPunCallbacks
{
  [SerializeField] private string _gameVersion       = "1"; // Версия игры
  [SerializeField] private int    _maxPlayersPerRoom = 2  ; // Максимальное число игроков в комнате

  private ScreensController _screenController; // Контроллер экранов
  
  public override void OnConnectedToMaster()
  {
    //    Если мы не в лобби --> заходим в лобби
    if (!PhotonNetwork.InLobby) { JoinLobby(); }
  }
  public override void OnDisconnected(DisconnectCause cause) { Connect(); }   // Вызываем метод Connect()
  public override void OnJoinedLobby() { _screenController.ShowScreen<LobbyScreen>(); } // Отображаем экран лобби
  public override void OnJoinedRoom()
  {
    RoomScreen roomScreen = _screenController.ShowScreen<RoomScreen>(); // Отображаем экран комнаты
    roomScreen.SetRoomNameText(PhotonNetwork.CurrentRoom.Name);         // Устанавливаем имя текущей комнаты
    roomScreen.PlayerListView.SetPlayers(PhotonNetwork.PlayerList);     // НОВОЕ: Задаём список игроков

    RefreshPlayButton(); // НОВОЕ: Вызываем метод RefreshPlayButton()
  }
  public override void OnCreateRoomFailed(short returnCode, string message)
  {
    _screenController.ShowScreen<ErrorScreen>()                                        // Отображаем экран ошибки
                     .SetErrorText($"Код ошибки: {returnCode}; сообщение: {message}"); // Устанавливаем текст ошибки
  }
  public override void OnLeftRoom() 
  { 
    _screenController.ShowPrevScreen();  // Отображаем предыдущий экран
    
    RoomScreen roomScreen = _screenController.GetScreen<RoomScreen>(); // НОВОЕ: Получаем окно комнаты
    roomScreen.PlayerListView.ClearContainer();                        // НОВОЕ: Очищаем его контейнер
  } 
  public override void OnRoomListUpdate(List<RoomInfo> roomList)
  {
    LobbyScreen lobbyScreen = _screenController.GetScreen<LobbyScreen>(); // Запрашиваем экран лобби
    lobbyScreen.RoomListView.SetRoomList(roomList); // Передаём на этот экран список комнат
  }
  private void Start() { Init(); } // Вызываем метод Init()

  private void Init()
  {
    PhotonNetwork.AutomaticallySyncScene = true; // Ставим автоматическую синхронизацию сцены для всех игроков

    _screenController  = FindAnyObjectByType<ScreensController>();  // Ищем контроллер экранов
    _screenController.Init();                                       // Вызываем у него метод Init()

    Connect(); // Вызываем метод Connect()
    
    LobbyScreen lobbyScreen = _screenController.GetScreen<LobbyScreen>(); // Запрашиваем экран лобби
    lobbyScreen.RoomListView.OnJoinRoomButtonClick += JoinRoom; // Обрабатываем событие OnJoinRoomButtonClick Вызываем метод JoinRoom()

    CreateRoomScreen createRoomScreen = _screenController.GetScreen<CreateRoomScreen>(); // Запрашиваем экран создания комнаты
    createRoomScreen.OnCreateRoomButtonClick += CreateRoom; // Обрабатываем событие OnCreateRoomButtonClick Вызываем метод CreateRoom()

    RoomScreen roomScreen = _screenController.GetScreen<RoomScreen>(); // Запрашиваем экран комнаты
    roomScreen.OnLeaveButtonClick += LeaveRoom; // Обрабатываем событие OnLeaveButtonClick Вызываем метод LeaveRoom()
    
    // Обрабатываем событие OnPlayButtonClick
    // Вызываем метод ScenesLoader.LoadGame()
    roomScreen.OnPlayButtonClick += ScenesLoader.LoadGame;
  }
  private void Connect()
  {
    if (PhotonNetwork.IsConnected) {                        // Если мы подключены к Photon
      JoinLobby();                                          // Вызываем метод JoinLobby()
    } else {                                                // Иначе
      _screenController.ShowScreen<LoadingScreen>();        // Отображаем экран загрузки
      PhotonNetwork.ConnectUsingSettings();                 // Активируем процесс подключения к Photon Используя заранее заданные настройки
      PhotonNetwork.GameVersion = _gameVersion;             // Задаём версию игры
    }
  }
  private void JoinLobby()
  {
    PhotonNetwork.JoinLobby();                     // Входим в лобби
    _screenController.ShowScreen<LoadingScreen>(); // Отображаем экран загрузки
  }
  private void CreateRoom(string name)
  {
    PhotonNetwork.CreateRoom(name,                                // Просим Photon создать новую комнату С определённым именем 
                            new RoomOptions()                     // и настройками
                            { MaxPlayers = _maxPlayersPerRoom }); // Где указывается максимальное число игроков
    _screenController.ShowScreen<LoadingScreen>();                // Отображаем экран загрузки
  }
  private void JoinRoom(string name)
  {
    PhotonNetwork.JoinRoom(name); // Просим Photon подключить нас к комнате С определённым именем 
    _screenController.ShowScreen<LoadingScreen>(); // Отображаем экран загрузки
  }
  private void LeaveRoom()
  {
    PhotonNetwork.LeaveRoom();                          // Просим Photon отключить нас от комнаты
    _screenController.ShowScreen<LoadingScreen>(false); // Не отображаем экран загрузки (присваиваем ему false)
  }
  private void OnDestroy()
  {
    if (!_screenController) { // Если контроллера экранов нет
      return;                 // Выходим из метода
    }
    
    CreateRoomScreen createRoomScreen = _screenController.GetScreen<CreateRoomScreen>(); // Запрашиваем экран создания комнаты
    if (createRoomScreen) {                                                              // Если есть экран создания комнаты
      createRoomScreen.OnCreateRoomButtonClick -= CreateRoom;                            // Отписываемся от события OnCreateRoomButtonClick
    }
    
    LobbyScreen lobbyScreen = _screenController.GetScreen<LobbyScreen>(); // Запрашиваем экран лобби
    if (lobbyScreen && lobbyScreen.RoomListView)  {                       // Если есть экран лобби со списком комнат
      lobbyScreen.RoomListView.OnJoinRoomButtonClick -= JoinRoom;         // Отписываемся от события OnJoinRoomButtonClick
    }
    
    RoomScreen roomScreen = _screenController.GetScreen<RoomScreen>(); // Запрашиваем экран комнаты
    if (roomScreen) {                                                  // Если есть экран комнаты
      roomScreen.OnLeaveButtonClick -= LeaveRoom;                      // Отписываемся от события OnLeaveButtonClick
      roomScreen.OnPlayButtonClick  -= ScenesLoader.LoadGame;          // НОВОЕ: Отписываемся от события OnPlayButtonClick
    }
  }
  
  // Вызывается при входе игрока в комнату
  public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
  {
    RoomScreen roomScreen = _screenController.GetScreen<RoomScreen>(); // Получаем окно комнаты
    roomScreen.PlayerListView.AddPlayer(newPlayer);                    // Добавляем игрока

    RefreshPlayButton(); // Вызываем метод RefreshPlayButton()
  }
  
  // Вызывается при выходе игрока из комнаты
  public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
  {
    RoomScreen roomScreen = _screenController.GetScreen<RoomScreen>(); // Получаем окно комнаты
    roomScreen.PlayerListView.RemovePlayer(otherPlayer); // Удаляем игрока

    RefreshPlayButton(); // Вызываем метод RefreshPlayButton()
  }
  
  // Вызывается при сетевой смене игрока (мастер-клиента)
  public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
  {
    RefreshPlayButton(); // Вызываем метод RefreshPlayButton()
  }
  private void RefreshPlayButton() // Обновляем кнопку «Начать игру»
  {
    bool isActive = PhotonNetwork.IsMasterClient  // Кнопка «Начать игру» активна только на мастер-клиенте
                 && PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers; // И когда комната не пустая
    _screenController.GetScreen<RoomScreen>().SetActivePlayButton(isActive);
  }
}   
