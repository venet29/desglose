using Desglose.WPF;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Desglose.Anotacion;
using Desglose.Ayuda;
using Autodesk.Revit.DB;
using Desglose.DImensionNh;

namespace Desglose
{

    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    // [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    //[Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.UsingCommandData)]
    public class cmd_crearDImension : IExternalCommand
    {

        /// <summary>
        /// Implement this method as an external command for Revit.
        /// </summary>
        /// <param name="commandData">An object that is passed to the external application 
        /// which contains data related to the command, 
        /// such as the application object and active view.</param>
        /// <param name="message">A message that can be set by the external application 
        /// which will be displayed if a failure or cancellation is returned by 
        /// the external command.</param>
        /// <param name="elements">A set of elements to which the external application 
        /// can add elements that are to be highlighted in case of failure or cancellation.</param>
        /// <returns>Return the status of the external command. 
        /// A result of Succeeded means that the API external method functioned as expected. 
        /// Cancelled can be used to signify that the user cancelled the external operation 
        /// at some point. Failure should be returned if the application is unable to proceed with 
        /// the operation.</returns>
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
        {
            CrearTipoDimension _CrearTipoDimension = new CrearTipoDimension(commandData.Application, "Dimension3");
            _CrearTipoDimension.CrearTipoDimensionConTrasn_concirculo();
            return Result.Succeeded;


        }

    }
    
    
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    // [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    //[Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.UsingCommandData)]
    public class cmd_cargarDesglose : IExternalCommand
    {

        /// <summary>
        /// Implement this method as an external command for Revit.
        /// </summary>
        /// <param name="commandData">An object that is passed to the external application 
        /// which contains data related to the command, 
        /// such as the application object and active view.</param>
        /// <param name="message">A message that can be set by the external application 
        /// which will be displayed if a failure or cancellation is returned by 
        /// the external command.</param>
        /// <param name="elements">A set of elements to which the external application 
        /// can add elements that are to be highlighted in case of failure or cancellation.</param>
        /// <returns>Return the status of the external command. 
        /// A result of Succeeded means that the API external method functioned as expected. 
        /// Cancelled can be used to signify that the user cancelled the external operation 
        /// at some point. Failure should be returned if the application is unable to proceed with 
        /// the operation.</returns>
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
        {
            ManejadorWPFDesglose _ManejadorWPFDesglose = new ManejadorWPFDesglose(commandData.Application, "Desglose");
            return _ManejadorWPFDesglose.Execute();


        }

    }


    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    // [Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
    //[Autodesk.Revit.Attributes.Journaling(Autodesk.Revit.Attributes.JournalingMode.UsingCommandData)]
    public class cmd_Config : IExternalCommand
    {

        /// <summary>
        /// Implement this method as an external command for Revit.
        /// </summary>
        /// <param name="commandData">An object that is passed to the external application 
        /// which contains data related to the command, 
        /// such as the application object and active view.</param>
        /// <param name="message">A message that can be set by the external application 
        /// which will be displayed if a failure or cancellation is returned by 
        /// the external command.</param>
        /// <param name="elements">A set of elements to which the external application 
        /// can add elements that are to be highlighted in case of failure or cancellation.</param>
        /// <returns>Return the status of the external command. 
        /// A result of Succeeded means that the API external method functioned as expected. 
        /// Cancelled can be used to signify that the user cancelled the external operation 
        /// at some point. Failure should be returned if the application is unable to proceed with 
        /// the operation.</returns>
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
        {
            ManejadorWPFDesglose _ManejadorWPFDesglose = new ManejadorWPFDesglose(commandData.Application, "Configuracion");
            return _ManejadorWPFDesglose.Execute();
            //ManejadorConfiguracionDesglose.cargar(commandData.Application, true);
            //return Result.Succeeded;


        }

    }


    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    public class cmd_CrearAnno : IExternalCommand
    {

        /// <summary>
        /// Implement this method as an external command for Revit.
        /// </summary>
        /// <param name="commandData">An object that is passed to the external application 
        /// which contains data related to the command, 
        /// such as the application object and active view.</param>
        /// <param name="message">A message that can be set by the external application 
        /// which will be displayed if a failure or cancellation is returned by 
        /// the external command.</param>
        /// <param name="elements">A set of elements to which the external application 
        /// can add elements that are to be highlighted in case of failure or cancellation.</param>
        /// <returns>Return the status of the external command. 
        /// A result of Succeeded means that the API external method functioned as expected. 
        /// Cancelled can be used to signify that the user cancelled the external operation 
        /// at some point. Failure should be returned if the application is unable to proceed with 
        /// the operation.</returns>
        public Autodesk.Revit.UI.Result Execute(ExternalCommandData commandData, ref string message, Autodesk.Revit.DB.ElementSet elements)
        {
            Document _doc = commandData.Application.ActiveUIDocument.Document;
            AnotacionMultipleBarra _AnotacionMultipleBarra = new AnotacionMultipleBarra(commandData.Application, commandData.Application.ActiveUIDocument.ActiveView, -1);

            var lista = CrearListaPtos.M2_ListaPtoSimple(commandData.Application, 2);
            if (lista.Count == 0) return Result.Cancelled;

            List<ElementId> listat = new List<ElementId>() {
            new ElementId(493875),new ElementId(493926)  ,new ElementId(493896)
            };

            AnotacionMultipleBarraDTO _AnotacionMultipleBarraDTO = new AnotacionMultipleBarraDTO()
            {
                Origen_ = lista[0],
                taghead_ = lista[1],
                nombrefamilia = "MRA Rebar Section"
            };
            _AnotacionMultipleBarra.CreateAnnotation(listat, _AnotacionMultipleBarraDTO);
            return Result.Succeeded;


        }

    }


}
