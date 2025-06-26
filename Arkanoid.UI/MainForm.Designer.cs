namespace Arkanoid.UI
{
  partial class MainForm
  {
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      this.controlPanel = new System.Windows.Forms.Panel();
      this.restartButton = new System.Windows.Forms.Button();
      this.resumeButton = new System.Windows.Forms.Button();
      this.pauseButton = new System.Windows.Forms.Button();
      this.startButton = new System.Windows.Forms.Button();
      this.scoreLabel = new System.Windows.Forms.Label();
      this.statusLabel = new System.Windows.Forms.Label();
      this.gameTimer = new System.Windows.Forms.Timer(this.components);
      this.controlPanel.SuspendLayout();
      this.SuspendLayout();
      //
      // controlPanel
      //
      this.controlPanel.BackColor = System.Drawing.Color.DarkGray;
      this.controlPanel.Controls.Add(this.restartButton);
      this.controlPanel.Controls.Add(this.resumeButton);
      this.controlPanel.Controls.Add(this.pauseButton);
      this.controlPanel.Controls.Add(this.startButton);
      this.controlPanel.Controls.Add(this.scoreLabel);
      this.controlPanel.Dock = System.Windows.Forms.DockStyle.Top;
      this.controlPanel.Location = new System.Drawing.Point(0, 0);
      this.controlPanel.Name = "controlPanel";
      this.controlPanel.Size = new System.Drawing.Size(800, 50);
      this.controlPanel.TabIndex = 0;
      //
      // restartButton
      //
      this.restartButton.BackColor = System.Drawing.Color.Orange;
      this.restartButton.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.restartButton.ForeColor = System.Drawing.Color.Black;
      this.restartButton.Location = new System.Drawing.Point(320, 10);
      this.restartButton.Name = "restartButton";
      this.restartButton.Size = new System.Drawing.Size(80, 30);
      this.restartButton.TabIndex = 3;
      this.restartButton.Text = "Restart";
      this.restartButton.UseVisualStyleBackColor = false;
      this.restartButton.Click += new System.EventHandler(this.RestartButton_Click);
      //
      // resumeButton
      //
      this.resumeButton.BackColor = System.Drawing.Color.LightSkyBlue;
      this.resumeButton.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.resumeButton.ForeColor = System.Drawing.Color.Black;
      this.resumeButton.Location = new System.Drawing.Point(220, 10);
      this.resumeButton.Name = "resumeButton";
      this.resumeButton.Size = new System.Drawing.Size(80, 30);
      this.resumeButton.TabIndex = 2;
      this.resumeButton.Text = "Resume";
      this.resumeButton.UseVisualStyleBackColor = false;
      this.resumeButton.Click += new System.EventHandler(this.ResumeButton_Click);
      //
      // pauseButton
      //
      this.pauseButton.BackColor = System.Drawing.Color.LightCoral;
      this.pauseButton.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.pauseButton.ForeColor = System.Drawing.Color.Black;
      this.pauseButton.Location = new System.Drawing.Point(120, 10);
      this.pauseButton.Name = "pauseButton";
      this.pauseButton.Size = new System.Drawing.Size(80, 30);
      this.pauseButton.TabIndex = 1;
      this.pauseButton.Text = "Pause";
      this.pauseButton.UseVisualStyleBackColor = false;
      this.pauseButton.Click += new System.EventHandler(this.PauseButton_Click);
      //
      // startButton
      //
      this.startButton.BackColor = System.Drawing.Color.LightGreen;
      this.startButton.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.startButton.ForeColor = System.Drawing.Color.Black;
      this.startButton.Location = new System.Drawing.Point(20, 10);
      this.startButton.Name = "startButton";
      this.startButton.Size = new System.Drawing.Size(80, 30);
      this.startButton.TabIndex = 0;
      this.startButton.Text = "Start";
      this.startButton.UseVisualStyleBackColor = false;
      this.startButton.Click += new System.EventHandler(this.StartButton_Click);
      //
      // scoreLabel
      //
      this.scoreLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.scoreLabel.AutoSize = true;
      this.scoreLabel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.scoreLabel.ForeColor = System.Drawing.Color.White;
      this.scoreLabel.Location = new System.Drawing.Point(690, 16);
      this.scoreLabel.Name = "scoreLabel";
      this.scoreLabel.Size = new System.Drawing.Size(73, 19);
      this.scoreLabel.TabIndex = 4;
      this.scoreLabel.Text = "Score: 0";
      //
      // statusLabel
      //
      this.statusLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
      this.statusLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
      this.statusLabel.Font = new System.Drawing.Font("Arial", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
      this.statusLabel.ForeColor = System.Drawing.Color.Red;
      this.statusLabel.Location = new System.Drawing.Point(0, 50);
      this.statusLabel.Name = "statusLabel";
      this.statusLabel.Size = new System.Drawing.Size(800, 550);
      this.statusLabel.TabIndex = 1;
      this.statusLabel.Text = "";
      this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      this.statusLabel.Visible = false;
      //
      // gameTimer
      //
      this.gameTimer.Interval = 16;
      this.gameTimer.Tick += new System.EventHandler(this.GameTimer_Tick);
      //
      // MainForm
      //
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.Color.LightBlue;
      this.ClientSize = new System.Drawing.Size(800, 600);
      this.Controls.Add(this.statusLabel);
      this.Controls.Add(this.controlPanel);
      this.DoubleBuffered = true;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.Name = "MainForm";
      this.Text = "Arkanoid";
      this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainForm_MouseMove);
      this.controlPanel.ResumeLayout(false);
      this.controlPanel.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Panel controlPanel;
    private System.Windows.Forms.Button restartButton;
    private System.Windows.Forms.Button resumeButton;
    private System.Windows.Forms.Button pauseButton;
    private System.Windows.Forms.Button startButton;
    private System.Windows.Forms.Label scoreLabel;
    private System.Windows.Forms.Label statusLabel;
    private System.Windows.Forms.Timer gameTimer;
  }
}