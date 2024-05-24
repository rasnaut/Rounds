using Photon.Pun;
public abstract class BaseStateChanger : MonoBehaviourPunCallbacks
{
  // Защищённое свойство ScreensController
  // Доступно вне класса только для чтения
  // При этом его можно менять внутри класса и его наследников
  protected ScreensController ScreensController { get; private set; }
  protected abstract void OnInit(); // Защищённый абстрактный метод OnInit()
  private void Start() {
    Init(); // Вызываем метод Init()
  }
  private void Init() // Инициализируем переменные
  {
    ScreensController = FindAnyObjectByType<ScreensController>(); // Находим объект типа ScreensController
    ScreensController.Init(); // Инициализируем его
    OnInit();                 // Вызываем метод OnInit()
  }
}