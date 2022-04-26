﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Desglose.Ayuda;
using Desglose.DTO;
using Desglose.enumNh;

namespace Desglose.WPF
{
    /// <summary>
    /// Interaction logic for UI.xaml
    /// </summary>
    public partial class UI_desglose : Window, INotifyPropertyChanged
    {

        public string BotonOprimido { get; set; }
        private readonly Document _doc;

        private UIApplication _uiApp;
        public ObservableCollection<float> ListaDiam { get; set; }
        public ObservableCollection<string> ListaEstribo { get; set; }

        public TipoBarraTraslapoDereArriba tipoBarra { get; set; }


        private string _id;

        public string _Id
        {
            get { return _id; }
            set
            {
                if (_id != value)
                {
                    _id = value;
                    RaisePropertyChanged("_Id");
                }
            }
        }


        private string orientacion;

        public string Orientacion
        {
            get { return orientacion; }
            set
            {
                if (orientacion != value)
                {
                    orientacion = value;
                    RaisePropertyChanged("Orientacion");
                }
            }
        }

        private System.Windows.Visibility isVisibleTolerancia;
        public System.Windows.Visibility IsVisibleTolerancia
        {
            get { return isVisibleTolerancia; }
            set
            {
                if (isVisibleTolerancia != value)
                {
                    isVisibleTolerancia = value;
                    RaisePropertyChanged("IsVisibleTolerancia");
                }
            }
        }

        private System.Windows.Visibility isVisibleToleranciaCuantia;
        public System.Windows.Visibility IsVisibleToleranciaCuantia
        {
            get { return isVisibleToleranciaCuantia; }
            set
            {
                if (isVisibleToleranciaCuantia != value)
                {
                    isVisibleToleranciaCuantia = value;
                    RaisePropertyChanged("IsVisibleToleranciaCuantia");
                }
            }
        }


        private System.Windows.Visibility isVisibleElevHorizontal;
        public System.Windows.Visibility IsVisibleElevHorizontal
        {
            get { return isVisibleElevHorizontal; }
            set
            {
                if (isVisibleElevHorizontal != value)
                {
                    isVisibleElevHorizontal = value;
                    RaisePropertyChanged("IsVisibleElevHorizontal");
                }
            }
        }


        private System.Windows.Visibility isVisibleCorteHorizontal;
        public System.Windows.Visibility IsVisibleCorteHorizontal
        {
            get { return isVisibleCorteHorizontal; }
            set
            {
                if (isVisibleCorteHorizontal != value)
                {
                    isVisibleCorteHorizontal = value;
                    RaisePropertyChanged("IsVisibleCorteHorizontal");
                }
            }
        }


        private System.Windows.Visibility isVisibleElevVertical;
        public System.Windows.Visibility IsVisibleElevVertical
        {
            get { return isVisibleElevVertical; }
            set
            {
                if (isVisibleElevVertical != value)
                {
                    isVisibleElevVertical = value;
                    RaisePropertyChanged("IsVisibleElevVertical");
                }
            }
        }


        private string espaciamiento;
        public string Espaciamiento
        {
            get { return espaciamiento; }
            set
            {
                if (espaciamiento != value)
                {
                    espaciamiento = value;
                    RaisePropertyChanged("Espaciamiento");
                }
            }
        }

        private string diametro;
        public string Diametro
        {
            get { return diametro; }
            set
            {
                if (diametro != value)
                {
                    diametro = value;
                    RaisePropertyChanged("Diametro");
                }
            }
        }




        private string _tipoDireccionBarra;
        public string _TipoDireccionBarra
        {
            get { return _tipoDireccionBarra; }
            set
            {
                if (_tipoDireccionBarra != value)
                {
                    _tipoDireccionBarra = value;
                    RaisePropertyChanged("_TipoDireccionBarra");
                }
            }
        }



        private TipoBarraTraslapoDereArriba tipoBarra_;
        public TipoBarraTraslapoDereArriba TipoBarra_
        {
            get { return tipoBarra_; }
            set
            {
                if (tipoBarra_ != value)
                {
                    tipoBarra_ = value;
                    RaisePropertyChanged("TipoBarra_");
                }
            }
        }




        public System.Windows.Visibility IsVisibleDesglose { get; set; }
        public System.Windows.Visibility IsVisibleConfiguracion { get; set; }
        public int AnchoTab { get; set; }
        public bool IsTabActivaDesglose { get; set; }
        public bool IsTabActivaCOnfig { get; set; }
        //private readonly UIApplication _uiApp;
        //private readonly Autodesk.Revit.ApplicationServices.Application _app;
        private readonly UIDocument _uiDoc;

        private readonly EventHandlerWithStringArg _mExternalMethodStringArg;
        private readonly EventHandlerWithWpfArg _mExternalMethodWpfArg;
        //  public SeleccionarPathReinfomentConPto _seleccionarPathReinfomentConPto { get; set; }
        public UI_desglose(UIApplication uiApp, EventHandlerWithStringArg evExternalMethodStringArg, EventHandlerWithWpfArg eExternalMethodWpfArg, string caso)
        {
            _uiApp = uiApp;
            _uiDoc = uiApp.ActiveUIDocument;
            _doc = _uiDoc.Document;
            //_app = _doc.Application;
            //_uiApp = _doc.Application;
            Closed += MainWindow_Closed;

            InitializeComponent();
            this.DataContext = this;
            _mExternalMethodStringArg = evExternalMethodStringArg;
            _mExternalMethodWpfArg = eExternalMethodWpfArg;

            this.Topmost = true;



            IsVisibleTolerancia = System.Windows.Visibility.Hidden;
            IsVisibleToleranciaCuantia = System.Windows.Visibility.Hidden;


            IsVisibleElevVertical = System.Windows.Visibility.Visible;
            IsVisibleElevHorizontal = System.Windows.Visibility.Hidden;

            IsVisibleCorteHorizontal = System.Windows.Visibility.Hidden;
            ListaDiam = new ObservableCollection<float>() { 8, 10, 12, 16, 18, 22, 25, 28, 32, 36 };

            ListaEstribo = new ObservableCollection<string>() { "Derecha", "Izquierda", "Superior", "Inferior" };

            if (caso == "Desglose")
            {
                IsVisibleDesglose = System.Windows.Visibility.Visible;
                IsVisibleConfiguracion = System.Windows.Visibility.Hidden;
                IsTabActivaDesglose = true;
                IsTabActivaCOnfig = false;
                AnchoTab = 100;
            }
            else if (caso == "Configuracion")
            {
                IsVisibleDesglose = System.Windows.Visibility.Hidden;
                IsVisibleConfiguracion = System.Windows.Visibility.Visible;
                AnchoTab = 0;
                IsTabActivaDesglose = false;
                IsTabActivaCOnfig = true;
            }
        }


        private void MainWindow_Closed(object sender, EventArgs e)
        {
            Close();
        }



        #region External Project Methods

        private void BExtString_Click(object sender, RoutedEventArgs e)
        {
            // Raise external event with a string argument. The string MAY
            // be pulled from a Revit API context because this is an external event
            _mExternalMethodStringArg.Raise($"Title: {_doc.Title}");
        }

        private void BExternalMethod1_Click(object sender, RoutedEventArgs e)
        {
            if (cbx_TipoLargo.Text == "Aprox 5cm")
            {
                if (!verificar(dtTole.Text, 5.0))
                    return;


            }
            else if (cbx_TipoLargo.Text == "Aprox 10cm")
            {
                if (!verificar(dtTole.Text, 10.0))
                    return;
            }

            Button bton = (Button)sender;

            BotonOprimido = bton.Name;
            // Raise external event with this UI instance (WPF) as an argument
            _mExternalMethodWpfArg.Raise(this);
        }


        private bool verificar(string tole, double referencia)
        {

            if (!Util.IsNumeric(tole))
            {
                Util.ErrorMsg($"La tolerancia debe ser un valor mayor que 0 y menor que {referencia}");
                return false;
            }
            var valor = Util.ConvertirStringInDouble(tole);
            if (valor < 0 || valor > referencia)
            {
                Util.ErrorMsg($"La tolerancia debe ser un valor mayor que 0 y menor que {referencia}");
                return false;
            }
            return true;
        }
        #endregion

        #region Non-External Project Methods



        public Config_EspecialCorte ObtenerConfiguraEspecialCOrte()
        {
            TipoCOnfLargo _TipoCOnfLargo = TipoCOnfLargo.normal;
            TipoCOnfCuantia _TipoCOnfCuantia = TipoCOnfCuantia.normal;

            if (cbx_TipoLargo.Text == "Aprox 5cm")
                _TipoCOnfLargo = TipoCOnfLargo.Aprox5;
            else if (cbx_TipoLargo.Text == "Aprox 10cm")
                _TipoCOnfLargo = TipoCOnfLargo.Aprox10;


            List<ParametroShareNhDTO> listaPAra = new List<ParametroShareNhDTO>();
            if (cbx_tipocuantia.Text == "Definir")
            {
                _TipoCOnfCuantia = TipoCOnfCuantia.SegunPlano;
                ParametroShareNhDTO _newParaMe = new ParametroShareNhDTO()
                {
                    Isok = true,
                    NombrePAra = "OpcionCuantia",
                    valor = dtTextCuantia.Text
                };
                listaPAra.Add(_newParaMe);
            }

            Config_EspecialCorte _Config_EspecialCorte = new Config_EspecialCorte()
            {
                TipoCOnfigLargo = _TipoCOnfLargo,
                TipoCOnfigCuantia = _TipoCOnfCuantia,
                ListaPAraShare = listaPAra,
                tolerancia = Util.ConvertirStringInDouble(dtTole.Text),
                TipoCasoAnalisis = (cbx_CasoCorte.Text.Contains("Vertical") ? CasoAnalisas.AnalsisVertical : CasoAnalisas.AnalisisHorizontal),
                DiamtroLateralMax = Util.ConvertirStringInInteger(cbx_diamMaxLat.Text), // falta interfase
                ParaBarraHorizontalEnCorteViga = (cbx_tipocuantiaLong.Text == "Definir" ? TipoTagBArraHorizontalENcorte.segunElevacion : TipoTagBArraHorizontalENcorte.mostrarDiamtro), // falta interfase
                dire = -1
            };

            return _Config_EspecialCorte;
        }

        internal Config_EspecialElev ObtenerConfiguraEspecialElev()
        {
            List<ParametroShareNhDTO> listaPAra = new List<ParametroShareNhDTO>();
            Config_EspecialElev _Config_EspecialElev = new Config_EspecialElev();

            char ch = char.Parse(dtNombre.Text);

            if (!char.IsLetter(ch))
            {
                Util.ErrorMsg("Error al obtener letra de grupo");
                _Config_EspecialElev = new Config_EspecialElev()
                {
                    ListaPAraShare = new List<ParametroShareNhDTO>()
                };
                return _Config_EspecialElev;
            }

            ParametroShareNhDTO _newParaMe = new ParametroShareNhDTO()
            {
                Isok = true,
                NombrePAra = "MLB",
                valor = dtNombre.Text
            };


            _Config_EspecialElev = new Config_EspecialElev()
            {
                tipoBarraElev_parameterShare = _newParaMe,
                IsAgregarId = ((bool)chb_id.IsChecked ? true : false),
                TipoCasoAnalisis = (cbx_CasoElev.Text.Contains("Vertical") ? CasoAnalisas.AnalsisVertical : CasoAnalisas.AnalisisHorizontal),
                DiamtroLateralMax = Util.ConvertirStringInInteger(cbx_diamMaxLatElevHor.Text)
            };

            return _Config_EspecialElev;
        }

        #endregion


        //**********************************************************************************************************
        #region codigo INotifyPropertyChanged


        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        private void Cbx_TipoLargo_DropDownClosed(object sender, EventArgs e)
        {

            if (cbx_TipoLargo.Text == "Normal")
            {
                IsVisibleTolerancia = System.Windows.Visibility.Hidden;
            }
            else if (cbx_TipoLargo.Text == "Aprox 5cm")
            {
                IsVisibleTolerancia = System.Windows.Visibility.Visible;
                dtTole.Text = "2.5";
            }
            else if (cbx_TipoLargo.Text == "Aprox 10cm")
            {
                IsVisibleTolerancia = System.Windows.Visibility.Visible;
                dtTole.Text = "5";
            }
        }

        private void Cbx_tipocuantia_DropDownClosed(object sender, EventArgs e)
        {
            if (cbx_tipocuantia.Text == "Normal")
            {
                IsVisibleToleranciaCuantia = System.Windows.Visibility.Hidden;
            }
            else
            {
                IsVisibleToleranciaCuantia = System.Windows.Visibility.Visible;
                dtTextCuantia.Text = "E.Ø S/Elev";
            }
        }

        private void Cbx_CasoElev_DropDownClosed(object sender, EventArgs e)
        {
            if (cbx_CasoElev.Text == "Elemento Vertical")
            {
                IsVisibleElevVertical = System.Windows.Visibility.Visible;
                IsVisibleElevHorizontal = System.Windows.Visibility.Hidden;
            }
            else
            {
                IsVisibleElevVertical = System.Windows.Visibility.Hidden;
                IsVisibleElevHorizontal = System.Windows.Visibility.Visible;
            }
        }

        private void Cbx_CasoCorte_DropDownClosed(object sender, EventArgs e)
        {
            if (cbx_CasoCorte.Text == "Elemento Horizontal")
                IsVisibleCorteHorizontal = System.Windows.Visibility.Visible;
            else
                IsVisibleCorteHorizontal = System.Windows.Visibility.Hidden;
        }
    }


}