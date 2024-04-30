using UnityEngine;

public class PlayerMovement : CharacterMovement
{
  // Константа с ключом горизонтального движения
  private const string MovementHorizontalKey = "Horizontal";

  [SerializeField] private float _extraGravityMultiplier = 2f ; // Множитель гравитации
  [SerializeField] private float _movementSpeed          = 20f; // Скорость движения
  [SerializeField] private float _jumpForce              = 45f; // Сила прыжка
  [SerializeField] private float _jumpDuration           = 1f ; // Длительность прыжка
  
  private Rigidbody _rigidbody; // Компонент Rigidbody на физическом объекте
  private Camera    _mainCamera;// Главная камера
  private bool      _canJump;   // Флаг того, что герой может прыгать
  private bool      _isJumping; // Флаг того, что герой в прыжке
  private float     _jumpTimer; // Таймер длительности прыжка

  protected override void OnInit()
  {
    _rigidbody = GetComponentInChildren<Rigidbody>(); // Получаем Rigidbody из дочерних объектов
   _mainCamera = Camera.main;                         // Присваиваем _mainCamera объект камеры
  }
  
  protected override void OnAction()
  {
    Movement(); // Вызываем метод Movement()
    Jumping();  // Вызываем метод Jumping()
  }
  private void FixedUpdate() {
    ExtraGravity(); // Вызываем метод ExtraGravity()
    base.Action();
  }
  private void ExtraGravity() {
    // Создаём переменную gravity типа Vector3
    // Присваиваем ей значение силы гравитации из физического движка Physics
    Vector3 gravity = Physics.gravity;
    gravity *= _extraGravityMultiplier * Time.fixedDeltaTime; // Умножаем гравитацию на множитель и время кадра
    _rigidbody.velocity += gravity;                           // Прибавляем гравитацию к текущей скорости игрока
  }

  private void Movement()
  {
    Vector3 movement = Vector3.zero; // Вводим переменную movement со значением (0, 0, 0)

    movement.x = Input.GetAxis(MovementHorizontalKey);  // Задаём movement.x значение горизонтального ввода с клавиатуры (клавиши A и D)
    movement   =  Vector3.ClampMagnitude(movement, 1f); // Если вектор движения больше 1, нормализуем его (Чтобы избежать быстрого движения по диагонали)
    movement  *= _movementSpeed;                        // Умножаем вектор движения на скорость 

    // Задаём новую скорость по X
    // При этом обнуляем скорость по Z
    // Так как предполагается двухмерное движение
    _rigidbody.velocity = new Vector3(movement.x, _rigidbody.velocity.y, 0f);
  }

  private void OnCollisionEnter(Collision collision) { OnAir(false); }
  private void OnCollisionStay (Collision collision) { OnAir(false); }

  private void Jumping()
  {
    if (Input.GetKeyDown(KeyCode.Space)  // Если нажата клавиша «пробел»
      && _canJump                        // И герой может прыгать
      && !_isJumping) {                  // И прыжок сейчас не выполняется
      OnAir(true);
      _jumpTimer = 0;                   // Обнуляем таймер прыжка

      _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, _jumpForce, 0f); // Задаём вертикальную скорость для прыжка
    }
    
    if (_isJumping) {                    // Если герой в прыжке
      _jumpTimer += Time.fixedDeltaTime; // Увеличиваем таймер прыжка

      if (_jumpTimer >= _jumpDuration) { // Если длительность прыжка превышена
        _isJumping = false;              // Ставим флаг «не прыгаю»
      }
    }
  }

  private void OnAir(bool onAir)
  {
    _canJump  = !onAir; // Ставим флаг «могу прыгать» если не в воздухе
    _isJumping = onAir; // Ставим флаг «не прыгаю» если в воздухе
  }

  private void Update()
  {
    // Если игрок не активен
    if (!IsActive) {
      // Выходим из метода
      return;
    }
    
    LookRotation(); // Вызываем метод LookRotation()
  }
  private void LookRotation()
  {
    // Конвертируем позицию игрока
    // Из мировых координат в экранные
    Vector3 playerOnScreenPosition = _mainCamera.WorldToScreenPoint(_rigidbody.position);
   
    float lookSign   = Mathf.Sign(Input.mousePosition.x - playerOnScreenPosition.x); // Вычисляем направление взгляда игрока Относительно курсора мыши
    float lookYEuler = lookSign * 90;                                                // Задаём угол поворота взгляда игрока

    _rigidbody.rotation = Quaternion.Euler(0f, lookYEuler, 0f); // Поворачиваем игрока на угол lookYEuler вокруг оси Y
  }
}
