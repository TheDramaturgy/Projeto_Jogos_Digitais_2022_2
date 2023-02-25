using UnityEngine;

public class ActionQueueReference : MonoBehaviour {

	public void InterruptCurrentAction() => ActionQueue.Instance.InterruptCurrentAction();
	public void ClearAllActions() => ActionQueue.Instance.ClearAllActions();

}
