using UnityEngine;

public class Bullet : MonoBehaviour
{
  [SerializeField] private GameObject _objectHitPrefab = null; // Префаб, который будет появляться при попадании пули
  [SerializeField] private float      _impulse         = 30f ; // Импульс (сила толчка) пули
  [SerializeField] private float      _lifeTime        = 15f ; // Максимальное время отображения пули

  private int _damage; // Урон от пули

  public void SetDamage(int value) { _damage = value; } // Приравниваем урон к value

  private void Start() {
    Init();  // Вызываем метод Init()
  }

  private void Init() {
    Rigidbody rigidbody = GetComponent<Rigidbody>(); // Получаем Rigidbody пули
    rigidbody.AddForce(transform.forward * _impulse, ForceMode.Impulse); // Добавляем импульс к пуле
  }

  private void Update() {
    ReduceLifeTime(); // Вызываем метод ReduceLifeTime()
  }

  private void ReduceLifeTime()
  {
    _lifeTime -= Time.deltaTime; // Сокращаем время отображения пули На время, прошедшее с последнего кадра

    if (_lifeTime <= 0) { // Если время отображения пули истекло
      DestroyBullet();    // Вызываем метод DestroyBullet()
    }
  }

  private void OnCollisionEnter(Collision collision)
  {
    if (collision.collider) { // Если пуля столкнулась с коллайдером
      Hit(collision); // Вызываем метод Hit() Передаём в него коллизию
    }
  }

  private void Hit(Collision collision)
  {
    bool isCharacterHit = CheckCharacterHit(collision); // Проверяем, столкнулась ли пуля с персонажем

    // Вызываем метод CheckPhysicObjectHit()
    // Передаём в него коллизию
    CheckPhysicObjectHit(collision);

    // Если пуля столкнулась не с персонажем
    if (!isCharacterHit) {
      // Создаём эффект попадания на месте столкновения пули
      GameObject hitSample = Instantiate(_objectHitPrefab, 
                                          collision.contacts[0].point, 
                                          Quaternion.LookRotation(-transform.up, -transform.forward));
    }
    DestroyBullet(); // Вызываем метод DestroyBullet()
  }

  private void DestroyBullet() {
    Destroy(gameObject); // Вызываем метод Destroy() для пули
  }

  private bool CheckCharacterHit(Collision collision)
  {
    CharacterHealth hitedHealth = collision.collider.GetComponentInParent<CharacterHealth>(); // Получаем CharacterHealth у объекта столкновения

    if (hitedHealth) {                       // Если у объекта есть компонент здоровья
      hitedHealth.AddHealthPoints(-_damage); // Наносим урон персонажу

      return true; // Возвращаем true
    }
    return false; // Возвращаем false
  }

  private bool CheckPhysicObjectHit(Collision collision)
  {
    // Получаем IPhysicHittable у объекта столкновения
    IPhysicHittable hittedPhysicObject = collision.collider.GetComponentInParent<IPhysicHittable>();

    if (hittedPhysicObject != null) {// Если пуля столкнулась с физическим объектом
      // Вызываем у объекта метод Hit()
      // Передаём в него вектор силы и точку столкновения
      hittedPhysicObject.Hit(transform.forward * _impulse, collision.contacts[0].point);

      return true; // Возвращаем true
    }
    return false; // Возвращаем false
  }
}
