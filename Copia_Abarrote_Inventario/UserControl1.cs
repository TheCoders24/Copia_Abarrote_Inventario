using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Copia_Abarrote_Inventario
{
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
        }

        private void UserControl1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

           
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        // Propiedad para acceder al TextBox
        public TextBox TxtMonto
        {
            get { return textBox1; }  // Retorna el control TextBox
        }

        // O puedes exponer solo el valor ingresado en el TextBox
        public decimal MontoRecibido
        {
            get
            {
                decimal monto;
                if (decimal.TryParse(textBox1.Text, out monto))
                {
                    return monto;
                }
                return 0; // Si el monto no es válido, se retorna 0
            }
        }
    }
}
