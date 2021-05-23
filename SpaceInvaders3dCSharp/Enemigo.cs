using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceInvaders3dCSharp
{
    class Enemigo
    {

        public static float ENEMIGO_RADIO = .3f;
        public static float ENEMIGO_VELOCIDAD = .00015f;
        public static int MOVIMIENTO_ABAJO = 0;
        public static int MOVIMIENTO_IZQUIERDA = 0;
        public static int MOVIMIENTO_DERECHA = 0;

        public Vector3 posicion;
        public float radio;

        public bool estaVivo;
        public int numVidas;
        int movimiento;
        public float angulo;


        public Enemigo(float x, float y, float z,float angulo,Random oRan) {
            estaVivo = true;
            movimiento = MOVIMIENTO_IZQUIERDA;
            radio = ENEMIGO_RADIO;
            posicion = new Vector3(x, y, z);
            numVidas = oRan.Next(Program.ronda)+1;
            this.angulo = angulo;
        }


        public void update(float delta){ //delta es el tiempo desde el ultimo ciclo
            if (estaVivo)
            {
                posicion.z += delta * ENEMIGO_VELOCIDAD + ((Program.ronda-1) * .001f);//en la ronda 1 no se da ningun boot de velocidad
                angulo += .15f * delta;

                if (angulo >= 360)
                    angulo = 0;

            }
            else if (!estaVivo)
            {
                radio -= .0015f*delta;
                if (radio <= 0)
                    radio = 0;
            }
        
        }


        public void muerto() {
            numVidas--;
            if(numVidas<=0)
                estaVivo = false;
        
        }
    }
}
