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
    public partial class MenuPrincipal : Form
    {
        public MenuPrincipal()
        {
            InitializeComponent();
        }

        private void btnGrafica_Click(object sender, EventArgs e)
        {
            Form1 graficos = new Form1();
            graficos.ShowDialog();
        }

        private void btnproductos_Click(object sender, EventArgs e)
        {
            Productos productos = new Productos();
            productos.ShowDialog();
        }

        private void btnproveedores_Click(object sender, EventArgs e)
        {
            Proveedores proveedores = new Proveedores();
            proveedores.ShowDialog();
        }

        private void btnclientes_Click(object sender, EventArgs e)
        {
            Clientes clientes = new Clientes();
            clientes.ShowDialog();
        }

        private void btnInventario_Click(object sender, EventArgs e)
        {
            Inventario inventario = new Inventario();
            inventario.ShowDialog();
        }

        private void btnDetallesIvnetario_Click(object sender, EventArgs e)
        {
            DetalleInventario detalleInventario = new DetalleInventario();
            detalleInventario.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ReporteForms reporteForms = new ReporteForms();
            reporteForms.ShowDialog();
        }

        private void btnsaldo_Click(object sender, EventArgs e)
        {
            Saldos saldos = new Saldos();
            saldos.ShowDialog();
        }
    }
}
