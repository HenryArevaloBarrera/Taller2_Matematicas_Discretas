using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;

namespace Secesiones
{
    public partial class Form1 : Form
    {

        Random random = new Random();
        // Declarar listas a nivel de clase
        private List<double> list1 = new List<double>();
        private List<double> list2 = new List<double>();
        private List<double> list3 = new List<double>();
        private List<double> list4 = new List<double>();
        private List<List<double>> iteracionesList1 = new List<List<double>>();
        private List<List<double>> iteracionesList2 = new List<List<double>>();
        private List<List<double>> iteracionesList3 = new List<List<double>>();
        private List<List<double>> iteracionesList4 = new List<List<double>>();
        int Elementos = 0;
        private Stopwatch stopwatch; // Declarar el cronómetro
        private Stopwatch stopwach;

        public Form1()
        {
            InitializeComponent();
            stopwatch = new Stopwatch(); // Inicializar el cronómetro
        }
        /**
 * Descripción: Este método se ejecuta cuando el usuario hace clic en el botón `btnStart`. Se encarga de generar datos, medir tiempos de ordenamiento utilizando los algoritmos de Burbuja, Inserción y Selección, y luego registrar estos tiempos en un `DataGridView`. Posteriormente, genera un gráfico que muestra los tiempos de cada algoritmo a lo largo de varias iteraciones.
 * @param iteracion - El número de la iteración que se está procesando.
 * @param nombreLista - El nombre de la lista que se está ordenando.
 * @param listaOriginal - La lista original que se va a ordenar y sobre la que se medirán los tiempos de los algoritmos.
 * Versión 1.1
 * @return - Este método no retorna ningún valor. Los resultados se agregan al `DataGridView` y al gráfico.
 */
        private void btnStart_Click(object sender, EventArgs e)
        {
            // Limpiar las listas individuales
            list1.Clear();
            list2.Clear();
            list3.Clear();
            list4.Clear();

            // Limpiar las listas de iteraciones
            iteracionesList1.Clear();
            iteracionesList2.Clear();
            iteracionesList3.Clear();
            iteracionesList4.Clear();
            comboBoxIteraciones.Items.Clear();

            Elementos = int.Parse(numericUpDown1.Text);
            int Rango = 5;
            int Iteraciones = int.Parse(numericUpDown2.Text);
            comboBoxIteraciones.Items.Clear();


            if (data.Columns.Count == 0)
            {
                data.Columns.Add("Iteración", "Iteración");
                data.Columns.Add("Lista", "Lista");
                data.Columns.Add("TiempoBurbuja", "Tiempo Burbuja (ms)");
                data.Columns.Add("TiempoInserción", "Tiempo Inserción (ms)");
                data.Columns.Add("TiempoSelección", "Tiempo Selección (ms)");
            }

            // Limpiar el DataGridView y las listas
            data.Rows.Clear();
            progressBar1.Maximum = Iteraciones;
            progressBar1.Value = 0;
            // Generar datos y medir tiempos
            for (int i = 0; i < Iteraciones; i++)
            {
                var list1 = GenerarLista1(Elementos);
                var list2 = GenerarLista2(Elementos, Rango);
                var list3 = GenerarLista3(list1);
                var list4 = GenerarLista4(Elementos);

                iteracionesList1.Add(new List<double>(list1));
                iteracionesList2.Add(new List<double>(list2));
                iteracionesList3.Add(new List<double>(list3));
                iteracionesList4.Add(new List<double>(list4));

                // Registrar tiempos de ordenamiento para cada lista
                RegistrarTiemposEnTabla($"Iteración {i + 1}", "Lista 1", list1);
                RegistrarTiemposEnTabla($"Iteración {i + 1}", "Lista 2", list2);
                RegistrarTiemposEnTabla($"Iteración {i + 1}", "Lista 3", list3);
                RegistrarTiemposEnTabla($"Iteración {i + 1}", "Lista 4", list4);
                comboBoxIteraciones.Items.Add(i + 1);
                progressBar1.Value = i + 1;
                Application.DoEvents();
            }

            MessageBox.Show("Datos generados y tiempos medidos correctamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);

            chart2.Series.Clear();

            // Crear series para cada algoritmo
            Series serieBurbuja = new Series("Burbuja");
            Series serieInsercion = new Series("Inserción");
            Series serieSeleccion = new Series("Selección");

            // Configurar las series
            serieBurbuja.ChartType = SeriesChartType.Column;
            serieInsercion.ChartType = SeriesChartType.Column;
            serieSeleccion.ChartType = SeriesChartType.Column;

            // Agregar las series al gráfico
            chart2.Series.Add(serieBurbuja);
            chart2.Series.Add(serieInsercion);
            chart2.Series.Add(serieSeleccion);

            // Recorrer los datos del DataGridView
            foreach (DataGridViewRow row in data.Rows)
            {
                if (row.Cells["Iteracion"].Value != null)
                {
                    string iteracion = row.Cells["Iteracion"].Value.ToString();
                    double tiempoBurbuja = Convert.ToDouble(row.Cells["TiempoBurbuja"].Value);
                    double tiempoInsercion = Convert.ToDouble(row.Cells["TiempoInsercion"].Value);
                    double tiempoSeleccion = Convert.ToDouble(row.Cells["TiempoSeleccion"].Value);

                    // Agregar puntos a las series
                    serieBurbuja.Points.AddXY(iteracion, tiempoBurbuja);
                    serieInsercion.Points.AddXY(iteracion, tiempoInsercion);
                    serieSeleccion.Points.AddXY(iteracion, tiempoSeleccion);
                }
            }

            // Configurar el gráfico
            chart2.ChartAreas[0].AxisX.Title = "Iteración";
            chart2.ChartAreas[0].AxisY.Title = "Tiempo (ms)";
            chart2.ChartAreas[0].RecalculateAxesScale();

            MessageBox.Show("Gráfico generado correctamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);



        }



        /**
 * Descripción: Este método registra los tiempos de ejecución de tres algoritmos de ordenación (Burbuja, Inserción y Selección) sobre una lista original. Los tiempos de ejecución se miden utilizando el método `MedirTiempo` y luego se agregan a un `DataGridView` para su visualización.
 * @param iteracion - El número de la iteración que se está procesando.
 * @param nombreLista - El nombre de la lista que se está ordenando.
 * @param listaOriginal - La lista original que se va a ordenar y sobre la que se medirán los tiempos de los algoritmos.
 * Versión 1.1
 * @return - Este método no retorna ningún valor. Los resultados se agregan directamente al `DataGridView`.
 */
        private void RegistrarTiemposEnTabla(string iteracion, string nombreLista, List<double> listaOriginal)
        {
            List<double> listaBurbuja = new List<double>(listaOriginal);
            List<double> listaInsercion = new List<double>(listaOriginal);
            List<double> listaSeleccion = new List<double>(listaOriginal);

            long tiempoBurbuja = MedirTiempo(() => Buble(listaBurbuja));
            long tiempoInsercion = MedirTiempo(() => Insertion(listaInsercion));
            long tiempoSeleccion = MedirTiempo(() => Selection(listaSeleccion));

            // Agregar los datos al DataGridView
            data.Rows.Add(iteracion, nombreLista, tiempoBurbuja, tiempoInsercion, tiempoSeleccion);
        }

        /**
 * Descripción: Este método mide el tiempo de ejecución de un algoritmo pasado como parámetro en forma de acción (`Action`). Utiliza un `Stopwatch` para cronometrar el tiempo y devuelve el tiempo transcurrido en milisegundos.
 * @param algoritmo - La acción que representa el algoritmo cuya ejecución se desea medir.
 * Versión 1.1
 * @return - Retorna el tiempo que ha tardado en ejecutarse el algoritmo, en milisegundos.
 */
        private long MedirTiempo(Action algoritmo)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            algoritmo();
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds; // Tiempo en milisegundos
        }






        /**
 * Descripción: Este método genera una lista de números aleatorios. Cada número es generado de manera independiente utilizando el generador de números aleatorios `NextDouble()`, el cual devuelve un valor entre 0 y 1.
 * @param elementos - El número de elementos que se desean en la lista.
 * Versión 1.1
 * @return - Retorna una lista de números aleatorios generados de forma independiente.
 */
        List<double> GenerarLista1(int elementos)
        {
            List<double> lista = new List<double>();
            for (int j = 0; j < elementos; j++)
            {
                double random01 = random.NextDouble();
                lista.Add(random01);
            }
            return lista;
        }
        /**
 * Descripción: Este método genera una lista de números aleatorios distribuidos de manera uniforme dentro de un rango especificado. El rango se ajusta automáticamente si el número de elementos es menor que el rango. Los números aleatorios generados se distribuyen en intervalos de tamaño `1.0 / rango`.
 * @param elementos - El número de elementos que se desean en la lista.
 * @param rango - El número de intervalos en los que se dividirá el rango de valores.
 * Versión 1.1
 * @return - Retorna una lista de números aleatorios distribuidos uniformemente dentro del rango especificado.
 */
        List<double> GenerarLista2(int elementos, int rango)
        {
            if (elementos < rango)
            {

                rango = elementos;
                MessageBox.Show("El número de elementos es menor que el rango. El rango ha sido ajustado al número de elementos.");
            }
            List<double> lista2 = new List<double>();
            double intervalo = 1.0 / rango;

            for (int i = 0; i < rango; i++)
            {
                for (int j = 0; j < (elementos / rango); j++)
                {
                    double random02 = random.NextDouble();

                    random02 = random02 * intervalo + intervalo * i;

                    lista2.Add(random02);
                }
            }

            return lista2;
        }
        /**
 * Descripción: Este método genera una nueva lista basada en una lista original proporcionada como parámetro. La nueva lista es una copia de la lista original, pero ordenada en orden descendente.
 * @param listaOriginal - La lista de números que se utilizará para generar la nueva lista ordenada.
 * Versión 1.1
 * @return - Retorna una nueva lista ordenada en orden descendente.
 */
        List<double> GenerarLista3(List<double> listaOriginal)
        {
            List<double> lista3 = new List<double>(listaOriginal);
            lista3.Sort((a, b) => b.CompareTo(a));
            return lista3;
        }
        /**
 * Descripción: Este método genera una lista de números aleatorios. La lista tiene una cantidad de elementos especificada por el parámetro `elementos`. Cada décimo elemento de la lista es un número aleatorio entre 0 y 1, mientras que los demás elementos son iguales al último número aleatorio generado.
 * @param elementos - El número de elementos que se desean en la lista.
 * Versión 1.1
 * @return - Retorna la lista generada con números aleatorios.
 */
        List<double> GenerarLista4(int elementos)
        {
            List<double> lista4 = new List<double>();
            Random random = new Random();
            double random04 = 0.0;

            for (int j = 0; j < elementos; j++)
            {
                if (j % 10 == 0)
                {
                    random04 = random.NextDouble();
                }
                lista4.Add(random04);
            }

            return lista4;
        }

        /**
 * Descripción: Este método implementa el algoritmo de ordenamiento por inserción (Insertion Sort) sobre una lista de números. El algoritmo recorre la lista, toma un elemento y lo coloca en la posición correcta dentro de la parte ordenada de la lista. Además, mide el tiempo de ejecución utilizando un cronómetro.
 * @param list - La lista de números que será ordenada.
 * Versión 1.1
 * @return - Retorna la lista ordenada después de aplicar el algoritmo de ordenamiento por inserción.
 */
        List<double> Insertion(List<double> list)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            for (int i = 1; i < list.Count; i++)
            {
                double item = list[i];
                int j = i - 1;

                while (j >= 0 && list[j] > item)
                {
                    list[j + 1] = list[j];
                    j--;
                }

                list[j + 1] = item;
            }

            stopwatch.Stop();
            return list;
        }



        private void listRandom_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void chartRandom_Click(object sender, EventArgs e)
        {

        }
        /**
 * Descripción: Este método implementa el algoritmo de ordenamiento por burbuja (Bubble Sort) sobre una lista de números. El algoritmo recorre repetidamente la lista, comparando elementos adyacentes y los intercambia si están en el orden incorrecto. Además, mide el tiempo de ejecución utilizando un cronómetro.
 * @param list - La lista de números que será ordenada.
 * Versión 1.1
 * @return - Retorna la lista ordenada después de aplicar el algoritmo de ordenamiento por burbuja.
 */
        List<double> Buble(List<double> list)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < list.Count - 1 - i; j++)
                {
                    if (list[j] > list[j + 1])
                    {
                        double temp = list[j];
                        list[j] = list[j + 1];
                        list[j + 1] = temp;
                    }
                }
            }

            stopwatch.Stop();
            return list;
        }
        /**
 * Descripción: Este método implementa el algoritmo de ordenamiento por selección (Selection Sort) sobre una lista de números. El algoritmo recorre la lista, encuentra el valor mínimo en cada iteración y lo coloca en la posición correcta. Además, mide el tiempo de ejecución utilizando un cronómetro.
 * @param list - La lista de números que será ordenada.
 * Versión 1.1
 * @return - Retorna la lista ordenada después de aplicar el algoritmo de ordenamiento por selección.
 */
        List<double> Selection(List<double> list)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            for (int i = 0; i < list.Count - 1; i++)
            {
                int minIndex = i;
                for (int j = i + 1; j < list.Count; j++)
                {
                    if (list[j] < list[minIndex])
                    {
                        minIndex = j;
                    }
                }
                if (minIndex != i)
                {
                    double temp = list[i];
                    list[i] = list[minIndex];
                    list[minIndex] = temp;
                }
            }

            stopwatch.Stop();
            return list;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
        /**
 * Descripción: Este método maneja el evento de clic en un botón. Obtiene el índice de la iteración seleccionada en el comboBox, limpia las series de los gráficos, y luego muestra los datos correspondientes en cada gráfico basado en la iteración seleccionada. Si el índice no es válido, muestra un mensaje de error.
 * @param sender - El objeto que genera el evento (en este caso, el botón que fue clickeado).
 * @param e - Los datos del evento que se generan al hacer clic en el botón.
 * Versión 1.1
 * @return - No retorna ningún valor, ya que es un manejador de evento.
 */
        private void button3_Click(object sender, EventArgs e)
        {
            // Obtener el índice de la iteración seleccionada
            int iteracionSeleccionada = comboBoxIteraciones.SelectedIndex;

            // Limpiar todas las series del gráfico
            chart1.Series.Clear();
            chart3.Series.Clear();
            chart4.Series.Clear();
            chart5.Series.Clear();

            // Comprobar que el índice de iteración es válido
            if (iteracionSeleccionada >= 0 && iteracionSeleccionada < iteracionesList1.Count)
            {
                // Mostrar los datos en el gráfico para cada lista
                MostrarDatosEnGrafico(iteracionesList1[iteracionSeleccionada], "Lista 1", 1);
                MostrarDatosEnGrafico(iteracionesList2[iteracionSeleccionada], "Lista 2", 2);
                MostrarDatosEnGrafico(iteracionesList3[iteracionSeleccionada], "Lista 3", 3);
                MostrarDatosEnGrafico(iteracionesList4[iteracionSeleccionada], "Lista 4", 4);
            }
            else
            {
                // Si el índice no es válido, puedes mostrar un mensaje de error o manejarlo como desees
                MessageBox.Show("Por favor, seleccione una iteración válida.");
            }
        }
        /**
 * Descripción: Este método agrega una serie de datos a un gráfico específico según el valor de 'num'. Además, muestra los datos en un `ListBox` correspondiente y agrega los puntos a la serie en el gráfico con una representación de línea.
 * @param datos - La lista de datos que se desea mostrar en el gráfico y el ListBox.
 * @param nombreSerie - El nombre que se asignará a la serie dentro del gráfico.
 * @param num - El número del gráfico y ListBox en el cual se deben agregar los datos (1 para chart1 y listBox1, 2 para chart3 y listBox2, etc.).
 * Versión 1.1
 * @return - No retorna ningún valor, ya que solo actualiza los gráficos y los `ListBox`.
 */
        private void MostrarDatosEnGrafico(List<double> datos, string nombreSerie, int num)
        {
            // Crear la nueva serie
            var serie = new Series(nombreSerie)
            {
                ChartType = SeriesChartType.Line
            };

            // Agregar los puntos a la serie
            for (int i = 0; i < datos.Count; i++)
            {
                serie.Points.AddXY($"Elemento {i + 1}", datos[i]);
            }

            // Seleccionar el gráfico correspondiente según el valor de 'num'
            switch (num)
            {
                case 1:
                    chart1.Series.Add(serie);
                    // Limpiar y agregar los datos de la serie al listBox1
                    listBox1.Items.Clear();
                    foreach (var item in datos)
                    {
                        listBox1.Items.Add(item.ToString()); // Mostrar solo los datos
                    }
                    break;
                case 2:
                    chart3.Series.Add(serie);
                    // Limpiar y agregar los datos de la serie al listBox2
                    listBox2.Items.Clear();
                    foreach (var item in datos)
                    {
                        listBox2.Items.Add(item.ToString()); // Mostrar solo los datos
                    }
                    break;
                case 3:
                    chart4.Series.Add(serie);
                    // Limpiar y agregar los datos de la serie al listBox3
                    listBox3.Items.Clear();
                    foreach (var item in datos)
                    {
                        listBox3.Items.Add(item.ToString()); // Mostrar solo los datos
                    }
                    break;
                case 4:
                    chart5.Series.Add(serie);
                    // Limpiar y agregar los datos de la serie al listBox4
                    listBox4.Items.Clear();
                    foreach (var item in datos)
                    {
                        listBox4.Items.Add(item.ToString()); // Mostrar solo los datos
                    }
                    break;
                default:
                    // Opción para manejar un caso en el que el valor de 'num' no sea válido
                    MessageBox.Show("Número de gráfico no válido.");
                    break;
            }
        }


        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void label22_Click(object sender, EventArgs e)
        {

        }
        /**
 * Descripción: Este método maneja el evento de clic en un botón. Realiza las siguientes acciones: limpia el gráfico, agrega nuevas series, genera un número aleatorio, realiza la búsqueda secuencial y exponencial en diversas listas, y agrega los resultados tanto a una tabla como a un gráfico. También configura los ejes y las etiquetas del gráfico.
 * @param sender - El objeto que genera el evento (en este caso, el botón que fue clickeado).
 * @param e - Los datos del evento que se generan al hacer clic en el botón.
 * Versión 1.1
 * @return - No retorna ningún valor, ya que es un manejador de evento.
 */
        private void button1_Click(object sender, EventArgs e)
        {
            chart6.Series.Clear(); // Limpiar el gráfico antes de agregar nuevas series

            // Agregar las series
            chart6.Series.Add("Serie 1 - Tiempo Secuencial");
            chart6.Series.Add("Serie 2 - Tiempo Exponencial");
            chart6.Series.Add("Serie 3 - Tiempo Secuencial Extendida");
            chart6.Series.Add("Serie 4 - Tiempo Exponencial Extendida");

            // Configurar el tipo de gráfico de cada serie a columnas
            foreach (var serie in chart6.Series)
            {
                serie.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Column;
                serie.BorderWidth = 1;  // Establecer un borde para las columnas
            }

            // Crear una instancia de la clase Random
            Random random = new Random();
            double randomNumber = Math.Round(random.NextDouble(), 1); // Generar un número aleatorio entre 0 y 1 con una sola cifra decimal

            int Iteraciones = iteracionesList1.Count;
            dataGridView1.Rows.Clear();

            double maxYValue = 0; // Variable para almacenar el valor máximo

            for (int i = 0; i < Iteraciones; i++)
            {
                // Procesar todas las listas por iteración
                var listas = new List<List<double>> { iteracionesList1[i], iteracionesList2[i], iteracionesList3[i], iteracionesList4[i] };

                for (int j = 0; j < listas.Count; j++)
                {
                    var listaActual = listas[j];

                    var resultado = BuscarSecuencial(listaActual, randomNumber);
                    var resultado2 = BuscarExponencial(listaActual, randomNumber);

                    // Añadir datos a la tabla
                    dataGridView1.Rows.Add(
                        $"Iteración {i + 1}",
                        $"Lista {j + 1}",
                        resultado.ocurrencias,
                        resultado.tiempoPrimerValor,
                        resultado.tiempoTotal,
                        resultado2.ocurrencias,
                        resultado2.tiempoPrimerValor,
                        resultado2.tiempoTotal
                    );

                    // Convertir valores a milisegundos
                    double tiempoPrimerValorSecuencial = resultado.tiempoPrimerValor;
                    double tiempoTotalSecuencial = resultado.tiempoTotal;

                    double tiempoPrimerValorExponencial = resultado2.tiempoPrimerValor;
                    double tiempoTotalExponencial = resultado2.tiempoTotal;

                    // Añadir datos al gráfico
                    chart6.Series["Serie 1 - Tiempo Secuencial"].Points.AddXY(i * 4 + j + 1, tiempoPrimerValorSecuencial);
                    chart6.Series["Serie 2 - Tiempo Exponencial"].Points.AddXY(i * 4 + j + 1, tiempoTotalSecuencial);
                    chart6.Series["Serie 3 - Tiempo Secuencial Extendida"].Points.AddXY(i * 4 + j + 1, tiempoPrimerValorExponencial);
                    chart6.Series["Serie 4 - Tiempo Exponencial Extendida"].Points.AddXY(i * 4 + j + 1, tiempoTotalExponencial);

                    // Actualizar el valor máximo para el eje Y
                    maxYValue = Math.Max(maxYValue, Math.Max(tiempoPrimerValorSecuencial, Math.Max(tiempoTotalSecuencial, Math.Max(tiempoPrimerValorExponencial, tiempoTotalExponencial))));
                }
            }

            // Configurar las etiquetas del eje X para que reflejen las iteraciones y listas
            var chartArea = chart6.ChartAreas[0];
            chartArea.AxisX.LabelStyle.Interval = 1;
            chartArea.AxisX.Title = "Iteraciones (por Lista)";
            chartArea.AxisY.Title = "Tiempo (ms)";

            // Ajustar el espaciado entre las columnas para que no se sobrepasen
            chartArea.AxisX.Interval = 1;  // Cada unidad en el eje X será una columna
            chartArea.AxisX.Minimum = 0;   // Asegurarse de que el eje X comienza en 0
            chartArea.AxisX.Maximum = Iteraciones * 4; // Ajustar el límite superior del eje X

            // Establecer el límite máximo del eje Y con un margen adicional
            chartArea.AxisY.Maximum = maxYValue * 1.1; // Asegurarse de que las columnas no toquen la parte superior
            chartArea.AxisY.Minimum = 0; // Asegurarse de que el eje Y comienza en 0

            // Establecer el ancho de las barras para que se ajusten mejor al gráfico
            foreach (var serie in chart6.Series)
            {
                serie["PointWidth"] = "0.7"; // Ajustar el tamaño de las barras
            }

            // Mostrar el número generado en el Label llamado label30
            label30.Text = $": {randomNumber} ";
        }








        /**
 * Descripción: Este método realiza una búsqueda secuencial en una lista de números decimales, contando las ocurrencias de un valor específico y midiendo el tiempo que tarda en encontrar el primer valor y el tiempo total de ejecución.
 * @param lista - La lista de números en la que se realizará la búsqueda.
 * @param valorBuscado - El valor que se va a buscar en la lista.
 * Versión 1.1
 * @return - Retorna un tuple con tres valores: el número de ocurrencias del valor buscado, el tiempo en milisegundos que tardó en encontrar el primer valor, y el tiempo total en milisegundos que tardó en realizar la búsqueda.
 */

        public (int ocurrencias, double tiempoPrimerValor, double tiempoTotal) BuscarSecuencial(List<double> lista, double valorBuscado)
        {
            Stopwatch cronometro = new Stopwatch();
            int ocurrencias = 0;
            double tiempoPrimerValor = 0;  // Cambiado a double para almacenar los milisegundos
            bool encontradoPrimerValor = false;
            List<double> listaConUnDecimal = lista.Select(x => Math.Floor(x * 10) / 10).ToList();
            cronometro.Start();

            foreach (var elemento in listaConUnDecimal)
            {
                if (elemento == valorBuscado)
                {
                    ocurrencias++;
                    if (!encontradoPrimerValor)
                    {
                        tiempoPrimerValor = cronometro.Elapsed.TotalMilliseconds;  // Convertir a milisegundos
                        encontradoPrimerValor = true;
                    }
                }
            }

            cronometro.Stop();

            double tiempoTotal = cronometro.Elapsed.TotalMilliseconds;  // Convertir a milisegundos

            return (ocurrencias, tiempoPrimerValor, tiempoTotal);
        }
        /**
 * Descripción: Este método realiza una búsqueda exponencial en una lista de números decimales, contando las ocurrencias de un valor específico y midiendo el tiempo que tarda en encontrar el primer valor y el tiempo total de ejecución.
 * @param lista - La lista de números en la que se realizará la búsqueda.
 * @param valorBuscado - El valor que se va a buscar en la lista.
 * Versión 1.1
 * @return - Retorna un tuple con tres valores: el número de ocurrencias del valor buscado, el tiempo en milisegundos que tardó en encontrar el primer valor, y el tiempo total en milisegundos que tardó en realizar la búsqueda.
 */
        public (int ocurrencias, double tiempoPrimerValor, double tiempoTotal) BuscarExponencial(List<double> lista, double valorBuscado)
        {
            Stopwatch cronometro = new Stopwatch();
            cronometro.Start();

            int ocurrencias = 0;
            double tiempoPrimerValor = 0;
            bool encontradoPrimerValor = false;

            // Truncamos cada valor de la lista a un decimal (si es necesario)
            List<double> listaConUnDecimal = lista.Select(x => Math.Floor(x * 10) / 10).ToList();

            // Si la lista está vacía, retornamos los valores iniciales
            if (listaConUnDecimal.Count == 0)
            {
                return (0, 0, 0);  // No hay elementos para buscar
            }

            // Ordenamos la lista antes de realizar la búsqueda exponencial
            listaConUnDecimal.Sort();

            // Búsqueda exponencial: encontrar el rango en el que el valor podría estar
            int i = 1;
            while (i < listaConUnDecimal.Count && listaConUnDecimal[i] < valorBuscado)
            {
                i *= 2;
            }

            // Ajustamos los límites del rango
            int inicio = Math.Max(0, i / 2);
            int fin = Math.Min(i, listaConUnDecimal.Count - 1);

            // Búsqueda de ocurrencias dentro del rango encontrado
            for (int j = inicio; j <= fin; j++)
            {
                if (listaConUnDecimal[j] == valorBuscado)
                {
                    ocurrencias++;
                    if (!encontradoPrimerValor)
                    {
                        // Guardar el tiempo cuando encontramos la primera ocurrencia
                        tiempoPrimerValor = cronometro.Elapsed.TotalMilliseconds;
                        encontradoPrimerValor = true;
                    }

                    // Contar las ocurrencias consecutivas (si las hay)
                    int k = j + 1;
                    while (k < listaConUnDecimal.Count && listaConUnDecimal[k] == valorBuscado)
                    {
                        ocurrencias++;
                        k++;
                    }

                    // Ajustamos j para evitar contar las ocurrencias ya verificadas
                    j = k - 1;
                }
            }

            // Detenemos el cronómetro y obtenemos el tiempo total
            cronometro.Stop();
            double tiempoTotal = cronometro.Elapsed.TotalMilliseconds;

            return (ocurrencias, tiempoPrimerValor, tiempoTotal);
        }






        private void label29_Click(object sender, EventArgs e)
        {

        }

        private void label38_Click(object sender, EventArgs e)
        {

        }

        private void label36_Click(object sender, EventArgs e)
        {

        }
    }
}
