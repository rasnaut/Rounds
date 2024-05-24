using TMPro;
using UnityEngine;

public class PlayerGamePanel : MonoBehaviour
{
  [SerializeField] private TextMeshProUGUI _playerNameText                   ; // Надпись с именем игрока
  [SerializeField] private Transform       _playerHealthPercentsViewTransform; // Трансформа отображения здоровья игрока
  [SerializeField] private Vector3         _deltaPosition                    ; // Вектор смещения

  private CharacterHealth _target    ; // Здоровье целевого игрока
  private Camera          _mainCamera; // Главная камера
  public void SetTarget(CharacterHealth target)
  {
    if (target == null) { // Если нет целевого игрока
      return;             // Выходим из метода
    }
    _target = target; // Присваиваем _target заданный объект

    // Обрабатываем событие OnAddHealthPoints
    // Вызываем метод RefreshPlayerHealthView()
    target.OnAddHealthPoints += RefreshPlayerHealthView;

    if (_playerNameText != null) {                             // Если есть надпись с именем игрока
      _playerNameText.text = target.photonView.Owner.NickName; // Делаем имя игрока равным Owner.NickName
    }
  }
  private void RefreshPlayerHealthView()
  {
    if ( _playerHealthPercentsViewTransform   // Если есть трансформа отображения здоровья игрока
      && _target)                             // И есть объект целевого игрока
    {
      // Масштабируем индикатор здоровья
      // В зависимости от текущего здоровья
      _playerHealthPercentsViewTransform.localScale = new Vector3((float)_target.GetHealthPoints() / 
                                                                         _target.GetStartHealthPoints(), 1f, 1f);
    }
  }
  private void Start() {
    _mainCamera = Camera.main; // Присваиваем _mainCamera объект камеры
  }
  private void Update()
  {
    CheckForDestroy(); // Вызываем метод CheckForDestroy()
    RefreshPosition(); // Вызываем метод RefreshPosition()
  }
  private void CheckForDestroy()
  {
    if (!_target) {        // Если нет целевого игрока
      Destroy(gameObject); // Удаляем объект шкалы здоровья
    }
  }
  private void RefreshPosition()
  {
    Vector3 targetPosition = _target.transform.position;                     // Получаем текущую позицию игрока
    targetPosition         = _mainCamera.WorldToScreenPoint(targetPosition); // Преобразуем её из мировых координат в экранные
    transform.position     = targetPosition + _deltaPosition;                // Прибавляем к позиции вектор смещения
  }
  private void OnDestroy()
  {
    if (!_target) {                                          // Если нет целевого игрока
      _target.OnAddHealthPoints -= RefreshPlayerHealthView;  // Отписываемся от события OnAddHealthPoints
    }
  }
}
