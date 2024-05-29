using System.Collections.Generic;
using UnityEngine;

public class PlayerListView : MonoBehaviour
{
  [SerializeField] private PlayerListElement _playerListElementPrefab; // Префаб элемента списка игроков
  [SerializeField] private Transform         _playerListContainer;     // Трансформа контейнера списка

  private List<PlayerListElement> _playerListElements = new List<PlayerListElement>(); // Список элементов с игроками
  public void SetPlayers(Photon.Realtime.Player[] newPlayers)
  {
    ClearContainer();                               // Вызываем метод ClearContainer()
    for (int i = 0; i < newPlayers.Length; i++)  {  // Проходим по заданному списку
      AddPlayer(newPlayers[i]);                     // Вызываем метод AddPlayer() Передаём в него очередного игрока
    }
  }
  public void AddPlayer(Photon.Realtime.Player newPlayer)
  {
    Debug.Log("Player List View Add Player");
    // Создаём новый элемент интерфейса для заданного игрока
    PlayerListElement element = Instantiate(_playerListElementPrefab, _playerListContainer);
    element.SetPlayer(newPlayer);     // Ставим данные игрока в элемент интерфейса
    _playerListElements.Add(element); // Добавляем элемент в список
  }
  public void RemovePlayer(Photon.Realtime.Player otherPlayer)
  {
    PlayerListElement element;                               // Задаём переменную для текущего элемента
    for (int i = _playerListElements.Count - 1; i >= 0; i--) // Проходим по списку элементов в обратном порядке
    { 
      element = _playerListElements[i]; // Получаем текущий элемент
      if (element.CheckPlayer(otherPlayer)) // Если он соответствует удаляемому игроку
      {
        _playerListElements.Remove(element); // Удаляем элемент из списка
        Destroy(element.gameObject); // Удаляем объект элемента из игры
      } 
    }
  }
  public void ClearContainer()
  {
    _playerListElements.Clear(); // Очищаем список элементов
  }
}
