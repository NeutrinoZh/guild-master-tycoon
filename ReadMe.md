# Guild Master Tycoon

В сумме на проект было потрачено около 20 часов рабочего времени. 

Так как нельзя было использовать внешние библиотеки, я написал собственный набор утилит, который находиться в AssemblyDefination 'MyToolkit'. Он включает в себя такие модули:
- MyTween - минимальная самописная альтернатива DOTween. Автоматически создает инстанс твинера в RuntimeInitializeOnLoadMethod. Пример использования: 
```
    MyTween.To(
        () => _filling.fillAmount,
        value => _filling.fillAmount = value,
        duration,
        1
    );
```
- ServiceLocator - минимальная самописная альтернатива DI-фреймворкам. Но без рефлексии. Вобщем у нас есть два контейнера сервисов: глобальный, который добавляеться в DontDestoryOnLoad и локальный, то-есть в контексте сцены. Оба создаються после запуска GameStateMachine. При получение сервиса, сначала он ищёться в контексте сцены, если там его нет, то дальше пытаемся найти его в контексте приложения. Пример использования:
```
// Регистрация сервисов в контексте сцены 
ServiceContainer.Instance.Register(new BuildingFactory());
ServiceContainer.Instance.Register(new PlayerInput());
ServiceContainer.Instance.Register(new TableManager());

// Регистрация сервиса в контексте приложения 
ServiceContainer.Instance.RegisterGlobal(new SavesManager());

// Получение сервисов 
ServiceContainer.Instance.Get<PlayerStats>(),
ServiceContainer.Instance.Get<SavesManager>()
```
- NavigationGraph - для создания и поиска путей на сцене. Включает в себя такие возможности:
    - Кастомный Editor для редактирования графа в EditMode.
    - Слияние графов (Используеться для создания префабов)
    - Поиск пути по графу (Используеться алгоритм A*)
- SaveSystem - система для сохранений. Работает с интерфейсом сериализатора. Предоставляет готовый JSONSerializer. Также подерживает AES encryption.
- StateMachine - базовая реализация стейт машыны. Используеться в GameStateMachine и в NPC AI. Пример использования:
```
public class AdventurerSM : StateMachine
{
    public AdventurerSM(Transform transform) : base(new()
    {
        [typeof(WalkingState)] = new WalkingState(transform),
        [typeof(ServingState)] = new ServingState(transform),
        [typeof(StayingState)] = new StayingState(transform)
    })
    {
    }
}
```
- Timer - используеться для создания интервалов. Автоматически региструеться как сервис в контексте сцены. Пример:
```
ServiceContainer.Instance.SetCallback<MTKTimer>(() =>
{
    ServiceContainer.Instance.Get<MTKTimer>().StartInterval(SaveInterval, k_saveInterval);
});
```
 
Сама игра состоит из следующих модулей:

- Game.cs - Точка входа в програму. Наивищий приоритет в ScriptOrderDefination. Создает GameStateMachine.
- GameStateMachine - Відповідає за глобальне управління станами гри, такими як основне меню, активна гра, пауза тощо. (По-факту используеться только BootstrapState для регистрации базовых сервисов)
- Adventurer - NPC который создаеться в AdventurersPool (используеться ObjectPool). Использует StateMachine. Основные состояния: 
    - WalkingState
    - ServingState
    - StayingState 