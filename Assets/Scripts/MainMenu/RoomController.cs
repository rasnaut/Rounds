using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class RoomController : MonoBehaviourPunCallbacks // Наследуем RoomController от MonoBehaviourPunCallbacks
{
  [SerializeField] private PlayerSpawner _playerSpawner; // Объект появления игроков
  public static RoomController Instance { get; private set; } // Статический экземпляр класса для доступа извне

  public override void OnEnable() // Вызывается, когда MonoBehaviour включен
  {
    Debug.Log("RoomController OnEnable()");
    base.OnEnable(); //  Вызываем базовую реализацию метода

    // Обрабатываем событие sceneLoaded
    // Вызываем метод SpawnPlayerSpawner()
    SceneManager.sceneLoaded += SpawnPlayerSpawner;
  }
  public override void OnDisable() // Вызывается, когда MonoBehaviour отключен
  {
    Debug.Log("RoomController OnDisable()");
    base.OnDisable();                               // Вызываем базовую реализацию метода
    SceneManager.sceneLoaded -= SpawnPlayerSpawner; // Отписываемся от события sceneLoaded
  }
  private void Awake()     // Выполняется первым в MonoBehaviour
  {
    Debug.Log("RoomController Awake");
    if (Instance) {        // Если экземпляр класса существует
      Destroy(gameObject); // Уничтожаем этот объект
    }
    DontDestroyOnLoad(gameObject); // Не уничтожаем объект при загрузке новой сцены
    
    Instance = this;               // Сохраняем этот экземпляр как доступный статически
  }

  // Запускаем появление игроков после загрузки сцены
  private void SpawnPlayerSpawner(Scene scene, LoadSceneMode loadSceneMode)
  {
    Debug.Log("RoomController SpawnPlayerSpawner()");
    if (scene.name == ScenesLoader.GameSceneName) // Если мы в игровой сцене
    {
      Debug.Log("RoomController SPAAAAAAAAAAWN");
      // Создаём объект появления игроков через Photon
      PhotonNetwork.Instantiate(_playerSpawner.name, Vector3.zero, Quaternion.identity);
    }
  }
}