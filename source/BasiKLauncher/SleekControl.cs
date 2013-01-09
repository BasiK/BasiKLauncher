using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace BasiK.BasiKLauncher
{
    internal partial class SleekControl : UserControl
    {
        public static Random Random = new Random();

        public SleekControl()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        private int imageIndex = 1;

        private ApplicationEntry _entry;
        private Bitmap _unselectedBitmap;
        private Bitmap _selectedBitmap;
        public void LoadEntry(ApplicationEntry entry)
        {
            _entry = entry;
            imageIndex = Random.Next(1,6);

            Bitmap sbmp = new Bitmap(400,120);
            Image simg = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream(string.Format("BasiK.BasiKLauncher.Resources.transpbox{0}_selected.png", imageIndex)));

            Bitmap ubmp = new Bitmap(400, 120);
            Image uimg = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream(string.Format("BasiK.BasiKLauncher.Resources.transpbox{0}_unselected.png", imageIndex)));

            
            Graphics g = Graphics.FromImage(sbmp);
            g.Clear(Color.Transparent);
            if (!string.IsNullOrEmpty(_entry.DisplayIcon))
                try
                {
                    g.DrawImage(System.Drawing.Image.FromFile(_entry.DisplayIcon), new Rectangle(70, 22, 65, 65));
                }
                catch { }

            int textHeight = 30;
            foreach (string subs in _entry.DisplayName.Split(new char[] { ':' }))
            {
                if (string.IsNullOrEmpty(subs.Trim()))
                    continue;
                g.DrawString(subs.Trim(), new Font("Arial", 14, FontStyle.Bold), new SolidBrush(Color.Black), 152, textHeight);
                textHeight += 22;
            }
            g.DrawImage(simg, new Rectangle(0, 0, 400, 120));
            _selectedBitmap = sbmp;

            g = Graphics.FromImage(ubmp);
            g.Clear(Color.Transparent);
            if (!string.IsNullOrEmpty(_entry.DisplayIcon))
                try
                {
                    g.DrawImage(System.Drawing.Image.FromFile(_entry.DisplayIcon), new Rectangle(70, 22, 65, 65));
                }
                catch { }

            textHeight = 30;
            foreach (string subs in _entry.DisplayName.Split(new char[] { ':' }))
            {
                if (string.IsNullOrEmpty(subs.Trim()))
                    continue;
                g.DrawString(subs.Trim(), new Font("Arial", 14, FontStyle.Bold), new SolidBrush(Color.Black), 152, textHeight);
                textHeight += 22;
            }
            g.DrawImage(uimg, new Rectangle(0, 0, 400, 120));
            _unselectedBitmap = ubmp;

        }



        public ApplicationEntry AppEntry
        {
            get
            {
                return _entry;
            }
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            if (_entry != null)
            {
                Graphics g = e.Graphics;
                if (_selected)
                    g.DrawImage(_selectedBitmap, new Point(0, 0));
                else
                    g.DrawImage(_unselectedBitmap, new Point(0, 0));
               
            }

            base.OnPaint(e);
        }

        private bool _selected = true;
        public new void Select()
        {
            _selected = true;
            this.Refresh();
        }

        public void Unselect()
        {
            _selected = false;
            this.Refresh();
        }

    }
}
