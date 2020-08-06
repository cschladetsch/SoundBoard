namespace MidiWatcher
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if(disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.channelListBox = new System.Windows.Forms.ListBox();
            this.channelMessageGroupBox = new System.Windows.Forms.GroupBox();
            this.channelMessageGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // channelListBox
            // 
            this.channelListBox.FormattingEnabled = true;
            this.channelListBox.ItemHeight = 20;
            this.channelListBox.Location = new System.Drawing.Point(9, 29);
            this.channelListBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.channelListBox.Name = "channelListBox";
            this.channelListBox.Size = new System.Drawing.Size(385, 224);
            this.channelListBox.TabIndex = 0;
            // 
            // channelMessageGroupBox
            // 
            this.channelMessageGroupBox.Controls.Add(this.channelListBox);
            this.channelMessageGroupBox.Location = new System.Drawing.Point(18, 18);
            this.channelMessageGroupBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.channelMessageGroupBox.Name = "channelMessageGroupBox";
            this.channelMessageGroupBox.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.channelMessageGroupBox.Size = new System.Drawing.Size(405, 271);
            this.channelMessageGroupBox.TabIndex = 1;
            this.channelMessageGroupBox.TabStop = false;
            this.channelMessageGroupBox.Text = "Channel Messages";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(446, 330);
            this.Controls.Add(this.channelMessageGroupBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Form1";
            this.Text = "Sound Board";
            this.channelMessageGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox channelListBox;
        private System.Windows.Forms.GroupBox channelMessageGroupBox;
    }
}

