using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desglose.Ayuda
{
	public enum direccionBarra
	{
		Horizontal,Vertical,NONE

	}


    public enum CasoAnalisas
    {
        AnalisisHorizontal, AnalsisVertical, NONE

    }

    public enum TipoTagBArraHorizontalENcorte
    {
       segunElevacion , mostrarDiamtro

    }
    public enum TipobarraH
    {
        Lateral,
        Linea1SUP, Linea2SUP, Linea3SUP, Linea4SUP, Linea5SUP,
        Linea1INF, Linea2INF, Linea3INF, Linea4INF, Linea5INF,
        NONE
    }

    public enum OrientacionBArra
    {
        Horizontal, Vertical, NONE

    }

    public enum casoAgrupar
    {
        Principal,Repetido,NoAnalizada

    }
    public enum TipoRebarElev
    {
        Sinpata,PataInferior,PataSuperior, AmbasPata,Estribo,EstriboTraba,
        NONE,
        EstriboViga,
        EstriboVigaTraba
    }
    public enum TipoCUrva
    {
        linea,arco

    }
    public enum TipoCOnfLargo
    {
        normal,Aprox5,Aprox10

    }
    public enum TipoCOnfCuantia
    {
        normal, SegunPlano

    }
    public enum direccion
    {
        planoHorizontal,
        planovertical
    }
    public enum DireccionNormal
    {
        entrandoEnView = 1,
        paraleloAView,
    }
}
