using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceInvaders3dCSharp
{
    class Disparo
    {

        static float DISPARO_VELOCIDAD = .005f;
        public static float DISPARO_RADIO = .05f;

        public Vector3 posicion;
        public bool disparoDesdeNave;
        public float radio;
        public bool esSuperDisparo;

        public Disparo(float x, float y, float z, bool esDeLaNave,bool superDisparo) {
            posicion = new Vector3(x, y, z);
            disparoDesdeNave = esDeLaNave;
            if (superDisparo)
            {
                radio = .3f;
                esSuperDisparo = superDisparo;
            }
            else
            {
                radio = DISPARO_RADIO;
                esSuperDisparo = false;
            }

        }

        public void update(float delta){

            if (disparoDesdeNave)
            {
                posicion.z -= DISPARO_VELOCIDAD * delta;
            }
            else {
                posicion.z += DISPARO_VELOCIDAD * delta;
            }
        
        }

    }
}
