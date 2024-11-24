using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;


namespace Sensor_de_Temperatura_e_Umidade
{
    public partial class Form1 : Form
    {
        // ---------------------------- Variáveis Globais ----------------------------

        // Timer para atualização automática
        private Timer timer;

        // Listas para histórico de medições
        private List<Medicao> historicoMedicoes;

        // Controles dinâmicos para histórico e gráficos
        private ListView listViewHistorico;

        // Variáveis de temperatura e umidade
        private double temperaturaNova = -1; // Valor inicial indica que a temperatura ainda não foi inicializada
        private double umidadeNova = -1; // Valor inicial indica que a umidade ainda não foi inicializada
        private double ultimaTemperatura;
        private double ultimaUmidade;

        // Instância de Random para simular dados de temperatura e umidade
        private Random rnd = new Random();

        // ---------------------------- Classe para Medições ----------------------------
        private class Medicao
        {
            public DateTime Timestamp { get; set; }
            public double Temperatura { get; set; }
            public double Umidade { get; set; }
        }

        // ---------------------------- Construtor ----------------------------
        public Form1()
        {
            InitializeComponent(); // Inicializa o layout básico
            ConfigurarInterface(); // Configura a interface personalizada

            // Inicializa variáveis
            historicoMedicoes = new List<Medicao>();

            // Configuração do Timer
            timer = new Timer
            {
                Interval = 5000 // Intervalo de 5 segundos
            };
            timer.Tick += timer1_Tick; // Evento disparado a cada tick
        }

        // ---------------------------- Configuração da Interface ----------------------------
        private void ConfigurarInterface()
        {
            // Configuração geral do formulário
            this.Size = new Size(800, 620);
            this.MinimumSize = new Size(800, 600);

            // Painel para exibição de medições
            ConfigurarPainelMedicoes();

            // Painel para botões de controle
            ConfigurarPainelControles();

            // ListView para exibição do histórico
            ConfigurarListViewHistorico();

            // Botão para limpar histórico
            ConfigurarBotaoLimparHistorico();
        }

        private void ConfigurarPainelMedicoes()
        {
            // Painel das medições
            Panel painelMedicoes = new Panel
            {
                Location = new Point(20, 20),
                Size = new Size(500, 100)
            };

            // Estilos padrão
            Font fontLabels = new Font("Segoe UI", 12, FontStyle.Regular);
            Font fontValues = new Font("Segoe UI", 14, FontStyle.Bold);

            // Configuração de temperatura
            ConfigurarLabelETextBox(painelMedicoes, label1, tbTemperatura, "Temperatura:", fontLabels, fontValues, 20);

            // Configuração da variação de temperatura
            ConfigurarLabelETextBox(painelMedicoes, label4, tbVariacaoTemperatura, "Variação Temp.:", fontLabels, fontValues, 140);

            // Configuração de umidade
            ConfigurarLabelETextBox(painelMedicoes, label2, tbUmidade, "Umidade:", fontLabels, fontValues, 260);

            // Configuração da variação de umidade
            ConfigurarLabelETextBox(painelMedicoes, label5, tbVariacaoUmidade, "Variação Umid.:", fontLabels, fontValues, 380);

            // Adiciona o painel ao formulário
            this.Controls.Add(painelMedicoes);
        }

        private void ConfigurarLabelETextBox(Panel painel, Label label, TextBox textBox, string textoLabel, Font fontLabel, Font fontTextBox, int x)
        {
            // Configuração do Label
            label.Font = fontLabel;
            label.Text = textoLabel;
            label.Location = new Point(x, 20);
            label.Size = new Size(100, 25);
            label.Parent = painel;

            // Configuração do TextBox
            textBox.Font = fontTextBox;
            textBox.Size = new Size(100, 30);
            textBox.Location = new Point(x, 45);
            textBox.TextAlign = HorizontalAlignment.Center;
            textBox.ReadOnly = true;
            textBox.BackColor = Color.White;
            textBox.Parent = painel;
        }

        private void ConfigurarPainelControles()
        {
            Panel painelControles = new Panel
            {
                Location = new Point(540, 20),
                Size = new Size(220, 100)
            };

            // Botão para atualização manual
            buttonAtualizacaoManual.Font = new Font("Segoe UI", 10);
            buttonAtualizacaoManual.Size = new Size(180, 35);
            buttonAtualizacaoManual.Location = new Point(20, 10);
            buttonAtualizacaoManual.BackColor = Color.FromArgb(240, 240, 240);
            buttonAtualizacaoManual.FlatStyle = FlatStyle.System;
            buttonAtualizacaoManual.Parent = painelControles;

            // Checkbox para atualização automática
            checkBoxAtualizacaoAutomatica.Font = new Font("Segoe UI", 10);
            checkBoxAtualizacaoAutomatica.Location = new Point(20, 55);
            checkBoxAtualizacaoAutomatica.AutoSize = true;
            checkBoxAtualizacaoAutomatica.Parent = painelControles;

            this.Controls.Add(painelControles);
        }

        private void ConfigurarListViewHistorico()
        {
            listViewHistorico = new ListView
            {
                Location = new Point(20, 160),
                Size = new Size(740, 400),
                View = View.Details,
                GridLines = true,
                FullRowSelect = true,
                Font = new Font("Segoe UI", 9)
            };

            // Colunas do ListView
            listViewHistorico.Columns.Add("Data/Hora", 150);
            listViewHistorico.Columns.Add("Temperatura", 120);
            listViewHistorico.Columns.Add("Umidade", 120);
            listViewHistorico.Columns.Add("Variação Temp.", 150);
            listViewHistorico.Columns.Add("Variação Umid.", 150);

            this.Controls.Add(listViewHistorico);
        }

        private void ConfigurarBotaoLimparHistorico()
        {
            Button btnLimparHistorico = new Button
            {
                Text = "Limpar Histórico",
                Location = new Point(640, 125),
                Size = new Size(120, 30),
                Font = new Font("Segoe UI", 9),
                BackColor = Color.FromArgb(240, 240, 240),
                FlatStyle = FlatStyle.System
            };
            btnLimparHistorico.Click += LimparHistorico;
            this.Controls.Add(btnLimparHistorico);
        }

        // ---------------------------- Lógica de Atualização ----------------------------
        private void atualizarManualmente(object sender, EventArgs e)
        {
            // Desativa temporariamente o timer
            if (checkBoxAtualizacaoAutomatica.Checked)
                timer.Stop(); // Evita que o evento do timer dispare simultaneamente

            // Realiza a atualização manual
            atualizarTemperatura();
            atualizarUmidade();
            AdicionarMedicaoAoHistorico();

            // Reinicia o timer, se necessário
            if (checkBoxAtualizacaoAutomatica.Checked)
                timer.Start();
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            atualizarTemperatura();
            atualizarUmidade();
            AdicionarMedicaoAoHistorico();
        }

        private void atualizacaoAutomatica(object sender, EventArgs e)
        {
            if (!checkBoxAtualizacaoAutomatica.Checked)
                timer.Stop();
            else
                timer.Start();
        }

        // ---------------------------- Funções de Temperatura e Umidade ----------------------------

        // Método que inicializa o valor de temperatura com um valor aleatório
        public void inicializaTemperatura()
        {
            temperaturaNova = Math.Round(rnd.NextDouble() * 50, 1); // Gera uma temperatura entre 0 e 50 graus com uma casa decimal
            tbTemperatura.Text = (temperaturaNova.ToString() + "°C"); // Exibe a temperatura no TextBox
            ultimaTemperatura = temperaturaNova; // Define o valor inicial da última temperatura         
        }

        public void inicializaUmidade()
        {
            umidadeNova = (rnd.Next(20, 91)); // Gera uma umidade entre 20% e 90%
            tbUmidade.Text = (umidadeNova.ToString() + "%"); // Exibe a umidade no TextBox
            ultimaUmidade = umidadeNova; // Define o valor inicial da última umidade
        }

        // Método que atualiza o valor da temperatura com variação aleatória
        public void atualizarTemperatura()
        {
            if (temperaturaNova == -1) // Se a temperatura ainda não foi inicializada
            {
                inicializaTemperatura(); // Inicializa a temperatura
                tbVariacaoTemperatura.Text = ("+" + Math.Round((temperaturaNova - ultimaTemperatura), 1).ToString() + "°C"); // Exibe a variação              
            }
            else
            {
                // Calcula uma variação aleatória entre -5 e +5 graus
                double variacao = Math.Round(rnd.NextDouble() * 10, 1) - 5;
                temperaturaNova += variacao; // Atualiza a temperatura com a variação

                // Mantém a temperatura no intervalo entre 0 e 50 graus
                if (temperaturaNova < 0)
                {
                    temperaturaNova = 0;
                }
                else if (temperaturaNova > 50)
                {
                    temperaturaNova = 50;
                }

                tbTemperatura.Text = (temperaturaNova.ToString() + "°C"); // Exibe a nova temperatura no TextBox

                // Calcula a variação
                double variacaoTemp = Math.Round((temperaturaNova - ultimaTemperatura), 1);

                // Define o texto e a cor com base no valor da variação
                tbVariacaoTemperatura.Text = (variacaoTemp >= 0 ? "+" : "") + variacaoTemp.ToString() + "°C";
                tbVariacaoTemperatura.ForeColor = variacaoTemp >= 0 ? Color.Green : Color.Red;

                ultimaTemperatura = temperaturaNova;


            }
        }

        // Método que atualiza o valor da umidade com variação aleatória
        public void atualizarUmidade()
        {
            if (umidadeNova == -1) // Se a umidade ainda não foi inicializada
            {
                inicializaUmidade(); // Inicializa a umidade
                tbVariacaoUmidade.Text = ("+" + (umidadeNova - ultimaUmidade).ToString() + "%"); // Exibe a variação
            }
            else
            {
                // Calcula uma variação aleatória entre -5% e +5%
                int variacao = ((rnd.Next(0, 10) - 5));
                umidadeNova += variacao; // Atualiza a umidade com a variação

                // Mantém a umidade no intervalo entre 20% e 90%
                if (umidadeNova < 20)
                {
                    umidadeNova = 20;
                }
                else if (umidadeNova > 90)
                {
                    umidadeNova = 90;
                }

                tbUmidade.Text = (umidadeNova.ToString() + "%"); // Exibe a nova umidade no TextBox

                // Calcula a variação
                int variacaoUmid = (int)(umidadeNova - ultimaUmidade);

                // Define o texto e a cor com base no valor da variação
                tbVariacaoUmidade.Text = (variacaoUmid >= 0 ? "+" : "") + variacaoUmid.ToString() + "%";
                tbVariacaoUmidade.ForeColor = variacaoUmid >= 0 ? Color.Green : Color.Red;

                ultimaUmidade = umidadeNova;

            }
        }


        // ---------------------------- Histórico ----------------------------
        private void AdicionarMedicaoAoHistorico()
        {
            var medicao = new Medicao
            {
                Timestamp = DateTime.Now,
                Temperatura = temperaturaNova,
                Umidade = umidadeNova
            };

            historicoMedicoes.Add(medicao);

            // Adicionar ao ListView
            var item = new ListViewItem(medicao.Timestamp.ToString("dd/MM/yyyy HH:mm:ss"));
            item.SubItems.Add(medicao.Temperatura.ToString("F1") + "°C");
            item.SubItems.Add(medicao.Umidade.ToString("F0") + "%");
            item.SubItems.Add(tbVariacaoTemperatura.Text);
            item.SubItems.Add(tbVariacaoUmidade.Text);

            listViewHistorico.Items.Insert(0, item);

            // Manter apenas as últimas 100 medições no histórico
            if (listViewHistorico.Items.Count > 100)
            {
                listViewHistorico.Items.RemoveAt(100);
                historicoMedicoes.RemoveAt(0);
            }
        }

        private void LimparHistorico(object sender, EventArgs e)
        {
            historicoMedicoes.Clear();
            listViewHistorico.Items.Clear();
        }

    }
}
