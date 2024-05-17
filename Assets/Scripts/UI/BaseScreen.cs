using UnityEngine;

public class BaseScreen : MonoBehaviour
{
  // Управляем активностью экрана
  public void SetActive(bool value)
  {
    // Показываем или скрываем экран
    // В зависимости от переданного значения
    gameObject.SetActive(value);
  }
}