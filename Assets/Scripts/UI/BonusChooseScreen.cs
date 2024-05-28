using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class BonusChooseScreen : BaseScreen
{
  // Префаб элемента списка бонусов
  [SerializeField] private BonusChooseListElement _bonusListElementPrefab;
  [SerializeField] private Button     _okButton;           // Кнопка ОК
  [SerializeField] private Transform  _bonusListContainer; // Трансформа контейнера списка бонусов
  [SerializeField] private GameObject _allBonusTakenGO;    // Объект, который показывает, что все бонусы собраны

  // Список элементов бонусов для выбора
  private List<BonusChooseListElement> _bonusChooseListElements = new List<BonusChooseListElement>();

  private BonusType? _selectedBonusType;      // Текущий выбранный тип бонуса
  public  Action<BonusType?> OnOkButtonClick; // Событие нажатия на кнопку ОК
  public void ShowBonuses(List<BonusData> randomBonuses)
  {
    _selectedBonusType = null;   // Сбрасываем выбранный бонус

    // Проверяем, есть ли бонусы Задаём значение флага hasBonuses
    bool hasBonuses = randomBonuses.Count == 0;
    SetActiveAllBonusTaken(hasBonuses); // Вызываем метод SetActiveAllBonusTaken()
    ClearContainer();                   // Вызываем метод ClearContainer()

    for (int i = 0; i < randomBonuses.Count; i++) { // Проходим по случайным бонусам
      AddBonusElement(randomBonuses[i]);            // Добавляем каждый в контейнер
    }
    SelectFirstBonusElement();  // Вызываем метод SelectFirstBonusElement()
  }
  private void SetActiveAllBonusTaken(bool value)
  {
    // Устанавливаем активность объекта Который сообщает о том, что все бонусы собраны
    _allBonusTakenGO.SetActive(value);
  }
  private void ClearContainer()
  {
    // Проходим по бонусам в обратном порядке
    for (int i = _bonusChooseListElements.Count - 1; i >= 0; i--) 
    {
      // И последовательно отписываемся от события OnSelectButtonClick
      _bonusChooseListElements[i].OnSelectButtonClick -= SelectElement;

      Destroy(_bonusChooseListElements[i].gameObject); // Удаляем каждый элемент
    }
    _bonusChooseListElements.Clear(); // Очищаем список бонусов
  }
  private void AddBonusElement(BonusData bonusData)
  {
    // Создаём экземпляр префаба
    // Элемента списка бонусов
    BonusChooseListElement element = Instantiate(_bonusListElementPrefab, _bonusListContainer);
    
    element.SetBonus(bonusData.Type, bonusData.Title); // Устанавливаем тип и название бонуса
    element.OnSelectButtonClick += SelectElement;      // Обрабатываем событие OnSelectButtonClick

    _bonusChooseListElements.Add(element); // Добавляем элемент в список выбора
  }
  private void SelectElement(BonusChooseListElement selectedElement)
  {
    // Запоминаем выбранный тип бонуса
    _selectedBonusType = selectedElement.BonusType;

    // Проходим по бонусам в обратном порядке
    for (int i = _bonusChooseListElements.Count - 1; i >= 0; i--)
    {
      // Создаём элемент, равный текущему
      BonusChooseListElement element = _bonusChooseListElements[i];

      // Указываем, выбран ли этот элемент
      element.SetSelected(element == selectedElement);
    }
  }
  private void SelectFirstBonusElement()
  {
    if (_bonusChooseListElements.Count <= 0) { // Если элементов нет
      return; // Выходим из метода
    }
    SelectElement(_bonusChooseListElements[0]); // Выбираем первый элемент
  }
  private void Start()
  {
    // Обрабатываем нажатие на кнопку ОК
    _okButton.onClick.AddListener(OkButtonClick);
  }
  private void OkButtonClick()
  {
    // Вызываем событие OnOkButtonClick
    // Передаём в него выбранный тип бонуса
    OnOkButtonClick?.Invoke(_selectedBonusType);
  }
}
