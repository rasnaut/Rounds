using UnityEngine;

public class Bullet : MonoBehaviour
{
  [SerializeField] private GameObject _objectHitPrefab = null; // Префаб, который будет появляться при попадании пули
  [SerializeField] private float      _impulse         = 30f ; // Импульс (сила толчка) пули
  [SerializeField] private float      _lifeTime        = 15f ; // Максимальное время отображения пули

  private int _damage;    // Урон от пули
  private BulletHit _hit; // Поведение при попадании

  public void Init(int damage, BulletHit hit) {
    Rigidbody rigidbody = GetComponent<Rigidbody>(); // Получаем Rigidbody пули
    rigidbody.AddForce(transform.forward * _impulse, ForceMode.Impulse); // Добавляем импульс к пуле

    // НОВОЕ: Присваиваем _hit заданное поведение
    _hit = hit;

    // НОВОЕ: Вызываем у поведения метод Init()
    // Передаём в него урон и импульс пули
    hit.Init(damage, _impulse);
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
    if (collision.collider) {         // Если пуля столкнулась с коллайдером
      _hit.Hit(collision, transform); // Вызываем у поведения метод Hit()
      DestroyBullet();                // Вызываем метод DestroyBullet()
    }
  }

  private void DestroyBullet() {
    Destroy(gameObject); // Вызываем метод Destroy() для пули
  }
}
