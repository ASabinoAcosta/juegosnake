using juegosnake;
using System.Drawing;

Ventana ventana;
Snake snake;
Comida comida;
bool jugar = true;

void Iniciar()
{
    ventana = new Ventana("Serpiente", 65, 20, ConsoleColor.Black, ConsoleColor.White, new Point(5,3), new Point(59,18));
    ventana.DibujarMarco();
    comida = new Comida(ConsoleColor.Red, ventana);
    snake = new Snake(new Point(8,5), ConsoleColor.Green, ConsoleColor.White, ventana, comida);
    snake.IniciarCuerpo(2);

    comida.GenerarComida(snake);
}

void Game()
{
    while(jugar)
    {
        snake.Mover();
        Thread.Sleep(100);
    }
}

Iniciar();
Game();
Console.ReadKey();
