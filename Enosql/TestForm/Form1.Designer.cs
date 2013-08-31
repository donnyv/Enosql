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
            this.btnInsertJSONPerf = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCount = new System.Windows.Forms.TextBox();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.txtRunTimes = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnInsertObjectPerf = new System.Windows.Forms.Button();
            this.txtWriteTime = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.btnQueryPerfTest = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnInsertJSONPerf
            // 
            this.btnInsertJSONPerf.Location = new System.Drawing.Point(12, 38);
            this.btnInsertJSONPerf.Name = "btnInsertJSONPerf";
            this.btnInsertJSONPerf.Size = new System.Drawing.Size(101, 36);
            this.btnInsertJSONPerf.TabIndex = 0;
            this.btnInsertJSONPerf.Text = "Insert JSON Perf Test";
            this.btnInsertJSONPerf.UseVisualStyleBackColor = true;
            this.btnInsertJSONPerf.Click += new System.EventHandler(this.btnInsertJSONPerf_Click);
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
            this.txtCount.Text = "1000";
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
            this.txtRunTimes.Text = "3";
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
            // btnInsertObjectPerf
            // 
            this.btnInsertObjectPerf.Location = new System.Drawing.Point(119, 38);
            this.btnInsertObjectPerf.Name = "btnInsertObjectPerf";
            this.btnInsertObjectPerf.Size = new System.Drawing.Size(101, 36);
            this.btnInsertObjectPerf.TabIndex = 6;
            this.btnInsertObjectPerf.Text = "Insert Obj Perf Test";
            this.btnInsertObjectPerf.UseVisualStyleBackColor = true;
            this.btnInsertObjectPerf.Click += new System.EventHandler(this.btnInsertObjectPerf_Click);
            // 
            // txtWriteTime
            // 
            this.txtWriteTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtWriteTime.Location = new System.Drawing.Point(556, 6);
            this.txtWriteTime.Name = "txtWriteTime";
            this.txtWriteTime.Size = new System.Drawing.Size(58, 26);
            this.txtWriteTime.TabIndex = 8;
            this.txtWriteTime.Text = "100";
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
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(556, 38);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(101, 36);
            this.button4.TabIndex = 9;
            this.button4.Text = "Insert Obj Perf Test";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // btnQueryPerfTest
            // 
            this.btnQueryPerfTest.Location = new System.Drawing.Point(226, 38);
            this.btnQueryPerfTest.Name = "btnQueryPerfTest";
            this.btnQueryPerfTest.Size = new System.Drawing.Size(101, 36);
            this.btnQueryPerfTest.TabIndex = 10;
            this.btnQueryPerfTest.Text = "Query Perf Test";
            this.btnQueryPerfTest.UseVisualStyleBackColor = true;
            this.btnQueryPerfTest.Click += new System.EventHandler(this.btnQueryPerfTest_Click);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(670, 437);
            this.Controls.Add(this.btnQueryPerfTest);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.txtWriteTime);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnInsertObjectPerf);
            this.Controls.Add(this.txtRunTimes);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.txtCount);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnInsertJSONPerf);
            this.Name = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        
        private System.Windows.Forms.TextBox txtRunTimes;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.Button btnInsertJSONPerf;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCount;
        private System.Windows.Forms.Button btnInsertObjectPerf;
        private System.Windows.Forms.TextBox txtWriteTime;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button btnQueryPerfTest;
    }
}

