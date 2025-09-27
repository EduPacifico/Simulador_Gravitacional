using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;

namespace anker3
{
    public partial class Form1 : Form
    {
        private Universo universo;
        private Persistencia persistencia = new PersistenciaArquivoTxt();

        private Timer timer;
        private int qtdInteracoes;
        private double tempInteracao;
        private int interacaoAtual;

        private List<Color> corpoColors = new List<Color>();
        private Random rand = new Random();
        public Form1()
        {
            InitializeComponent();

            if(splitContainer1.Panel2 != null)
                splitContainer1.Panel2.Paint += Panel2_Paint;

            if (button1 != null) button1.Click += button1_Click; //Aleatório
            if (button2 != null) button2.Click += button2_Click; //Executar
            if (button4 != null) button4.Click += button4_Click; //Gravar

            timer = new Timer();
            timer.Interval = 80;
            timer.Tick += Timer_Tick;
        }

         //Botão Aleatório

        private void button1_Click(object sender, EventArgs e)
        {
        try
            {
                var culturaInvariavel = System.Globalization.CultureInfo.InvariantCulture;
                if(!int.TryParse(textBox1.Text.Trim(), System.Globalization.NumberStyles.Any, culturaInvariavel, out int qtdCorpos))
                {
                    MessageBox.Show("Erro: O valor de 'Qtd Corpos' não é um numnero válido.");
                    return;
                }
                if (!double.TryParse(textBox5.Text.Trim(), System.Globalization.NumberStyles.Any, culturaInvariavel, out double xMax))
                {
                    MessageBox.Show("Erro: O valor de 'X Máximo' não é um numnero válido.");
                    return;
                }
                if (!double.TryParse(textBox8.Text.Trim(), System.Globalization.NumberStyles.Any, culturaInvariavel, out double yMax))
                {
                    MessageBox.Show("Erro: O valor de 'Y Máximo' não é um numnero válido.");
                    return;
                }
                if (!double.TryParse(textBox4.Text.Trim(), System.Globalization.NumberStyles.Any, culturaInvariavel, out double massaMinima))
                {
                    MessageBox.Show("Erro: O valor de 'Massa Mínima' não é um numnero válido.");
                    return;
                }
                    if (!double.TryParse(textBox7.Text.Trim(), System.Globalization.NumberStyles.Any, culturaInvariavel, out double massaMaxima))
                    {
                        MessageBox.Show("Erro: O valor de 'Massa Máxima' não é um numnero válido.");
                        return;
                    }

                    universo = new Universo(qtdCorpos);
                    universo.gerarCorpos(qtdCorpos, xMax, yMax, massaMaxima, massaMinima);

                    if(universo.corpos == null)
                    {
                        MessageBox.Show("Erro: universo.corpos está nulo. Verifique o construtor de Universo.");
                        return;
                    }
                    corpoColors.Clear();
                    for(int i = 0; i < universo.Quantidade; i++)
                        {
                        if (universo.corpos[i] == null)
                        {
                            corpoColors.Add(Color.Gray);
                        }
                        else
                        {
                            corpoColors.Add(Color.FromArgb(rand.Next(60, 230), rand.Next(60, 230), rand.Next(60, 230)));
                        }
                    }

                    textBox2.Text = universo.corpos.Count(c => c != null).ToString();
                    interacaoAtual = 0;
                    splitContainer1.Panel2.Invalidate();
                    textBox3.Text = $"Gerados {universo.Quantidade} corpos.";
                    MessageBox.Show("Corpos gerados aleatoriamente!");
                    }
                    catch (Exception ex)
                    {
                    MessageBox.Show("Erro ao gerar corpos: " + ex.Message);
                    }
           
                }
            //Botão Executar
            private void button2_Click(object sender, EventArgs e)
            {
                if(universo == null)
                {
                    MessageBox.Show("Primeirp gere os corpos (Aleatório).");
                    return;
                }
                try
                {
                    var culturaInvariavel = System.Globalization.CultureInfo.InvariantCulture;
                    if (!int.TryParse(textBox9.Text.Trim(), System.Globalization.NumberStyles.Any, culturaInvariavel, out qtdInteracoes))
                    {
                        MessageBox.Show("Erro: O valor de 'Num Interações' não é um numnero válido.");
                        return;
                    }
                    if (!double.TryParse(textBox6.Text.Trim(), System.Globalization.NumberStyles.Any, culturaInvariavel, out tempInteracao))
                    {
                        MessageBox.Show("Erro: O valor de 'Temp Interações' não é um numnero válido.");
                        return;
                    }

                    interacaoAtual = 0;
                    timer.Start();
                    textBox3.Text = "Simulação iniciada.";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao iniciar simulação: " + ex.Message );
                }
            }
            private void Timer_Tick(object sender, EventArgs e)
            {
                if(universo == null)
                {
                    timer.Stop();
                    return;
                }
            if (interacaoAtual < qtdInteracoes)
            {
                universo.CalcularInteracao(tempInteracao, universo.Quantidade);
                interacaoAtual++;
                splitContainer1.Panel2?.Invalidate();
                textBox3.Text = $"Interação {interacaoAtual}/{qtdInteracoes}";
            }
            else
            {
                timer.Stop();
                textBox3.Text = "Simulação concluida.";
                MessageBox.Show("Simulação concluida!");
            }
        }
        //Botão Gravar
        private void button4_Click(object sender, EventArgs e)
        {
            if(universo == null)
            {
                MessageBox.Show("Nunhum universo para salvar.");
                return;
            }
            try
            {
                qtdInteracoes = int.Parse(textBox9.Text);
                tempInteracao = double.Parse(textBox6.Text);

                using(SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "Text file (*.txt)|*.txt|All files (*.*)|*.*";
                    sfd.FileName = "simulacao.txt";
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        persistencia.Salvar(sfd.FileName, universo, qtdInteracoes, tempInteracao);
                        textBox3.Text = "Arquivo salvo: " + sfd.FileName;
                        MessageBox.Show("Simulação salca com sucesso!");
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Erro ao salvar: " + ex.Message);
            }
        }
        //Desenho no painel
        private void Panel2_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.Clear(Color.Black);

            if (universo == null || universo.corpos == null) return;

            double minX = double.MaxValue, minY = double.MaxValue, maxX = double.MinValue, maxY = double.MinValue;
            bool any = false;
            foreach(var c in universo.corpos)
            {
                if(c == null) continue;
                any = true;
                if(c.PosX < minX) minX = c.PosX;
                if(c.PosY < minY) minY = c.PosY;
                if(c.PosX > maxX) maxX = c.PosX;
                if(c.PosX > maxY) maxY = c.PosX;
            }
            if (!any) return;

            double widthRange = maxX - minX;
            double heightRange = maxY - minY;
            if (widthRange == 0) widthRange = 1;
            if (heightRange == 0) heightRange = 1;

            double margin = 20;
            double scaleX = (splitContainer1.Panel2.Width - margin * 2) / widthRange;
            double scaleY = (splitContainer1.Panel2.Height - margin * 2) / heightRange;
            double scale = Math.Min(scaleX, scaleY);

            //desenha cada corpo na sua posição escalada
            for(int i = 0; i < universo.Quantidade; i++)
            {
                var corpo = universo.corpos[i];
                if(corpo == null) continue;

                float x = (float)((corpo.PosX - minX) * scale + margin);
                float y = (float)((corpo.PosY - minY) * scale + margin);

                float r = Math.Max(3f, (float)(corpo.Raio * scale));
                Color color = (i < corpoColors.Count) ? corpoColors[i] : Color.White;

                using(Brush b = new SolidBrush(color))
                {
                    g.FillEllipse(b, x - r, y - r, r * 2, r * 2);
                }
            }
        }

    }
}

