using AplicacionProspectos.BD;
using AplicacionProspectos.Entidades;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace AplicacionProspectos
{
    public partial class _Default : Page
    {
        ConexionBD db = new ConexionBD();
        protected void Page_Load(object sender, EventArgs e)
        {

            //ViewState["pantallaActual"] = (int)ViewState["pantallaActual"] == 0 || ViewState["pantallaActual"]  == null ? 0 : (int)ViewState["pantallaActual"];
            if (!Page.IsPostBack)
            {
                inicializarValores();
            }

            //Si es mayor que 0 entonces esta en la pantalla de evaludacion, se desactiva el boton, esto es por que el boton btnAgregarDocumentosModal se activa al ejecutarse el postback
            /*if ((int)ViewState["pantallaActual"] > 0) { 
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "javascript: inhabilitarBoton(true);", true);
            }*/
        }
        public void cargarImagen()
        {
           //db.cargarDocumento();
        }
        

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            //Agregar el modal para agregar el prospecto
            //agregarRegistroProspecto();
            limpiarValoresFormulario();
            agregarRegistroProspecto();
            iniciarlizarDtDocumentos();
            this.lblEncabezadoModal.Text = "Captura del prospecto";
            //llenarDocumentos();
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "javascript: mostrarModalProspecto();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "javascript: inhabilitarBoton(false);", true);
        }

        protected void btnEliminar_click(object sender, EventArgs e)
        {
            //Mensaje de confirmación
            
        }
        protected void btnGuardar_click(object sender, EventArgs e)
        {
            bool existe = false;
            try
            {
                //Validar que todos esten eseleccioados
                if (this.txtnumeroCliente.Text.Trim() != "" && this.txtnombre.Text.Trim() != "" && this.txtPrimerApellido.Text.Trim() != "" && this.txtCalle.Text.Trim() != ""
                && this.txtNumeroCasa.Text.Trim() != "" && this.txtColonia.Text.Trim() != "" && this.txtCodigoPostal.Text.Trim() != "" && this.txtTelefono.Text.Trim() != "" && this.ddlEstatus.SelectedValue != "-1"
                && this.txtRfc.Text.Trim() != "" )
                {
                    if (gvDocumentosAltaProspecto.Rows.Count <= 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "javascript: mensaje('Favor capturar al menos un documento','info','#modalAgregarProspecto');", true);
                        return;
                    }

                    //Validar que se haya ingresado una observacion 
                    if (this.txtObservacion.Text.Trim() == "" && this.ddlEstatus.SelectedValue =="R")
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "javascript: mensaje('Favor capturar el motivo de rechazo','info','#modalAgregarProspecto');", true);
                        return;

                    }
           
                    //validar si es nuevo
                    #region Validar si es nuevo o actualizarlo

                    Prospecto prospecto = new Prospecto(int.Parse(this.txtnumeroCliente.Text));
                    List<Prospecto> listaProspecto = prospecto.consultarProspectos();
                    existe = listaProspecto.Count > 0 ? true : false;
                
                    #endregion


                    prospecto.numeroCliente = int.Parse(this.txtnumeroCliente.Text);
                    prospecto.nombre = this.txtnombre.Text;
                    prospecto.apellidoPaterno = this.txtPrimerApellido.Text;
                    prospecto.apellidoMaterno = this.txtSegundoApellido.Text;
                    prospecto.calle = this.txtCalle.Text;
                    prospecto.numeroDireccion = this.txtNumeroCasa.Text;
                    prospecto.colonia = this.txtColonia.Text;
                    prospecto.codigoPostal = int.Parse(this.txtCodigoPostal.Text);
                    prospecto.telefono = this.txtTelefono.Text;
                    prospecto.estatus = this.ddlEstatus.SelectedValue;
                    prospecto.observaciones = this.txtObservacion.Text;
                    prospecto.rfc = this.txtRfc.Text;
                    prospecto.existe = existe;


                    //Guardar solamente los registros nuevos

                    DataTable dt = (DataTable)ViewState["dtdocumentosGuardar"];
                    if(dt != null)
                    {
                        foreach (DataRow row in dt.Rows)
                        {

                            ObjectId objectIdDocumento = db.subirDocumento(row["rutaArchivo"].ToString(), row["nombreArchivo"].ToString());

                            //Guardar archivo
                            Documentos documentos = new Documentos();
                            documentos.descripcion = row["nombreDocumento"].ToString();
                            documentos.files_id = objectIdDocumento;
                            documentos.numeroCliente = int.Parse(this.txtnumeroCliente.Text);
                            documentos.rutaArchivo = row["rutaArchivo"].ToString();
                            documentos.nombreArchivo = row["nombreArchivo"].ToString();
                            documentos.existe = existe;
                            /*documentos.descripcion = row.Cells[0].Text;
                            documentos.files_id = objectIdDocumento;
                            documentos.numeroCliente = int.Parse (this.txtnumeroCliente.Text);
                            documentos.rutaArchivo = row.Cells[2].Text;
                            documentos.nombreArchivo = row.Cells[1].Text;
                            documentos.existe = existe;*/
                            documentos.guardar();

                            //subirDocumento

                        }

                    }
                   

                

                    prospecto.guardar();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "javascript: mensaje('Los datos fueron guardados exitosamente','success','');", true);
                    inicializarValores();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "javascript: mensaje('Favor de llenar los campos obligatorios','info','#modalAgregarProspecto');", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "javascript: mensaje('Ocurrió un error al guardar el archivo "+ ex.Message+"','error','');", true);
            }
        }


        protected void btnEvaluar_click(object sender, EventArgs e)
        {
            ViewState["pantallaActual"] = 1;
            this.lblEncabezadoModal.Text = "Evaluación del prospecto";
            activarComponentes(false);

            this.ddlEstatus.Enabled = true;
            this.txtObservacion.Enabled = true;

            this.btnEvaluar.Visible = false;
            this.btnGuardar.Visible = true;
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "javascript: mostrarModalProspecto();", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "javascript: inhabilitarBoton(true);", true);
            
        }
        protected void btnCancelar_click(object sender, EventArgs e)
        {
            //Validar en que pantalla estamos 
            if (this.lblEncabezadoModal.Text != "Información del prospecto")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "javascript: mostrarModalMensajeConfirmacion();", true);
            }
            
        }
        protected void btnCancelarEliminar_click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "javascript: mostrarModalProspecto();", true);
        }
        protected void btnConfirmar_click(object sender, EventArgs e)
        {
            inicializarValores();
        }

        protected void btnAceptarDocumentos_click(object sender, EventArgs e)
        {

            //Llenar el grid de la pantalla de alta de prospectos 
            DataTable dtdocumentos = (DataTable)ViewState["dtDocumentos"];
            gvDocumentosAltaProspecto.DataSource = dtdocumentos;
            gvDocumentosAltaProspecto.DataBind();

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "javascript: mostrarModalProspecto();", true);
        }
        protected void btnAgregarDocumentos_click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "javascript: mostrarModalDocumentos();", true);
        }
        protected void btnAgregarDocumentosGrid_click(object sender, EventArgs e)
        {
            //Validar que se seleccione un archivo y que se agregue un tipo de archivo

            if (this.txtNombreDocumento.Text.Trim() == "" || this.FileUpload1.FileName == "")
            {
                //Mandar mensaje que son obligatorios
                //ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "javascript: mensaje('La cantidad no debe ser mayor al stock','info');", true);
                this.lblErrorDocumentos.Visible = true;
            }
            else {
                DataTable dtdocumentos = (DataTable)ViewState["dtDocumentos"];
                DataRow dr = dtdocumentos.NewRow();
                dr["nombreDocumento"] = this.txtNombreDocumento.Text;
                dr["nombreArchivo"] = this.FileUpload1.FileName;
                dr["rutaArchivo"] = Server.MapPath("~/" + FileUpload1.FileName);

                FileUpload1.SaveAs(Server.MapPath("~/" + FileUpload1.FileName));

                dtdocumentos.Rows.Add(dr);
                gridAgregarDocumentos.DataSource = dtdocumentos;
                gridAgregarDocumentos.DataBind();

                //Poner de nuevo el datatable actualizado en la ViewState
                ViewState["dtDocumentos"] = dtdocumentos;

                //Desactivar el error
                this.lblErrorDocumentos.Visible = false;

                

                #region Guardar solo los documentos seleccionados en la sesion

                DataTable dtdocumentosGuardar = new DataTable();

                if ((DataTable)ViewState["dtdocumentosGuardar"] == null)
                {
                    dtdocumentosGuardar.Columns.Add(new DataColumn("nombreDocumento"));
                    dtdocumentosGuardar.Columns.Add(new DataColumn("nombreArchivo"));
                    dtdocumentosGuardar.Columns.Add(new DataColumn("rutaArchivo"));
                }
                else
                {
                    dtdocumentosGuardar = (DataTable)ViewState["dtdocumentosGuardar"];
                }

                DataRow datarow = dtdocumentosGuardar.NewRow();
                datarow["nombreDocumento"] = this.txtNombreDocumento.Text;
                datarow["nombreArchivo"] = this.FileUpload1.FileName;
                datarow["rutaArchivo"] = Server.MapPath("~/" + FileUpload1.FileName);

                FileUpload1.SaveAs(Server.MapPath("~/" + FileUpload1.FileName));

                dtdocumentosGuardar.Rows.Add(datarow);
                gridAgregarDocumentos.DataSource = dtdocumentosGuardar;
                gridAgregarDocumentos.DataBind();

                //Poner de nuevo el datatable actualizado en la ViewState
                ViewState["dtdocumentosGuardar"] = dtdocumentosGuardar;

                #endregion
            }
            //Volver a ejecutar la funcion para que vuelva a cargar el mismo modal 
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "javascript: mostrarModalDocumentos();", true);
            this.txtNombreDocumento.Text = "";

        }
        protected void btnCancelarDocumentos_click(object sender, EventArgs e)
        {
            iniciarlizarDtDocumentos();
            //Volver a ejecutar la funcion para que vuelva a cargar el mismo modal 
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "javascript: mostrarModalProspecto();", true);

        }
        
        private DataTable obtenerProspectos()
        {
            //Inicializar el grid
            List<Prospecto> listaProspectos = db.consultarProspectos();
            
            //Inicializar grid
            DataTable dtbResultados = new DataTable();


            dtbResultados.Columns.Add(new DataColumn("numeroCliente"));
            dtbResultados.Columns.Add(new DataColumn("nombre"));
            dtbResultados.Columns.Add(new DataColumn("apellidoPaterno"));
            dtbResultados.Columns.Add(new DataColumn("apellidoMaterno"));
            dtbResultados.Columns.Add(new DataColumn("estatus"));
            dtbResultados.Columns.Add(new DataColumn("observaciones"));

            foreach (Prospecto prospecto in listaProspectos)
            {
                DataRow row = dtbResultados.NewRow();
                row["numeroCliente"] = prospecto.numeroCliente;
                row["nombre"] = prospecto.nombre;
                row["apellidoPaterno"] = prospecto.apellidoPaterno;
                row["apellidoMaterno"] = prospecto.apellidoMaterno;
                row["estatus"] = prospecto.estatus;
                row["observaciones"] = prospecto.estatus == "R" ? row["observaciones"] = prospecto.observaciones : "";
                dtbResultados.Rows.Add(row);

            }


            return dtbResultados;
        }
        private void agregarRegistroProspecto()
        {
            inicializarValores();
            DataTable dt = this.obtenerProspectos();

            int siguienteNumero = Convert.ToInt32 ((dt.Compute("max([numeroCliente])", string.Empty))) +1 ;

            this.txtnumeroCliente.Text = siguienteNumero.ToString();
            this.ddlEstatus.SelectedValue = "E";
            this.ddlEstatus.Enabled = false;
        }

        private void llenarEstatus(int opcion)
        {

            ddlEstatus.Items.Clear();
            ddlEstatus.Items.Add(new ListItem("Seleccione el estatus", "-1"));
            if (opcion == 1)
            {
                ddlEstatus.Items.Add(new ListItem("Enviado", "E"));
                ddlEstatus.Items.Add(new ListItem("Autorizado", "A"));
                ddlEstatus.Items.Add(new ListItem("Rechazado", "R"));
            }
            else {

                ddlEstatus.Items.Add(new ListItem("Enviado", "E"));
                ddlEstatus.Items.Add(new ListItem("Autorizado", "A"));
                ddlEstatus.Items.Add(new ListItem("Rechazado", "R"));
            }
        }


        private void inicializarValores()
        {
            #region LimpiarTextbox
            limpiarValoresFormulario();

            #endregion

            #region LlenarGrid
            gvProspectos.DataSource = obtenerProspectos();
            gvProspectos.DataBind();
            #endregion

            #region Llenar estatus
            //Llenar los estatus
            llenarEstatus(2);
            #endregion

            //llenarDocumentos();

            iniciarlizarDtDocumentos();

            this.lblErrorDocumentos.Visible = false;

            this.lblEncabezadoModal.Text = "Captura del prospecto";

            activarComponentes(true);

            this.btnEvaluar.Visible = false;
            this.btnGuardar.Visible = true;

            //ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "javascript: inhabilitarBoton('false');", true);

        }

        public void limpiarValoresFormulario()
        {

            this.txtNumeroCasa.Text = "";
            this.txtnombre.Text = "";
            this.txtPrimerApellido.Text = "";
            this.txtSegundoApellido.Text = "";
            this.txtColonia.Text = "";
            this.txtCodigoPostal.Text = "";
            this.txtCalle.Text = "";
            this.txtRfc.Text = "";
            this.txtnumeroCliente.Text = "";
            this.txtObservacion.Text = "";
            this.txtTelefono.Text = "";
            this.txtnumeroCliente.Text = "";
            this.ddlEstatus.SelectedIndex = -1;
        }
       
        private void iniciarlizarDtDocumentos()
        {
            //Inicializar grid
            DataTable dtDocumentos = new DataTable();
            //dtDocumentos.Columns.Add(new DataColumn("tipoDocumento"));
            dtDocumentos.Columns.Add(new DataColumn("nombreDocumento"));
            dtDocumentos.Columns.Add(new DataColumn("nombreArchivo"));
            dtDocumentos.Columns.Add(new DataColumn("rutaArchivo"));

            ViewState["dtDocumentos"] = dtDocumentos;
            ViewState["dtdocumentosGuardar"] = null;
            gvDocumentosAltaProspecto.DataSource = dtDocumentos;
            gvDocumentosAltaProspecto.DataBind();

            gridAgregarDocumentos.DataSource = dtDocumentos;
            gridAgregarDocumentos.DataBind();
                        
        }

        protected void gvProspectos_RowDataBound1(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(this.gvProspectos, "Select$" + e.Row.RowIndex, true);
                e.Row.ToolTip = "Click para seleccionar";
            }
        }

        protected void gvProspectos_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow rowselected = gvProspectos.SelectedRow;
            this.lblEncabezadoModal.Text = "Información del prospecto";
            DataTable dtProspectos = obtenerProspectos();

            // DataRow[] dataRowProspecto = dtProspectos.Select("numeroCliente = " + rowselected.Cells[0].Text);
            Prospecto prospecto = new Prospecto(int.Parse(rowselected.Cells[0].Text));

            Prospecto seleccionado = prospecto.consultarProspectos()[0];
            
            this.txtNumeroCasa.Text = seleccionado.numeroDireccion;
            this.txtnombre.Text = seleccionado.nombre;
            this.txtPrimerApellido.Text = seleccionado.apellidoPaterno;
            this.txtSegundoApellido.Text = seleccionado.apellidoMaterno;
            this.txtColonia.Text = seleccionado.colonia;
            this.txtCodigoPostal.Text = seleccionado.codigoPostal.ToString();
            this.txtCalle.Text = seleccionado.calle;
            this.txtRfc.Text = seleccionado.rfc;
            this.txtnumeroCliente.Text = seleccionado.numeroCliente.ToString();
            this.txtObservacion.Text = seleccionado.observaciones;
            this.txtTelefono.Text = seleccionado.telefono;
            this.ddlEstatus.SelectedValue = seleccionado.estatus;



            activarComponentes(false);

            this.btnEvaluar.Visible = true;
            this.btnGuardar.Visible = false;
            

            Documentos doc = new Documentos(int.Parse(rowselected.Cells[0].Text));

            List<Documentos> documentos =  doc.consultarDocumentos();

            iniciarlizarDtDocumentos();
            DataTable dtdocumentos = (DataTable)ViewState["dtDocumentos"];
            foreach (Documentos d in documentos)
            {
                DataRow dr = dtdocumentos.NewRow();
                dr["nombreDocumento"] = d.descripcion;
                dr["nombreArchivo"] = d.nombreArchivo;
                dr["rutaArchivo"] = d.rutaArchivo;

                dtdocumentos.Rows.Add(dr);
            }

            gvDocumentosAltaProspecto.DataSource = dtdocumentos;
            gvDocumentosAltaProspecto.DataBind();

            //Poner de nuevo el datatable actualizado en la ViewState
            ViewState["dtDocumentos"] = dtdocumentos;

            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "javascript: mostrarModalProspecto();", true);

            //Ejecutar el metodo de javascript para inhabilitar el boton de tipo <button> 
            ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), "javascript: inhabilitarBoton(true);", true);
        }

        protected void gvProspectos_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }
        private void activarComponentes( bool tipo)
        {

            this.txtnombre.Enabled = tipo;
            this.txtPrimerApellido.Enabled = tipo;
            this.txtSegundoApellido.Enabled = tipo;
            this.txtCalle.Enabled = tipo;
            this.txtColonia.Enabled = tipo;
            this.txtCodigoPostal.Enabled = tipo;
            this.txtNumeroCasa.Enabled = tipo;
            this.txtObservacion.Enabled = tipo;
            this.txtTelefono.Enabled = tipo;
            this.txtRfc.Enabled = tipo;
            this.ddlEstatus.Enabled = tipo;

        }
            
    }
}