using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Kinect;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;
using Microsoft.Speech.Recognition;


namespace CSKinectSkeletonApplication1
{
    public partial class MainWindow : Window
    {

        //variaveis aqui!!
        
        float minYJ = -0.40f;
        float maxYJ = -0.25f;
        bool verificaRH = true;
        bool verificaLH = true;
        bool verificaAndando = false;
        int mouseX = 683;
        int mouseY = 384;
        bool modoSVR = false;
        bool verificaC = false;


        internal enum ScanCodeShort : short
        {
            KEY_ESC = 0x1B,
            KEY_0,
            KEY_1,
            KEY_2,
            KEY_3,
            KEY_4,
            KEY_5,
            KEY_6,
            KEY_7,
            KEY_8,
            KEY_9,
            KEY_A,
            KEY_B,
            KEY_C,
            KEY_D,
            KEY_E = 0x45,
            KEY_F,
            KEY_G,
            KEY_H,
            KEY_I,
            KEY_J,
            KEY_K,
            KEY_L,
            KEY_M,
            KEY_N,
            KEY_O,
            KEY_P,
            KEY_Q,
            KEY_R,
            KEY_S,
            KEY_T,
            KEY_U,
            KEY_V,
            KEY_W = 0x57,
            KEY_X,
            KEY_Y,
            KEY_Z,
            VK_SHIFT = 0x10,
            VK_SPACE = 0X20

        }

        /// <summary>
        /// Speech grammar used during game play.
        /// </summary>
        private Grammar gameGrammar;
        private const string GameSpeechRule = "welcomeRule";
        private SpeechRecognizer speechRecognizer;


        private Grammar CreateGrammar(string ruleName)
        {
            Grammar grammar;

            using (var memoryStream = new MemoryStream(Encoding.ASCII.GetBytes(Properties.Resources.SpeechGrammar)))
            {
                grammar = new Grammar(memoryStream, ruleName);
            }

            return grammar;
        }

        /// <summary>
        /// Enable specified grammar and disable all others.
        /// </summary>
        /// <param name="grammar">
        /// Grammar to be enabled. May be null to disable all grammars.
        /// </param>
        /// 
        private void EnableGrammar(Grammar grammar)
        {
            if (null != this.gameGrammar)
            {
                this.gameGrammar.Enabled = grammar == this.gameGrammar;
            }


        }

        #region importação de dll para funcionamento do mouse e teclado
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName,
            string lpWindowName);

        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

        const int VK_UP = 0x26; //up key
        const int VK_DOWN = 0x28;  //down key
        const int VK_LEFT = 0x25;
        const int VK_RIGHT = 0x27;
        const uint KEYEVENTF_KEYUP = 0x0002;
        const uint SCANCODE = 0x0008;
        const int KEY_0 = 11;
        const int VK_SHIFT = 0x10;
        const int VK_SPACE = 0X20;

       



        
        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        
        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);
        


        [DllImport("user32.dll")]
        static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, UIntPtr dwExtraInfo);

        [Flags]
        public enum MouseEventFlags
        {
            LEFTDOWN = 0X00000002,
            LEFTUP = 0X00000004,
            MIDDLEDOWN = 0X00000020,
            MIDDLEUP = 0X00000040,
            MOVE = 0X00000001,
            ABSOLUTE = 0X00008000,
            RIGHTDOWN = 0X00000008,
            RIGHTUP = 0X00000010


        }

        #endregion

        // É importante que o kinect esteja conectado, porque ele só verifica se ele tem energia ou não. 
        // E quando ele verifica ele alerta com uma MessageBox.

        KinectSensor sensor = KinectSensor.KinectSensors[0];
        byte[] pixelData;
        Skeleton[] skeletons;

        public MainWindow()
        {
            InitializeComponent();
            
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
            this.Unloaded += new RoutedEventHandler(MainWindow_Unloaded);
            

           
            try
            {
                sensor.ColorStream.Enable();
                sensor.SkeletonStream.Enable();

            }
            catch
            {
                System.Windows.MessageBox.Show("Kinect não detectado!", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                System.Environment.Exit(0);
            }
            

        }

        private void InitializeSensor(KinectSensor sensor)
        {
            if (null == sensor)
            {
                return;
            }



            this.gameGrammar = CreateGrammar(GameSpeechRule);
            this.speechRecognizer = SpeechRecognizer.Create(new[] { gameGrammar });

            if (null != speechRecognizer)
            {
                this.speechRecognizer.SpeechRecognized += SpeechRecognized;

                this.speechRecognizer.Start(sensor.AudioSource);
                
            }

            
        }

        void runtime_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            
            bool receivedData = false;

            using (SkeletonFrame SFrame = e.OpenSkeletonFrame())
            {
                if (SFrame == null)
                {
                    // The image processing took too long. More than 2 frames behind.
                }
                else
                {
                    skeletons = new Skeleton[SFrame.SkeletonArrayLength];
                    SFrame.CopySkeletonDataTo(skeletons);
                    receivedData = true;
                }
            }

            if (receivedData)
            {

                Skeleton currentSkeleton = (from s in skeletons
                                            where s.TrackingState == SkeletonTrackingState.Tracked
                                            select s).FirstOrDefault();

                if (currentSkeleton != null)
                {
                    verificaC = true;
                    
                    SetEllipsePosition(head, currentSkeleton.Joints[JointType.Head]);
                    SetEllipsePosition(leftHand, currentSkeleton.Joints[JointType.HandLeft]);
                    SetEllipsePosition(rightHand, currentSkeleton.Joints[JointType.HandRight]);
                    SetEllipsePosition(leftLeg, currentSkeleton.Joints[JointType.KneeLeft]);
                    SetEllipsePosition(rightLeg, currentSkeleton.Joints[JointType.KneeRight]);
                    SetEllipsePosition(hipcenter, currentSkeleton.Joints[JointType.HipCenter]);

                    #region Pegar os eixos das juntas (mãos, joelhos, cintura e cabeça)
                    // essa parte aqui embaixo elas capturam e mostram a posição XYZ da cabeça, mão esquerda e mão direita
                    // agora eu coloquei duas elipses nas pernas também, joelhos de preferencia
                    // o statusText são do xaml com o nome alterado para cada um dos membros
                    

                    //mão esquerda
                    Joint lhand = currentSkeleton.Joints[JointType.HandLeft];
                    SkeletonPoint lhPosition = lhand.Position;

                    string lmen = string.Format("Mão esquerda: X:{0:0.00} Y:{1:0.00} Z:{2:0.0000}",
                         lhPosition.X, lhPosition.Y, lhPosition.Z);

                    statustextLH.Text = lmen;

                    //mão direita
                    Joint rhand = currentSkeleton.Joints[JointType.HandRight];
                    SkeletonPoint rhPosition = rhand.Position;

                    string rmen = string.Format("Mão Direita: X:{0:0.0000} Y:{1:0.0000} Z:{2:0.0000}",
                         rhPosition.X, rhPosition.Y, rhPosition.Z);

                    statustextRH.Text = rmen;

                    //cabeça
                    Joint jhead = currentSkeleton.Joints[JointType.Head];
                    SkeletonPoint hPosition = jhead.Position;

                    string hmen = string.Format("Cabeça: X:{0:0.0000} Y:{1:0.0000} Z:{2:0.0000}",
                         hPosition.X, hPosition.Y, hPosition.Z);

                    statustextJH.Text = hmen;

                    //joelho esquerdo
                    Joint Lleg = currentSkeleton.Joints[JointType.KneeLeft];
                    SkeletonPoint LlPosition = Lleg.Position;

                    string llmen = string.Format("Joelho esquerdo: X:{0:0.0000} Y:{1:0.0000} Z:{2:0.0000}",
                         LlPosition.X, LlPosition.Y, LlPosition.Z);

                    statustextLK.Text = llmen;

                    //Joelho direito
                    Joint Rleg = currentSkeleton.Joints[JointType.KneeRight];
                    SkeletonPoint RlPosition = Rleg.Position;

                    string rlmen = string.Format("Joelho direito: X:{0:0.0000} Y:{1:0.0000} Z:{2:0.0000}",
                         RlPosition.X, RlPosition.Y, RlPosition.Z);

                    statustextRK.Text = rlmen;

                    //Cintura centro
                    Joint HC = currentSkeleton.Joints[JointType.HipCenter];
                    SkeletonPoint HCPosition = Rleg.Position;

                    string HCmen = string.Format("Cintura: X:{0:0.0000} Y:{1:0.0000} Z:{2:0.0000}",
                         HCPosition.X, HCPosition.Y, HCPosition.Z);

                    statustextHC.Text = HCmen;

                    // fim da captura
                    #endregion

                    #region testes do modo sem o VR
                    //SetCursorPos(mouseX += hPosition.X, mouseY += hPosition.Y);
                    //essa parte aqui é para testar a movimentação da camera sem o óculos

                    if (modoSVR)
                    {
                        int mousevelo = 2;

                        if (hPosition.X >= 0.15)
                        {
                            SetCursorPos(mouseX += mousevelo + 2, mouseY);

                        }
                        if (hPosition.X <= -0.15)
                        {
                            SetCursorPos(mouseX -= mousevelo + 2, mouseY);

                        }
                        if (hPosition.Y >= 0.46f && hPosition.Y <= 0.50f)
                        {


                        }
                        if (hPosition.Z >= 1.53f  )
                        {
                            SetCursorPos(mouseX, mouseY -= mousevelo);

                        }
                        if (hPosition.Y >= 0.30f && hPosition.Y < 0.53f)
                        {
                            SetCursorPos(mouseX, mouseY += mousevelo);

                        }



                    }

                    #endregion

                    #region detectar movimento dos braços
                    if (rhPosition.Y > 0f && verificaRH)
                    {
                        verificaRH = false;
                        moveu1.Text = "Braço direito levantado";
                        if (rhPosition.Z < 1.20f)
                        {
                            moveu1.Text = "Clique";

                            mouse_event((uint)MouseEventFlags.LEFTDOWN, 0, 0, 0, UIntPtr.Zero);

                        }

                    }
                    else if (rhPosition.Y < 0.5f)
                    {
                        verificaRH = true;

                        mouse_event((uint)MouseEventFlags.LEFTUP, 0, 0, 0, UIntPtr.Zero);
                    }

                    if (lhPosition.Y > 0f && verificaLH)
                    {
                        verificaLH = false;
                        moveu1.Text = "Braço esquerdo levantado";

                        if (lhPosition.Z < 1.20f)
                        {

                            mouse_event((uint)MouseEventFlags.RIGHTDOWN, 0, 0, 0, UIntPtr.Zero);

                        }

                    }
                    else if (lhPosition.Y < 0.5f)
                    {
                        verificaLH = true;
                        mouse_event((uint)MouseEventFlags.RIGHTUP, 0, 0, 0, UIntPtr.Zero);

                    }
                    // fim da verificação das duas mãos
                    #endregion

                    #region movimento das pernas
                    //movimento das pernas aqui, representadas pelas variaveis RlPosition e LlPosition
                    //o verificaAndando já é auto explicativo, e é uma variavel global
                    if (RlPosition.Y >= minYJ && RlPosition.Y <= maxYJ && !verificaAndando || LlPosition.Y >= minYJ && LlPosition.Y <= maxYJ && !verificaAndando)
                    {
                        keybd_event((byte)ScanCodeShort.KEY_W, 0x45, 0, 0);
                        verificaAndando = true;

                    }
                    else if (RlPosition.Y <= minYJ && LlPosition.Y <= minYJ && verificaAndando == true) 
                    {
                        keybd_event((byte)ScanCodeShort.KEY_W, 0x45, KEYEVENTF_KEYUP, 0);
                        verificaAndando = false;
                    }
                    #endregion

                    #region comentarios para fazer o pulo
                    //if (HCPosition.Y > -0.52 && jhead.Position.Y > 0)
                    //{
                    //    keybd_event((byte)ScanCodeShort.VK_SPACE, 0x45, 0, 0);
                    //}
                    //else if (HCPosition.Y < -0.40 && jhead.Position.Y < -0.1)
                    //{
                    //    keybd_event((byte)ScanCodeShort.VK_SHIFT, 0x45, 0, 0);
                    //}
                    //else
                    //{
                    //    keybd_event((byte)ScanCodeShort.VK_SHIFT, 0x45, KEYEVENTF_KEYUP, 0);
                    //    keybd_event((byte)ScanCodeShort.VK_SPACE, 0x45, KEYEVENTF_KEYUP, 0);
                    //}   
                    #endregion
                }

                    // aqui fica a parte que se o esqueleto não for detectado, as teclas param de ser pressionadas
                else
                {
                    if (verificaC == true) 
                    {
                        verificaC = false;
                        mouse_event((uint)MouseEventFlags.RIGHTUP, 0, 0, 0, UIntPtr.Zero);
                        mouse_event((uint)MouseEventFlags.LEFTUP, 0, 0, 0, UIntPtr.Zero);
                        keybd_event((byte)ScanCodeShort.KEY_W, 0x45, KEYEVENTF_KEYUP, 0);
                        SetCursorPos(683, 384);

                    }
                    moveu1.Text = "Esperando pelo Usuario";
                    


                }
            }
        }

        #region voice reconigtion
        private void SpeechRecognized(object sender, SpeechRecognizerEventArgs e)
        {
            // Semantic value associated with speech commands meant to start a new game.
            const string abririnventario = "start";
            const string fazertour = "tour";

            if (null == e.SemanticValue)
            {
                return;
            }

            // Handle game mode control commands
            switch (e.SemanticValue)
            {
                case abririnventario:
                    keybd_event((byte)ScanCodeShort.KEY_E, 0x45, 0, 0);
                    keybd_event((byte)ScanCodeShort.KEY_E, 0x45, KEYEVENTF_KEYUP, 0);
                    RespostaBox.Text = "reconhecido";
                    return;
                case fazertour:

                    return;


                
            }

            
           
        }
        #endregion

        private void SetEllipsePosition(Ellipse ellipse, Joint joint)
        {
            Microsoft.Kinect.SkeletonPoint vector = new Microsoft.Kinect.SkeletonPoint();
            vector.X = ScaleVector(500, joint.Position.X);
            vector.Y = ScaleVector(500, -joint.Position.Y);
            vector.Z = joint.Position.Z;

            Joint updatedJoint = new Joint();
            updatedJoint = joint;
            updatedJoint.TrackingState = JointTrackingState.Tracked;
            updatedJoint.Position = vector;

            Canvas.SetLeft(ellipse, updatedJoint.Position.X);
            Canvas.SetTop(ellipse, updatedJoint.Position.Y);
        }

        private float ScaleVector(int length, float position)
        {
            float value = (((((float)length) / 1f) / 2f) * position) + (length / 2);
            if (value > length)
            {
                return (float)length;
            }
            if (value < 0f)
            {
                return 0f;
            }
            return value;
        }

        void MainWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            sensor.Stop();
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            sensor.SkeletonFrameReady += runtime_SkeletonFrameReady;
            sensor.ColorFrameReady += runtime_VideoFrameReady;
            sensor.Start();
            InitializeSensor(sensor);
        }

        void runtime_VideoFrameReady(object sender, ColorImageFrameReadyEventArgs e)
        {

            bool receivedData = false;

            using (ColorImageFrame CFrame = e.OpenColorImageFrame())
            {
                if (CFrame == null)
                {
                   
                }
                else
                {
                    pixelData = new byte[CFrame.PixelDataLength];
                    CFrame.CopyPixelDataTo(pixelData);
                    receivedData = true;
                }
            }

            if (receivedData)
            {
                BitmapSource source = BitmapSource.Create(640, 480, 96, 96,
                        PixelFormats.Bgr32, null, pixelData, 640 * 4);

                videoImage.Source = source;
            }
        }


        
    }
}