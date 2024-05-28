using Photon.Pun;
using System.Collections.Generic;

public class PlayerBonusesAccesser
{
  private List<BonusType> _existingBonusTypes = new List<BonusType>(); // Список собранных бонусов игрока
  public  List<BonusType> ExistingBonusTypes  => _existingBonusTypes;  // Свойство для доступа к списку собранных бонусов

  public void Init() {
    RefreshExistingBonuses(); // Вызываем метод RefreshExistingBonuses()
  }
  public void AddBonus(BonusType bonusType) // Добавляем новый бонус
  {
    _existingBonusTypes.Add(bonusType); // Помещаем его в список собранных

    // Создаём хеш-таблицу для сохранения свойств
    ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
    for (int i = 0; i < _existingBonusTypes.Count; i++)       // Проходим по списку собранных бонусов
    {     
      hashtable.Add($"BonusType{i}", _existingBonusTypes[i]); // Добавляем каждый в хеш-таблицу
    }
    // Сохраняем хеш-таблицу
    // Как пользовательские свойства игрока в Photon
    PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
  }
  private void RefreshExistingBonuses() // Обновляем список собранных бонусов
  {
    _existingBonusTypes.Clear();   // Очищаем его

    // Получаем пользовательские свойства игрока из Photon
    ExitGames.Client.Photon.Hashtable playerCustomProperties = PhotonNetwork.LocalPlayer.CustomProperties;

    for (int i = 0; i < playerCustomProperties.Count; i++)  { // Проходим по всем элементам в свойствах игрока
      
      var element = playerCustomProperties[$"BonusType{i}"];  // Получаем бонус под текущим индексом
      if (element != null) {                         // Если его значение не пустое
        _existingBonusTypes.Add((BonusType)element); // Добавляем этот бонус в список собранных
      }
    }
  }
}