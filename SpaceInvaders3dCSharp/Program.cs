using System;
using System.Collections.Generic;
using System.Text;
using Tao.OpenGl;
using Tao.FreeGlut;
using System.Threading;
using System.Diagnostics;

namespace SpaceInvaders3dCSharp
{
    class Program
    {
        public static int x0=0,y0=0,alfa=0,beta=0;
        public static int MIN_X = -10;
        public static int MAX_X = 10;
        public static int TIEMPO_PARA_SIQUIENTE_NIVEL = 1500;

        //Este stopWatch es para tratar de correr el juego a la misma velocidad en diferentes computadoras
        static Stopwatch stopWatch = new Stopwatch();

        static float[] mat_ambient_verde = { 0.0f, 1.0f, 0.0f, 0.0f };
        static float[] mat_ambient_rojo = { 1.0f, 0.0f, 0.0f, 0.0f };
        static float[] mat_ambient_azul = { 0.0f, 0.0f, 1.0f, 0.0f };
        static float[] mat_ambient_amarillo = { 3.0f, 3.0f, 0.0f, 0.0f };
        static float[] mat_ambient_rojiso = { 10.0f, 1.0f, 0.0f, 0.0f };
        static float[] mat_ambient_negro = { 0, 0, 0, 0 };
        static float[] mat_ambient_gris = { 0.7f, 0.7f, 0.7f, 1.0f };
        static float[] mat_ambient_plateado = { 0.999f, 0.999f, 0.9999f, 0f };


        static float tiempoNextLevel = TIEMPO_PARA_SIQUIENTE_NIVEL;
        public static int ronda;
        static bool izqDown = false;
        static bool derDown = false;
        static bool autoMatar = false;
        static Nave oNave;
        static List<Enemigo> arrEnemigos;
        static List<Disparo> arrDisparos;
        static Random oRan;
        static int puntuacion;
        static int disparosRealizados;
        static void Main(string[] args)
        {

            Glut.glutInit();
            Glut.glutInitDisplayMode(Glut.GLUT_DOUBLE | Glut.GLUT_RGB | Glut.GLUT_DEPTH);
            Glut.glutInitWindowSize(1140, 648);
            Glut.glutInitWindowPosition(100, 100);
            Glut.glutCreateWindow("Space Invaders by Yayo");
            Glut.glutKeyboardFunc(keyboardDown);
            Glut.glutKeyboardUpFunc(keyboardUp);
            Glut.glutIdleFunc(update);
            Glut.glutMotionFunc(onMotion);
            Glut.glutMouseFunc(onMouse);

            Glut.glutDisplayFunc(dibujarTocho);
            Gl.glEnable(Gl.GL_LIGHTING);
            Gl.glEnable(Gl.GL_LIGHT0);
            Gl.glShadeModel(Gl.GL_SMOOTH);
            Gl.glEnable(Gl.GL_DEPTH_TEST); //para eliminar las caras ocultas
            Gl.glEnable(Gl.GL_NORMALIZE); //normaliza el vector para ombrear apropiadamente
            Gl.glClearColor(1.0f, 1.0f, 1.0f, 0.0f); //El color de fondo es gris
            Gl.glViewport(0, 0, 48, 32);

            //Inicio todos los valores;
            inicializarJuego();

           
            stopWatch.Start();
            Glut.glutMainLoop();

        

        }
        static void inicializarJuego() {
            ronda = 1;
            oNave = new Nave(0, 0, .7f);
            arrEnemigos = new List<Enemigo>();
            arrDisparos = new List<Disparo>();
            oRan = new Random();
            agregarOvnis();
           
            puntuacion = 0;
            disparosRealizados = 0;
            
        }

        static void agregarOvnis(){
            for (int r = 0; r < 10; r++)
            {
                for (int c = 0; c < 5; c++)
                {
                    arrEnemigos.Add(new Enemigo((MIN_X / 2) + r + .5f, 0, -15 + c, oRan.Next(360), oRan));
                }
            }
           
        }

        static void dibujarTocho()
        {
            //Asigna los apropiados materiales a las superficies
            float[] mat_diffuse = { 0.6f, 0.6f, 0.6f, 0.0f };
            float[] mat_specular = { 1.0f, 1.0f, 1.0f, 1.0f };
            float[] mat_shininess = { 50.0f };

            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_AMBIENT, mat_ambient_gris);
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_DIFFUSE, mat_diffuse);
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_SPECULAR, mat_specular);
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_SHININESS, mat_shininess);
            // asigna la apropiada fuente de luz
            float[] lightIntensity = { 0.7f, 0.7f, 0.7f, 1.0f };
            //float[] lightIntensity = { 1.0f, 1.0f, 1.0f, 1.0f };
            float[] light_position = { 2.0f, 2.0f, 3.0f, 0.0f };
            //float[] light_position = {5.0f,10.0f, 0.0f, 0.0f};

            Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_POSITION, light_position);
            Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_DIFFUSE, lightIntensity);
            //asigna la cámara
            Gl.glMatrixMode(Gl.GL_PROJECTION);
            Gl.glLoadIdentity();

 
            //Fin dibujar fondo
            Glu.gluPerspective(67,48/32,.1f,100);
            Gl.glMatrixMode(Gl.GL_MODELVIEW);
            Gl.glLoadIdentity();
            //los primeros 3 son la posicion de la camara, los siguientes 3 son lookAt de la camra y los ultimos 3 son la rotacion del acamra
            Glu.gluLookAt(oNave.posicion.x, 6, 2, oNave.posicion.x, 0, -4, 0, 1, 0);
            //comienza el dibujo
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT); // Limpia la pantalla


            Gl.glRotated(alfa, 1,0 , 0);
            Gl.glRotated(beta, 0, 1, 0);

            dibujarVertices();

            dibujarNave();
            dibujarEnemigo();
            dibujarDisparo();


            //Dibujo instrucciones
            Gl.glRasterPos3d(oNave.posicion.x - 7.2f, 1.5, -9);
            Glut.glutBitmapString(Glut.GLUT_BITMAP_HELVETICA_12, "A Move left\nD Move Right\nS Fire \nQ Super Fire \nE Destroy all ovnis");

            //Dibujo la puntuacion en pantalla
            Gl.glRasterPos3d(oNave.posicion.x - 6.3f, 3.5, -9);
            Glut.glutBitmapString(Glut.GLUT_BITMAP_HELVETICA_12, "Lifes " + oNave.numeroVidas);

            //Dibujo la puntuacion en pantalla
            Gl.glRasterPos3d(oNave.posicion.x-4.3f,3.5,-9);
            Glut.glutBitmapString(Glut.GLUT_BITMAP_HELVETICA_12, "Points " + puntuacion);

            //Dibujo el numero de disparos que se han realizado
            Gl.glRasterPos3d(oNave.posicion.x -2.3f, 3.5, -9);
            Glut.glutBitmapString(Glut.GLUT_BITMAP_HELVETICA_12, "Shoots " + disparosRealizados);

            //Dibujo en que ronda vamos
            Gl.glRasterPos3d(oNave.posicion.x - 0.3f, 3.5, -9);
            Glut.glutBitmapString(Glut.GLUT_BITMAP_HELVETICA_12, "Round " + ronda);

            //Cuaddo mato a todas los ovnis
            if (arrEnemigos.Count <= 0) {
                Gl.glRasterPos3d(oNave.posicion.x - 1, 1, -3);
                Glut.glutBitmapString(Glut.GLUT_BITMAP_TIMES_ROMAN_24, "Round "+ ronda +" in "+ tiempoNextLevel);
             
            }

             if (!oNave.estaVivo)
             {
                Gl.glRasterPos3d(oNave.posicion.x - 1, 1, -3);
                Glut.glutBitmapString(Glut.GLUT_BITMAP_TIMES_ROMAN_24, "Game Over");
                Gl.glRasterPos3d(oNave.posicion.x - 1.5, .3, -3);
                Glut.glutBitmapString(Glut.GLUT_BITMAP_TIMES_ROMAN_24, "Press R to play again");
             }


            Glut.glutSwapBuffers();

         

        }

        //Aqui se actualizan las posiciones, se checan colisiones, etc.
        public static void update()
        {
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            //Delta es el tiempo transcurrido desde la ultima iteracion. Todos nuestros movimientos dependeran del tiempo.
            float delta=(float)ts.TotalMilliseconds;
            Console.WriteLine("RunTime " + delta);

            //Actualizo la nave y su posicion
            oNave.update(delta, izqDown, derDown);
            
            //Actualizo los enemigos y su posicion, y tambien genero un disparo aleatorio
            int len = arrEnemigos.Count;
            Enemigo oEnemigo;
            for (int i = 0; i < len; i++) {
                oEnemigo= arrEnemigos[i];
                oEnemigo.update(delta);
                if(oEnemigo.estaVivo  && oRan.Next(1500)<3) //Solo disapara si el enemigo esta vivo
                    arrDisparos.Add(new Disparo(oEnemigo.posicion.x,oEnemigo.posicion.y,oEnemigo.posicion.z,false,false));

                if (oEnemigo.radio <= 0)
                {
                    arrEnemigos.Remove(oEnemigo);
                    len--;
                }
            }

            //actualizo las posiciones de los disparos y si llegan a los limites los elimino para que no consuman recursos
            len = arrDisparos.Count;
            Disparo oDisparo;
            for (int i = 0; i < len; i++)
            {
                oDisparo=arrDisparos[i];
                oDisparo.update(delta);
                if (oDisparo.posicion.z < -25 || oDisparo.posicion.z > 5)
                {
                    arrDisparos.Remove(oDisparo);
                    len--;
                }

            }

            //Tengo que revisar si chocan los disparos con la nave o con los Ovnis
            #region Revisar Colisiones
            len = arrEnemigos.Count;
            int lenDis = arrDisparos.Count;
            for (int i = 0; i < len; i++)
            {
                oEnemigo = arrEnemigos[i];
                for (int j = 0; j < lenDis; j++)
                {
                    oDisparo = arrDisparos[j];
                    if (oDisparo.disparoDesdeNave)
                    {
                        double sumaRadios = oEnemigo.radio + oDisparo.radio;
                        double distanciaZ = oEnemigo.posicion.z - oDisparo.posicion.z;
                        double distanciaX = oEnemigo.posicion.x - oDisparo.posicion.x;
                        if (oEnemigo.estaVivo && ((sumaRadios * sumaRadios) > ((distanciaZ * distanciaZ) + (distanciaX * distanciaX))))
                        {

                            oEnemigo.muerto();
                            if (!oDisparo.esSuperDisparo)
                            {
                                arrDisparos.Remove(oDisparo);
                                lenDis--;
                            }
                            puntuacion += 10;
                        }
                    }
                    else {
                        double sumaRadios = Nave.RADIO + oDisparo.radio;
                        double distanciaZ = oNave.posicion.z - oDisparo.posicion.z;
                        double distanciaX = oNave.posicion.x - oDisparo.posicion.x;
                        if (((sumaRadios * sumaRadios) > ((distanciaZ * distanciaZ) + (distanciaX * distanciaX))))
                        {
                            oNave.muerto();
                            arrDisparos.Remove(oDisparo);
                            lenDis--;
                        }
                    }
                }
            }
            #endregion

            len = arrEnemigos.Count;
            if (len <= 0) {
                if(tiempoNextLevel==TIEMPO_PARA_SIQUIENTE_NIVEL)
                    ronda++;
                tiempoNextLevel -= delta;
                if (tiempoNextLevel <= 0) {
                    tiempoNextLevel = TIEMPO_PARA_SIQUIENTE_NIVEL;
                    agregarOvnis();
                }
            }


            if (autoMatar)
            {
                len = arrEnemigos.Count;
                if(len>0){
                    arrEnemigos.Remove(arrEnemigos[0]);
                }
            }

            stopWatch.Reset();
            stopWatch.Start();
            Glut.glutPostRedisplay();
              
        }

        public static void dibujarNave()
        {
            if (!oNave.estaVivo) return;
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_AMBIENT, mat_ambient_negro);
            Gl.glPushMatrix();
            Gl.glTranslated(
               oNave.posicion.x,
                oNave.posicion.y,
               oNave.posicion.z);
            Gl.glRotated(180, 1, 0, 0);
            Glut.glutSolidCone(Nave.RADIO, .8, 100, 100);
            Gl.glPopMatrix();

        }

        static void dibujarEnemigo() {
            Enemigo oEnemigo;
            int len = arrEnemigos.Count;

            for (int i = 0; i < len; i++)
            {
                oEnemigo = arrEnemigos[i];
                if(oEnemigo.numVidas<=1)
                    Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_AMBIENT, mat_ambient_gris);
                else if(oEnemigo.numVidas==2)
                    Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_AMBIENT, mat_ambient_verde);
                else if (oEnemigo.numVidas ==3)
                    Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_AMBIENT, mat_ambient_amarillo);
                else if (oEnemigo.numVidas >= 4)
                    Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_AMBIENT, mat_ambient_rojo);

                Gl.glPushMatrix();
                Gl.glTranslated(oEnemigo.posicion.x,oEnemigo.posicion.y,oEnemigo.posicion.z);
                Gl.glRotated(oEnemigo.angulo, 0, 1, 0);
                Gl.glRotated(270, 1, 0, 0);
                Glut.glutSolidCone(oEnemigo.radio,.3f, 100, 100);

                if (oEnemigo.numVidas > 0)
                {
                    Gl.glTranslated(0, .2f, .1);
                    Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_AMBIENT, mat_ambient_amarillo);
                    Glut.glutSolidCube(.1f);
                }
                Gl.glPopMatrix();
            }
        
        }

        static void dibujarDisparo() {
            Disparo oDisparo;
            int len = arrDisparos.Count;

            for (int i = 0; i < len; i++)
            {
                oDisparo = arrDisparos[i];
                Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_AMBIENT, mat_ambient_rojo);
                Gl.glPushMatrix();
                Gl.glTranslated(
                    oDisparo.posicion.x,
                    oDisparo.posicion.y,
                    oDisparo.posicion.z);
                Glut.glutSolidSphere(oDisparo.radio, 100, 100);
                Gl.glPopMatrix();
            }
        }



        static void dibujarVertices()
        {
            //Dibuja la pared con espesor y tapa = palno xz, esquina en el origen
            Gl.glPushMatrix();
            Gl.glLineWidth(2.0f);
            Gl.glDisable(Gl.GL_LIGHTING);
            Gl.glDisable(Gl.GL_LIGHT0);

            Gl.glBegin(Gl.GL_LINES);
            //x rojo
            Gl.glColor3d(1f, 0, 0);
            Gl.glVertex3d(-10, 0, 0);
            Gl.glVertex3d(10, 0, 0);
            //y verde
            Gl.glColor3d(0, 1, 0);
            Gl.glVertex3d(0, -10, 0);
            Gl.glVertex3d(0, 10, 0);
            //z azul
            Gl.glColor3d(0, 0, 1);
            Gl.glVertex3d(0, 0, -10);
            Gl.glVertex3d(0, 0, 10);

            Gl.glEnd();

            //glutWireCube(1.0);
            Gl.glPopMatrix();
            Gl.glEnable(Gl.GL_LIGHTING);
            Gl.glEnable(Gl.GL_LIGHT0);
        }

        static void keyboardDown(byte key, int x, int y)
        {
            char k = (char)key;
            switch (k)
            {
                case 'S':
                case 's':
                    if (oNave.estaVivo)
                    {
                        arrDisparos.Add(new Disparo(oNave.posicion.x, oNave.posicion.y, oNave.posicion.z - .85f, true,false));
                        disparosRealizados++;
                    }
                    break;
                case 'q':
                case 'Q':
                    if (oNave.estaVivo)
                    {
                        arrDisparos.Add(new Disparo(oNave.posicion.x, oNave.posicion.y, oNave.posicion.z - .85f, true, true));
                        disparosRealizados++;
                    }
                    break;
                case 'r':
                case 'R':
                    if (!oNave.estaVivo) {
                        inicializarJuego();
                    }
                    break;
                case 'a':
                case 'A':
                    izqDown = true;
                    break;
                case 'd':
                case 'D':
                    derDown = true;
                    break;
                case 'e':
                case 'E':
                    autoMatar = true;
                    break;
                case 'z':
                    oNave.posicion.z += 1;
                    break;
                case 'Z':
                    oNave.posicion.z -= 1;
                    break;
            }
            Glut.glutPostRedisplay();
        }

        static void keyboardUp(byte key, int x, int y)
        {
            char k = (char)key;
            switch (k)
            {
                case 'a':
                case 'A':
                    izqDown = false;
                    break;
                case 'd':
                case 'D':
                    derDown = false;
                    break;
                case 'e':
                case 'E':
                    autoMatar = false;
                    break;
            }
            Glut.glutPostRedisplay();
        }

        static void onMotion(int x, int y) {
            alfa = (alfa + (y - y0));
            beta = (beta + (x - x0));
            x0 = x;
            y0 = y;
            Glut.glutPostRedisplay();

        }


        static void onMouse(int button, int state, int x, int y) {
            if ((button == Glut.GLUT_LEFT_BUTTON) && (state == Glut.GLUT_DOWN)) {
                x0 = x;
                y0 = y;
                
            }
        
        
        }
    }
}
