using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DRCars.Controls;

namespace DRCars
{
    public partial class Form1 : Form
    {
        // Panel principal donde cargaremos los UserControls
        private Panel mainContainer;

        // Control actual mostrado
        private UserControl currentControl;

        // Colores de la aplicación
        public static Color BaseColor = Color.FromArgb(44, 53, 57);      // Gris carbón #2C3539
        public static Color SecondaryColor = Color.FromArgb(192, 192, 192); // Plata #C0C0C0
        public static Color AccentColor = Color.FromArgb(8, 146, 208);    // Azul eléctrico #0892D0
        public static Color TextColor = Color.FromArgb(255, 255, 255);    // Blanco #FFFFFF

        public Form1()
        {
            InitializeComponent();
            InitializeCustomComponents();

            // Eliminar bordes de la ventana y configurar estilo moderno
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "DRCars - Gestión de Vehículos de Lujo";

            // Aumentar el tamaño de la ventana
            this.Size = new Size(1280, 800);

            // Habilitar DoubleBuffered para evitar parpadeos
            this.DoubleBuffered = true;

            // Cargar el dashboard como pantalla inicial
            LoadUserControl(new Dashboard());
        }

        private void InitializeCustomComponents()
        {
            // Configurar el panel principal con DoubleBuffered
            mainContainer = new BufferedPanel
            {
                Dock = DockStyle.Fill,
                BackColor = BaseColor
            };

            // Crear panel superior para la barra de título personalizada
            Panel titleBar = new BufferedPanel
            {
                Dock = DockStyle.Top,
                Height = 50,
                BackColor = Color.FromArgb(30, 39, 41), // Un poco más oscuro que el color base
            };

            // Botón de cierre
            RoundedButton closeButton = new RoundedButton
            {
                Size = new Size(40, 40),
                FlatStyle = FlatStyle.Flat,
                Text = "✕",
                ForeColor = TextColor,
                Font = new Font("Segoe UI", 12),
                Dock = DockStyle.Right,
                Cursor = Cursors.Hand,
                Margin = new Padding(5),
                BackColor = Color.FromArgb(30, 39, 41),
                BorderRadius = 20
            };
            closeButton.FlatAppearance.BorderSize = 0;
            closeButton.Click += (s, e) => Application.Exit();

            // Botón de minimizar
            RoundedButton minimizeButton = new RoundedButton
            {
                Size = new Size(40, 40),
                FlatStyle = FlatStyle.Flat,
                Text = "—",
                ForeColor = TextColor,
                Font = new Font("Segoe UI", 12),
                Dock = DockStyle.Right,
                Cursor = Cursors.Hand,
                Margin = new Padding(5),
                BackColor = Color.FromArgb(30, 39, 41),
                BorderRadius = 20
            };
            minimizeButton.FlatAppearance.BorderSize = 0;
            minimizeButton.Click += (s, e) => this.WindowState = FormWindowState.Minimized;

            // Título de la aplicación
            Label titleLabel = new Label
            {
                Text = "DRCars",
                ForeColor = AccentColor,
                Font = new Font("Segoe UI Semibold", 16),
                AutoSize = false,
                Size = new Size(200, 50),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0),
                Dock = DockStyle.Left
            };

            // Agregar controles a la barra de título
            titleBar.Controls.Add(closeButton);
            titleBar.Controls.Add(minimizeButton);
            titleBar.Controls.Add(titleLabel);

            // Crear panel lateral para navegación
            Panel sidePanel = new BufferedPanel
            {
                Width = 250,
                Dock = DockStyle.Left,
                BackColor = Color.FromArgb(35, 42, 45), // Un poco más oscuro que el color base
                Padding = new Padding(0, 0, 0, 20)
            };

            // Agregar logo o imagen en la parte superior del panel lateral
            PictureBox logoPictureBox = new PictureBox
            {
                Size = new Size(250, 150),
                BackColor = Color.FromArgb(35, 42, 45),
                Image = null, // Aquí cargaríamos el logo
                SizeMode = PictureBoxSizeMode.CenterImage,
                Dock = DockStyle.Top
            };
            sidePanel.Controls.Add(logoPictureBox);

            // Agregar botones de navegación al panel lateral
            AddNavigationButton(sidePanel, "Dashboard", () => LoadUserControl(new Dashboard()));
            AddNavigationButton(sidePanel, "Inventario", () => LoadUserControl(new InventoryControl()));
            AddNavigationButton(sidePanel, "Añadir Vehículo", () => LoadUserControl(new AddVehicleControl()));
            AddNavigationButton(sidePanel, "Estadísticas", () => LoadUserControl(new StatisticsControl()));

            // Agregar los paneles principales al formulario
            this.Controls.Add(mainContainer);
            this.Controls.Add(sidePanel);
            this.Controls.Add(titleBar);

            // Hacer que el formulario sea arrastrable desde la barra de título
            titleBar.MouseDown += (s, e) => {
                if (e.Button == MouseButtons.Left)
                {
                    const int WM_NCLBUTTONDOWN = 0xA1;
                    const int HT_CAPTION = 0x2;

                    var releaseCapture = typeof(User32).GetMethod("ReleaseCapture");
                    var sendMessage = typeof(User32).GetMethod("SendMessage");

                    releaseCapture.Invoke(null, null);
                    sendMessage.Invoke(null, new object[] { this.Handle, WM_NCLBUTTONDOWN, (IntPtr)HT_CAPTION, IntPtr.Zero });
                }
            };
        }

        // Método para agregar botones de navegación
        private void AddNavigationButton(Panel container, string text, Action clickAction)
        {
            RoundedButton button = new RoundedButton
            {
                Text = text,
                Size = new Size(220, 50),
                FlatStyle = FlatStyle.Flat,
                ForeColor = TextColor,
                Font = new Font("Segoe UI", 12),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(20, 0, 0, 0),
                Margin = new Padding(15, 10, 15, 0),
                Cursor = Cursors.Hand,
                BackColor = Color.FromArgb(40, 48, 52),
                BorderRadius = 10,
                Dock = DockStyle.Top
            };

            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(50, AccentColor.R, AccentColor.G, AccentColor.B);
            button.FlatAppearance.MouseDownBackColor = Color.FromArgb(70, AccentColor.R, AccentColor.G, AccentColor.B);

            button.Click += (s, e) => clickAction();

            container.Controls.Add(button);
        }

        // Método para cargar un UserControl en el contenedor principal con animación
        public void LoadUserControl(UserControl userControl)
        {
            if (currentControl != null)
            {
                // Animación de salida para el control actual
                Timer fadeOutTimer = new Timer { Interval = 10 };
                int opacity = 100;

                fadeOutTimer.Tick += (s, e) => {
                    opacity -= 5;
                    if (opacity <= 0)
                    {
                        fadeOutTimer.Stop();
                        mainContainer.Controls.Remove(currentControl);
                        currentControl.Dispose();

                        // Mostrar el nuevo control con animación de entrada
                        ShowNewControl(userControl);
                    }
                    else
                    {
                        // En lugar de usar Opacity directamente, modificamos la transparencia del BackColor
                        currentControl.BackColor = Color.FromArgb((opacity * 255) / 100,
                                                         currentControl.BackColor.R,
                                                         currentControl.BackColor.G,
                                                         currentControl.BackColor.B);
                    }
                };

                fadeOutTimer.Start();
            }
            else
            {
                // Si no hay control actual, mostrar el nuevo directamente
                ShowNewControl(userControl);
            }
        }

        private void ShowNewControl(UserControl userControl)
        {
            // Configurar el nuevo control
            userControl.Dock = DockStyle.Fill;
            // Iniciar con color transparente
            userControl.BackColor = Color.FromArgb(0,
                                          Form1.BaseColor.R,
                                          Form1.BaseColor.G,
                                          Form1.BaseColor.B);

            mainContainer.Controls.Add(userControl);
            currentControl = userControl;

            // Animación de entrada
            Timer fadeInTimer = new Timer { Interval = 10 };
            int opacity = 0;

            fadeInTimer.Tick += (s, e) => {
                opacity += 5;
                if (opacity >= 100)
                {
                    fadeInTimer.Stop();
                    userControl.BackColor = Form1.BaseColor; // Restaurar color original
                }
                else
                {
                    // Modificar la transparencia del BackColor
                    userControl.BackColor = Color.FromArgb((opacity * 255) / 100,
                                                  Form1.BaseColor.R,
                                                  Form1.BaseColor.G,
                                                  Form1.BaseColor.B);
                }
            };

            fadeInTimer.Start();
        }
    }

    // Clase para importar funciones nativas de Windows para mover el formulario
    internal static class User32
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
    }
}
