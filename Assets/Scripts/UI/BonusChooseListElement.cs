using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class BonusChooseListElement : MonoBehaviour
{
  [SerializeField] private Image           _backImage      ; // Фоновая картинка
  [SerializeField] private TextMeshProUGUI _titleText      ; // Надпись с заголовком
  [SerializeField] private Button          _selectButton   ; // Кнопка выбора
  [SerializeField] private Color           _selectedColor  ; // Выбранный цвет
  [SerializeField] private Color           _unselectedColor; // Не выбранный цвет

  private BonusType _bonusType; // Тип бонуса
  public  BonusType BonusType => _bonusType;                 // Свойство для доступа к типу бонуса

  public Action<BonusChooseListElement> OnSelectButtonClick; // Событие нажатия на кнопку выбора
  
  public void SetBonus(BonusType bonusType, string title) // Устанавливаем бонус
  {
    _bonusType = bonusType; // Задаём тип
    _titleText.text = title; // Задаём заголовок
  }
  // Устанавливаем выбранный цвет
  public void SetSelected(bool value)
  {
    // Задаём цвет фоновой картинки
    _backImage.color = value ? _selectedColor : _unselectedColor;
  }
  // Вызывается при запуске игры
  private void Start()
  {
    Debug.Log("Button " + _titleText.text + " Created");
    // Обрабатываем нажатие на кнопку выбора
    _selectButton.onClick.AddListener(SelectButtonClick);
    //_selectButton.onClick.AddListener(() => { OnSelectButtonClick?.Invoke(this); });

    int eventCount = _selectButton.onClick.GetPersistentEventCount();
    Debug.Log("Number of listeners added to button: " + eventCount);
  }
  // Вызывается при нажатии на кнопку выбора
  public void SelectButtonClick()
  {
    Debug.Log("SelectButtonClick");
    // Вызываем событие OnSelectButtonClick
    // Передаём в него текущий элемент
    OnSelectButtonClick?.Invoke(this);
  }
}