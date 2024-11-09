﻿using CapaDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class NGraficas
    {
        private readonly DGraficas _repository;
        public NGraficas()
        {
            _repository = new DGraficas();
        }
        public async Task<List<(object XValue, object YValue)>> ObtenerVentasPorMesAsync()
        {
            return await _repository.GetVentasPorMesAsync();
        }

        public async Task<List<(object XValue, object YValue)>> ObtenerVentasPorClienteAsync()
        {
            return await _repository.GetVentasPorClienteAsync();
        }

        public async Task<List<(object XValue, object YValue)>> ObtenerProductosVendidosAsync()
        {
            return await _repository.GetProductosVendidosAsync();
        }

        public async Task<List<(object XValue, object YValue)>> ObtenerIngresoDiarioAsync()
        {
            return await _repository.GetIngresoDiarioAsync();
        }
    }
}