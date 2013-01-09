using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace BasiK.BasiKLauncher
{
    
    /// <summary>
    /// XInput controller object.
    /// Updates controller state using the XInput dll-imported calls.
    /// </summary>
    internal class XInputController
    {
        private int _playerIndex;
        private bool _keepRunning;
        private int _waitTime = 5;
        private Thread _pollingThread;

        XInputState gamepadStatePrev = new XInputState();
        XInputState gamepadStateCurrent = new XInputState();

        public XInputController(int index)
        {
            _playerIndex = index;
            gamepadStatePrev.Copy(gamepadStateCurrent);
        }

        public event EventHandler<XboxControllerStateChangedEventArgs> StateChanged = null;
        protected void OnStateChanged()
        {
            if (StateChanged != null)
                StateChanged(this, new XboxControllerStateChangedEventArgs() { CurrentInputState = gamepadStateCurrent, PreviousInputState = gamepadStatePrev });
        }


        #region State Polling
        public void Start()
        {
            _keepRunning = true;

            if (_pollingThread == null || !_pollingThread.IsAlive)
            {
                _pollingThread = new Thread(PollerLoop);
                _pollingThread.Start();
            }
        }

        public void Stop()
        {
            _keepRunning = false;

            try
            {
                if (_pollingThread != null && _pollingThread.IsAlive)
                {
                    if (!_pollingThread.Join(500))
                        try
                        {
                            _pollingThread.Abort();
                        }
                        catch { }
                }
            }
            catch { }
        }

        private void PollerLoop()
        {
            while (_keepRunning)
            {
                try
                {
                    this.UpdateState();
                    Thread.Sleep(_waitTime);
                }
                catch
                {
                    Thread.Sleep(500);
                }
            }
        }

        private void UpdateState()
        {
            int result = XInput.XInputGetState(_playerIndex, ref gamepadStateCurrent);
            if (gamepadStateCurrent.PacketNumber != gamepadStatePrev.PacketNumber)
                OnStateChanged();
            gamepadStatePrev.Copy(gamepadStateCurrent);
        }
        #endregion

        public override string ToString()
        {
            return _playerIndex.ToString();
        }

    }

    internal class XboxControllerStateChangedEventArgs : EventArgs
    {
        public XInputState CurrentInputState { get; set; }
        public XInputState PreviousInputState { get; set; }
    }
}
