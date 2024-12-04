Henry Hair Arévalo Barrera - 202112106
henry.arevalo@uptc.edu.co
Estudiante de Ingeniería de Sistemas y Computación


Universidad Pedagógica y Tecnológica de Colombia (UPTC)
Ingeniería de Sistemas y Computación
Matemáticas Discretas
Taller Final

Secesiones.exe >> Ejecutable del proyecto compilado 

---------------------------------------------------------------
# Proyecto Final - Algoritmos de Ordenamiento y Búsqueda

### Descripción
Este proyecto consiste en la implementación de un sistema que genera listas aleatorias, las ordena usando diferentes algoritmos y mide el tiempo de ejecución de cada uno. El sistema también incluye una funcionalidad de búsqueda para encontrar coincidencias basadas en la primera posición decimal de los números en las listas. Los resultados son presentados en tablas y gráficos comparativos para facilitar la visualización de los tiempos de ejecución.

### Requisitos de Entrada
1. **Cantidad de elementos en las listas**: El sistema permite definir la cantidad de elementos que cada lista contendrá.
2. **Cantidad de iteraciones**: El sistema permite especificar el número de iteraciones para realizar el proceso de generación, ordenamiento y medición de tiempos.

### Proceso del Sistema
#### 1. Inicialización de las Listas:
Por cada iteración, el sistema genera 4 listas de datos:
- **Lista 1**: Datos aleatorios generados.
- **Lista 2**: Datos aleatorios organizados en rangos ascendentes.
- **Lista 3**: Los mismos datos de la lista 1, pero ordenados de forma descendente.
- **Lista 4**: Datos aleatorios con valores repetidos.

#### 2. Aplicación de Algoritmos de Ordenamiento:
El sistema aplica tres algoritmos de ordenamiento a las 4 listas:
- **Algoritmo Burbuja**
- **Algoritmo de Inserción**
- **Algoritmo de Ordenamiento Asignado por el Usuario** (según lo definido en la tabla de especificaciones).

#### 3. Medición de Tiempos:
El sistema registra el tiempo de ejecución para cada algoritmo aplicado a cada lista, sumando un total de 12 mediciones por iteración (4 listas x 3 algoritmos).

#### 4. Presentación de Resultados:
- Los tiempos de ejecución se muestran en una tabla organizada.
- Se genera una gráfica comparativa de los tiempos de ejecución de cada algoritmo para cada lista.

#### 5. Iteración del Proceso:
Si el número de iteraciones es mayor a 1, el proceso se repite con nuevas listas y se generan los resultados correspondientes para cada iteración.

#### 6. Diseño de la Interfaz:
La interfaz muestra los resultados de todas las iteraciones de manera clara y organizada, permitiendo visualizar los tiempos y comparar los algoritmos de manera fácil.

### Algoritmos de Búsqueda
El proyecto incluye una funcionalidad para realizar búsquedas en las listas generadas:
- **Búsqueda Secuencial**: Recorre las listas buscando coincidencias basadas en la primera posición decimal del valor a buscar.
- **Búsqueda Exponencial**: Realiza una búsqueda optimizada con la misma condición de la primera posición decimal.

Cuando se hace clic en el botón de búsqueda, el sistema genera un número aleatorio entre 0 y 1 con una sola cifra decimal (por ejemplo, 0.1, 0.2, etc.), y busca coincidencias en las listas utilizando los dos métodos de búsqueda.

Los resultados incluyen:
- El valor a buscar.
- El número total de coincidencias encontradas.
- Los tiempos de ejecución de cada algoritmo de búsqueda (primer hallazgo y total).
- Una gráfica comparativa de los tiempos de ejecución de ambos métodos de búsqueda.

### Tecnologías Utilizadas
- **Lenguaje**: C#
- **IDE**: Visual Studio Community
- **Plataforma**: .NET Framework
