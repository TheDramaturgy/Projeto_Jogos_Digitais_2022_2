using TMPro;
using UnityEngine;

public class ChannelText : MonoBehaviour {
	[SerializeField] private TMP_Text _channelText;
	[SerializeField] private StringRuntimeSet _channelTextSet;
	[SerializeField] private int _startingChannelIndex;
	private int _currentChannelIndex;

	private void Start() {
		_currentChannelIndex = _startingChannelIndex;
		_channelText.text = _channelTextSet.Items[_currentChannelIndex];
	}

	public void NextChannel() {
		if (_currentChannelIndex >= _channelTextSet.Items.Count) _currentChannelIndex = 0;
		else _currentChannelIndex++;
		_channelText.text = _channelTextSet.Items[_currentChannelIndex];
	}

	public void PreviousChannel() {
		if (_currentChannelIndex <= 0) _currentChannelIndex = _channelTextSet.Items.Count-1;
		else _currentChannelIndex--;
		_channelText.text = _channelTextSet.Items[_currentChannelIndex];
	}
}
