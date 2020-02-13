using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdminFrutas
{
    public partial class Form1 : Form
    {
        ArrayList nombres=new ArrayList();
        ArrayList precios = new ArrayList();
        double p_total;
        string codigo="frutas";

        public Form1()
        {
            /*nombres = new String[]{ "chirimoya", "ciruela", "fresa", "kiwi", "mandarina", "melocoton", "melon",
                "naranja", "nectarina", "papaya", "peras", "piña", 
                "platanos", "pomelos", "prunus", "sandias","uvas" };

            precios = new String[]{ "1,25", "2,75", "3,25", "1,50", "2,75", "3,25", "1",
            "2", "3", "1", "2", "3", "1", "2", "3", "1", "2"}; */

            
            InitializeComponent();

            
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            cargarPrecios();
            cargarBotones();

        }
        private void cargarPrecios()
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                int con = 0;
                nombres.Clear();
                precios.Clear();
                String nombreFichero = openFileDialog1.FileName.ToString();
                System.IO.StreamReader sr = new System.IO.StreamReader(nombreFichero);
                while (sr.Peek() != -1)
                {
                    con++;
                    String cadena = sr.ReadLine();
                    String[] tramos = cadena.Split(':');
                    nombres.Add(tramos[0]);
                    precios.Add(tramos[1]);
                    //MessageBox.Show("tramos-" + tramos[0] + "," + tramos[1] + "\nNombre y precio" + nombres[con] + "," + precios[con]);

                }
                sr.Close();
            }
        }

        private void cargarBotones()
        {
            int cantidadBotones = nombres.Count;
            Button botonX;
            TextBox txtX;
            for(int i = 0; i < cantidadBotones; i++)
            {
                botonX = new Button();
                txtX = new TextBox();
                txtX.Enabled = false;
                txtX.Text = precios[i].ToString();

                botonX.Tag = i;
                
                botonX.BackgroundImage = Image.FromFile(@"imagenes\" + nombres[i].ToString()+ ".PNG");
                botonX.BackgroundImageLayout = ImageLayout.None;
                botonX.Height=90;
                botonX.Width = 90;
                
                
                botonX.Click += new EventHandler(botonClick);

                flp1.Controls.Add(botonX);
                flp1.Controls.Add(txtX);
            }
        }
        private void botonClick(Object sender, EventArgs e)
        {
            Button b = (Button)sender;
            String[] tupla ;

            if (sender.GetType() == typeof(Button))
            {try
                {


                    String cadena = Interaction.InputBox("Cantidad en kg", nombres[Convert.ToInt16(b.Tag)].ToString());
                    tupla = new String[4]{
                    nombres[Convert.ToInt16(b.Tag)].ToString(),
                    precios[Convert.ToInt16(b.Tag)].ToString(),
                    cadena,
                   (Convert.ToDouble(cadena)*Convert.ToDouble(precios[Convert.ToInt16(b.Tag)])).ToString()};

                    dgv1.Rows.Add(tupla);
                    p_total += Convert.ToDouble(cadena) * Convert.ToDouble(precios[Convert.ToInt16(b.Tag)]);
                    total.Text = Math.Round(p_total, 2) + " €";
                }
                catch (Exception)
                {
                    MessageBox.Show("ERROR CAZURRO", "title", MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
        }

        private void btnAdmin_Click(object sender, EventArgs e)
        {
            txtPass.Visible = true;
            btnMod.Visible = true;
            txtPass.Focus();
        }

        private void btnNewClient_Click(object sender, EventArgs e)
        {
            p_total = 0;
            total.Text = "0";
            dgv1.Rows.Clear();

        }

        private void txtPass_TextChanged(object sender, EventArgs e)
        { 
            TextBox t = (TextBox)sender;
            if (t.Text.Equals(codigo)){
                foreach(Control ctrl in flp1.Controls)
                {   
                    if (ctrl.GetType() == typeof(TextBox))
                    {
                        TextBox txt = (TextBox)ctrl;
                        txt.Enabled = true;
                    }
                    else
                    {
                        Button b = (Button)ctrl;
                        b.Enabled = false;
                    }
                }
            }

        }

        private void btnMod_Click(object sender, EventArgs e)
        {
            int index = -1;
            foreach (Control ctrl in flp1.Controls)
            {
                if (ctrl.GetType() == typeof(TextBox))
                {
                    index++;
                    TextBox txt = (TextBox)ctrl;
                    txt.Enabled = false;
                    precios[index] = txt.Text;

                }
                else
                {
                    Button b = (Button)ctrl;
                    b.Enabled = true;
                }
            }

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                String file = openFileDialog1.FileName.ToString();
                actualizarFicheroPrecios(file);
            }

            txtPass.Visible = false;
            btnMod.Visible = false;
        }

        private void actualizarFicheroPrecios(String file)
        {
            System.IO.StreamWriter sw = new System.IO.StreamWriter(file);
            for (int i = 0; i < precios.Count; i++)
            {
                String cadena = nombres[i] + ":" + precios[i];
                sw.WriteLine(cadena);
            }
            sw.Close();
        }

        private void dgv1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            p_total -= Convert.ToDouble(dgv1.CurrentRow.Cells[3].Value);
            total.Text =Math.Round(p_total,2)+" €";

            dgv1.Rows.RemoveAt(dgv1.CurrentRow.Index);
        }

        private void btnTicket_Click(object sender, EventArgs e)
        {
            if (p_total > 0)
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    String cad = "";
                    String file = openFileDialog1.FileName.ToString();
                    System.IO.StreamWriter sw = new System.IO.StreamWriter(file);
                    for (int i = 0; i < dgv1.RowCount; i++)
                    {
                        cad = "Produto: " + dgv1.Rows[i].Cells[0].Value + "\tPrecio: " + dgv1.Rows[i].Cells[1].Value
                             + "€\tCantidad: " + dgv1.Rows[i].Cells[2].Value + "Kg\tSubtotal: " + dgv1.Rows[i].Cells[3].Value + "€";
                        sw.WriteLine(cad);

                    }
                    cad= "Importe Total: " + Math.Round(p_total, 2) + "€";
                    sw.WriteLine(cad);
                    sw.Close();

                    p_total = 0;
                    total.Text = "0";
                    dgv1.Rows.Clear();
                }
                else MessageBox.Show("ERROR ocn el fichero");
            }
            else MessageBox.Show("NO has comprado NADA");
        }
    }
}
