using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerNameInput : MonoBehaviour
{
  private const string PlayerNamePrefKey = "PlayerName";

  public void Init()
  {
    string         playerName = GetPlayerName();                          // Получаем имя игрока из сохранений
    TMP_InputField inputField = GetComponentInChildren<TMP_InputField>(); // Находим компонент для ввода текста

    if (inputField != null) {       // Если компонент нашёлся
      inputField.text = playerName; // Устанавливаем имя игрока в поле ввода

      inputField.onValueChanged.AddListener(SetPlayerName); // Добавляем действие, которое будет выполняться При изменении текста (вводе имени)
    }
    
    SetPlayerName(playerName); // Вызываем метод SetPlayerName() Передаём в него имя игрока
  }

  private void SetPlayerName(string value)
  {
    if (string.IsNullOrEmpty(value)) { // Если переданное значение пустое
      return;                          // Выходим из метода
    }
    
    PhotonNetwork.NickName = value;                  // Задаём псевдоним игрока в Photon
    PlayerPrefs.SetString(PlayerNamePrefKey, value); // Устанавливаем имя игрока в настройках
    PlayerPrefs.Save();                              // Сохраняем изменения в настройках
  }

  private string GetPlayerName()
  {
    if (PlayerPrefs.HasKey(PlayerNamePrefKey)) { // Проверяем, сохранено ли уже имя игрока
      
      return PlayerPrefs.GetString(PlayerNamePrefKey); // Возвращаем сохранённое имя
    }
    return $"Игрок{Random.Range(0, 10000):00000}"; // Генерируем новое случайное имя и возвращаем его
  }
}
