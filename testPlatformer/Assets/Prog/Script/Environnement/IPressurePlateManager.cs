using Prog.Script.Environnement;

public interface IPressurePlateManager
{
    void addPressurePlatesOnLoad();
    void PressurePlatesStateVerification();
    void PressurePlateIsPressed(string pressurePlateName);
    void PresurePlateIsReleased(string pressurePlateName);
    bool CheckPressurePlateState(string pressurePlateName);
    int GetNbOfPressurePlates();
    bool GetActivationStateOfManager();
    void AddPressurePlate(PressurePlate pressurePlate);
    void SetDoor(Door door);
}