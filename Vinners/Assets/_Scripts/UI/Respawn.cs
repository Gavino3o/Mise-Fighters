using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
 * Displays and updates information relevant to the main gameplay
 */
public class Respawn : View
{

    [SerializeField] private Button dummyRespawnButton;

    public override void Initialise()
    {
        base.Initialise();

        dummyRespawnButton.onClick.AddListener(() =>
        {
            Player.LocalInstance.RespawnCharacter(); // this enables the input...not sure why i cant do it any other way.
            Player.LocalInstance.ServerRespawnCharacter(); // this updates the gameview
             
        });
    }
}
