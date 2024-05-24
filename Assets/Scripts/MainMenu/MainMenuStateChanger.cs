using UnityEngine;                // Используем команды Unity
using Photon.Pun;                 // Работаем с инструментами Photon.Pun
using Photon.Realtime;            // Используем подключения к серверам Photon
using System.Collections.Generic; // Работаем с коллекциями

public class MainMenuStateChanger : BaseStateChanger
{
  [SerializeField] private string _gameVersion       = "1"; // Версия игры
  [SerializeField] private int    _maxPlayersPerRoom = 2  ; // Максимальное число игроков в комнате
  
  public override void OnConnectedToMaster()
  {
    //    Если мы не в лобби --> заходим в лобби
    if (!PhotonNetwork.InLobby) { JoinLobby(); }
  }
  public override void OnDisconnected(DisconnectCause cause) { Connect(); }   // Вызываем метод Connect()
  public override void OnJoinedLobby() { ScreensController.ShowScreen<LobbyScreen>(); } // Отображаем экран лобби
  public override void OnJoinedRoom()
  {
    RoomScreen roomScreen = ScreensController.ShowScreen<RoomScreen>(); // Отображаем экран комнаты
    roomScreen.SetRoomNameText(PhotonNetwork.CurrentRoom.Name);         // Устанавливаем имя текущей комнаты
    roomScreen.PlayerListView.SetPlayers(PhotonNetwork.PlayerList);     // НОВОЕ: Задаём список игроков

    RefreshPlayButton(); // НОВОЕ: Вызываем метод RefreshPlayButton()
  }
  public override void OnCreateRoomFailed(short returnCode, string message)
  {
    ScreensController.ShowScreen<ErrorScreen>()                                        // Отображаем экран ошибки
                     .SetErrorText($"Код ошибки: {returnCode}; сообщение: {message}"); // Устанавливаем текст ошибки
  }
  public override void OnLeftRoom() 
  { 
    ScreensController.ShowPrevScreen();  // Отображаем предыдущий экран
    
    RoomScreen roomScreen = ScreensController.GetScreen<RoomScreen>(); // НОВОЕ: Получаем окно комнаты
    roomScreen.PlayerListView.ClearContainer();                        // НОВОЕ: Очищаем его контейнер
  } 
  public override void OnRoomListUpdate(List<RoomInfo> roomList)
  {
    LobbyScreen lobbyScreen = ScreensController.GetScreen<LobbyScreen>(); // Запрашиваем экран лобби
    lobbyScreen.RoomListView.SetRoomList(roomList); // Передаём на этот экран список комнат
  }
  // НОВОЕ: Переопределили метод из BaseStateChanger
  protected override void OnInit()
  {
    PhotonNetwork.AutomaticallySyncScene = true;

    Connect();

    LobbyScreen lobbyScreen = ScreensController.GetScreen<LobbyScreen>();
    lobbyScreen.RoomListView.OnJoinRoomButtonClick += JoinRoom;

    CreateRoomScreen createRoomScreen = ScreensController.GetScreen<CreateRoomScreen>();
    createRoomScreen.OnCreateRoomButtonClick += CreateRoom;

    RoomScreen roomScreen = ScreensController.GetScreen<RoomScreen>();
    roomScreen.OnLeaveButtonClick += LeaveRoom;
    roomScreen.OnPlayButtonClick += ScenesLoader.LoadGame;
  }
  private void Connect()
  {
    if (PhotonNetwork.IsConnected) {                        // Если мы подключены к Photon
      JoinLobby();                                          // Вызываем метод JoinLobby()
    } else {                                                // Иначе
      ScreensController.ShowScreen<LoadingScreen>();        // Отображаем экран загрузки
      PhotonNetwork.ConnectUsingSettings();                 // Активируем процесс подключения к Photon Используя заранее заданные настройки
      PhotonNetwork.GameVersion = _gameVersion;             // Задаём версию игры
    }
  }
  private void JoinLobby()
  {
    PhotonNetwork.JoinLobby();                     // Входим в лобби
    ScreensController.ShowScreen<LoadingScreen>(); // Отображаем экран загрузки
  }
  private void CreateRoom(string name)
  {
    PhotonNetwork.CreateRoom(name,                                // Просим Photon создать новую комнату С определённым именем 
                            new RoomOptions()                     // и настройками
                            { MaxPlayers = _maxPlayersPerRoom }); // Где указывается максимальное число игроков
    ScreensController.ShowScreen<LoadingScreen>();                // Отображаем экран загрузки
  }
  private void JoinRoom(string name)
  {
    PhotonNetwork.JoinRoom(name); // Просим Photon подключить нас к комнате С определённым именем 
    ScreensController.ShowScreen<LoadingScreen>(); // Отображаем экран загрузки
  }
  private void LeaveRoom()
  {
    PhotonNetwork.LeaveRoom();                          // Просим Photon отключить нас от комнаты
    ScreensController.ShowScreen<LoadingScreen>(false); // Не отображаем экран загрузки (присваиваем ему false)
  }
  private void OnDestroy()
  {
    if (!ScreensController) { // Если контроллера экранов нет
      return;                 // Выходим из метода
    }
    
    CreateRoomScreen createRoomScreen = ScreensController.GetScreen<CreateRoomScreen>(); // Запрашиваем экран создания комнаты
    if (createRoomScreen) {                                                              // Если есть экран создания комнаты
      createRoomScreen.OnCreateRoomButtonClick -= CreateRoom;                            // Отписываемся от события OnCreateRoomButtonClick
    }
    
    LobbyScreen lobbyScreen = ScreensController.GetScreen<LobbyScreen>(); // Запрашиваем экран лобби
    if (lobbyScreen && lobbyScreen.RoomListView)  {                       // Если есть экран лобби со списком комнат
      lobbyScreen.RoomListView.OnJoinRoomButtonClick -= JoinRoom;         // Отписываемся от события OnJoinRoomButtonClick
    }
    
    RoomScreen roomScreen = ScreensController.GetScreen<RoomScreen>(); // Запрашиваем экран комнаты
    if (roomScreen) {                                                  // Если есть экран комнаты
      roomScreen.OnLeaveButtonClick -= LeaveRoom;                      // Отписываемся от события OnLeaveButtonClick
      roomScreen.OnPlayButtonClick  -= ScenesLoader.LoadGame;          // НОВОЕ: Отписываемся от события OnPlayButtonClick
    }
  }
  
  // Вызывается при входе игрока в комнату
  public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
  {
    RoomScreen roomScreen = ScreensController.GetScreen<RoomScreen>(); // Получаем окно комнаты
    roomScreen.PlayerListView.AddPlayer(newPlayer);                    // Добавляем игрока

    RefreshPlayButton(); // Вызываем метод RefreshPlayButton()
  }
  
  // Вызывается при выходе игрока из комнаты
  public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
  {
    RoomScreen roomScreen = ScreensController.GetScreen<RoomScreen>(); // Получаем окно комнаты
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
    ScreensController.GetScreen<RoomScreen>().SetActivePlayButton(isActive);
  }
}   
