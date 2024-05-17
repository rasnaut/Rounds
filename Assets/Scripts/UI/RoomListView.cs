using UnityEngine; // Используем команды Unity
using System; // Работаем с событиями
using System.Collections.Generic; // Используем коллекции
using Photon.Realtime; // Работаем с подключениями к серверам Photon

public class RoomListView : MonoBehaviour
{
  [SerializeField] private RoomListElement _roomListElementPrefab; // Префаб элемента списка комнат
  [SerializeField] private Transform _roomListContainer; // Трансформа контейнера для размещения элементов

  private List<RoomListElement> _roomListElements = new List<RoomListElement>(); // Список комнат
  public Action<string> OnJoinRoomButtonClick;                                   // Событие входа в комнату
  public void SetRoomList(List<RoomInfo> roomList) // Задаём список комнат
  {
    ClearContainer(); // Вызываем метод ClearContainer()

    for (int i = 0; i < roomList.Count; i++) {  // Проходим по списку комнат
      if (roomList[i].RemovedFromList) {        // Если текущий элемент удалён из списка
        continue;                               // Переходим к новому
      }
      
      RoomListElement element = Instantiate(_roomListElementPrefab, _roomListContainer); // Создаём префаб элемента списка комнат и Добавляем его в контейнер
      element.SetRoomName(roomList[i].Name);            // Устанавливаем название комнаты
      element.OnJoinButtonClick += JoinRoomButtonClick; // Обрабатываем событие OnJoinButtonClick Вызываем метод JoinRoomButtonClick()

      _roomListElements.Add(element); // Добавляем элемент в список комнат
    }
  }
  private void ClearContainer() // Очищаем контейнер с комнатами
  {
    for (int i = _roomListElements.Count - 1; i >= 0; i--) {// Проходим по списку комнат в обратном порядке
      Destroy(_roomListElements[i].gameObject); // Удаляем каждую комнату
    } 
    _roomListElements.Clear(); // Очищаем список комнат
  }
  private void JoinRoomButtonClick(string roomName) // Вызывается при нажатии на кнопку входа в комнату
  {
    OnJoinRoomButtonClick?.Invoke(roomName); // Вызываем событие OnJoinRoomButtonClick Передаём в него комнату с заданным именем
  }
}

