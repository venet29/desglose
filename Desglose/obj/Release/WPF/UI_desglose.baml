   M S B A M L   `   `   `  ���� C  ?Desglose, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:Desglose.enumNhDesglose.enumNh  O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem     Desglose.WPF.UI_desglose   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     0local1clr-namespace:Desglose.enumNh     9System&clr-namespace:System;assembly=mscorlib     -   $��DebugUtility��  � Title$  Armadura Elevaciones��$	��310��$	��380�� � WindowStartupLocation$ CenterScreen=�
�������
�������$��12(� � Icon$7 1/Desglose;component/Resources/Revit-icon_DESG.png��$	��310��$	��310��$	��380��$	��380��.���  � RowDefinitions �� $
��152* ��� $
��169* � � ColumnDefinitions �� $
��243* ��� $	��38* ��� $	��91* �.+��� ���	#FFACACAC$��1q�$��32��$	��Top=�$��0,0,0,0q�$��3���� -    ��Name$ tabnh��$
��Left=�$��0��$��0��$��2��$��0,0,0,0q�$	��352��$��3����Z� ��Z� .@��� -    ��Name$ tbDatos��$��
0,-2,-4,-2q�$��5,0,5,0q� �� 
IsSelected �� *IsTabActivaDesglose+ �� UpdateSourceTrigger$ PropertyChanged=�y��� *IsVisibleDesglose+$ PropertyChanged=����� *
AnchoTab+$ PropertyChanged=����� 	 �� Orientation$	 
Horizontal=�.+��� $��Crear Barras��.��� .+��� -   
 ��Name$
 Datos��$��0��$��2��$��1��$��Datos��$��	10,5,10,5q�.���  �� $��10 ��� $��1* ��� $��1* ��� $��1* ��� $��1* ��� $��20 � �� $��10 ��� $��95 ��� $��90 ��� $��10 ��� $��1* ��� $��10 �.+��� $��1��$��1��.��Caso�� -   $��cbx_CasoElev��$��1��$��2��$��3��$��20��$	��186��$��Center=�$
��Left=�.@���  �� 
IsSelected . .��Elemento Vertical�� .��Elemento Horizontal�� $��3��$��1��$��Center=�y��� *IsVisibleElevVertical+$ PropertyChanged=�.��Espaciamiento�� $��4��$��1��$��Center=�y��� *IsVisibleElevVertical+$ PropertyChanged=�.��Nombre�� -   $��chb_id��$��1��$��2��$��Center=�$��Center=�$��Agregar ID?�� � ToolTip$ 	Agrgar ID��y��� *IsVisibleElevVertical+$ PropertyChanged=��� -    ��Name$ dtEsp��$��3��$��2��$��30��$��Center=�$��50�� �� TextAlignment$ Center=�$��Center=�y��� *IsVisibleElevVertical+$ PropertyChanged=��� -   $ dtNombre��$��4��$��2��$��30��$��A��$ Center=�$��Center=�y��� *IsVisibleElevVertical+$ PropertyChanged=��� -	    ��Name$ btnGenerar_Elev��$��3��$��4��$��25��$��80��$��Generar��$ Crear barras de elevacion�����	#FFF9F9F9�� -
   $ btnActualizar��$��4��$��4��$��25��$��80��$��
Actualizar��$A ;Actualiza datos de pathReinforment previamente seleccionado�����	#FFF9F9F9y��� *IsVisibleElevVertical+$ PropertyChanged=��� $��Ø Max Lat ��$
��Left=�$��Center=�$��1��$��3��$I CDiamtro maximo de los laterales,para diferencial de barras de borde��y��� *IsVisibleElevHorizontal+$ PropertyChanged=��� -   $��cbx_diamMaxLatElevHor��$��3�� �� Text$ 12��$��2��$��20��$��90��$��Center=� �� 
IsEditable . y��� *IsVisibleElevHorizontal+$ PropertyChanged=�����  �� Path$ 	ListaDiam��� -   $ 
Editconfig��$��
0,-2,-4,-2q�$��5,0,5,0q�y��� *IsVisibleDesglose+$ PropertyChanged=����� *
AnchoTab+$ PropertyChanged=����� $	 
Horizontal=�.+��� $��Generar corte��.���� -   $
 pathpTH��$��0��$��0��$��2��$��Datos��$��10,5,9,5q�.��� $��0,0,0,1q� �� $��5 ��� $
��1.1* ��� $��1* ��� $��1* ��� $��1* ��� $��1* ��� $��1* ��� $��5 � �� $��80 ��� $��90 ��� $��10 ��� $��1* ��� $��20 �.+��� $��1��$��0��.��Caso�� -   $��cbx_CasoCorte��$��1��$��1��$��3��$��20��$	��213��$��Center=�$
��Left=�.@���  . .��Elemento Vertical�� .��Elemento Horizontal�� $��2��$��0��.��Nombre�� -   $ dtnameCorte��$��2��$��1��$��3��$��Seccion��$ Center=�$��0,1,1,5q�$��Center=��� -   $��	GenCorteV��$��6��$��3��$��25��$	��100��$��Right=�$��Generar Corte�����	#FFF9F9F9�� $��Largo��$
��Left=�$��Center=�$��0��$��3���� -   $��cbx_TipoLargo��$��3��$��1��$��20��$��90��$��Center=�.@���  . .��Normal�� .��	Aprox 5cm�� .��
Aprox 10cm�� $	 
Horizontal=�$��3��$��3��y��� *IsVisibleTolerancia+$ PropertyChanged=�.+��� $��
tolerancia���� -   $ dtTole��$��51��$��20��$��50��$ Center=�$��Center=��� $��Cuantia Trans��$
��Left=�$��Center=�$��0��$��4��$% Cuantia de estribos trasnversal���� -   $��cbx_tipocuantia��$��4��$��1��$��20��$��90��$��Center=�.@���  . .��Normal�� .��	Definir�� $	 
Horizontal=�$��3��$��4��y��� *IsVisibleToleranciaCuantia+$ PropertyChanged=�.+��� $��Texto���� -   $ dtTextCuantia��$��75��$��25��$��E.Ø S/Elev��$ Center=�$��Center=��� $��Cuantia Long ��$
��Left=�$��Center=�$��0��$��5��$# Cuantia de barra longitudinal��y��� *IsVisibleCorteHorizontal+$ PropertyChanged=��� -   $��cbx_tipocuantiaLong��$��5��$��1��$��20��$��90��$��Center=�y��� *IsVisibleCorteHorizontal+$ PropertyChanged=�.@���  . .��Normal�� .��	Definir�� $	 
Horizontal=�$��3��$��5��y��� *IsVisibleCorteHorizontal+$ PropertyChanged=�.+��� $��Ø Max Lat ��$
��Left=�$��Center=�$��0��$��5��$I CDiamtro maximo de los laterales,para diferencial de barras de borde��y��� *IsVisibleCorteHorizontal+$ PropertyChanged=��� -   $��cbx_diamMaxLat��$��5��$ 12��$��1��$��20��$��40��$��Center=� . y��� *IsVisibleCorteHorizontal+$ PropertyChanged=����� $ 	ListaDiam��� -   $ EditPAth��$��
0,-2,-4,-2q�$��5,0,5,0q� �� *IsTabActivaCOnfig+$ PropertyChanged=���Z� ��Z� y��� *IsVisibleConfiguracion+$ PropertyChanged=����� *AnchoTabConfig+$ PropertyChanged=����� $	 
Horizontal=�.+��� $��Configuración��.���� -   $
 
pathconifg��$��0��$��0��$��2��$��Datos��$��	10,5,10,5q�.��� $��0,0,0,1q� �� $��10 ��� $��1* ��� $��1* ��� $��1* ��� $��1* ��� $��10 � �� $��20 ��� $��1* ��� $��20 �.+��� -   $��Bton_Cargarconfig��$��1��$��1��$��Center=�$��25��$	��140��$��Cargar configuraciones�����	#FFAEA0A0���	#FFF9F9F9�� -   $��Bton_RecargarFamilias��$��2��$��1��$��Center=�$��25��$	��140��$��Recargar Familias�����	#FFF9F9F9�� -   $��Bton_BorrarFamilia��$��3��$��1��$��Center=�$��25��$	��140��$��Borrar Familias�����	#FFF9F9F9�� -   $��Bton_BorrarConfi��$��4��$��1��$��Center=�$��25��$	��140��$��Borrar Configuracion�����	#FFF9F9F9�� -   $ 
BorrarPAth��$��
0,-2,-4,-2q�$��5,0,5,0q� �� *IsTabActivaBorrar+$ PropertyChanged=���Z� ��Z� y��� *IsVisibleBorrar+$ PropertyChanged=����� $	 
Horizontal=�.+��� $��Borrar��.���� -   $
 
pathBorrar��$��0��$��0��$��2��$��Datos��$��	10,5,10,5q�.��� $��0,0,0,1q� �� $��10 ��� $��1* ��� $��1* ��� $��1* ��� $��1* ��� $��10 � �� $��20 ��� $��1* ��� $��20 �.+��� -   $��Bton_BorrarElev��$��2��$��1��$��Center=�$	��Top=�$��30��$	��140��$��Borrar Elevaciones�����	#FFAEA0A0���	#FFF9F9F9�� -    $��Bton_BorrarCorte��$��3��$��1��$��Center=�$��Bottom=�$��30��$	��140��$��Borrar Cortes�����	#FFF9F9F9