namespace TestForm
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
            this.btnInsertPerf = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCount = new System.Windows.Forms.TextBox();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.txtRunTimes = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.txtWriteTime = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnInsertPerf
            // 
            this.btnInsertPerf.Location = new System.Drawing.Point(12, 38);
            this.btnInsertPerf.Name = "btnInsertPerf";
            this.btnInsertPerf.Size = new System.Drawing.Size(101, 36);
            this.btnInsertPerf.TabIndex = 0;
            this.btnInsertPerf.Text = "Insert Perf";
            this.btnInsertPerf.UseVisualStyleBackColor = true;
            this.btnInsertPerf.Click += new System.EventHandler(this.btnInsertPerf_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "counts:";
            // 
            // txtCount
            // 
            this.txtCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCount.Location = new System.Drawing.Point(67, 6);
            this.txtCount.Name = "txtCount";
            this.txtCount.Size = new System.Drawing.Size(254, 26);
            this.txtCount.TabIndex = 2;
            // 
            // txtResult
            // 
            this.txtResult.AcceptsReturn = true;
            this.txtResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtResult.Location = new System.Drawing.Point(12, 79);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResult.Size = new System.Drawing.Size(646, 346);
            this.txtResult.TabIndex = 3;
            // 
            // txtRunTimes
            // 
            this.txtRunTimes.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRunTimes.Location = new System.Drawing.Point(410, 6);
            this.txtRunTimes.Name = "txtRunTimes";
            this.txtRunTimes.Size = new System.Drawing.Size(58, 26);
            this.txtRunTimes.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(331, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "run times:";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(256, 38);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(101, 36);
            this.button3.TabIndex = 6;
            this.button3.Text = "Insert Perf";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // txtWriteTime
            // 
            this.txtWriteTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtWriteTime.Location = new System.Drawing.Point(556, 6);
            this.txtWriteTime.Name = "txtWriteTime";
            this.txtWriteTime.Size = new System.Drawing.Size(58, 26);
            this.txtWriteTime.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(477, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 20);
            this.label3.TabIndex = 7;
            this.label3.Text = "write time:";
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(670, 437);
            this.Controls.Add(this.txtWriteTime);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.txtRunTimes);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.txtCount);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnInsertPerf);
            this.Name = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox txtRunTimes;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.Button btnInsertPerf;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCount;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button WarmUp;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox txtWriteTime;
        private System.Windows.Forms.Label label3;
    }
}

