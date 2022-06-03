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
        NONE,
        LineaNOLateral
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
        Sinpata,PataInferior,PataSuperior, AmbasPata,
        SinpataH, PataInferiorH, PataSuperiorH, AmbasPataH,// para vigS en elevacion
        Estribo_ColumnaCorte,EstriboTraba_ColumnaCorte,
        NONE,
        Estribo_VigaCorte, EstriboVigaLatelaElev, EstriboTraba_VigaCorte,
        EstriboVigaElv,
        EstriboVigaTrabaElev
    }

    //v : significa 90 grados
    //H : significa entre  entre 89 y -89
    public enum CasoRebar
    {
        SinpataV, PataInferiorV, PataSuperiorV, AmbasPataV,
        SinpataH, PataInferiorH, PataSuperiorH, AmbasPataH,// para vigS en elevacion
        EstriboV, EstriboLateralV, EstriboTrabaV, 
        EstriboH, EstriboLateralH, EstriboTrabaH,
        NONE
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
