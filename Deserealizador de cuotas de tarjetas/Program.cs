using Deserealizador_de_cuotas_de_tarjetas.Modelos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Deserealizador_de_cuotas_de_tarjetas
{
    class Program
    {
        static void Main(string[] args)
        {
            //cambiar la ruta depende querramos el archivo
            string rutaArchivo = "../../../txt/Plan-cuotas.txt"; //en este guardamos el texto
            string rutaArchivoResultante = "../../../txt/POST-Plan-Cuotas.txt"; //este archivo se crea con lo cargado en el anterior texto
            string resultadoFS = GetCuotas(rutaArchivo);
            var cuotas = CleanString(resultadoFS);
            var resumenListo = BuildCuotas(cuotas);
            var CuotasGrabar = ObtenerResumenes(resumenListo);
            bool operacionExitosa = GrabarArchivo(CuotasGrabar, rutaArchivoResultante);

            string respuesta = operacionExitosa ? "El archivo se genero correctamente" : "ocurrio un error al grabar el archivo";
            Console.WriteLine(respuesta);
        }

        /// <summary>
        /// Crear el modelo de cuotas con el array de string obtenido desde el archivo de entrada
        /// </summary>
        /// <param name="cuotas"></param>
        /// <returns></returns>
        public static List<Cuota> BuildCuotas(string[] cuotas)
        {
            List<Cuota> resumen = new List<Cuota>();
            cuotas.ToList().ForEach(cuota =>
            {
                var cuotaclean = cuota.Split(" ");
                Cuota newcuota = new Cuota(cuotaclean[0], cuotaclean[1], cuotaclean[2], cuotaclean[3], cuotaclean[4]);
                resumen.Add(newcuota);
            });
            return resumen;
        }

        /// <summary>
        /// Obteiene el mes actual de tu resumen haciendo una validacion previa
        /// </summary>
        /// <param name="tipoMes"></param>
        /// <param name="mes"></param>
        /// <returns></returns>
        public static TipoMes ObtenerMes(TipoMes tipoMes, int mes)
        {
            int ValorMes = (int)tipoMes;
            if ((ValorMes + mes) > 12)
            {
                int mesActual = (ValorMes + mes) - 12;
                return (TipoMes)mesActual;
            }
            else
            {
                int mesActual = ValorMes + mes;
                return (TipoMes)mesActual;

            }
        }

        /// <summary>
        /// Metodo que crear el string a grabar en el archivo txt, recibiendo el modelo de resumen
        /// </summary>
        /// <param name="resumen"></param>
        /// <returns></returns>
        public static StringBuilder ObtenerResumenes(List<Cuota> resumen)
        {
            StringBuilder registro = new StringBuilder();
            int cuotaMayorPlazo = resumen.Max(x => x.CuotaPendiente);
            int indice = 0;
            TipoMes mesActual = (TipoMes)DateTime.Now.Month;

            while (cuotaMayorPlazo > 0)
            {
                MostrarCuotasMensuales(registro, resumen, mesActual, indice);

                cuotaMayorPlazo--;
                indice++;
            }

            return registro;

        }

        /// <summary>
        /// Graba las cuotas mensuales en el string builder
        /// </summary>
        /// <param name="registro"></param>
        /// <param name="cuotas"></param>
        /// <param name="mesActual"></param>
        /// <param name="indice"></param>
        /// <param name="marca"></param>
        public static void MostrarCuotasMensuales(StringBuilder registro, List<Cuota> cuotas, TipoMes mesActual, int indice)
        {
            List<decimal> TotalResumen = new List<decimal>();
            registro.AppendLine($"-------------------- Resumen de mes {ObtenerMes(mesActual, indice)} --------------------\n Nombre   |   Cuota actual    |   Cuota Total |   valor cuota | restante \n");

            cuotas.ForEach(cuota =>
            {
                int pocicionCuota = cuota.CuotaPendiente - indice;
                if (pocicionCuota > 0)
                {
                    TotalResumen.Add(cuota.ValorCuota);
                    string comentarioAddicional = pocicionCuota == 1 ? "ULTIMA CUOTA -->" : null;
                    string registroCuota = $" {comentarioAddicional} {cuota.FechaConsumo.ToShortDateString()}    |   {cuota.Nombre}  |   {pocicionCuota} |   {cuota.CuotaTotal}  |   {cuota.ValorCuota}  |   {pocicionCuota * cuota.ValorCuota} ";
                    registro.AppendLine(registroCuota);

                }
            });
            registro.AppendLine("\n");
            registro.AppendLine($"Total mensual: {TotalResumen.Sum()}");
        }

        /// <summary>
        /// Metodo que graba el archivo txt de tu resumen recibe el string
        /// </summary>
        /// <param name="stringBuilder"></param>
        /// <returns></returns>
        public static bool GrabarArchivo(StringBuilder stringBuilder,string ruta)
        {
            try
            {
                using (StreamWriter Archivo = new StreamWriter(ruta, true, Encoding.UTF8))
                {
                    Archivo.WriteLine(stringBuilder.ToString());
                }
                return true;
            }
            catch
            {
                return false;

            }
        }

        /// <summary>
        /// Pasamos el string obtenido del archivo y lo preparamos para ser mapeado con el modelo
        /// </summary>
        /// <param name="cuotas"></param>
        /// <returns></returns>
        public static string[] CleanString(string cuotas)
        {
            string cuotasClean = cuotas.Trim();
            var cuotasConIones = cuotasClean.Replace("\t", "-");
            string cuotasConEspacio = cuotasConIones.Replace("-", " ");
            string[] cuotasIterables = cuotasConEspacio.Split('\n');
            return cuotasIterables;
        }

        /// <summary>
        /// Metodo que se encarga de obtener la informacion del archivo solicitado
        /// </summary>
        /// <param name="ruta"></param>
        /// <returns></returns>
        public static string GetCuotas(string ruta)
        {
            string cuotas = string.Empty;
            try
            {
                cuotas = File.ReadAllText(ruta);

            }
            catch (Exception e)
            {
                Console.WriteLine("verifique la ruta del archivo solicitado");
                Console.WriteLine($"Error {e.Message}");
                throw;
            }
            return cuotas;
        }
    }
}