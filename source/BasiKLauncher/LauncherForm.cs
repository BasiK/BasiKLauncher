using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace BasiK.BasiKLauncher
{
    internal partial class LauncherForm : Form
    {
        private ApplicationLibrary _dictionary;
        private XInputController _gamepad;

        internal LauncherForm(ApplicationLibrary gdict)
        {
            InitializeComponent();

            _dictionary = gdict;

            this.FormClosing += LWBPForm_FormClosing;

            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);

            

            try
            {
                gameControlsPanel.BackgroundImage = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("BasiK.BasiKLauncher.Resources.full_background.png"));
                gameControlsPanel.BackgroundImageLayout = ImageLayout.Stretch;

                ComputeControls();
                DrawPage();

                gameControlsPanel.Resize += Panel2_Resize;

                _gamepad = new XInputController(0);
                _gamepad.StateChanged += _gamepad_StateChanged;
                _gamepad.Start();
            }
            catch (Exception exc)
            {
                this.WindowState = FormWindowState.Minimized;
                this.TopMost = false;

                MessageBox.Show("Exception occured: \r\n" + exc.Message + "\r\n" + exc.StackTrace);
                Application.Exit();
            }

            
        }

        void LWBPForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_gamepad != null)
            {
                try
                {
                    _gamepad.Stop();
                }
                catch { }
            }
        }

        

        void Panel2_Resize(object sender, EventArgs e)
        {
            ComputeControls();
            current_page = 0;
            current_row = 0;
            current_column = 0;
            DrawPage();
        }


        #region Selection Movement
        private delegate void SelectionMoveDelegate();
        void _gamepad_StateChanged(object sender, XboxControllerStateChangedEventArgs e)
        {
            if (!HasFocus())
                return;

            if (e.CurrentInputState.Gamepad.IsButtonPressed((int)ButtonFlags.XINPUT_GAMEPAD_DPAD_RIGHT) && !e.PreviousInputState.Gamepad.IsButtonPressed((int)ButtonFlags.XINPUT_GAMEPAD_DPAD_RIGHT))
                this.BeginInvoke(new SelectionMoveDelegate(SelectNextColumn), new object[] { });
            if (e.CurrentInputState.Gamepad.IsButtonPressed((int)ButtonFlags.XINPUT_GAMEPAD_DPAD_LEFT) && !e.PreviousInputState.Gamepad.IsButtonPressed((int)ButtonFlags.XINPUT_GAMEPAD_DPAD_LEFT))
                this.BeginInvoke(new SelectionMoveDelegate(SelectPreviousColumn), new object[] { });
            if (e.CurrentInputState.Gamepad.IsButtonPressed((int)ButtonFlags.XINPUT_GAMEPAD_DPAD_UP) && !e.PreviousInputState.Gamepad.IsButtonPressed((int)ButtonFlags.XINPUT_GAMEPAD_DPAD_UP))
                this.BeginInvoke(new SelectionMoveDelegate(SelectPreviousRow), new object[] { });
            if (e.CurrentInputState.Gamepad.IsButtonPressed((int)ButtonFlags.XINPUT_GAMEPAD_DPAD_DOWN) && !e.PreviousInputState.Gamepad.IsButtonPressed((int)ButtonFlags.XINPUT_GAMEPAD_DPAD_DOWN))
                this.BeginInvoke(new SelectionMoveDelegate(SelectNextRow), new object[] { });
            if (e.CurrentInputState.Gamepad.IsButtonPressed((int)ButtonFlags.XINPUT_GAMEPAD_A) && !e.PreviousInputState.Gamepad.IsButtonPressed((int)ButtonFlags.XINPUT_GAMEPAD_A))
                this.BeginInvoke(new SelectionMoveDelegate(StartGame), new object[] { });
            if (e.CurrentInputState.Gamepad.IsButtonPressed((int)ButtonFlags.XINPUT_GAMEPAD_B) && !e.PreviousInputState.Gamepad.IsButtonPressed((int)ButtonFlags.XINPUT_GAMEPAD_B))
                this.BeginInvoke(new SelectionMoveDelegate(Application.Exit), new object[] { });
        }

        protected override bool ProcessCmdKey(ref Message m, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.BeginInvoke(new SelectionMoveDelegate(Application.Exit), new object[] { });
                return true;
            }
            else if (keyData == Keys.Down)
            {
                this.BeginInvoke(new SelectionMoveDelegate(SelectNextRow), new object[] { });
                return true;
            }
            else if (keyData == Keys.Up)
            {
                this.BeginInvoke(new SelectionMoveDelegate(SelectPreviousRow), new object[] { });
                return true;
            }
            else if (keyData == Keys.Left)
            {
                this.BeginInvoke(new SelectionMoveDelegate(SelectPreviousColumn), new object[] { });
                return true;
            }
            else if (keyData == Keys.Right)
            {
                this.BeginInvoke(new SelectionMoveDelegate(SelectNextColumn), new object[] { });
                return true;
            }
            else if (keyData == Keys.Enter)
            {
                this.BeginInvoke(new SelectionMoveDelegate(StartGame), new object[] { });
                return true;
            }
            return false;
        }

        void b_DoubleClick(object sender, EventArgs e)
        {
            SleekControl sctrl = sender as SleekControl;
            if (sctrl == null)
                return;

            int idx = _gameControls[current_page].IndexOf(sctrl);
            int row = idx / max_controls_width;
            int col = idx % max_controls_width;

            current_row = row;
            current_column = col;
            SelectCurrentControl();
            StartGame();
        }

        void b_Click(object sender, EventArgs e)
        {
            SleekControl sctrl = sender as SleekControl;
            if (sctrl == null)
                return;

            int idx = _gameControls[current_page].IndexOf(sctrl);
            int row = idx / max_controls_width;
            int col = idx % max_controls_width;

            current_row = row;
            current_column = col;
            SelectCurrentControl();
        }

        private void StartGame()
        {
            if (_selectedControl != null)
                ApplicationStarter.StartApplication(_selectedControl.AppEntry);
        }

        private void SelectPreviousRow()
        {
            current_row--;
            if (current_row < 0)
            {
                current_page--;
                if (current_page < 0)
                    current_page = _gameControls.Count-1;

                current_row = max_controls_height - 1;
                while (current_row > 0 && current_row * max_controls_width + current_column >= _gameControls[current_page].Count)
                    current_row--;

                DrawPage();
            }

            SelectCurrentControl();
        }
        private void SelectPreviousColumn()
        {
            current_column--;
            if (current_column < 0)
            {
                current_page--;
                if (current_page < 0)
                    current_page = _gameControls.Count - 1;

                current_column = max_controls_width - 1;
                while (current_column > 0 && current_row * max_controls_width + current_column >= _gameControls[current_page].Count)
                    current_column--;

                DrawPage();
            }

            SelectCurrentControl();
        }
        private void SelectNextRow()
        {
            current_row++;
            if (current_row >= max_controls_height || (current_row * max_controls_width + current_column >= _gameControls[current_page].Count))
            {
                current_page++;
                current_row = 0;
                if (current_page >= _gameControls.Count)
                    current_page = 0;

                while (current_column > 0 && current_row * max_controls_width + current_column >= _gameControls[current_page].Count)
                    current_column--;

                DrawPage();
            }

            SelectCurrentControl();
        }
        private void SelectNextColumn()
        {
            current_column++;
            if (current_column >= max_controls_width || (current_row * max_controls_width + current_column >= _gameControls[current_page].Count))
            {
                current_page++;
                current_column = 0;
                if (current_page >= _gameControls.Count)
                    current_page = 0;

                while (current_row > 0 && current_row * max_controls_width + current_column >= _gameControls[current_page].Count)
                    current_row--;

                DrawPage();
            }

            SelectCurrentControl();
        }
        #endregion
        #region Only accept gamepad input if app has focus
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);

        private bool HasFocus()
        {
            var activatedHandle = GetForegroundWindow();
            if (activatedHandle == IntPtr.Zero)
                return false;

            var procId = Process.GetCurrentProcess().Id;
            int activeProcId;
            GetWindowThreadProcessId(activatedHandle, out activeProcId);

            return activeProcId == procId;
        }
        #endregion

        private int widget_x = 10;
        private int widget_y = 10;
        private List<List<SleekControl>> _gameControls = new List<List<SleekControl>>();

        private int current_page = 0;
        private int current_row = 0;
        private int current_column = 0;

        private int max_controls_height = 1;
        private int max_controls_width = 1;
        private void ComputeControls()
        {
            ClearPage();
            _gameControls.Clear();

            // Compute controls per page
            SleekControl testControl = new SleekControl();
            max_controls_width = gameControlsPanel.ClientSize.Width / testControl.Size.Width;
            if (max_controls_width <= 0)
                max_controls_width = 1;

            max_controls_height = gameControlsPanel.ClientSize.Height / testControl.Size.Height;
            if (max_controls_height <= 0)
                max_controls_height = 1;

            List<ApplicationEntry> gameList = _dictionary.ApplicationList;

            int cp_page = 0;
            int cp_x = 0;
            int cp_y = 0;
            foreach (ApplicationEntry game in gameList)
            {
                SleekControl b = new SleekControl();
                b.LoadEntry(game);
                b.Unselect();
                b.Click += b_Click;
                b.DoubleClick += b_DoubleClick;

                if (cp_page >= _gameControls.Count)
                    _gameControls.Add(new List<SleekControl>());

                _gameControls[cp_page].Add(b);

                cp_x++;
                if (cp_x >= max_controls_width)
                {
                    cp_y++;
                    cp_x = 0;
                }
                if (cp_y >= max_controls_height)
                {
                    cp_page++;
                    cp_y = 0;
                    cp_x = 0;
                }
            }
        }

        


        #region Page Selection
        private SleekControl _selectedControl = null;
        private void SelectCurrentControl()
        {
            if (_selectedControl != null)
                _selectedControl.Unselect();

            try
            {
                _selectedControl = _gameControls[current_page][current_row * max_controls_width + current_column];
                _selectedControl.Select();
            }
            catch 
            {
                try
                {
                    _selectedControl = _gameControls[current_page][0];
                    _selectedControl.Select();
                }
                catch { }
            }
        }

        private void ClearPage()
        {
            // Clear current controls
            lock (_selectionLock)
            {
                List<SleekControl> ctrlsToRemove = new List<SleekControl>();
                foreach (Control ctrl in this.gameControlsPanel.Controls)
                {
                    SleekControl sctrl = ctrl as SleekControl;
                    if (sctrl != null)
                        ctrlsToRemove.Add(sctrl);
                }

                foreach (SleekControl sctrl in ctrlsToRemove)
                    this.gameControlsPanel.Controls.Remove(sctrl);
            }
        }


        private object _selectionLock = new object();
        private void DrawPage()
        {
            this.SuspendLayout();

            lock (_selectionLock)
            {
                ClearPage();

                int cp_x = 0;
                int cp_y = 0;

                // Populate panel with controls
                foreach (SleekControl gameControl in _gameControls[current_page])
                {
                    gameControl.Location = new Point(widget_x + cp_x * gameControl.Width, widget_y + cp_y * gameControl.Height);
                    gameControlsPanel.Controls.Add(gameControl);

                    cp_x++;
                    if (cp_x >= max_controls_width)
                    {
                        cp_y++;
                        cp_x = 0;
                    }
                }

                SelectCurrentControl();
            }

            this.ResumeLayout();
        }
        #endregion

    }
}
