using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ChannelText : MonoBehaviour {
	[Header("Event")]
	[SerializeField] private UnityEvent _onRightChannel;
	[SerializeField] private string _rightChannelText;

	[Header("Channel Text")]
	[SerializeField] private TMP_Text _channelTMP;
	[SerializeField] private StringRuntimeSet _channelTextSet;
	[SerializeField] private int _startingChannelIndex;
	private int _currentChannelIndex;

	private void Start() {
		_currentChannelIndex = _startingChannelIndex;
		_channelTMP.text = _channelTextSet.Items[_currentChannelIndex];
		CheckChannel(_channelTextSet.Items[_currentChannelIndex]);
	}

	public void NextChannel() {
		if (_currentChannelIndex >= (_channelTextSet.Items.Count - 1)) _currentChannelIndex = 0;
		else _currentChannelIndex++;
		_channelTMP.text = _channelTextSet.Items[_currentChannelIndex];
		CheckChannel(_channelTextSet.Items[_currentChannelIndex]);
	}

	public void PreviousChannel() {
		if (_currentChannelIndex <= 0) _currentChannelIndex = _channelTextSet.Items.Count-1;
		else _currentChannelIndex--;
		_channelTMP.text = _channelTextSet.Items[_currentChannelIndex];
		CheckChannel(_channelTextSet.Items[_currentChannelIndex]);
	}

	private void CheckChannel(string value) {
		if (value == _rightChannelText) _onRightChannel.Invoke();
	}
}
