using CapaNegocio;
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

namespace Copia_Abarrote_Inventario
{
    public partial class Form1 : Form
    {
        public readonly NGraficas _reportService;
        private CheckBox checkBoxVentasPorMes;
        private CheckBox checkBoxVentasPorCliente;
        private CheckBox checkBoxProductosVendidos;
        private CheckBox checkBoxIngresoDiario;
        public Form1()
        {
            InitializeComponent();
            ConfigurarControles();
            _reportService = new NGraficas();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        //private void ConfigurarControles()
        //{
        //    // Inicializar los CheckBox dinámicamente
        //    checkBoxVentasPorMes = new CheckBox
        //    {
        //        Text = "Ventas por Mes",
        //        Location = new System.Drawing.Point(10, 10),
        //        Checked = false
        //    };
        //    checkBoxVentasPorMes.CheckedChanged += async (s, e) => await ActualizarGraficoAsync();

        //    checkBoxVentasPorCliente = new CheckBox
        //    {
        //        Text = "Ventas por Cliente",
        //        Location = new System.Drawing.Point(10, 40),
        //        Checked = false
        //    };
        //    checkBoxVentasPorCliente.CheckedChanged += async (s, e) => await ActualizarGraficoAsync();

        //    checkBoxProductosVendidos = new CheckBox
        //    {
        //        Text = "Productos Vendidos",
        //        Location = new System.Drawing.Point(10, 70),
        //        Checked = false
        //    };
        //    checkBoxProductosVendidos.CheckedChanged += async (s, e) => await ActualizarGraficoAsync();

        //    checkBoxIngresoDiario = new CheckBox
        //    {
        //        Text = "Ingreso Diario",
        //        Location = new System.Drawing.Point(10, 100),
        //        Checked = false
        //    };
        //    checkBoxIngresoDiario.CheckedChanged += async (s, e) => await ActualizarGraficoAsync();

        //    // Agregar los CheckBox al formulario
        //    this.Controls.Add(checkBoxVentasPorMes);
        //    this.Controls.Add(checkBoxVentasPorCliente);
        //    this.Controls.Add(checkBoxProductosVendidos);
        //    this.Controls.Add(checkBoxIngresoDiario);
        //}
        //private async Task ActualizarGraficoAsync()
        //{
        //    chart1.Series.Clear();

        //    if (checkBoxVentasPorMes.Checked)
        //    {
        //        var datos = await _reportService.ObtenerVentasPorMesAsync();
        //        AgregarDatosAlGrafico("Ventas Totales Por Mes", datos);
        //    }
        //    if (checkBoxVentasPorCliente.Checked)
        //    {
        //        var datos = await _reportService.ObtenerVentasPorClienteAsync();
        //        AgregarDatosAlGrafico("Ventas Por Cliente", datos);
        //    }
        //    if (checkBoxProductosVendidos.Checked)
        //    {
        //        var datos = await _reportService.ObtenerProductosVendidosAsync();
        //        AgregarDatosAlGrafico("Productos Vendidos", datos);
        //    }
        //    if (checkBoxIngresoDiario.Checked)
        //    {
        //        var datos = await _reportService.ObtenerIngresoDiarioAsync();
        //        AgregarDatosAlGrafico("Ingreso Diario", datos);
        //    }
        //}

        //private void AgregarDatosAlGrafico(string serieNombre, List<(object XValue, object YValue)> datos)
        //{
        //    Series series = new Series(serieNombre)
        //    {

        //        ChartType = SeriesChartType.Column

        //    };
        //    foreach (var dato in datos)
        //    {
        //        series.Points.AddXY(dato.XValue, dato.YValue);
        //    }
        //    chart1.Series.Add(series);
        //}

        private void ConfigurarControles()
        {
            // Inicializar los CheckBox dinámicamente
            checkBoxVentasPorMes = new CheckBox
            {
                Text = "Ventas por Mes",
                Location = new System.Drawing.Point(10, 10),
                Checked = false
            };
            checkBoxVentasPorMes.CheckedChanged += async (s, e) => await ActualizarGraficoAsync();

            checkBoxVentasPorCliente = new CheckBox
            {
                Text = "Ventas por Cliente",
                Location = new System.Drawing.Point(10, 40),
                Checked = false
            };
            checkBoxVentasPorCliente.CheckedChanged += async (s, e) => await ActualizarGraficoAsync();

            checkBoxProductosVendidos = new CheckBox
            {
                Text = "Productos Vendidos",
                Location = new System.Drawing.Point(10, 70),
                Checked = false
            };
            checkBoxProductosVendidos.CheckedChanged += async (s, e) => await ActualizarGraficoAsync();

            checkBoxIngresoDiario = new CheckBox
            {
                Text = "Ingreso Diario",
                Location = new System.Drawing.Point(10, 100),
                Checked = false
            };
            checkBoxIngresoDiario.CheckedChanged += async (s, e) => await ActualizarGraficoAsync();

            // Agregar los CheckBox al formulario
            this.Controls.Add(checkBoxVentasPorMes);
            this.Controls.Add(checkBoxVentasPorCliente);
            this.Controls.Add(checkBoxProductosVendidos);
            this.Controls.Add(checkBoxIngresoDiario);
        }

        private async Task ActualizarGraficoAsync()
        {
            chart1.Series.Clear();

            if (checkBoxVentasPorMes.Checked)
            {
                var datos = await _reportService.ObtenerVentasPorMesAsync();
                AgregarDatosAlGrafico("Ventas Totales Por Mes", datos, SeriesChartType.Column);
            }
            if (checkBoxVentasPorCliente.Checked)
            {
                var datos = await _reportService.ObtenerVentasPorClienteAsync();
                AgregarDatosAlGrafico("Ventas Por Cliente", datos, SeriesChartType.Pie);
            }
            if (checkBoxProductosVendidos.Checked)
            {
                var datos = await _reportService.ObtenerProductosVendidosAsync();
                AgregarDatosAlGrafico("Productos Vendidos", datos, SeriesChartType.Column);
            }
            //if (checkBoxIngresoDiario.Checked)
            //{
            //    var datos = await _reportService.ObtenerIngresoDiarioAsync();
            //    AgregarDatosAlGrafico("Ingreso Diario", datos, SeriesChartType.Point);
            //}
            if (checkBoxIngresoDiario.Checked)
            {
                // Obtener las fechas seleccionadas de los DateTimePicker
                DateTime fechaInicio = dateTimePickerinicio.Value;
                DateTime fechaFin = dateTimePickerfinal.Value;

                // Pasar las fechas al método de servicio
                var datos = await _reportService.ObtenerIngresoDiarioAsync(fechaInicio, fechaFin);

                // Llamar al método para agregar los datos al gráfico con el tipo "Point"
                AgregarDatosAlGrafico("Ingreso Diario", datos, SeriesChartType.Point);
            }

        }

        private void AgregarDatosAlGrafico(string serieNombre, List<(object XValue, object YValue)> datos, SeriesChartType chartType)
        {
            Series series = new Series(serieNombre)
            {
                ChartType = chartType
            };

            foreach (var dato in datos)
            {
                series.Points.AddXY(dato.XValue, dato.YValue);
            }

            chart1.Series.Add(series);
        }


    }
}
