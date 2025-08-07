using UnityEngine;

public class ThreatTrigger : MonoBehaviour


{

    public ToxicType Type;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player"))
            { return; }
        var playerController = other.gameObject.GetComponent<PlayerController>();
        var messageType = playerController.GetCurrentMessage().isToxic;
        if (messageType == Type)
        {
            playerController.IncreaseLife();
            playerController.ResetPosition();
            playerController.ShowRightCategory();
         


        }
        else
        {
            playerController.ResetPosition();
            playerController.ShowWrongCategory();
        }
    }   

   


}
