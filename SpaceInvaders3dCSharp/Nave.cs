using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceInvaders3dCSharp
{

    
    class Nave
    {
        static public float VELOCIDAD = .0055f;
        static public float RADIO = .25f;
 
        public Vector3 posicion;
        public bool estaVivo;
        public int numeroVidas;

        public Nave(float x, float y, float z) {

            estaVivo = true;
            posicion = new Vector3(x, y, z);
            numeroVidas = 1;
        }


        public void update(float delta,bool izq,bool der){
            if (izq)
            {
                posicion.x -= Nave.VELOCIDAD * delta;
             //   Console.WriteLine("X " + posicion.x);
            }
            else if (der)
            {
                posicion.x += Nave.VELOCIDAD * delta;
            //    Console.WriteLine("X " + posicion.x);

            }

            if (posicion.x < Program.MIN_X) {
                posicion.x = Program.MIN_X;
            }

            if (posicion.x > Program.MAX_X)
            {
                posicion.x = Program.MAX_X;
            }
        
        }

        public void muerto() {
            numeroVidas--;
            if(numeroVidas<=0)
                estaVivo = false;
        }

    }
}
