﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Modern.Forms
{
    /// <summary>
    /// Represents a popup dialog used to inform the user of a message.
    /// </summary>
    public class MessageBoxForm : Form
    {
        private readonly Button ok_button;
        private readonly TextBox label;
        //private static MessageBoxForm _form = new MessageBoxForm ();

        /// <summary>
        /// Initializes a new instance of the MessageBoxForm class.
        /// </summary>
        public MessageBoxForm ()
        {
            StartPosition = FormStartPosition.CenterParent;
            AllowMinimize = false;
            AllowMaximize = false;

            label = Controls.Add (new TextBox {
                Dock = DockStyle.Fill,
                Height = 105,
                MultiLine = true,
                ReadOnly = true,
                Padding = new Padding (10)
            });

            var label_panel = Controls.Add (new Panel {
                Dock = DockStyle.Bottom,
                Height = 45
            });

            label.Style.BackgroundColor = Theme.FormBackgroundColor;
            label.Style.Border.Width = 0;

            ok_button = label_panel.Controls.Add (new Button {
                Text = "OK",
                Left = 150,
                Top = 6
            });

            ok_button.Click += (o, e) => Close ();
        }

        /// <summary>
        /// Initializes a new instance of the MessageBoxForm class with the specified title and message.
        /// </summary>
        public MessageBoxForm (string title, string message) : this ()
        {
            Text = title;
            label.Text = message;

            CalculateDialogSize ();
        }

        public static void Show (Control control, string title, string message)
        {
            if (control == null) return;
            
            var parentForm = control.FindForm ();
            if (parentForm != null) {
                MessageBoxForm form = new MessageBoxForm ();
                form.Text = title;
                form.Message = message;
                
                form.ShowDialog (parentForm);
                form.Dispose ();
            }
        }

        public static void Show (Form form, string title, string message)
        {
            if (form == null) return;
            if (form != null) {
                MessageBoxForm f = new MessageBoxForm ();
                f.Text = title;
                f.Message = message;

                f.ShowDialog (form);
                f.Dispose ();
            }
        }

        private void CalculateDialogSize ()
        {
            var num_lines = label?.Text?.Count (c => c == '\n') ?? 0;

            if (num_lines > 10)
                Size = new Size (800, 400);
            else if (num_lines > 4)
                Size = new Size (600, 300);
            else
                Size = new Size (400, 200);

            ok_button.Left = (int)((Size.Width - ok_button.Width) / 2);
        }

        /// <inheritdoc/>
        protected override Size DefaultSize => new Size (400, 200);

        /// <summary>
        /// Gets or sets the message of the dialog.
        /// </summary>
        public string Message {
            get => label.Text;
            set {
                if (label.Text != value) {
                    label.Text = value;
                    CalculateDialogSize ();
                }
            }
        }
    }
}
