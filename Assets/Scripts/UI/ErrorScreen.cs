using TMPro;          // Используем элементы TMPro
using UnityEngine;    // Работаем с командами Unity
using UnityEngine.UI; // Используем элементы UI 

public class ErrorScreen : BaseScreen
{
  [SerializeField] private TextMeshProUGUI _errorText; // Надпись с текстом ошибки
  [SerializeField] private Button          _okButton;  // Кнопка ОК
  public void SetErrorText(string value) { // Задаём текст ошибки
    _errorText.text = value;               // Делаем его равным value
  }
  private void Start() {
    _okButton.onClick.AddListener(OkButtonClick); // Обрабатываем нажатие на кнопку ОК
  }
  private void OkButtonClick() {                // Вызывается при нажатии на кнопку ОК
    ScreensController.Current.ShowPrevScreen(); // Отображаем предыдущий экран
  }
}