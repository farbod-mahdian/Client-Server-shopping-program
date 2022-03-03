
namespace WinClient1
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
            this.components = new System.ComponentModel.Container();
            this.listOrders = new System.Windows.Forms.ListBox();
            this.comboProducts = new System.Windows.Forms.ComboBox();
            this.btnBuy = new System.Windows.Forms.Button();
            this.tmrUpdate = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // listOrders
            // 
            this.listOrders.FormattingEnabled = true;
            this.listOrders.Location = new System.Drawing.Point(13, 13);
            this.listOrders.Name = "listOrders";
            this.listOrders.Size = new System.Drawing.Size(230, 277);
            this.listOrders.TabIndex = 0;
            // 
            // comboProducts
            // 
            this.comboProducts.FormattingEnabled = true;
            this.comboProducts.Location = new System.Drawing.Point(263, 13);
            this.comboProducts.Name = "comboProducts";
            this.comboProducts.Size = new System.Drawing.Size(190, 21);
            this.comboProducts.TabIndex = 1;
            // 
            // btnBuy
            // 
            this.btnBuy.Location = new System.Drawing.Point(263, 267);
            this.btnBuy.Name = "btnBuy";
            this.btnBuy.Size = new System.Drawing.Size(190, 23);
            this.btnBuy.TabIndex = 2;
            this.btnBuy.Text = "Purchase";
            this.btnBuy.UseVisualStyleBackColor = true;
            this.btnBuy.Click += new System.EventHandler(this.btnBuy_Click);
            // 
            // tmrUpdate
            // 
            this.tmrUpdate.Interval = 2000;
            this.tmrUpdate.Tick += new System.EventHandler(this.tmrUpdate_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(473, 303);
            this.Controls.Add(this.btnBuy);
            this.Controls.Add(this.comboProducts);
            this.Controls.Add(this.listOrders);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listOrders;
        private System.Windows.Forms.ComboBox comboProducts;
        private System.Windows.Forms.Button btnBuy;
        private System.Windows.Forms.Timer tmrUpdate;
    }
}

