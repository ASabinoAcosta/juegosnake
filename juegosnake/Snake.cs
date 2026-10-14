using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Diagnostics;

namespace juegosnake
{
    internal class Snake
    {
        enum Direccion
        {
            Arriba,
            Abajo,
            Derecha,
            Izquierda
        }
        public bool Vivo { get; set; }
        public ConsoleColor ColorCabeza { get; set; }
        public ConsoleColor ColorCuerpo { get; set; }
        public Ventana Ventanac { get; set; }
        public List<Point> Cuerpo { get; set; }
        public Point Cabeza { get; set; }
        public Comida Comidac { get; set; }
        public int Puntaje { get; set; }
        public int PuntajeMaximo { get; set; }

        private Direccion _direccion;
        private bool _comiendo;

        public Snake(Point posicion, ConsoleColor colorCabeza, ConsoleColor colorCuerpo, Ventana ventana, Comida comida)
        {
            ColorCabeza = colorCabeza;
            ColorCuerpo = colorCuerpo;
            Ventanac = ventana;
            Comidac = comida;
            Cuerpo = new List<Point>();
            Cabeza = posicion;
            Puntaje = 0;
            PuntajeMaximo = 0;

            _direccion = Direccion.Derecha;
        }
        public void IniciarCuerpo(int numPartes)
        {
            Console.ForegroundColor = ColorCuerpo;
            int x = Cabeza.X - 1;
            for (int i = 0; i < numPartes; i++)
            {
                Console.SetCursorPosition(x, Cabeza.Y);
                Console.Write("█");
                Cuerpo.Add(new Point(x, Cabeza.Y));
                x--;
            }
            
        }
        public void Mover()
        {
            // Implementar la lógica de movimiento de la serpiente
            Teclado();
            Point posCabezaAnterior = Cabeza;
            MoverCabeza();
            MoverCuerpo(posCabezaAnterior);
            ColisionesComida();
            if (ColisionesCuerpo())
            {
                Muerte();
                Environment.Exit(0);
            }
        }
        public void MoverCabeza()
        {
            Console.ForegroundColor = ColorCabeza;
            Console.SetCursorPosition(Cabeza.X, Cabeza.Y);
            Console.WriteLine(" ");

            switch (_direccion)
            {
                case Direccion.Derecha:
                    Cabeza = new Point(Cabeza.X + 1, Cabeza.Y);
                    break;
                case Direccion.Izquierda:
                    Cabeza = new Point(Cabeza.X - 1, Cabeza.Y);
                    break;
                case Direccion.Arriba:
                    Cabeza = new Point(Cabeza.X, Cabeza.Y - 1);
                    break;
                case Direccion.Abajo:
                    Cabeza = new Point(Cabeza.X, Cabeza.Y + 1);
                    break;
            }
            ColisionesMarco();
            Console.SetCursorPosition(Cabeza.X,Cabeza.Y);
            Console.WriteLine("█");
        }
        private void MoverCuerpo(Point posCabezaAnterior)
        {
            Console.ForegroundColor = ColorCuerpo;
            Console.SetCursorPosition(posCabezaAnterior.X, posCabezaAnterior.Y);
            Console.WriteLine("█");
            Cuerpo.Insert(0, posCabezaAnterior);

            if (_comiendo)
            {
                _comiendo = false;
                return;
            }

            Console.SetCursorPosition(Cuerpo[Cuerpo.Count-1].X, Cuerpo[Cuerpo.Count - 1].Y);
            Console.WriteLine(" ");
            Cuerpo.Remove(Cuerpo[Cuerpo.Count - 1]);
        }
        private void ColisionesComida()
        {
            if (Cabeza == Comidac.Posicion)
            {
                if (!Comidac.GenerarComida(this))
                {
                    Vivo = false;
                    Environment.Exit(0);
                }
                Cuerpo.Add(Cuerpo[Cuerpo.Count - 1]);
                _comiendo = true;
            }
        }
        private void Teclado()
        {
            if(Console.KeyAvailable)
            {
                ConsoleKeyInfo tecla = Console.ReadKey(true);
                if (tecla.Key == ConsoleKey.RightArrow && (_direccion != Direccion.Izquierda)) 
                {
                    _direccion = Direccion.Derecha;
                }
                else if(tecla.Key==ConsoleKey.LeftArrow && (_direccion != Direccion.Derecha))
                {
                    _direccion = Direccion.Izquierda;
                }
                else if(tecla.Key==ConsoleKey.UpArrow && (_direccion != Direccion.Abajo))
                {
                    _direccion = Direccion.Arriba;
                }
                else if(tecla.Key==ConsoleKey.DownArrow && (_direccion != Direccion.Arriba))
                {
                    _direccion = Direccion.Abajo;
                }
            }
        }
        private void ColisionesMarco()
        {
            if (Cabeza.X <= Ventanac.LimiteSuperior.X)
            {
                Cabeza = new Point(Ventanac.LimiteInferior.X-1, Cabeza.Y);
            }
            if(Cabeza.X >= Ventanac.LimiteInferior.X)
            {
                Cabeza = new Point(Ventanac.LimiteSuperior.X + 1, Cabeza.Y);
            }
            if(Cabeza.Y <=Ventanac.LimiteSuperior.Y)
            {
                Cabeza = new Point(Cabeza.X, Ventanac.LimiteInferior.Y - 1);
            }
            if(Cabeza.Y >= Ventanac.LimiteInferior.Y)
            {
                Cabeza = new Point(Cabeza.X, Ventanac.LimiteSuperior.Y + 1);
            }
        }
        private bool ColisionesCuerpo()
        {
            foreach(Point item in Cuerpo)
            {
                if(Cabeza == item)
                {
                    Vivo = false;
                    return true;
                }
            }
            return false;
            
        }
        public void Muerte()
        {
            Console.ForegroundColor = ColorCuerpo;

            foreach(Point item in Cuerpo)
            {
                if (item == Cabeza)
                {
                    continue;
                }
                Console.SetCursorPosition(item.X, item.Y);
                Console.Write("░");
                Thread.Sleep(120);
            }

            Console.Clear();
            Console.SetCursorPosition(Ventanac.Ancho / 2 - 5, Ventanac.Altura / 2);
            Console.WriteLine("GAME OVER");
            Console.SetCursorPosition(Ventanac.Ancho / 2 - 10, Ventanac.Altura / 2 + 1);
            Console.WriteLine($"Puntaje: {Cuerpo.Count}");
        }
        public void Informacion (int distanciaX1, int distanciaX2)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.SetCursorPosition(Ventanac.LimiteSuperior.X+distanciaX1, Ventanac.LimiteSuperior.Y-1);
            Console.Write("Puntaje: "+Puntaje+" ");
            Console.SetCursorPosition(Ventanac.LimiteSuperior.X + distanciaX2, Ventanac.LimiteSuperior.Y - 1);
            Console.Write("Puntaje Maximo: " + PuntajeMaximo + " ");
        }
    }
}
