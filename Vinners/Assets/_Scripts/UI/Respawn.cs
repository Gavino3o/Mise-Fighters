using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FishNet;

/*
 * Displays and updates information relevant to the main gameplay
 */
public class Respawn : View
{

    [SerializeField] private Button respawnButton;
    [SerializeField] private Button leaveButton;

    public TextMeshProUGUI remainingLives;

    public override void Initialise()
    {
        base.Initialise();

        respawnButton.onClick.AddListener(() =>
        {
            Player.LocalInstance.RespawnCharacter(); 
        });

        if (InstanceFinder.IsHost)
        {
            leaveButton.onClick.AddListener(() =>
            {
                InstanceFinder.ServerManager.StopConnection(false);
                InstanceFinder.ClientManager.StopConnection();

            });

        }
        else
        {

            leaveButton.onClick.AddListener(() => {
                InstanceFinder.ClientManager.StopConnection();
            });

        }
    }

    private void Update()
    {
        if (!Initialised) return;
        respawnButton.interactable = GameManager.Instance.livesTotal > 0;
        leaveButton.interactable = GameManager.Instance.livesTotal <= 0;
        remainingLives.text = "Lives Left:" + GameManager.Instance.livesTotal.ToString();
        
    }

}
