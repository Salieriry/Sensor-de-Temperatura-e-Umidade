namespace Sensor_de_Temperatura_e_Umidade
{
    partial class Form1
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.buttonAtualizacaoManual = new System.Windows.Forms.Button();
            this.checkBoxAtualizacaoAutomatica = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbVariacaoUmidade = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbTemperatura = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbUmidade = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbVariacaoTemperatura = new System.Windows.Forms.TextBox();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.cbSerial = new System.Windows.Forms.ComboBox();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // buttonAtualizacaoManual
            // 
            this.buttonAtualizacaoManual.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.buttonAtualizacaoManual.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonAtualizacaoManual.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAtualizacaoManual.Location = new System.Drawing.Point(440, 20);
            this.buttonAtualizacaoManual.Name = "buttonAtualizacaoManual";
            this.buttonAtualizacaoManual.Size = new System.Drawing.Size(150, 35);
            this.buttonAtualizacaoManual.TabIndex = 0;
            this.buttonAtualizacaoManual.Text = "Atualização Manual";
            this.buttonAtualizacaoManual.UseVisualStyleBackColor = false;
            this.buttonAtualizacaoManual.Click += new System.EventHandler(this.atualizarManualmente);
            // 
            // checkBoxAtualizacaoAutomatica
            // 
            this.checkBoxAtualizacaoAutomatica.AutoSize = true;
            this.checkBoxAtualizacaoAutomatica.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxAtualizacaoAutomatica.Location = new System.Drawing.Point(440, 70);
            this.checkBoxAtualizacaoAutomatica.Name = "checkBoxAtualizacaoAutomatica";
            this.checkBoxAtualizacaoAutomatica.Size = new System.Drawing.Size(213, 27);
            this.checkBoxAtualizacaoAutomatica.TabIndex = 17;
            this.checkBoxAtualizacaoAutomatica.Text = "Atualização Automática";
            this.checkBoxAtualizacaoAutomatica.UseVisualStyleBackColor = true;
            this.checkBoxAtualizacaoAutomatica.CheckedChanged += new System.EventHandler(this.atualizacaoAutomatica);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(147, 76);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(86, 28);
            this.label5.TabIndex = 18;
            this.label5.Text = "Variação";
            // 
            // tbVariacaoUmidade
            // 
            this.tbVariacaoUmidade.BackColor = System.Drawing.Color.White;
            this.tbVariacaoUmidade.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbVariacaoUmidade.Location = new System.Drawing.Point(147, 101);
            this.tbVariacaoUmidade.Name = "tbVariacaoUmidade";
            this.tbVariacaoUmidade.ReadOnly = true;
            this.tbVariacaoUmidade.Size = new System.Drawing.Size(100, 39);
            this.tbVariacaoUmidade.TabIndex = 16;
            this.tbVariacaoUmidade.Text = "?";
            this.tbVariacaoUmidade.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(27, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 28);
            this.label1.TabIndex = 12;
            this.label1.Text = "Temperatura";
            // 
            // tbTemperatura
            // 
            this.tbTemperatura.BackColor = System.Drawing.Color.White;
            this.tbTemperatura.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbTemperatura.Location = new System.Drawing.Point(27, 36);
            this.tbTemperatura.Name = "tbTemperatura";
            this.tbTemperatura.ReadOnly = true;
            this.tbTemperatura.Size = new System.Drawing.Size(100, 39);
            this.tbTemperatura.TabIndex = 11;
            this.tbTemperatura.Text = "?";
            this.tbTemperatura.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(147, 11);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 28);
            this.label4.TabIndex = 14;
            this.label4.Text = "Variação";
            // 
            // tbUmidade
            // 
            this.tbUmidade.BackColor = System.Drawing.Color.White;
            this.tbUmidade.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbUmidade.Location = new System.Drawing.Point(27, 101);
            this.tbUmidade.Name = "tbUmidade";
            this.tbUmidade.ReadOnly = true;
            this.tbUmidade.Size = new System.Drawing.Size(100, 39);
            this.tbUmidade.TabIndex = 10;
            this.tbUmidade.Text = "?";
            this.tbUmidade.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(27, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 28);
            this.label2.TabIndex = 13;
            this.label2.Text = "Umidade";
            // 
            // tbVariacaoTemperatura
            // 
            this.tbVariacaoTemperatura.BackColor = System.Drawing.Color.White;
            this.tbVariacaoTemperatura.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbVariacaoTemperatura.Location = new System.Drawing.Point(147, 36);
            this.tbVariacaoTemperatura.Name = "tbVariacaoTemperatura";
            this.tbVariacaoTemperatura.ReadOnly = true;
            this.tbVariacaoTemperatura.Size = new System.Drawing.Size(100, 39);
            this.tbVariacaoTemperatura.TabIndex = 15;
            this.tbVariacaoTemperatura.Text = "?";
            this.tbVariacaoTemperatura.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // serialPort1
            // 
            this.serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.recebeLeitura);
            // 
            // cbSerial
            // 
            this.cbSerial.FormattingEnabled = true;
            this.cbSerial.Location = new System.Drawing.Point(440, 115);
            this.cbSerial.Name = "cbSerial";
            this.cbSerial.Size = new System.Drawing.Size(204, 24);
            this.cbSerial.TabIndex = 19;
            this.cbSerial.Text = "Portas seriais...";
            // 
            // timer
            // 
            this.timer.Interval = 5000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 673);
            this.Controls.Add(this.cbSerial);
            this.Controls.Add(this.checkBoxAtualizacaoAutomatica);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbVariacaoUmidade);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbTemperatura);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbUmidade);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbVariacaoTemperatura);
            this.Controls.Add(this.buttonAtualizacaoManual);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(800, 720);
            this.MinimumSize = new System.Drawing.Size(800, 720);
            this.Name = "Form1";
            this.Text = "Simulador de Sensor de Temperatura e Umidade";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonAtualizacaoManual;
        private System.Windows.Forms.CheckBox checkBoxAtualizacaoAutomatica;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbVariacaoUmidade;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbTemperatura;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbUmidade;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbVariacaoTemperatura;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.ComboBox cbSerial;
        private System.Windows.Forms.Timer timer;
    }
}

