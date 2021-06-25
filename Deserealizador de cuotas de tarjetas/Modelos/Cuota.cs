using System;
using System.Collections.Generic;
using System.Text;

namespace Deserealizador_de_cuotas_de_tarjetas.Modelos
{
    public class Cuota
    {
        /// <summary>
        /// Atributo que guarda la fecha donde se realizo el consumo
        /// </summary>
        public DateTime FechaConsumo { get; set; }
        /// <summary>
        /// Elemento que contiene el elemento nombre de la cuota o movimiento
        /// </summary>
        public string Nombre { get; set; }
        /// <summary>
        /// Elemento que contiene el numero de cuotas pendientes que debes 
        /// </summary>
        public int CuotaPendiente { get; set; }
        /// <summary>
        /// Elemento que contiene el numero total de cuotas que tiene el movimeinto
        /// </summary>
        public int CuotaTotal { get; set; }
        /// <summary>
        /// Elemento que contiene el valor decimal del valor de la cuota
        /// </summary>
        public decimal ValorCuota { get; set; }
        /// <summary>
        /// Elemento que contiene el valor restante de la cuota
        /// </summary>
        public decimal TotalRestante { get; set; }

        /// <summary>
        /// constructor de la clase Cuota
        /// </summary>
        /// <param name="FechaConsumo"></param>
        /// <param name="Nombre"></param>
        /// <param name="CuotaPendiente"></param>
        /// <param name="CuotaTotal"></param>
        /// <param name="ValorCuota"></param>
        public Cuota(string FechaConsumo, string Nombre,string CuotaPendiente, string CuotaTotal, string ValorCuota)
        {
            this.FechaConsumo = Convert.ToDateTime(FechaConsumo);
            this.Nombre = Nombre;
            this.CuotaTotal = Convert.ToInt32(CuotaTotal);
            this.CuotaPendiente = Convert.ToInt32(CuotaPendiente);
            this.ValorCuota = Convert.ToDecimal(ValorCuota);
            this.TotalRestante = this.CuotaPendiente * this.ValorCuota;
        }

    }
    /// <summary>
    /// Enumerable de tipo mes  en el cual podemos castear el numero del mes con su respectivo nombre
    /// </summary>
    public enum TipoMes
    {
        Enero = 1,
        Febrero = 2,
        Marzo = 3,
        Abril = 4,
        Mayo = 5,
        Junio = 6,
        Julio = 7,
        Agosto = 8,
        Septiembre = 9,
        Octubre = 10,
        Noviembre = 11,
        Diciembre = 12
    }
}
