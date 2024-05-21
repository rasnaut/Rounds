using UnityEngine;

public class Location : MonoBehaviour
{
  private PlayerSpawnPoint[] _spawnPoints;                // Массив точек появления игроков
  public  PlayerSpawnPoint[] SpawnPoints => _spawnPoints; // Свойство для доступа к массиву точек
  public void Init() // Инициализируем переменные
  { 
    _spawnPoints = GetComponentsInChildren<PlayerSpawnPoint>(); // Получаем все точки из дочерних объектов
    Debug.Log("Location Init");                                 // Выводим сообщение об инициализации в консоль
  }
}