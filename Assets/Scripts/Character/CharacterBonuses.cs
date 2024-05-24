using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Collections;

// Сделали класс абстрактным
// Наследуем его от CharacterPart
public abstract class CharacterBonuses : CharacterPart
{
  [SerializeField] private List<BonusType> _existingBonusTypes; // Список активированных бонусов

  private List<BonusApplier> _bonusAppliers = new List<BonusApplier>() {   // Массив объектов применения бонусов
    new ShootCountBonusApplier() // Создаём экземпляр объекта Для подсчёта количества выстрелов
  };
  // Добавляем бонус в список активированных
  public void AddBonus(BonusType type) {
    _existingBonusTypes.Add(type); // Вызываем у списка метод Add()
  }
  // Вызывается, когда свойства игрока обновляются по сети
  public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, 
                                                ExitGames.Client.Photon.Hashtable changedProps)
  {
    if (  PhotonView                        // Если у объекта есть компонент PhotonView
      && !PhotonView.IsMine                 // И он не принадлежит текущему игроку
      && targetPlayer == PhotonView.Owner)  // И его владелец — целевой игрок То есть у него обновились свойства
    {
      _existingBonusTypes.Clear(); // Очищаем список собранных бонусов

      for (int i = 0; i < changedProps.Count; i++)     // Проходим по изменённым свойствам
      {   
        var element = changedProps[$"BonusType{i}"];   // Извлекаем из каждого элемент С индексом, указывающим на тип бонуса
        if (element != null) {                         // Если элемент не пустой
          _existingBonusTypes.Add((BonusType)element); // Добавляем его в список собранных бонусов Приводя его к типу BonusType
        }
      }
      ApplyBonuses(); // Вызываем метод ApplyBonuses()
    }
  }
  protected override void OnInit() 
  {
    if ( PhotonView          // Если у объекта есть компонент PhotonView
      && PhotonView.IsMine)  // И он принадлежит текущему игроку
    {
      ApplyBonuses(); // Вызываем метод ApplyBonuses()

      // Создаём специальную хеш-таблицу
      // Она будет хранить пары «ключ-значение»
      // Для сетевых свойств игрока
      ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();

      // Проходим по списку собранных бонусов
      for (int i = 0; i < _existingBonusTypes.Count; i++) {
        // Добавляем для каждого «ключ-значение»
        // Здесь ключ — строка типа BonusType{i}
        // А значение — текущий тип бонуса из списка
        hashtable.Add($"BonusType{i}", _existingBonusTypes[i]);
      }
      // Устанавливаем хеш-таблицу
      // Как пользовательские свойства локального игрока
      // Обновляя сетевую информацию о собранных бонусах
      PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
    }
  }

  private void ApplyBonuses()
  {
    // Добавляем в список компоненты типа BonusApplier
    // Из дочерних объектов текущего объекта бонусов персонажа
    _bonusAppliers.AddRange(GetComponentsInChildren<BonusApplier>());

    for (int i = 0; i < _bonusAppliers.Count; i++) {                 // Проходим по объектам применения бонусов
      _bonusAppliers[i].ApplyBonus(_existingBonusTypes, gameObject); // Вызываем у каждого метод ApplyBonus()
    }
  }
}