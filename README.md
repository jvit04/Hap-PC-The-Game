# Hap-PC - Videojuego 2D

Materia: Lenguajes de Programación | Periodo: 2026 | Estado: Completado

## Equipo de trabajo
* Leonor Molina Zapata ([Leomz21](https://github.com/Leomz21))
* José Andrés Viteri Hoyos ([jvit04](https://github.com/jvit04))

## Capturas / Demo
<img width="595" height="292" alt="image" src="https://github.com/user-attachments/assets/d1ccbe73-a170-4791-a592-7e9d3ed7383b" />
<img width="595" height="298" alt="image" src="https://github.com/user-attachments/assets/a9317b71-8e27-41e4-a350-71e4d6d9a0dd" />
<img width="594" height="292" alt="image" src="https://github.com/user-attachments/assets/0a185c84-e1ef-4536-8f40-581463b81cf6" />


https://github.com/user-attachments/assets/1083422d-c847-4d90-92f3-e1910f5fc538





## Funcionalidad

* [x] Motor de Físicas y Control de Personaje: Implementación de scripts de movimiento como `PlayerController.cs` y `PlayerControllerLevel2.cs`, con detección de colisiones mediante `LayerMask` en capas específicas como "Ground", y mecánicas físicas 2D que incluyen saltos y caídas rápidas.
* [x] Gestión de Estados y Escenas: Control centralizado del ciclo de vida del juego a través de controladores principales (`GameManager.cs` y `level2_GameManager.cs`), manejando la transición progresiva sin interrupciones entre las escenas `MainMenu`, `Level1_Adventure` y `Level2_Boss`.
* [x] Lógica de Combate y Enemigos: Desarrollo de un jefe final ("Botnet") encapsulando sus rutinas complejas en `BotnetAtaques.cs` y `BotnetHealth.cs`, incorporando sistemas de daño, estados de invulnerabilidad e instanciación de proyectiles aéreos.
* [x] Sistemas de Spawning Dinámico: Optimización de carga en el escenario lineal de aventura mediante los scripts `ZoneSpawner.cs` y `PositionWaveSpawner.cs` para la generación procedimental de obstáculos y recoleccionables en tiempo de ejecución basado en el posicionamiento en vivo del jugador.
* [x] Interfaz de Usuario (UI) y Audio: Implementación de interfaces Canvas escalables utilizando `TextMesh Pro` para renderización nítida de tipografías y un `AudioManager.cs` centralizado, diseñado para no borrarse al cambiar de escena y gestionar de forma persistente los efectos de sonido y la banda sonora.

## Tecnologías
`Unity` `C#` `TextMeshPro` `Git` `GitHub`

## Ejecución
### Instrucciones paso a paso
#### Prerrequisitos

* Unity Editor instalado con el módulo de soporte para Windows Build.
* Sistema de control de versiones Git instalado.

#### Pasos de Despliegue

1. Clonar el repositorio del proyecto en su máquina local:

```bash
    git clone https://github.com/jvit04/Hap-PC-The-Game.git
    cd Hap-PC-The-Game

```

2. Abrir el proyecto desde Unity Hub seleccionando la carpeta raíz del repositorio clonado.
3. Dirigirse a la pestaña **File > Build Settings** y verificar que las escenas maestras estén organizadas en el orden correcto para la compilación continua[cite: 3]:
* `0 MainMenu`
* `1 Level1_Adventure`
* `2 Level2_Boss`


4. Compilar el proyecto pulsando "Build and Run" o probarlo directamente en el entorno de desarrollo. De manera alternativa, para evaluación inmediata, se puede descomprimir y ejecutar el archivo binario precompilado ubicado en la ruta del repositorio `Ejecutable/Hap-PC-The-Game-Windows.zip`[cite: 3].

## Métricas de Progreso
| Indicador             | Valor      |
|-----------------------|------------|
| Commits totales       | 14         |
| Issues/PRs fusionados | 0/2        |
| Cobertura de pruebas  | 100%        |
| Última actualización  | 2026-08-11 |

## Reflexión y Aprendizajes
* **Habilidades desarrolladas:** Diseño modular e implementación de mecánicas de videojuegos 2D, manipulación profunda del motor de físicas físicas de Unity, cableado de secuencias de animación mediante el Animator Controller y administración técnica de un repositorio colaborativo mediante Git.
* **Qué funcionó bien:** La segregación estricta de la organización del trabajo. Utilizar una rama base compartida (`main`) y forzar el desarrollo de cada nivel en archivos de escenas individuales con ramas git aisladas (`nivel-1-aventura` y `feature/nivel-2-jefe-botnet`) blindó la estructura del proyecto e impidió que el trabajo de un integrante sobreescribiera el código del otro.
* **Qué se podría mejorar:** La gestión de dependencias en elementos prefabricados (*prefabs*) compartidos, ya que la utilización paralela de instancias comunes sin el uso de variantes independientes causó pequeños cuellos de botella y conflictos sintácticos al momento de unificar las ramas de trabajo en el control de versiones.
* **Conceptos clave aplicados de la materia:** Uso extensivo y práctico de la programación orientada a objetos en C#, manejo estructurado de disparadores de eventos lógicos y colisiones (`OnTriggerEnter2D`), implementación de corrutinas (`IEnumerator`) para la ejecución asíncrona de rutinas y pausas, y diseño de controladores lógicos que dictan el flujo de la aplicación.
