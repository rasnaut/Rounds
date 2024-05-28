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
  

  // Устанавливаем бонус
  public void SetBonus(BonusType bonusType, string title)
  {
    // Задаём тип
    _bonusType = bonusType;

    // Задаём заголовок
    _titleText.text = title;
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
    // Обрабатываем нажатие на кнопку выбора
    _selectButton.onClick.AddListener(SelectButtonClick);
  }
  // Вызывается при нажатии на кнопку выбора
  private void SelectButtonClick()
  {
    // Вызываем событие OnSelectButtonClick
    // Передаём в него текущий элемент
    OnSelectButtonClick?.Invoke(this);
  }
}