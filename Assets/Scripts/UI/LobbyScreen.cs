using UnityEngine.UI; // Используем элементы UI
using UnityEngine;    // Работаем с командами Unity
public class LobbyScreen : BaseScreen
{
  [SerializeField] private PlayerNameInput _playerNameInput;  // Поле ввода имени игрока
  [SerializeField] private RoomListView    _roomListView;     // Окно со списком комнат
  [SerializeField] private Button          _createRoomButton; // Кнопка создания комнаты
  public RoomListView RoomListView => _roomListView;          // Свойство для доступа к списку комнат
  private void Start()
  {
    _playerNameInput.Init(); // Вызываем у поля ввода метод Init()
    _createRoomButton.onClick.AddListener(CreateRoomButtonClick); // Обрабатываем нажатие на кнопку создания комнаты
  }
  private void CreateRoomButtonClick() // Вызывается при нажатии на кнопку создания комнаты
  {
    ScreensController.Current.ShowScreen<CreateRoomScreen>(); // Отображаем экран создания комнаты
  }
}