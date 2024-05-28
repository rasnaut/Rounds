﻿using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviour
{
  [SerializeField] private GameObject _objectHitPrefab = null; // Префаб, который будет появляться при попадании пули
  [SerializeField] private float      _impulse         = 30f ; // Импульс (сила толчка) пули
  [SerializeField] private float      _lifeTime        = 15f ; // Максимальное время отображения пули

  private int         _damage;    // Урон от пули
  private BulletHit   _hit;       // Поведение при попадании
  private PhotonView _photonView; // Переменная для работы с сетевым представлением объекта

  public void Init(int damage, BulletHit hit) {
    Rigidbody rigidbody = GetComponent<Rigidbody>(); // Получаем Rigidbody пули
    rigidbody.AddForce(transform.forward * _impulse, ForceMode.Impulse); // Добавляем импульс к пуле

    // НОВОЕ: Присваиваем _hit заданное поведение
    _hit = hit;

    // НОВОЕ: Вызываем у поведения метод Init()
    // Передаём в него урон и импульс пули
    hit.Init(damage, _impulse);
  }

  // Специальный атрибут // Для синхронизации действий игроков
  [PunRPC]
  public void RPCSpawnHitEffect(string propertiesJson) // Создаём эффект появления пули по сети
  {
    // Десериализуем JSON в объект свойств эффекта попадания
    HitEffectProperties properties = JsonUtility.FromJson<HitEffectProperties>(propertiesJson);

    // Создаём эффект попадания с этими свойствами
    _hit.SpawnHitEffect(properties);
  }
  private void Update() {
    if (!_photonView.IsMine) { // НОВОЕ: Если у игрока нет PhotonView
      return; // НОВОЕ: Выходим из метода
    }
    ReduceLifeTime(); // Вызываем метод ReduceLifeTime()
  }
  private void Awake()
  {
    _photonView = GetComponent<PhotonView>(); // Получаем компонент PhotonView
    if (!_photonView.IsMine) {                // Если у игрока его нет
      // Уничтожаем компонент физики на персонаже
      // Чтобы физика пули отрабатывалась на клиенте, который создал пулю
      // Её позиция будет синхронизироваться с другими клиентами
      Destroy(GetComponent<Rigidbody>());
    }
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
    if (!_photonView.IsMine) { // НОВОЕ: Если у игрока нет PhotonView
        return;                // НОВОЕ: Выходим из метода
    }
    if (collision.collider) {         // Если пуля столкнулась с коллайдером
      // НОВОЕ: Получаем свойства эффекта попадания
      HitEffectProperties hitEffectProps = _hit.Hit(collision, transform);

      // НОВОЕ: Если они существуют
      if (hitEffectProps != null)
      {
        // НОВОЕ: Сериализуем свойства в JSON
        string hitEffectPropsJson = JsonUtility.ToJson(hitEffectProps);

        // НОВОЕ: Отправляем эти данные с помощью RPC
        _photonView.RPC(nameof(RPCSpawnHitEffect), RpcTarget.All, hitEffectPropsJson);
      }
      DestroyBullet();                // Вызываем метод DestroyBullet()
    }
  }

  private void DestroyBullet() {
    // НОВОЕ: Удаляем объект пули через PhotonNetwork
    PhotonNetwork.Destroy(gameObject);
  }
}
