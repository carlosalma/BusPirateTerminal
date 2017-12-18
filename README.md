# Microdatos2Tabla

La siguiente aplicación permite adaptar los ficheros de Microdatos INE a diferentes formatos.

## Microdatos

Los ficheros de microdatos contienen los datos individuales de una estadística, convenientemente anonimizados, con el fin de preservar la confidencialidad de la información. Se trata de ficheros ASCII con estructura de campos que recogen para cada registro individual de la encuesta los valores que toma cada variable. Los datos aparecen sin agregar, por lo que para el estudio de los mismos se necesitan programas adecuados para el tratamiento de datos estadísticos. Fuente: [INE - Microdatos](http://www.ine.es/prodyser/microdatos.htm)

### Trabajando con Microdatos

Cada enlace a una estadistica se compone como mínimo de cuatro documentos:

- Cuestionarios
- Metodología general
- Diseño de registro y valores válidos de las variables
- Fichero de microdatos

**FICHERO: Diseño de registro y valores válidos de las variables**

Se trata de un fichero de Hoja de cálculo, en el que se distribuye la información agrupada en pestañas. 

PESTAÑA: Registro_captura y depuracion

NOM VAR  | LITERAL ASOCIADO | Pregunta cuestionario | Tipo variable  | Long. Campo	 | Long. Fichero	| Valores válidos variables
---------|------------------|-----------------------|----------------|--------------|---------------|--------------------------
 IDQ     | Identificación del cuestionario |        | AN	            | 5	           | 5	            | >0
 CPRO	   | Código de provincia		           |        | AN	            | 2	           | 7	            | ver tabla
 CCAA	   | Código de comunidad autónoma		  |        | AN	            | 2	           | 9	            | ver tabla
 ...     | ...              | ...                   | ...            | ...          | ...           | ...          
 CIOCUP3 | Ocupación en primer empleo en España Ampliada | 5.13 | AN | 2	           | 3100	         | Ver tabla

 - *Longitud Campo*: muestra la longitud de cada uno de los campos que componen una fila.
 - *Longitud Fichero*: la suma de la longitud de todos los campos que componen una fila.

PESTAÑA: Provincia

 COD | PROVINCIA
-----|----------
 01  | Álava		
 02  | Albacete		
 03  | Alicante		
 04  | Almería		
 05  | Ávila		

PESTAÑA: Comunidad autónoma

COD_CCAA	| NOM_CCAA
---------|--------------------
 08	     | Castilla-La Mancha
 09	     | Catalunya
 10	     | Comunidad Valenciana
 11	     | Extremadura
 12	     | Galicia

**FICHERO: Fichero de microdatos**

- 000010310 5 1 0228.1998  8.12003 ...6.   . ..228    2 0228.1995 11.12003 ...6.   . ..228    3 0228.2002
- 000020310 6 1 1228.1969 37.21998 ...6.   . ..228    2 2228.1970 36.11993 ...6.   . ..228    3 0108.1998

**Extracción de campos**

NOM VAR | Long. Campo | Long. Fichero | Contenido del campo | Información resultante
--------|-------------|---------------|---------------------|-----------------------  
 IDQ    | 5           | 5             | 00001               | 1
 CPRO   | 2           | 7             | 03                  | Alicante
 CCAA   | 2           | 9             | 10                  | Comunidad Valenciana
 ...    | ...         | ...           | ...                 | ...
 
## Enlaces

- [Instituto Nacional de Estadística - INE](http://www.ine.es)
- [INE - Microdatos](http://www.ine.es/prodyser/microdatos.htm)
