namespace AStar
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.panelMap = new System.Windows.Forms.Panel();
            this.btnCtStoneRodom = new System.Windows.Forms.Button();
            this.btnCtStoneHand = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnStartSearch = new System.Windows.Forms.Button();
            this.btnRest = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.cbSafeMode = new System.Windows.Forms.CheckBox();
            this.cbGlobal = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // panelMap
            // 
            this.panelMap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelMap.Location = new System.Drawing.Point(0, 0);
            this.panelMap.Name = "panelMap";
            this.panelMap.Size = new System.Drawing.Size(1000, 1000);
            this.panelMap.TabIndex = 0;
            this.panelMap.Paint += new System.Windows.Forms.PaintEventHandler(this.panelMap_Paint);
            this.panelMap.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panelMap_MouseClick);
            // 
            // btnCtStoneRodom
            // 
            this.btnCtStoneRodom.Location = new System.Drawing.Point(1016, 12);
            this.btnCtStoneRodom.Name = "btnCtStoneRodom";
            this.btnCtStoneRodom.Size = new System.Drawing.Size(95, 23);
            this.btnCtStoneRodom.TabIndex = 1;
            this.btnCtStoneRodom.Text = "随机生成障碍物";
            this.btnCtStoneRodom.UseVisualStyleBackColor = true;
            this.btnCtStoneRodom.Click += new System.EventHandler(this.btnCreateStone_Click);
            // 
            // btnCtStoneHand
            // 
            this.btnCtStoneHand.Location = new System.Drawing.Point(1016, 52);
            this.btnCtStoneHand.Name = "btnCtStoneHand";
            this.btnCtStoneHand.Size = new System.Drawing.Size(95, 23);
            this.btnCtStoneHand.TabIndex = 2;
            this.btnCtStoneHand.Text = "手动生成障碍";
            this.btnCtStoneHand.UseVisualStyleBackColor = true;
            this.btnCtStoneHand.Click += new System.EventHandler(this.btnCtStoneHand_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label1.Location = new System.Drawing.Point(1016, 207);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "黑色代表障碍物";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label3.Location = new System.Drawing.Point(1016, 249);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "红色代表目的地";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label2.Location = new System.Drawing.Point(1016, 228);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "绿色代表起始点";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label4.Location = new System.Drawing.Point(1016, 312);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(131, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "ALT加鼠标左键设置起点";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label5.Location = new System.Drawing.Point(1016, 333);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(131, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "ALT加鼠标右键设置终点";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label6.Location = new System.Drawing.Point(1016, 270);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(113, 12);
            this.label6.TabIndex = 11;
            this.label6.Text = "鼠标左键设置障碍物";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label7.Location = new System.Drawing.Point(1016, 291);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(113, 12);
            this.label7.TabIndex = 12;
            this.label7.Text = "鼠标右键移除障碍物";
            // 
            // btnStartSearch
            // 
            this.btnStartSearch.Location = new System.Drawing.Point(1016, 92);
            this.btnStartSearch.Name = "btnStartSearch";
            this.btnStartSearch.Size = new System.Drawing.Size(95, 23);
            this.btnStartSearch.TabIndex = 3;
            this.btnStartSearch.Text = "开始寻路";
            this.btnStartSearch.UseVisualStyleBackColor = true;
            this.btnStartSearch.Click += new System.EventHandler(this.btnStartSearch_Click);
            // 
            // btnRest
            // 
            this.btnRest.Location = new System.Drawing.Point(1016, 132);
            this.btnRest.Name = "btnRest";
            this.btnRest.Size = new System.Drawing.Size(95, 23);
            this.btnRest.TabIndex = 4;
            this.btnRest.Text = "重置";
            this.btnRest.UseVisualStyleBackColor = true;
            this.btnRest.Click += new System.EventHandler(this.btnRest_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label8.Location = new System.Drawing.Point(1016, 354);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(113, 12);
            this.label8.TabIndex = 13;
            this.label8.Text = "黄色代表寻路经过点";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.label9.Location = new System.Drawing.Point(1016, 375);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(101, 12);
            this.label9.TabIndex = 14;
            this.label9.Text = "紫色代表最佳路径";
            // 
            // cbSafeMode
            // 
            this.cbSafeMode.AutoSize = true;
            this.cbSafeMode.Checked = true;
            this.cbSafeMode.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbSafeMode.Location = new System.Drawing.Point(1018, 167);
            this.cbSafeMode.Name = "cbSafeMode";
            this.cbSafeMode.Size = new System.Drawing.Size(96, 16);
            this.cbSafeMode.TabIndex = 15;
            this.cbSafeMode.Text = "保持安全距离";
            this.cbSafeMode.UseVisualStyleBackColor = true;
            this.cbSafeMode.CheckedChanged += new System.EventHandler(this.cbSafeMode_CheckedChanged);
            // 
            // cbGlobal
            // 
            this.cbGlobal.AutoSize = true;
            this.cbGlobal.Checked = true;
            this.cbGlobal.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbGlobal.Location = new System.Drawing.Point(1018, 185);
            this.cbGlobal.Name = "cbGlobal";
            this.cbGlobal.Size = new System.Drawing.Size(72, 16);
            this.cbGlobal.TabIndex = 16;
            this.cbGlobal.Text = "全局最优";
            this.cbGlobal.UseVisualStyleBackColor = true;
            this.cbGlobal.CheckedChanged += new System.EventHandler(this.cbGlobal_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1158, 1001);
            this.Controls.Add(this.cbGlobal);
            this.Controls.Add(this.cbSafeMode);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.btnRest);
            this.Controls.Add(this.btnStartSearch);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCtStoneHand);
            this.Controls.Add(this.btnCtStoneRodom);
            this.Controls.Add(this.panelMap);
            this.KeyPreview = true;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = " ";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelMap;
        private System.Windows.Forms.Button btnCtStoneRodom;
        private System.Windows.Forms.Button btnCtStoneHand;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnStartSearch;
        private System.Windows.Forms.Button btnRest;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox cbSafeMode;
        private System.Windows.Forms.CheckBox cbGlobal;
    }
}

