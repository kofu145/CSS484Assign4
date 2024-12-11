namespace Audio_Classification
{
    partial class AudioGUI
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
            if (disposing && (components != null))
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AudioGUI));
            lstBox = new ListBox();
            gboAudioSelector = new GroupBox();
            gboExport = new GroupBox();
            btnExport = new Button();
            gboFilters = new GroupBox();
            rBtnMusic = new RadioButton();
            rBtnSpeech = new RadioButton();
            MediaPlayer = new AxWMPLib.AxWindowsMediaPlayer();
            lblFileName = new Label();
            waveViewer1 = new NAudio.Gui.WaveViewer();
            lblWaveLength = new Label();
            gboAudioSelector.SuspendLayout();
            gboExport.SuspendLayout();
            gboFilters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)MediaPlayer).BeginInit();
            SuspendLayout();
            // 
            // lstBox
            // 
            lstBox.FormattingEnabled = true;
            lstBox.ItemHeight = 15;
            lstBox.Location = new Point(36, 160);
            lstBox.Name = "lstBox";
            lstBox.Size = new Size(200, 184);
            lstBox.TabIndex = 0;
            lstBox.SelectedIndexChanged += lstBox_SelectedIndexChanged;
            // 
            // gboAudioSelector
            // 
            gboAudioSelector.Controls.Add(gboFilters);
            gboAudioSelector.Controls.Add(lstBox);
            gboAudioSelector.Location = new Point(588, 138);
            gboAudioSelector.Name = "gboAudioSelector";
            gboAudioSelector.Size = new Size(272, 367);
            gboAudioSelector.TabIndex = 1;
            gboAudioSelector.TabStop = false;
            gboAudioSelector.Text = "Audio File Selection";
            // 
            // gboExport
            // 
            gboExport.Controls.Add(btnExport);
            gboExport.Location = new Point(588, 51);
            gboExport.Name = "gboExport";
            gboExport.Size = new Size(272, 81);
            gboExport.TabIndex = 8;
            gboExport.TabStop = false;
            gboExport.Text = "Export to File";
            // 
            // btnExport
            // 
            btnExport.Location = new Point(62, 22);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(143, 48);
            btnExport.TabIndex = 0;
            btnExport.Text = "Export JSON and CSV";
            btnExport.UseVisualStyleBackColor = true;
            btnExport.Click += btnExport_Click;
            // 
            // gboFilters
            // 
            gboFilters.Controls.Add(rBtnMusic);
            gboFilters.Controls.Add(rBtnSpeech);
            gboFilters.Location = new Point(36, 38);
            gboFilters.Name = "gboFilters";
            gboFilters.Size = new Size(200, 100);
            gboFilters.TabIndex = 2;
            gboFilters.TabStop = false;
            gboFilters.Text = "File Filter";
            // 
            // rBtnMusic
            // 
            rBtnMusic.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            rBtnMusic.AutoSize = true;
            rBtnMusic.Location = new Point(39, 56);
            rBtnMusic.Name = "rBtnMusic";
            rBtnMusic.Size = new Size(118, 19);
            rBtnMusic.TabIndex = 1;
            rBtnMusic.TabStop = true;
            rBtnMusic.Text = "Music Audio Files";
            rBtnMusic.UseVisualStyleBackColor = true;
            // 
            // rBtnSpeech
            // 
            rBtnSpeech.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            rBtnSpeech.AutoSize = true;
            rBtnSpeech.Location = new Point(39, 31);
            rBtnSpeech.Name = "rBtnSpeech";
            rBtnSpeech.Size = new Size(124, 19);
            rBtnSpeech.TabIndex = 0;
            rBtnSpeech.TabStop = true;
            rBtnSpeech.Text = "Speech Audio Files";
            rBtnSpeech.UseVisualStyleBackColor = true;
            rBtnSpeech.CheckedChanged += rBtn_CheckedChanged;
            // 
            // MediaPlayer
            // 
            MediaPlayer.Enabled = true;
            MediaPlayer.Location = new Point(55, 51);
            MediaPlayer.Name = "MediaPlayer";
            MediaPlayer.OcxState = (AxHost.State)resources.GetObject("MediaPlayer.OcxState");
            MediaPlayer.Size = new Size(486, 217);
            MediaPlayer.TabIndex = 2;
            // 
            // lblFileName
            // 
            lblFileName.AutoSize = true;
            lblFileName.Location = new Point(55, 33);
            lblFileName.Name = "lblFileName";
            lblFileName.Size = new Size(66, 15);
            lblFileName.TabIndex = 3;
            lblFileName.Text = "File Name: ";
            // 
            // waveViewer1
            // 
            waveViewer1.BackColor = SystemColors.ActiveCaption;
            waveViewer1.BorderStyle = BorderStyle.Fixed3D;
            waveViewer1.Location = new Point(55, 311);
            waveViewer1.Name = "waveViewer1";
            waveViewer1.SamplesPerPixel = 128;
            waveViewer1.Size = new Size(486, 194);
            waveViewer1.StartPosition = 0L;
            waveViewer1.TabIndex = 5;
            waveViewer1.WaveStream = null;
            // 
            // lblWaveLength
            // 
            lblWaveLength.AutoSize = true;
            lblWaveLength.Location = new Point(55, 293);
            lblWaveLength.Name = "lblWaveLength";
            lblWaveLength.Size = new Size(104, 15);
            lblWaveLength.TabIndex = 6;
            lblWaveLength.Text = "Wavelength Visual";
            // 
            // AudioGUI
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(931, 530);
            Controls.Add(gboExport);
            Controls.Add(lblWaveLength);
            Controls.Add(waveViewer1);
            Controls.Add(lblFileName);
            Controls.Add(MediaPlayer);
            Controls.Add(gboAudioSelector);
            Name = "AudioGUI";
            Text = "Audio Classification";
            Load += AudioGUI_Load;
            gboAudioSelector.ResumeLayout(false);
            gboExport.ResumeLayout(false);
            gboFilters.ResumeLayout(false);
            gboFilters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)MediaPlayer).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox lstBox;
        private GroupBox gboAudioSelector;
        private RadioButton rBtnMusic;
        private RadioButton rBtnSpeech;
        private AxWMPLib.AxWindowsMediaPlayer MediaPlayer;
        private Label lblFileName;
        private NAudio.Gui.WaveViewer waveViewer1;
        private Label lblWaveLength;
        private GroupBox gboFilters;
        private GroupBox gboExport;
        private Button button3;
        private Button button2;
        private Button btnExport;
    }
}