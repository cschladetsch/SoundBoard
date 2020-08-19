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
            this._soundList = new System.Windows.Forms.ListBox();
            this.channelMessageGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // channelListBox
            // 
            this.channelListBox.FormattingEnabled = true;
            this.channelListBox.Location = new System.Drawing.Point(6, 19);
            this.channelListBox.Name = "channelListBox";
            this.channelListBox.Size = new System.Drawing.Size(258, 95);
            this.channelListBox.TabIndex = 0;
            // 
            // channelMessageGroupBox
            // 
            this.channelMessageGroupBox.Controls.Add(this.channelListBox);
            this.channelMessageGroupBox.Location = new System.Drawing.Point(12, 12);
            this.channelMessageGroupBox.Name = "channelMessageGroupBox";
            this.channelMessageGroupBox.Size = new System.Drawing.Size(275, 130);
            this.channelMessageGroupBox.TabIndex = 1;
            this.channelMessageGroupBox.TabStop = false;
            this.channelMessageGroupBox.Text = "Channel Messages";
            // 
            // _soundList
            // 
            this._soundList.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._soundList.FormattingEnabled = true;
            this._soundList.ItemHeight = 24;
            this._soundList.Location = new System.Drawing.Point(18, 158);
            this._soundList.Name = "_soundList";
            this._soundList.Size = new System.Drawing.Size(459, 340);
            this._soundList.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(508, 509);
            this.Controls.Add(this._soundList);
            this.Controls.Add(this.channelMessageGroupBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Sound Board";
            this.channelMessageGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox channelListBox;
        private System.Windows.Forms.GroupBox channelMessageGroupBox;
        private System.Windows.Forms.ListBox _soundList;
    }
}

