using Prog.Script.Environnement;

public interface IActivatorManager
{
    void addActivatorsOnLoad();
    void ActivatorsStateVerification();
    void EnableActivator(string pressurePlateName);
    void DisableActivator(string pressurePlateName);
    bool CheckActivatorState(string pressurePlateName);
    int GetNbOfActivator();
    bool GetActivationStateOfManager();
    void AddActivator(Activator activator);
    void SetDoor(Door door);
}