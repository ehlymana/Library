namespace LibraryForms
{
    partial class SearchBooks
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
            this.buttonDashboard = new System.Windows.Forms.Button();
            this.listBoxGenre = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonFilter = new System.Windows.Forms.Button();
            this.listBoxAuthor = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonDashboard
            // 
            this.buttonDashboard.Location = new System.Drawing.Point(313, 514);
            this.buttonDashboard.Name = "buttonDashboard";
            this.buttonDashboard.Size = new System.Drawing.Size(140, 32);
            this.buttonDashboard.TabIndex = 0;
            this.buttonDashboard.Text = "Back to Dashboard";
            this.buttonDashboard.UseVisualStyleBackColor = true;
            this.buttonDashboard.Click += new System.EventHandler(this.buttonDashboard_Click);
            // 
            // listBoxGenre
            // 
            this.listBoxGenre.FormattingEnabled = true;
            this.listBoxGenre.ItemHeight = 16;
            this.listBoxGenre.Location = new System.Drawing.Point(28, 49);
            this.listBoxGenre.Name = "listBoxGenre";
            this.listBoxGenre.Size = new System.Drawing.Size(290, 116);
            this.listBoxGenre.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.listBoxGenre);
            this.groupBox1.Location = new System.Drawing.Point(31, 37);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(344, 192);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Genres";
            // 
            // buttonFilter
            // 
            this.buttonFilter.Location = new System.Drawing.Point(313, 245);
            this.buttonFilter.Name = "buttonFilter";
            this.buttonFilter.Size = new System.Drawing.Size(140, 32);
            this.buttonFilter.TabIndex = 2;
            this.buttonFilter.Text = "Filter";
            this.buttonFilter.UseVisualStyleBackColor = true;
            this.buttonFilter.Click += new System.EventHandler(this.buttonFilter_Click);
            // 
            // listBoxAuthor
            // 
            this.listBoxAuthor.FormattingEnabled = true;
            this.listBoxAuthor.ItemHeight = 16;
            this.listBoxAuthor.Location = new System.Drawing.Point(28, 49);
            this.listBoxAuthor.Name = "listBoxAuthor";
            this.listBoxAuthor.Size = new System.Drawing.Size(286, 116);
            this.listBoxAuthor.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.listBoxAuthor);
            this.groupBox2.Location = new System.Drawing.Point(409, 37);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(336, 192);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Authors";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dataGridView);
            this.groupBox3.Location = new System.Drawing.Point(31, 290);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(714, 218);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Database";
            // 
            // dataGridView
            // 
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Location = new System.Drawing.Point(18, 35);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.RowTemplate.Height = 24;
            this.dataGridView.Size = new System.Drawing.Size(674, 165);
            this.dataGridView.TabIndex = 1;
            // 
            // SearchBooks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 558);
            this.Controls.Add(this.buttonFilter);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonDashboard);
            this.Name = "SearchBooks";
            this.Text = "Search Books";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonDashboard;
        private System.Windows.Forms.ListBox listBoxGenre;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonFilter;
        private System.Windows.Forms.ListBox listBoxAuthor;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView dataGridView;
    }
}