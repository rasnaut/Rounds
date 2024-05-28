using System.Collections.Generic;
using Photon.Pun;
using System.Collections;

// Сделали класс абстрактным
// Наследуем его от CharacterPart
public abstract class CharacterBonuses : CharacterPart
{
  private List<BonusType> _existingBonusTypes = new List<BonusType>();

  private List<BonusApplier> _bonusAppliers = new List<BonusApplier>() {   // Массив объектов применения бонусов
    new ShootCountBonusApplier() // Создаём экземпляр объекта Для подсчёта количества выстрелов
  };

  protected override void OnInit()
  {
    _existingBonusTypes.Clear(); // Очищаем список собранных бонусов

    // Получаем кастомные свойства контроллера в сетевой игре
    ExitGames.Client.Photon.Hashtable props = PhotonView.Controller.CustomProperties;

    for (int i = 0; i < props.Count; i++) { // Проходим по всем свойствам
      // Пытаемся получить значение свойства
      // Под ключом BonusType с индексом i
      var element = props[$"BonusType{i}"];
      if (element != null) {                         // Если значение найдено (не равно null)
        _existingBonusTypes.Add((BonusType)element); // Добавляем тип бонуса в список
      }
    }
    ApplyBonuses(); // Вызываем метод ApplyBonuses()
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