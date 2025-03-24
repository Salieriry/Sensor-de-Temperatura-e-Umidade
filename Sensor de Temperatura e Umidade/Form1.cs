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
using System.IO.Ports;


namespace Sensor_de_Temperatura_e_Umidade
{
    public partial class Form1 : Form
    {
        // ---------------------------- Variáveis Globais ----------------------------


        // Timer para atualização automática
        private Timer timer;

        // Listas para histórico de medições
        private List<Medicao> historicoMedicoes;

        // TabControl para alternar entre modo simulação e modo real
        TabControl tabControl;

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

            // TabControl para alternar entre modo simulação e real
            ConfigurarTabControl();

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

        private void ConfigurarTabControl()
        {
            tabControl = new TabControl
            {
                Location = new Point(540, 20),
                Size = new Size(220, 120),
                Font = new Font("Segoe UI", 9)
            };

            tabControl.SelectedIndexChanged += tabControl_SelectedIndexChanged;

            // Tab para modo simulação
            TabPage tabSimulacao = new TabPage("Simulação");
            ConfigurarModoSimulacao(tabSimulacao);

            // Tab para modo real
            TabPage tabReal = new TabPage("Sensor Real");
            ConfigurarModoReal(tabReal);

            // Adiciona as tabs ao TabControl
            tabControl.TabPages.Add(tabSimulacao);
            tabControl.TabPages.Add(tabReal);

            this.Controls.Add(tabControl);
        }

        private void ConfigurarModoSimulacao(TabPage tabPage)
        {
            // Botão para atualização manual
            buttonAtualizacaoManual.Font = new Font("Segoe UI", 10);
            buttonAtualizacaoManual.Size = new Size(180, 25);
            buttonAtualizacaoManual.Location = new Point(20, 10);
            buttonAtualizacaoManual.BackColor = Color.FromArgb(240, 240, 240);
            buttonAtualizacaoManual.FlatStyle = FlatStyle.System;
            buttonAtualizacaoManual.Text = "Atualização Manual";
            tabPage.Controls.Add(buttonAtualizacaoManual);

            // Checkbox para atualização automática
            checkBoxAtualizacaoAutomatica.Font = new Font("Segoe UI", 10);
            checkBoxAtualizacaoAutomatica.Location = new Point(20, 45);
            checkBoxAtualizacaoAutomatica.AutoSize = true;
            checkBoxAtualizacaoAutomatica.Text = "Atualização Automática";
            tabPage.Controls.Add(checkBoxAtualizacaoAutomatica);
        }

        private void ConfigurarModoReal(TabPage tabPage)
        {
            // Label para indicar portas seriais
            Label lblPortaSerial = new Label
            {
                Font = new Font("Segoe UI", 10),
                Text = "Porta Serial:",
                Location = new Point(20, 10),
                Size = new Size(100, 20),
                AutoSize = true
            };
            tabPage.Controls.Add(lblPortaSerial);

            // Combobox para exibição de portas seriais
            cbSerial.Font = new Font("Segoe UI", 10);
            cbSerial.Location = new Point(20, 35);
            cbSerial.Size = new Size(180, 25);
            cbSerial.DropDownStyle = ComboBoxStyle.DropDownList;
            tabPage.Controls.Add(cbSerial);

            // Botão para conectar ao dispositivo
            Button btnConectar = new Button
            {
                Text = "Conectar",
                Location = new Point(20, 65),
                Size = new Size(85, 25),
                Font = new Font("Segoe UI", 10),
                BackColor = Color.FromArgb(240, 240, 240),
                FlatStyle = FlatStyle.System
            };
            btnConectar.Click += ConectarDispositivo;
            tabPage.Controls.Add(btnConectar);

            // Botão para desconectar do dispositivo
            Button btnDesconectar = new Button
            {
                Text = "Desconectar",
                Location = new Point(115, 65),
                Size = new Size(85, 25),
                Font = new Font("Segoe UI", 10),
                BackColor = Color.FromArgb(240, 240, 240),
                FlatStyle = FlatStyle.System
            };
            btnDesconectar.Click += DesconectarDispositivo;
            tabPage.Controls.Add(btnDesconectar);
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


        // ---------------------------- Porta Serial ----------------------------

        private void CarregarPortasSeriais()
        {
            // Limpa o ComboBox antes de carregar as portas disponíveis
            cbSerial.Items.Clear();

            // Obtém a lista de portas seriais disponíveis no sistema
            string[] portas = System.IO.Ports.SerialPort.GetPortNames();

            if (portas.Length > 0)
            {
                // Adiciona as portas disponíveis ao ComboBox e seleciona a primeira
                cbSerial.Items.AddRange(portas);
                cbSerial.SelectedIndex = 0;
            }
            else
            {
                // Se não houver portas disponíveis, exibe uma mensagem e desabilita o ComboBox
                cbSerial.Items.Add("Nenhuma porta disponível");
                cbSerial.SelectedIndex = 0;
                cbSerial.Enabled = false;
            }
        }

        private void ConectarDispositivo(object sender, EventArgs e)
        {
            // Verifica se uma porta foi selecionada no ComboBox
            if (cbSerial.SelectedItem == null)
            {
                MessageBox.Show("Selecione uma porta serial!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Obtém a porta selecionada
                string portaSelecionada = cbSerial.SelectedItem.ToString();

                // Inicializa e configura a porta serial com baud rate de 9600
                serialPort1 = new SerialPort(portaSelecionada, 9600);
                serialPort1.Open(); // Abre a conexão com a porta serial

                // Associa o evento de recebimento de dados à função que processa os dados recebidos
                serialPort1.DataReceived += recebeLeitura;

                MessageBox.Show($"Conectado à porta {portaSelecionada}", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Desabilita o ComboBox e o botão de conexão para evitar múltiplas conexões
                cbSerial.Enabled = false;
                ((Button)sender).Enabled = false;

                // Habilita o botão de desconectar
                Button btnDesconectar = ((Button)sender).Parent.Controls.OfType<Button>().FirstOrDefault(b => b.Text == "Desconectar");
                if (btnDesconectar != null)
                    btnDesconectar.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao conectar: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DesconectarDispositivo(object sender, EventArgs e)
        {
            try
            {
                // Verifica se a porta serial está aberta antes de tentar desconectar
                if (serialPort1 != null && serialPort1.IsOpen)
                {
                    serialPort1.DataReceived -= recebeLeitura; // Remove o evento de leitura
                    serialPort1.Close(); // Fecha a conexão com a porta serial
                    serialPort1.Dispose(); // Libera os recursos da porta serial
                }

                MessageBox.Show("Dispositivo desconectado", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Reativa o ComboBox e o botão de conexão para permitir nova conexão
                cbSerial.Enabled = true;

                // Habilita o botão de conexão
                Button btnConectar = ((Button)sender).Parent.Controls.OfType<Button>().FirstOrDefault(b => b.Text == "Conectar");
                if (btnConectar != null)
                    btnConectar.Enabled = true;

                // Desabilita o botão de desconectar
                ((Button)sender).Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao desconectar: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void recebeLeitura(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                // Lê os dados recebidos da porta serial e remove espaços extras
                string dadosRecebidos = serialPort1.ReadLine().Trim();

                // Garante que as atualizações da UI ocorram na thread principal
                this.Invoke((MethodInvoker)delegate
                {
                    atualizarTemperatura(); // Atualiza a temperatura com base nos novos dados
                    atualizarUmidade(); // Atualiza a umidade com base nos novos dados
                    AdicionarMedicaoAoHistorico(); // Armazena a medição no histórico
                });

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro na leitura da porta serial: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ---------------------------- Controle de Abas ----------------------------

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabControl tab = (TabControl)sender;

            // Verifica qual aba foi selecionada e ativa o modo correspondente
            if (tab.SelectedTab.Text == "Simulação")
            {
                DesativarModoReal(); // Desativa o modo de sensores reais
                AtivarModoSimulacao(); // Ativa a simulação
            }
            else if (tab.SelectedTab.Text == "Sensor Real")
            {
                DesativarModoSimulacao(); // Desativa a simulação
                AtivarModoReal(); // Ativa o modo de sensores reais
            }
        }

        // ---------------------------- Modos de Operação ----------------------------

        private void AtivarModoSimulacao()
        {
            // Habilita os controles relacionados à simulação
            buttonAtualizacaoManual.Enabled = true;
            checkBoxAtualizacaoAutomatica.Enabled = true;
        }

        private void DesativarModoSimulacao()
        {
            // Desabilita os controles da simulação e para a atualização automática, se estiver ativa
            if (checkBoxAtualizacaoAutomatica.Checked)
            {
                checkBoxAtualizacaoAutomatica.Checked = false;
                // Aqui deve ser incluída a lógica para parar o timer ou outro mecanismo de atualização automática
            }
            buttonAtualizacaoManual.Enabled = false;
            checkBoxAtualizacaoAutomatica.Enabled = false;
        }

        private void AtivarModoReal()
        {
            // Recarrega as portas seriais disponíveis e habilita os controles do modo real
            CarregarPortasSeriais();
            // Outros códigos podem ser adicionados aqui, se necessário
        }

        private void DesativarModoReal()
        {
            // Desconecta da porta serial caso esteja conectada
            Button btnDesconectar = tabControl.TabPages[1].Controls.OfType<Button>().FirstOrDefault(b => b.Text == "Desconectar");

            if (btnDesconectar != null && !btnDesconectar.Enabled)
            {
                // Simula um clique no botão de desconectar para garantir que a conexão seja encerrada corretamente
                DesconectarDispositivo(btnDesconectar, EventArgs.Empty);
            }
        }


    }
}
