using System;                      // Будем работать с событиями
using Random = UnityEngine.Random; // И со случайными значениями
using UnityEngine;

public abstract class Weapon : MonoBehaviour, IShootCountBonusDependent
{
  [SerializeField] private int    _damage            = 10;    // Урон от оружия
  [SerializeField] private Bullet _bulletPrefab      = null;  // Префаб пули
  [SerializeField] private float  _bulletDelay       = 0.05f; // Задержка между выстрелами
  [SerializeField] private int    _bulletsInRow      = 7;     // Количество пуль в магазине
  [SerializeField] private float  _reloadingDuration = 4f;    // Продолжительность перезарядки

  private Transform _bulletSpawnPoint   ; // Точка появления пули
  private int       _currentBulletsInRow; // Текущее количество пуль в магазине
  private float     _bulletTimer        ; // Счётчик времени между выстрелами
  private float     _reloadingTimer     ; // Счётчик времени перезарядки
  private bool      _isShootDelayEnd    ; // Флаг окончания задержки
  private bool      _isReloading        ; // Флаг перезарядки
  private int       _shootCount         ; // Количество выстрелов

  public Action<int, int> OnBulletsInRowChange; // Событие обновления пуль в магазине
  public Action           OnEndReloading;       // Событие окончания перезарядки

  public bool IsReloading => _isReloading; // Свойство с данными о перезарядке  К нему можно будет обращаться из других скриптов

  public void Init()
  {
    _bulletSpawnPoint = GetComponentInChildren<BulletSpawnPoint>().transform; // Получаем компонент Transform для точки вылета пули
    FillBulletsToRow(); // Вызываем метод FillBulletsToRow()
  }

  public void SetActive(bool value)
  {
    gameObject.SetActive(value); // Меняем активность оружия на value
    OnBulletsInRowChange?.Invoke(_currentBulletsInRow, _bulletsInRow); // Вызываем событие и Передаём в него текущее и начальное число пуль
  }
  public void Shoot()
  {
    if ( !_isShootDelayEnd          // Если задержка между выстрелами не закончилась
      || !CheckHasBulletsInRow()) { // Или в магазине нет пуль
      return;                       // Выходим из метода
    } 
    _bulletTimer = 0;       // Обнуляем таймер выстрела
    DoShoot();              // Вызываем метод DoShoot()
    _currentBulletsInRow--; // Уменьшаем количество пуль

    OnBulletsInRowChange?.Invoke(_currentBulletsInRow, _bulletsInRow);
  }

  public void Reload()
  {
    if (_isReloading) { // Если оружие перезаряжается
      return;           // Выходим из метода
    }
    _isReloading = true; // Ставим флаг перезарядки
  }

  // Вычисляем текущее количество пуль
  public bool CheckHasBulletsInRow() { return _currentBulletsInRow > 0; } // Если оно > 0, возвращаем true

  // Вызываем метод SpawnBullet()
  protected void DoShoot() {
    for (int i = 0; i < _shootCount; i++) { // Проходим по количеству выстрелов
      // Передаём в SpawnBullet() новый параметр Угол, под которым нужно выстрелить пулю
      SpawnBullet(_bulletPrefab, _bulletSpawnPoint, GetShootAngle(i, _shootCount));
    }
  }

  // Создаём экземпляр префаба пули
  private void SpawnBullet(Bullet prefab, Transform spawnPoint, float extraAngle)
  {
    Bullet bullet = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation); // В точке появления с теми же параметрами
    
    Vector3 bulletEulerAngles    = bullet.transform.eulerAngles; // Получаем текущие углы поворота созданной пули
    bulletEulerAngles.x         += extraAngle;                   // Прибавляем к углу по X дополнительный угол поворота
    bullet.transform.eulerAngles = bulletEulerAngles;            // Применяем изменённые углы поворота к пуле
    
    InitBullet(bullet); // Вызываем метод InitBullet()
  }

  private void InitBullet(Bullet bullet) {  bullet.SetDamage(_damage); } // Вызываем метод SetDamage() у пули

  private void Update()
  {
    ShootDelaying(); // Вызываем метод ShootDelaying()
    Reloading();     // Вызываем метод Reloading()
  }

  private void ShootDelaying()
  {
    _bulletTimer    += Time.deltaTime;               // Увеличиваем таймер выстрела На время, прошедшее с последнего кадра
    _isShootDelayEnd = _bulletTimer >= _bulletDelay; // Присваиваем _isShootDelayEnd значение В зависимости от того, прошла ли задержка
  }

  private void Reloading()
  {
    if (_isReloading) // Если оружие перезаряжается
    {
      _reloadingTimer += Time.deltaTime;  // Увеличиваем таймер перезарядки На время, прошедшее с последнего кадра
      if (_reloadingTimer >= _reloadingDuration) { // Если прошла продолжительность перезарядки
        FillBulletsToRow();                        // Вызываем метод FillBulletsToRow()
        OnEndReloading?.Invoke();                  // Вызываем событие OnEndReloading
      }
    }
  }

  private void FillBulletsToRow()
  {
    _isReloading         = false;         // Снимаем флаг перезарядки
    _reloadingTimer      = 0;             // Обнуляем таймер перезарядки
    _currentBulletsInRow = _bulletsInRow; // Задаём текущее количество пуль

    // Вызываем событие OnBulletsInRowChange
    // Передаём в него текущее и начальное число пуль
    OnBulletsInRowChange?.Invoke(_currentBulletsInRow, _bulletsInRow);
  }

  public void SetShootCount(int value) { _shootCount = value; } // Приравниваем количество выстрелов к value

  private float GetShootAngle(int shootId, int shootCount)
  {
    float startAngle = 0; // Начальный угол стрельбы
    float stepAngle  = 0; // Шаг изменения угла
    switch (shootCount)   // Выбираем действие в зависимости от числа выстрелов
    {
      case 2:            // Если выстрелов два
        startAngle = -3; // Начальный угол приравниваем к -3 градусам
        stepAngle  =  6; // Шаг угла изменения приравниваем к 6 градусам
      break; 
      case 3:            // Если выстрелов три
        startAngle = -5; // Начальный угол приравниваем к -5 градусам
        stepAngle  =  5; // Шаг угла изменения приравниваем к 5 градусам
      break;  
      case 4:            // Если выстрелов четыре
        startAngle = -6; // Начальный угол приравниваем к -6 градусам
        stepAngle  =  4; // Шаг угла изменения приравниваем к 4 градусам
      break; 
      default:    // Если выстрелов другое количество
        return 0; // Возвращаем угол 0
    }
    // Возвращаем угол для конкретного выстрела
    // Считаем его как начальный угол плюс произведение
    // Шага угла и порядкового номера выстрела
    return startAngle + stepAngle * shootId;
  }
}
