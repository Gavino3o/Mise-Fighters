using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
 * Displays and updates information relevant to the main gameplay
 */
public class Respawn : View
{

    [SerializeField] private Button respawnButton;

    public TextMeshProUGUI remainingLives;

    public override void Initialise()
    {
        base.Initialise();

        respawnButton.onClick.AddListener(() =>
        {
            Player.LocalInstance.RespawnCharacter(); 
        });
    }

    private void Update()
    {
        if (!Initialised) return;
        respawnButton.interactable = (GameManager.Instance.livesTotal > 0);
        remainingLives.text = "Lives Left:" + GameManager.Instance.livesTotal.ToString();
        
    }

}
