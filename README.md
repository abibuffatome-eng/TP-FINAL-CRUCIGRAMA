Trabajo Practico Final - Programacion I

Este repositorio contiene el proyecto final desarrollado para la catedra de Programacion I. Se trata de un Juego de Crucigramas interactivo en entorno de escritorio, diseñado utilizando el lenguaje C# y la tecnologia Windows Forms sobre la plataforma .NET.

Requisitos del Proyecto y Consignas

El desarrollo de la aplicacion se rigio bajo los siguientes requisitos tecnicos y funcionales solicitados:

1. Arquitectura en Capas: Separacion limpia de responsabilidades dividida en capas de Formularios (UI), Logica de Negocio (Controladores/Juego), Persistencia de Datos y Modelos de Dominio.
2. Interfaz Grafica (WinForms): Creacion de controles dinamicos para la grilla del crucigrama, manejo de eventos de teclado/mouse, validaciones de interfaz y navegacion fluida entre ventanas.
3. Persistencia con Base de Datos: Uso obligatorio de un motor relacional local (SQLite) para la gestion completa de los datos sin perder informacion al cerrar la app.
4. Control de Roles (Autenticacion): Sistema de inicio de sesion con perfiles diferenciados:
* Administrador: Capacidad de gestion y control del sistema.
* Jugador: Acceso a la seleccion de niveles, juego activo, visualizacion de pistas y puntajes.


5. Reglas de Negocio: Validacion matematica de las celdas (cruce correcto de letras), control de estados (celdas bloqueadas, vacias o verificadas) y penalizaciones en la puntuacion por intentos fallidos.

Tecnologias y Entorno

* Lenguaje de Programacion: C#
* Framework/Entorno: Windows Forms (.NET)
* Base de Datos: SQLite (System.Data.SQLite)
* IDE recomendado: Visual Studio (2022 o superior)

Estructura del Codigo Fuente

Para cumplir con las buenas practicas de diseño de software, el proyecto organiza sus componentes en las siguientes carpetas:

* Modelos: Clases del dominio del problema (Usuario, Crucigrama, Celda, Palabra, Partida).
* Persistencia: Repositorios y mapeadores encargados de interactuar mediante SQL con la base de datos.
* Controladores (Logica): Clases que manejan las reglas del juego, flujos internos y calculo de puntajes.
* Formularios: Componentes de la interfaz grafica de usuario (FormLogin, FormCrucigrama, FormNiveles, etc.).

Credenciales para Evaluacion

La base de datos se inicializa automaticamente (crucigrama.db) con las siguientes cuentas de prueba integradas para evaluar las dos vistas del sistema:

* Administrador: Usuario: admin | Contraseña: admin123
* Jugador 1: Usuario: jugador1 | Contraseña: jugador123
* Jugador 2: Usuario: ana | Contraseña: ana123

Instrucciones de Ejecucion

1. Clonar este repositorio localmente.
2. Abrir la solucion .sln en Visual Studio.
3. Compilar el proyecto para restaurar las dependencias NuGet de SQLite de forma automatica.
4. Presionar F5 o el boton Iniciar para ejecutar la aplicacion.
